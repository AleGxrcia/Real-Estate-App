using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType

{
    public class SaleTypeUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
