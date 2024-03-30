using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.User;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<RegisterResponse> RegisterDevUserAsync(RegisterRequest request, string origin);
        Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request, string origin);
        Task SignOutAsync();

		Task<UserResponse> GetUserWithId(UserRequest request);
		Task<List<UserViewModel>> GetAllUsers();
        Task<string> ActivateUserAsync(string id);
        Task<string> InactivateUserAsync(string id);
    }
}