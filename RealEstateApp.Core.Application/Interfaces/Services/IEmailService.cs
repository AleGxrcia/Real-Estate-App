using RealEstateApp.Core.Application.Dtos.Email;
using RealEstateApp.Core.Domain.Settings;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}
