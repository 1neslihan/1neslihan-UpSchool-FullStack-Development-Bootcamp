﻿using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cities.Commands.Add
{
    public class CityAddCommandHandler : IRequestHandler<CityAddCommand, Response<int>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        public CityAddCommandHandler( IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<int>> Handle(CityAddCommand request, CancellationToken cancellationToken)
        {
            //if (!request.Name.IsContainsChar(3))
            //{
            //    throw new Exception();
            //}
            if (!await _applicationDbContext.Countries.AnyAsync(x=>x.Id==request.CountryId,cancellationToken))
            {
                throw new ArgumentNullException(nameof(request.CountryId));
            }
            if (await _applicationDbContext.Cities.AnyAsync(x => x.Name.ToLower()==request.Name, cancellationToken))
            {
                throw new ArgumentNullException(nameof(request.Name));
            }
            var city = new City()
            {
                Name = request.Name,
                CountryId = request.CountryId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                CreatedOn=DateTimeOffset.Now,
                CreatedByUserId=null,
                IsDeleted=false,
            };

            await _applicationDbContext.Cities.AddAsync(city,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new Response<int>($"The new city name \"{city.Name}\" was edited succesfully.", city.Id);
        }
    }
}
