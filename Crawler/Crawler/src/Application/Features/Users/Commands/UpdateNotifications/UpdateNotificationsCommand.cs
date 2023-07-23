using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.UpdateNotifications
{
    public class UpdateNotificationsCommand:IRequest<Response<Guid>>
    {
       ////public Guid Id { get; set; }

       public bool EmailNotificationsEnable { get; set; }
       public bool ToasterNotificationsEnable { get; set; }
    }
}
