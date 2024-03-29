using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.PropertyType
{
    public class SavePropertyTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar un nombre")]
		[DataType(DataType.Text)]
		public string Name { get; set; }
		[Required(ErrorMessage = "Debe colocar una descripcion")]
		[DataType(DataType.Text)]
		public string Description { get; set; }
    }
}
