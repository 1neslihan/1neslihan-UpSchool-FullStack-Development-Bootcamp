using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Commands.Add
{
    public class CityAddCommand:IRequest<Response<int>>
    {
        [Required]
        [MinLength(6)]
        public string Name { get; set; }
        [Required]
        public int CountryId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
