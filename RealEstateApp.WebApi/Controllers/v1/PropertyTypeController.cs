using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType;
using RealEstateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById;
using RealEstateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealEstateApp.Core.Application.Features.PropertyTypes.Queries.GetAllSaleType;
using RealEstateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]

    [SwaggerTag("Mantenimiento de tipo de propiedades")]
    public class PropertyTypeController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Developer, Admin")]


        [SwaggerOperation(
          Summary = "Listado de tipo de propiedades",
          Description = "Obtiene todas los tipos de propiedades creadas"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            return Ok(await Mediator.Send(new GetAllPropertyTypeQuery()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Developer, Admin")]

        [SwaggerOperation(
             Summary = "tipo de propiedad por id",
             Description = "Obtiene un tipo de propiedad por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SavePropertyTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
                 Summary = "Creacion de tipo de propiedad",
                 Description = "Recibe los parametros necesarios para crear un nuevo tipo de propiedad"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post(CreatePropertyTypeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
                  Summary = "Actualizacion de un tipo propiedad",
                  Description = "Recibe los parametros necesarios para modificar un tipo de propiedad existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SavePropertyTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Put(int id, UpdatePropertyTypeCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
              Summary = "Eliminar un tipo de propiedad",
              Description = "Recibe los parametros necesarios para eliminar un tipo de propiedad existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeletePropertyTypeByIdCommand { Id = id });
            return NoContent();
        }
    }
}
