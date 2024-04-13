using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe colocar el correo del usuario")]
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.Text)]
        public string EmailOrUserName { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
