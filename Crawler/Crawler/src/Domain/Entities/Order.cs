using Domain.Common;
using Domain.Enums;
using Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order:EntityBase<Guid>
    {
        public int? RequestedAmount { get; set; }
        public int? TotalFoundAmount { get; set; }
        public string UserId { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
