using Application.Features.Products.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetByOrderIdQuery : IRequest<List<OrderEventGetByOrderIdDto>>
    {
        
        public OrderEventGetByOrderIdQuery(bool? isDeleted, Guid orderId)
        {
            IsDeleted=isDeleted;
            OrderId=orderId;
        }
        public bool? IsDeleted { get; set; }
        public Guid OrderId { get; set; }
    }
}
