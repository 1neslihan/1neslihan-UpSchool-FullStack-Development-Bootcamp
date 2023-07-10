using Application.Common.Models.General;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetByOrderIdQuery:IRequest<PaginatedList<ProductGetByOrderIdDto>>
    {
        public ProductGetByOrderIdQuery(bool? isDeleted, Guid orderId, int pageNumber, int pageSize)
        {
            IsDeleted=isDeleted;
            OrderId=orderId;
            PageNumber=pageNumber;
            PageSize=pageSize;
        }


        public bool? IsDeleted { get; set; }
        public Guid OrderId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        
    }
}
