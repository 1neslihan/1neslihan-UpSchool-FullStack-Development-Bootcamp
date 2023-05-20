using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Addresses.Commands.SoftDelete
{
    public class AddressSoftDeleteCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
