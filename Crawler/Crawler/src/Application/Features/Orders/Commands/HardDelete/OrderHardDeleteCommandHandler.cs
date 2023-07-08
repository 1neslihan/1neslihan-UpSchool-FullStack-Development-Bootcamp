using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.HardDelete
{
    public class OrderHardDeleteCommandHandler : IRequestHandler<OrderHardDeleteCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderHardDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }
        public async Task<Response<Guid>> Handle(OrderHardDeleteCommand request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders.AsQueryable();
            var order = await dbQuery.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            _applicationDbContext.Orders.Remove(order);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>("The order  was succesfully deleted.");

        }

    }
    
}
