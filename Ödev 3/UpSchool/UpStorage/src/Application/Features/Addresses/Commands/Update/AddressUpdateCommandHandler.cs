using Application.Common.Interfaces;
using Application.Features.Addresses.Queries.GetAll;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.Update
{
    public class AddressUpdateCommandHandler : IRequestHandler<AddressUpdateCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AddressUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<Guid>> Handle(AddressUpdateCommand request, CancellationToken cancellationToken)
        {
            var dbQuery=_applicationDbContext.Addresses.AsQueryable();
        
            var address=await dbQuery.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (address==null)
            {
                return new Response<Guid>("The address info was not found.");
            }

            else
            {
                address.Name = request.Name;
                address.UserId=request.UserId;
                address.CountryId = request.CountryId;
                address.CityId= request.CityId;
                address.District=request.District;
                address.PostCode=request.PostCode;
                address.AddressLine1=request.AddressLine1;
                address.AddressLine2=request.AddressLine2;
                address.AddressType=request.AddressType;
                address.ModifiedOn=DateTimeOffset.Now;
                address.CreatedByUserId=null;
                address.IsDeleted=false;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return new Response<Guid>($"The new address info was edited succesfully.");
            }



        }

      


    }
}
