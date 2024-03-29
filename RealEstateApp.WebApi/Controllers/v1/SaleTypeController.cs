using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.SaleTypes.Commands.Create;
using RealEstateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById;
using RealEstateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RealEstateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleType;
using RealEstateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]

    [SwaggerTag("Mantenimiento de tipo de ventas")]
    public class SaleTypeController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Developer, Admin")]


        [SwaggerOperation(
          Summary = "Listado de tipo de ventas",
          Description = "Obtiene todas los tipos de venta creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            return Ok(await Mediator.Send(new GetAllSaleTypeQuery()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Developer, Admin")]

        [SwaggerOperation(
             Summary = "tipo de venta por id",
             Description = "Obtiene un tipo de venta por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveSaleTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetSaleTypeByIdQuery { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
                 Summary = "Creacion de tipo de venta",
                 Description = "Recibe los parametros necesarios para crear un nuevo tipo de venta"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post(CreateSaleTypeCommand command)
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
                  Summary = "Actualizacion de un tipo venta",
                  Description = "Recibe los parametros necesarios para modificar un tipo de venta existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveSaleTypeViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Put(int id, UpdateSaleTypeCommand command)
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
              Summary = "Eliminar un tipo de venta",
              Description = "Recibe los parametros necesarios para eliminar un tipo de venta existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });
            return NoContent();
        }
    }
}
