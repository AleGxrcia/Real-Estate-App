using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.Properties.Commands.CreateProperty;

using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyById;
using RealEstateApp.Core.Application.ViewModels.Property;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Developer, Admin")]


    [SwaggerTag("Mantenimiento de propiedades")]
    public class PropertyController : BaseApiController
    {
        [HttpGet]


        [SwaggerOperation(
          Summary = "Listado de propiedades",
          Description = "Obtiene todas las propiedades creadas"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            return Ok(await Mediator.Send(new GetAllPropertiesQuery()));
        }

        [HttpGet("{id}")]

        [SwaggerOperation(
             Summary = "Propiedades por id",
             Description = "Obtiene una propiedad por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SavePropertyViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetPropertiesByIdQuery { Id = id }));
        }

        [HttpGet("code/{code}")]

        [SwaggerOperation(
             Summary = "Propiedad por codigo de propiedad",
             Description = "Obtiene una propiedad por codigo de propiedad como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SavePropertyViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetByCode(int code)
        {
            return Ok(await Mediator.Send(new GetPropertiesByCodeQuery { PropertyCode = code }));
        }


    }
}
