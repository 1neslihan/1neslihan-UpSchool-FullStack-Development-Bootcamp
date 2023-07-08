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

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, List<OrderGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }

        public async Task<List<OrderGetAllDto>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders.AsQueryable();
            
            
            dbQuery = request.IsDeleted.HasValue
                ? dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value)
                : dbQuery.Where(x => x.IsDeleted == false || x.IsDeleted == true);

            var orderDtos= await dbQuery
                .Select(x=> new OrderGetAllDto(x.Id, x.RequestedAmount, x.TotalFoundAmount, x.ProductCrawlType, x.IsDeleted))
                .ToListAsync(cancellationToken);
            return orderDtos;
        }

        
    }
}
