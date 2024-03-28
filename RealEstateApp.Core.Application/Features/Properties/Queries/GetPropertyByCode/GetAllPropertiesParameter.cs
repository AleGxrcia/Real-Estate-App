using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyByCode
{
    /// <summary>
    /// Parámetros para filtrar los propertyos
    /// </summary>  
    public class GetAllPropertiesParameter
    {
        /// <example>1</example>
        [SwaggerParameter(Description = "Colocar el codigo de la propiedad por la cual quiere filtrar")]
        [DefaultValue("1")]
        public int PropertyCode { get; set; }
    }
}
