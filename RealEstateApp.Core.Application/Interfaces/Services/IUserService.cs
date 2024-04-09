using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin, IFormFile file);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task SignOutAsync();


		Task<UserResponse> getUserByIdAsync(string id);
		Task<List<UserViewModel>> GetAllUsers();
        Task<string> ChangeUserStatus(string id, bool activate);
        Task<RegisterResponse> RegisterAdminAsync(SaveUserAdminViewModel vm, string origin);
        Task UpdateAsync(SaveUserAdminViewModel vm, string id);
        Task<RegisterResponse> RegisterDevAsync(SaveUserAdminViewModel vm, string origin);

		Task<List<UserViewModel>> GetAllAgents();
        Task<string> UpdateUser(EditUserViewModel vm);


		Task AddFavorite(string clienteId, int propertyId);
		Task RemoveFavorite(string clienteId, int propertyId);

		Task<List<PropertyViewModel>> GetFavoriteProperties(string userId);
        Task<string> DeleteUser(string id);
    }
}