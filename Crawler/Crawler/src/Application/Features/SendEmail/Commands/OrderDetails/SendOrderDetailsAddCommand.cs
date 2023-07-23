using Application.Features.Orders.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SendEmail.Commands.OrderDetails
{
    public class SendOrderDetailsAddCommand:IRequest<SendOrderDetailsAddDto>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string OrderId { get; set; }
        public string GenerateDate { get; set; }
        public List<ProductDto> Products { get; set; } 
        public List<OrderEventDto> OrderEvents { get; set; }
    }
}
