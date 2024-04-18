using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using RealEstateApp.Core.Application.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly ApplicationContext _db;

        public AccountService(
              UserManager<ApplicationUser> userManager,
              SignInManager<ApplicationUser> signInManager,
              IEmailService emailService,
              IOptions<JWTSettings> jwtSettings,
              ApplicationContext db
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _db = db;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.EmailOrUserName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            }
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.EmailOrUserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.EmailOrUserName}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.EmailOrUserName}";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin, IFormFile file)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.Phone,
                IsActive = true,

            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var userCreated = await _userManager.FindByEmailAsync(user.Email);
                userCreated.PhotoUrl = FileManagerHelper.UploadFile(file, userCreated.Id, "UserProfile");
                await _userManager.UpdateAsync(userCreated);
                if (request.UserType == Roles.Agent.ToString())
                {

                    await _userManager.AddToRoleAsync(user, Roles.Agent.ToString());
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Roles.Client.ToString());

                }
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"Please confirm your account visiting this URL {verificationUri}",
                    Subject = "Confirm registration"
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                IdNumber = request.Identification,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<RegisterResponse> RegisterDevUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                IdNumber = request.Identification,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Developer.ToString());
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<string> UpdateUserAsync(RegisterRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.IdNumber = request.Identification;
            user.UserName = request.UserName;

            if (request.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (!resetResult.Succeeded)
                {
                    return $"An error occurred while trying to reset the password.";
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return $"Update successful.";
            }
            else
            {
                return $"An error ocurred trying to update the user.";
            }
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                FileManagerHelper.DeleteFile(userId, "UserProfile");
                return "User successfully deleted.";
            }
            else
            {
                return "An error occurred trying to delete the user.";
            }
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred wgile confirming {user.Email}.";
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your account visiting this URL {verificationUri}",
                Subject = "reset password"
            });


            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        public async Task<string> ChangeUserStatusAsync(string id, bool activate)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return "User not found";
            }

            user.IsActive = activate;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (activate)
                {
                    return $"User successfully activated.";
                }

                return $"User successfully inactivated.";
            }
            else
            {
                if (activate)
                {
                    return $"An error occurred trying to activate the user.";
                }

                return $"An error occurred trying to inactivate the user.";
            }
        }


        public async Task<UserResponse> GetUserWithId(UserRequest request)
        {
            UserResponse response = new();
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe la cuenta requerida";
                return response;
            }

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.UserName = user.UserName;
            response.Phone = user.PhoneNumber;
            response.Email = user.Email;
            response.PhotoUrl = user.PhotoUrl;
            response.IsActive = user.IsActive;
            response.IdNumber = user.IdNumber;

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault().ToString();
            response.Role = userRole;

            return response;
        }

        public async Task<UserResponse> UpdateIsActiveAgent(string id, bool isActive)
        {
            UserResponse response = new();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No existe la cuenta requerida";
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault().ToString();



            if (!userRole.Contains("Agent"))
            {
                response.HasError = true;
                response.Error = $"No existe el agente requerido";
                return response;
            }

            user.IsActive = isActive;
            await _userManager.UpdateAsync(user);

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.UserName = user.UserName;
            response.Phone = user.PhoneNumber;
            response.Email = user.Email;
            response.IsActive = isActive;

            return response;
        }


        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl,
                    IdNumber = user.IdNumber,
                    IsActive = user.IsActive
                };

                // Obtener los roles del usuario
                var roles = await _userManager.GetRolesAsync(user);

                // Obtener el primer rol asignado al usuario y asignarlo al tipo de usuario
                if (roles.Any())
                {
                    userViewModel.Role = roles.First().ToString();
                }
                if (userViewModel.Role != "Admin" || userViewModel.Role != "Developer")
                {
                    userViewModels.Add(userViewModel);
                }
            }

            return userViewModels;
        }


        public async Task<List<UserViewModel>> GetAllAgents()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl,
                    IsActive = user.IsActive
                };

                // Obtener los roles del usuario
                var roles = await _userManager.GetRolesAsync(user);

                // Obtener el primer rol asignado al usuario y asignarlo al tipo de usuario
                if (roles.Any())
                {
                    userViewModel.Role = roles.First().ToString();
                }
                if (userViewModel.Role == Roles.Agent.ToString() && user.IsActive)
                {
                    userViewModels.Add(userViewModel);
                }
            }

            return userViewModels;
        }




        #region PrivateMethods

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);


            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }


        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Auth/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }
        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Auth/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }

        public async Task<string> UpdateUser(EditUserViewModel vm)
        {
            string Error = "";
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
            {
                Error = $"No se encontró ningún usuario con el ID: {vm.Id}";
                return Error;
            }

            // Verificar si el nuevo nombre de usuario está en uso por otro usuario
            if (user.UserName != vm.Username)
            {
                var existingUser = await _userManager.FindByNameAsync(vm.Username);

                if (existingUser != null && existingUser.Id != user.Id)
                {
                    Error = $"El nombre de usuario '{vm.Username}' ya está en uso por otro usuario.";
                    return Error;
                }
            }

            if (user.PhoneNumber != vm.Phone)
            {
                var existingUser = await _userManager.FindByNameAsync(vm.Username);

                if (existingUser != null && existingUser.Id != user.Id)
                {
                    Error = $"El numero de telefono '{vm.Username}' ya está en uso por otro usuario.";
                    return Error;
                }
            }

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.UserName = vm.Username;
            user.PhoneNumber = vm.Phone;

            // Actualizar la foto solo si se proporciona una nueva
            if (vm.Photo != null)
            {
                user.PhotoUrl = user.PhotoUrl != null ? FileManagerHelper.UploadFile(vm.Photo, user.Id, "UserProfile", true, user.PhotoUrl) : //Operador ternario
                FileManagerHelper.UploadFile(vm.Photo, user.Id, "UserProfile");
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                Error = $"Error al actualizar el usuario: {string.Join(", ", errors)}";
                return Error;
            }


            return Error;
        }


        public async Task AddFavorite(string clienteId, int propertyId)
        {

            // Verificar si ya existe la propiedad en favoritos
            var existingFavorite = await _db.FavoriteProperties
                .FirstOrDefaultAsync(fp => fp.ClientId == clienteId && fp.PropertyId == propertyId);

            if (existingFavorite != null)
            {
                // La propiedad ya está en favoritos
                return;
            }

            // Crear una nueva relación de favoritos
            var newFavorite = new FavoriteProperty
            {
                ClientId = clienteId,
                PropertyId = propertyId
            };

            _db.FavoriteProperties.Add(newFavorite);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFavorite(string clienteId, int propertyId)
        {

            // Buscar la propiedad en favoritos
            var favorite = await _db.FavoriteProperties
                .FirstOrDefaultAsync(fp => fp.ClientId == clienteId && fp.PropertyId == propertyId);

            if (favorite != null)
            {
                // La propiedad está en favoritos, la eliminamos
                _db.FavoriteProperties.Remove(favorite);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Property>> GetFavoriteProperties(string userId)
        {
            var favoriteProperties = await _db.FavoriteProperties
                .Where(fp => fp.ClientId == userId)
                .Join(_db.Properties,
                    fp => fp.PropertyId,
                    p => p.Id,
                    (fp, p) => p)
                .ToListAsync();

            return favoriteProperties;
        }

        #endregion
    }


}
