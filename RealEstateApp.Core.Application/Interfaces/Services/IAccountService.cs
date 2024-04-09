using Microsoft.AspNetCore.Http;

using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin, IFormFile file);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<RegisterResponse> RegisterDevUserAsync(RegisterRequest request, string origin);
        Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request, string origin);
        Task SignOutAsync();

        Task<UserResponse> GetUserWithId(UserRequest request);
        Task<List<UserViewModel>> GetAllUsers();
        Task<List<UserViewModel>> GetAllAgents();
        Task<UserResponse> UpdateIsActiveAgent(string id, bool isActive);
        Task<string> ChangeUserStatusAsync(string id, bool activate);
        Task<string> UpdateUserAsync(RegisterRequest request, string userId);

        Task<string> UpdateUser(EditUserViewModel vm);
        Task AddFavorite(string clienteId, int propertyId);
		Task RemoveFavorite(string clienteId, int propertyId);

        Task<List<Property>> GetFavoriteProperties(string userId);
        Task<string> DeleteUserAsync(string userId);
    }
}