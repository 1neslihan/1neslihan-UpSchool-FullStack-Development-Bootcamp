using Application.Common.Interfaces;
using Application.Features.Orders.Commands.Delete;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.UndoDelete
{
    public class OrderUndoDeleteCommandHandler : IRequestHandler<OrderUndoDeleteCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderUndoDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;

        }

        public async Task<Response<Guid>> Handle(OrderUndoDeleteCommand request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Orders.AsQueryable();

            var order = await dbQuery
                .Include(x => x.Products)
                .Include(x => x.OrderEvents)
                .Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if(order != null)
            {
                order.IsDeleted= false;
                order.DeletedOn= DateTime.Now;
                order.ModifiedOn=DateTime.Now;

                if (order.Products != null)
                {
                    foreach (var product in order.Products)
                    {
                        product.IsDeleted = false;
                        product.DeletedOn = DateTime.Now;
                        product.ModifiedOn= DateTime.Now;
                        //product.DeletedByUserId = request.Id.ToString();
                    }
                }

                if (order.OrderEvents != null)
                {
                    foreach (var orderEvent in order.OrderEvents)
                    {
                        orderEvent.IsDeleted = false;
                        orderEvent.DeletedOn = DateTime.Now;
                        orderEvent.ModifiedOn= DateTime.Now;
                        //orderEvent.DeletedByUserId = request.Id.ToString();
                    }
                }

                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return new Response<Guid>("Order retrieved from trash");
            }
            return new Response<Guid>("Order was not found");
        }
    }
}
