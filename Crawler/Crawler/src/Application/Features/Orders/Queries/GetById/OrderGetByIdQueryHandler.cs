using Application.Common.Interfaces;
using Application.Common.Models.Email;
using Application.Features.Orders.Queries.GetByDate;
using Application.Features.Products.Queries.GetAll;
using Domain.Entities;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetByIdQueryHandler : IRequestHandler<OrderGetByIdQuery, List<OrderGetByIdDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public OrderGetByIdQueryHandler(IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService, UserManager<User> userManager, IEmailService emailService)
        {
            _applicationDbContext=applicationDbContext;
            _currentUserService=currentUserService;
            _userManager=userManager;
            _emailService=emailService;
        }

        public async Task<List<OrderGetByIdDto>> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == _currentUserService.UserId)
                .Where(x=>x.Id==request.Id);


            
            var userQuery = await _userManager.Users
                .Where(x => x.Id ==_currentUserService.UserId)
                .FirstOrDefaultAsync();


            dbQuery = request.IsDeleted.HasValue
                ? dbQuery.Where(x => x.IsDeleted == request.IsDeleted.Value)
                : dbQuery.Where(x => x.IsDeleted == false || x.IsDeleted == true);

            var order = await dbQuery
                .Include(x => x.OrderEvents)
                .Include(x => x.Products)
                .ToListAsync(cancellationToken);

            var orderRecords = order.Select(order => new OrderGetByIdDto
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

            

            if (userQuery!=null && orderRecords !=null)
            {
                var userEmail = userQuery.Email;
                var isEmailNotificationEnabled = userQuery.EmailNotification;

                if (isEmailNotificationEnabled == true)
                {
                    _emailService.SendOrderDetails(new SendOrderDetailsDto()
                    {
                        Name=userQuery.FirstName,
                        Email=userEmail,
                        OrderId=orderRecords.Select(order => order.Id).FirstOrDefault().ToString(),
                        GenerateDate=orderRecords.Select(order=>order.OrderCreatedOn).FirstOrDefault().ToString(),
                        Products=orderRecords
                        .SelectMany(order => order.ProductDtos)
                        .ToList(),
                        OrderEvents=orderRecords
                        .SelectMany(order=>order.OrderEventDtos)
                        .ToList(),
                    });
                }
            }

            //var orderDtos= await dbQuery
            //    .Select(x=> new OrderGetByIdDto(x.Id, x.RequestedAmount, x.TotalFoundAmount, x.ProductCrawlType, x.IsDeleted, x.UserId))
            //    .ToListAsync(cancellationToken);
            return orderRecords;
        }

        
    }

  
}
