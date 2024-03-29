using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.User;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]

    [SwaggerTag("Mantenimiento de mejoras")]
    public class AgentController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Developer, Admin")]


        [SwaggerOperation(
          Summary = "Listado de agentes",
          Description = "Obtiene todas los agentes creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {

            return Ok(await Mediator.Send(new GetAllAgentsQuery()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Developer, Admin")]

        [SwaggerOperation(
             Summary = "obtiene un agente por id",
             Description = "Obtiene un agente por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveImprovementViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(string id)
        {
            return Ok(await Mediator.Send(new GetAgentByIdQuery { Id = id }));
        }


        [HttpGet("Properties/{id}")]
        [Authorize(Roles = "Developer, Admin")]

        [SwaggerOperation(
             Summary = "obtiene las propiedades de un agente por id",
             Description = "Obtiene las propiedades de un agente por id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveImprovementViewModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAgentProperties(string id)
        {
            return Ok(await Mediator.Send(new GetAgentPropertiesQuery { Id = id }));
        }

    }
}
