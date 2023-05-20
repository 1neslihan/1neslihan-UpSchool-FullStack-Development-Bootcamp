using Application.Common.Models.General;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Countries.Queries.GetAll
{
    public class CountriesGetAllQuery: IRequest<PaginatedList<CountriesGetAllDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
