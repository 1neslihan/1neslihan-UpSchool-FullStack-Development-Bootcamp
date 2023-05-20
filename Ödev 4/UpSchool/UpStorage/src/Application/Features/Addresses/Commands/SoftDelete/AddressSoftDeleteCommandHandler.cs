using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.SoftDelete
{
    public class AddressSoftDeleteCommandHandler : IRequestHandler<AddressSoftDeleteCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public AddressSoftDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(AddressSoftDeleteCommand request, CancellationToken cancellationToken)
        {
            var dbQuery = _applicationDbContext.Addresses.AsQueryable();

            var address = await dbQuery.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (address==null)
            {
                return new Response<Guid>("The address was not found.");
            }

            else
            {
                address.IsDeleted= true;
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                return new Response<Guid>("The address was successfully deleted.");
            }
            
        }
    }
}
