using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Properties.Commands.CreateProperty
{
    /// <summary>
    /// Parámetros para la creacion de un propertyo
    /// </summary>  
    public class CreatePropertyCommand : IRequest<Response<int>>
    {
        /// <example>1, 2, 3</example>
        [SwaggerParameter(Description = "El tipo de propiedad")]
        public int PropertyTypeId { get; set; }


        /// <example>Propiedad ubicada en los alamos con vista al mar</example>
        [SwaggerParameter(Description = "Una descripcion de la propiedad")]
        public string? Description { get; set; }


        /// <example>500.00</example>
        [SwaggerParameter(Description = "El precio por el cual se vendera la propiedad")]
        public decimal Price { get; set; }


        /// <example>200</example>
        [SwaggerParameter(Description = "El tamaño de la propiedad")]
        public decimal LandSize { get; set; }


        /// <example>3</example>
        [SwaggerParameter(Description = "El numero de habitaciones de la propiedad")]
        public int NumberOfRooms { get; set; }



        /// <example>200</example>
        [SwaggerParameter(Description = "La cantidad de habitaciones de la propiedad")]
        public int NumberOfBathrooms { get; set; }

        /// <example>1,2</example>
        [SwaggerParameter(Description = "El listados de mejoras de la propiedad")]

        public List<int>? ImprovementsId { get; set; }


        /// <example>07d360ae-7821-4199-a5b3-f6cd602645b8</example>
        [SwaggerParameter(Description = "El id del agente que agrego esta propiedad")]
        public string AgentId { get; set; }


        //Images 

        /// <example>Real-Estate-App\RealEstateApp\images</example>

        [SwaggerParameter(Description = "lista de imagenes de la propiedad")]
        public List<string?> ImagesUrl { get; set; }
    }

    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Response<int>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<Property>(command);

            var existingCodes = await _propertyRepository.GetAllAsync();

            // Generar un nuevo codigo de propiedad
            int newPropertyCode;
            do
            {
                newPropertyCode = GeneratePropertyCode();
            }
            while (existingCodes.Any(property => property.Code == newPropertyCode));

            // Agregar el codigo de propiedad
            property.Code = newPropertyCode;

            property = await _propertyRepository.AddAsync(property);

            // Agregar las imagenes a la tabla propertyImages
            if (command.ImagesUrl != null)
            {
                await _propertyRepository.AddImagesAsync(command.ImagesUrl, property.Id);
            }

            if (command.ImprovementsId != null)
            {
                //chequear si hay mejoras repetidas en los improvements
                bool repeat = command.ImprovementsId.GroupBy(x => x).Any(g => g.Count() > 1);
                if (!repeat)
                {
                    await _propertyRepository.AddImagesAsync(command.ImagesUrl, property.Id);
                }
                else
                {
                    throw new ApiException($"Cannot have repeated improvements.", (int)HttpStatusCode.BadRequest);
                }
            }

            return new Response<int>(property.Id);
        }


        private int GeneratePropertyCode()
        {
            Random random = new Random();
            StringBuilder propertyCode = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                propertyCode.Append(random.Next(0, 10)); // Genera un dígito aleatorio entre 0 y 9
            }

            return int.Parse(propertyCode.ToString());
        }
    }


}
