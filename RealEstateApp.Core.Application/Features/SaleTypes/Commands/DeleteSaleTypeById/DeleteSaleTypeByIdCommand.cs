using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById
{
    /// <summary>
    /// Parámetros para la eliminacion de una tipo de venta
    /// </summary> 
    public class DeleteSaleTypeByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se desea eliminar")]
        public int Id { get; set; }
    }
    public class DeleteSaleTypeByIdCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, Response<int>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        public DeleteSaleTypeByIdCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }
        public async Task<Response<int>> Handle(DeleteSaleTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);
            if (saleType == null) throw new ApiException($"Sale Type not found.", (int)HttpStatusCode.NotFound);
            await _saleTypeRepository.DeleteAsync(saleType);
            return new Response<int>(saleType.Id);
        }
    }
}
