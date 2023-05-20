using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands.Remove
{
    public class AccountRemoveCommand:IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }

        public AccountRemoveCommand(Guid id)
        {
            Id=id;
        }

    }
}
