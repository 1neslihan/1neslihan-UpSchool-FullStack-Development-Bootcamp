using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetByOrderIdDto
    {
        public ProductGetByOrderIdDto(Guid id, Guid orderId, string name, string picture, bool isOnSale, decimal price, decimal? salePrice, bool isDeleted)
        {
            Id=id;
            OrderId=orderId;
            Name=name;
            Picture=picture;
            IsOnSale=isOnSale;
            Price=price;
            SalePrice=salePrice;
            IsDeleted=isDeleted;
        }

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsOnSale { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public bool IsDeleted { get; set; }
    }
}
