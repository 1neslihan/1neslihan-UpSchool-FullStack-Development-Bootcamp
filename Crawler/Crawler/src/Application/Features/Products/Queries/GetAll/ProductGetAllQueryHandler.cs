using Application.Common.Interfaces;
using Application.Common.Models.General;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQueryHandler : IRequestHandler<ProductGetAllQuery, PaginatedList<ProductGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext=applicationDbContext;
        }

        public async Task<PaginatedList<ProductGetAllDto>> Handle(ProductGetAllQuery request, CancellationToken cancellationToken)
        {
            var dbQuery= _applicationDbContext.Products.AsQueryable();
            //dbQuery=dbQuery.Where(x=>x.Id==request.Id);

            dbQuery=dbQuery.Where(x=>x.IsDeleted==request.IsDeleted && x.OrderId==request.OrderId);

            if(request.IsDeleted.HasValue) dbQuery=dbQuery.Where(x=>x.IsDeleted==request.IsDeleted.Value);
            
            var productDtos= await dbQuery.Select(x => new ProductGetAllDto(x.Id, x.OrderId, x.Name, x.Picture, x.IsOnSale, x.Price, x.SalePrice, x.IsDeleted))
                .AsQueryable()
                .ToListAsync(cancellationToken);
            return PaginatedList<ProductGetAllDto>.Create(productDtos, request.PageNumber, request.PageSize);
            
        }

        
    }
}
