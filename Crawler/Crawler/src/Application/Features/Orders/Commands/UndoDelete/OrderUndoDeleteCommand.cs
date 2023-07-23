using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.UndoDelete
{
    public class OrderUndoDeleteCommand:IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
    }
}
