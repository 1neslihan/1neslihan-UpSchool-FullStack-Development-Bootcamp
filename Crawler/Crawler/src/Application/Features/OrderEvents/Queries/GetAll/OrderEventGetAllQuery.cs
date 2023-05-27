using Application.Features.Products.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetAllQuery : IRequest<List<OrderEventGetAllDto>>
    {
        public OrderEventGetAllQuery(bool? isDeleted)
        {
            IsDeleted=isDeleted;
        }
        public bool? IsDeleted { get; set; }
    }
}
