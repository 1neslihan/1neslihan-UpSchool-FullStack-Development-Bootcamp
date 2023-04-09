using Application.Common.Interfaces;
using Application.Features.Addresses.Commands.Add;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.Delete
{
    public class AddressDeleteCommandHandler: IRequestHandler<AddressDeleteCommand, Response<Guid>>
    {

        private readonly IApplicationDbContext _applicationDbContext;
        public AddressDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<Guid>> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
        {
            var dbQuery=_applicationDbContext.Addresses.AsQueryable();
            var address = await dbQuery.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            _applicationDbContext.Addresses.Remove(address);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>("The address was succesfully deleted.");



        }

    }
}
