using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetNotifications
{
    public class GetNotificationsDto
    {
        public GetNotificationsDto( bool emailNotificationEnable, bool toasterNotificationEnable)
        {
            EmailNotificationEnable=emailNotificationEnable;
            ToasterNotificationEnable=toasterNotificationEnable;
        }

        //public string Id { get; set; }
        public bool EmailNotificationEnable { get; set; }
        public bool ToasterNotificationEnable { get; set; }
    }
}
