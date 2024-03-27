using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.SaleType
{
    public class SaveSaleTypeViewModel
    {
		[Required(ErrorMessage = "Debe colocar un nombre")]
		[DataType(DataType.Text)]
		public string Name { get; set; }
		[Required(ErrorMessage = "Debe colocar una descripcion")]
		[DataType(DataType.Text)]
		public string Description { get; set; }
    }
}
