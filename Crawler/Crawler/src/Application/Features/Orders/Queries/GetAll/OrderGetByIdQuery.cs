using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetByIdQuery:IRequest<List<OrderGetByIdDto>>
    {
        public OrderGetByIdQuery(bool? isDeleted, Guid id)
        {
            IsDeleted=isDeleted;
            Id=id;
        }

        public bool? IsDeleted { get; set; }
        public Guid Id { get; set; }
    }
}
