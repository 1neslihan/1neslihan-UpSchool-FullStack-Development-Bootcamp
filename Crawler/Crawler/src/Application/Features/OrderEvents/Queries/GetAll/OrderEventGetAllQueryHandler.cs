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
    public class OrderEventGetAllQueryHandler : IRequestHandler<OrderEventGetAllQuery, List<OrderEventGetAllDto>>
    {
        public readonly IApplicationDbContext _applicationDbContext;

        public OrderEventGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }

        public async Task<List<OrderEventGetAllDto>> Handle(OrderEventGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.OrderEvents.AsQueryable();
            //dbQuery=dbQuery.Where(x=>x.Id==request.Id);
            dbQuery=dbQuery.Where(x => x.IsDeleted==request.IsDeleted && x.OrderId==request.OrderId);

            if (request.IsDeleted.HasValue) dbQuery=dbQuery.Where(x => x.IsDeleted==request.IsDeleted.Value);

            var orderEventDtos = await dbQuery
                .Select(x => new OrderEventGetAllDto(x.Id, x.OrderId, x.Status, x.IsDeleted))
                .ToListAsync(cancellationToken);

            return orderEventDtos;
            
        }

       
    }
}
