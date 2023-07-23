using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.Orders.Queries.GetByDate
{
    public class OrderGetByDateQueryHandler : IRequestHandler<OrderGetByDateQuery, List<OrderGetByDateDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public OrderGetByDateQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
        {
            _applicationDbContext=applicationDbContext;
            _currentUserService=currentUserService;
        }

        public async Task<List<OrderGetByDateDto>> Handle(OrderGetByDateQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == _currentUserService.UserId);

            //if (request.StartDate.HasValue && request.EndDate.HasValue)
            //{
            //    dbQuery = dbQuery.Where(x => x.CreatedOn.Date >= request.StartDate.Value.Date && x.CreatedOn.Date <= request.EndDate.Value.Date);
            //}

            //else if (request.StartDate.HasValue)
            //{
            //    dbQuery = dbQuery.Where(x => x.CreatedOn.Date >= request.StartDate.Value.Date && x.CreatedOn.Date <= DateTime.Now.Date);
            //}

            //else if (request.EndDate.HasValue)
            //{
            //    dbQuery = dbQuery.Where(x => x.CreatedOn.Date >= DateTime.MinValue.Date && x.CreatedOn.Date <= request.EndDate.Value.Date);
            //}

            //else if(request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate.Value.Date == request.EndDate.Value.Date)
            //{
            //    var date = request.StartDate.Value.Date;
            //    dbQuery = dbQuery.Where(x => x.CreatedOn.Date == date);
            //}

            dbQuery = request.IsDeleted.HasValue
                ? dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value)
                : dbQuery.Where(x => x.IsDeleted == false || x.IsDeleted == true);

            dbQuery=dbQuery.OrderByDescending(x => x.CreatedOn);



            var order = await dbQuery
                .Include(x => x.OrderEvents)
                .Include(x => x.Products)
                .ToListAsync(cancellationToken);



            var orderRecords = order.Select(order => new OrderGetByDateDto
            {
                Id = order.Id,
                RequestedAmount = order.RequestedAmount,
                TotalFoundAmount = order.TotalFoundAmount,
                UserId = order.UserId,
                ProductCrawlType = order.ProductCrawlType,
                IsDeleted = order.IsDeleted,
                OrderCreatedOn=order.CreatedOn,
                OrderEventDtos = order.OrderEvents.Select(orderEvent => new OrderEventDto
                {
                    OrderIdForEvents=order.Id,
                    Status = orderEvent.Status,
                    OrderEventCreatedOn=orderEvent.CreatedOn
                   
                }).ToList(),
                ProductDtos = order.Products.Select(product => new ProductDto
                {
                    OrderIdForProducts=order.Id,
                    Name = product.Name,
                    Picture = product.Picture,
                    IsOnSale = product.IsOnSale,
                    Price = product.Price,
                    SalePrice = product.SalePrice,
                    ProductCreatedOn=product.CreatedOn
                }).ToList()
            }).ToList();

            return orderRecords;

        }
    }
}

