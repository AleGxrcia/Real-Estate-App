using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.Property
{
	public class FiltersPropertyViewModel
	{
		public int? PropertyType { get; set; }
		public int? MinPrice { get; set; }
		public int? MaxPrice { get; set; }
		public int? NumberOfBathRooms { get; set; }
		public int? NumberOfRooms { get; set; }
	}
}
