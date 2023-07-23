using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetByIdDto
    {
        public Guid Id { get; set; } //order id
        public int? RequestedAmount { get; set; } //order
        public int? TotalFoundAmount { get; set; } //order
        public string UserId { get; set; } //order
        public ProductCrawlType ProductCrawlType { get; set; } //order
        public bool IsDeleted { get; set; } //order
        public DateTimeOffset OrderCreatedOn { get; set; }
        public List<OrderEventDto> OrderEventDtos { get; set; } //orderEvent
        public List<ProductDto> ProductDtos { get; set; } //product
    }

    public class OrderEventDto
    {
        public Guid OrderIdForEvents { get; set; }
        public OrderStatus Status { get; set; } //orderEvent
        public DateTimeOffset OrderEventCreatedOn { get; set; }
    }
    public class ProductDto
    {
        public Guid OrderIdForProducts { get; set; }
        public string Name { get; set; } //product
        public string Picture { get; set; } //product
        public bool IsOnSale { get; set; } //product
        public decimal Price { get; set; } //product
        public decimal? SalePrice { get; set; } //product
        public DateTimeOffset ProductCreatedOn { get; set; }
    }
}
