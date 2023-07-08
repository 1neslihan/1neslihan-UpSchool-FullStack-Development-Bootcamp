using Application.Common.Interfaces;
using Application.Features.Products.Queries.GetAll;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetByOrderIdQueryHandler : IRequestHandler<OrderEventGetByOrderIdQuery, List<OrderEventGetByOrderIdDto>>
    {
        public readonly IApplicationDbContext _applicationDbContext;

        public OrderEventGetByOrderIdQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }

        public async Task<List<OrderEventGetByOrderIdDto>> Handle(OrderEventGetByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.OrderEvents.AsQueryable();

            dbQuery= request.IsDeleted.HasValue
                ? dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value && x.OrderId==request.OrderId)
                : dbQuery.Where(x=>x.OrderId==request.OrderId);

            var orderEventDtos = await dbQuery
                .Select(x => new OrderEventGetByOrderIdDto(x.Id, x.OrderId, x.Status, x.IsDeleted))
                .ToListAsync(cancellationToken);

            return orderEventDtos;
            
        } 
    }
}
