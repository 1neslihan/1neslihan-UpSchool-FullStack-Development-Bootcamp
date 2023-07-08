using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetByOrderIdDto
    {
        public OrderEventGetByOrderIdDto(Guid id, Guid orderId, OrderStatus status, bool isDeleted)
        {
            Id=id;
            OrderId=orderId;
            Status=status;
            IsDeleted=isDeleted;
        }

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
