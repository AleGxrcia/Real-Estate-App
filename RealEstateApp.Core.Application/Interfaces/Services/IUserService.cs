using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.User;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task SignOutAsync();


		Task<UserResponse> getUserByIdAsync(string id);
		Task<List<UserViewModel>> GetAllUsers();
        Task<string> ActiveUser(string id);
        Task<string> InactiveUser(string id);
    }
}