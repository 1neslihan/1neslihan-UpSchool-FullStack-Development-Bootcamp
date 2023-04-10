using Application.Common.Interfaces;
using Application.Features.Cities.Commands.Add;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.Add
{
    public class AddressAddCommandHandler : IRequestHandler<AddressAddCommand, Response<Guid>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        public AddressAddCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<Guid>> Handle(AddressAddCommand request, CancellationToken cancellationToken)
        {
            
            var address = new Address()
            {
                Name = request.Name,
                UserId=request.UserId,
                CountryId = request.CountryId,
                CityId= request.CityId,
                District=request.District,
                PostCode=request.PostCode,
                AddressLine1=request.AddressLine1,
                AddressLine2=request.AddressLine2,
                AddressType=request.AddressType,
                CreatedOn=DateTimeOffset.Now,
                CreatedByUserId=request.UserId,
                IsDeleted=false,
            };

            await _applicationDbContext.Addresses.AddAsync(address, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new Response<Guid>($"The new adress {address.Name} was added succesfully.", address.Id);
        }
    }
}
