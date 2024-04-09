using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Core.Application.Services
{
	public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin, IFormFile file)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterBasicUserAsync(registerRequest, origin, vm.Photo);
        }

        public async Task<RegisterResponse> RegisterAdminAsync(SaveUserAdminViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterAdminUserAsync(registerRequest, origin);
        }

        public async Task<RegisterResponse> RegisterDevAsync(SaveUserAdminViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterDevUserAsync(registerRequest, origin);
        }

        public async Task UpdateAsync(SaveUserAdminViewModel vm, string id)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            await _accountService.UpdateUserAsync(registerRequest, id);
        }

        public async Task<string> DeleteUser(string id)
        {
            return await _accountService.DeleteUserAsync(id);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(forgotRequest, origin);
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }

        public async Task<string> ChangeUserStatus(string id, bool activate)
        {
           return await _accountService.ChangeUserStatusAsync(id, activate);
        }

        public async Task<List<UserViewModel>> GetAllUsers()
		{
			return await _accountService.GetAllUsers();
		}

		public async Task<List<UserViewModel>> GetAllAgents()
		{
			return await _accountService.GetAllAgents();
		}

		public async Task<UserResponse> getUserByIdAsync(string id)
		{
			UserRequest userId = new();
			userId.Id = id;
			return await _accountService.GetUserWithId(userId);

		}

        public async Task<string> UpdateUser(EditUserViewModel vm) 
        {
            return await _accountService.UpdateUser(vm);
            
        }

		public async Task AddFavorite(string clienteId, int propertyId)
		{
			await _accountService.AddFavorite(clienteId, propertyId);   
		}

		public async Task RemoveFavorite(string clienteId, int propertyId)
		{
			await _accountService.RemoveFavorite(clienteId, propertyId);
		}

        public async Task<List<PropertyViewModel>> GetFavoriteProperties(string userId) 
        {
            var favoriteProperties = _mapper.Map<List<PropertyViewModel>>(await _accountService.GetFavoriteProperties(userId));


            return favoriteProperties;
        }
	}
}
