using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery:IRequest<List<ProductGetAllDto>>
    {
        public ProductGetAllQuery(/*Guid id,*/ bool? isDeleted)
        {
            //Id=id;
            IsDeleted=isDeleted;
        }

        //public Guid Id { get; set; }

        public bool? IsDeleted { get; set; }

        
    }
}
