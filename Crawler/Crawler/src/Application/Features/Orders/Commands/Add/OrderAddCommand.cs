using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Add
{
    public class OrderAddCommand:IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public int? RequestedAmount { get; set; }
        public int? TotalFoundAmount { get; set; }
        public string UserId { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
       
    }
}
