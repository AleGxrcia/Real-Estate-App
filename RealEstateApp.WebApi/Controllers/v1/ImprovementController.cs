using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]

    [SwaggerTag("Mantenimiento de mejoras")]
    public class ImprovementController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Developer, Admin")]


        [SwaggerOperation(
          Summary = "Listado de mejoras",
          Description = "Obtiene todas las mejoras creadas"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            return Ok(await Mediator.Send(new GetAllImprovementQuery()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Developer, Admin")]

        [SwaggerOperation(
             Summary = "mejora por id",
             Description = "Obtiene una mejora por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveImprovementViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetImprovementByIdQuery { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
                 Summary = "Creacion de mejora",
                 Description = "Recibe los parametros necesarios para crear una nueva mejora"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post(CreateImprovementCommand command)
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
                  Summary = "Actualizacion de una mejora",
                  Description = "Recibe los parametros necesarios para modificar una mejora"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveImprovementViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Put(int id, UpdateImprovementCommand command)
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
              Summary = "Eliminar una mejora",
              Description = "Recibe los parametros necesarios para eliminar ua mejora existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });
            return NoContent();
        }
    }
}
