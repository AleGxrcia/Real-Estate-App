using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Improvement
{
    public class SaveImprovementViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar un nombre")]
        [Display(Name = "Nombre")]
        [DataType(DataType.Text)]
		public string Name { get; set; }
		[Required(ErrorMessage = "Debe colocar una descripcion")]
        [Display(Name = "Descripción")]
		[DataType(DataType.Text)]
		public string Description { get; set; }
    }
}
