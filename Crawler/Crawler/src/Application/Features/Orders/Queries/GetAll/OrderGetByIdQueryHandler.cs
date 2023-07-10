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
    public class OrderGetByIdQueryHandler : IRequestHandler<OrderGetByIdQuery, List<OrderGetByIdDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public OrderGetByIdQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext=applicationDbContext;
            _currentUserService=currentUserService;
        }

        public async Task<List<OrderGetByIdDto>> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == _currentUserService.UserId)
                .Where(x=>x.Id==request.Id);
                     
            dbQuery = request.IsDeleted.HasValue
                ? dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value)
                : dbQuery.Where(x => x.IsDeleted == false || x.IsDeleted == true);

            var orderDtos= await dbQuery
                .Select(x=> new OrderGetByIdDto(x.Id, x.RequestedAmount, x.TotalFoundAmount, x.ProductCrawlType, x.IsDeleted, x.UserId))
                .ToListAsync(cancellationToken);
            return orderDtos;
        }

        
    }
}
