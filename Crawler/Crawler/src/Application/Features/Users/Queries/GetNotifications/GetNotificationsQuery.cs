using Application.Features.OrderEvents.Queries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<GetNotificationsDto>
    {
        public GetNotificationsQuery(string id)
        {
            Id=id;

        }

        public string Id { get; set; }
 
    }
}
