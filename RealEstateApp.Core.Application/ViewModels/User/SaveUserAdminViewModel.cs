using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.User
{
    public class SaveUserAdminViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cédula")]
        [DataType(DataType.Text)]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public string? UserType { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
