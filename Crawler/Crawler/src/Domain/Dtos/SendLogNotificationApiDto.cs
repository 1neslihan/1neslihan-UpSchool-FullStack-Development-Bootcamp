using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class SendLogNotificationApiDto
    {
        public UserLogDto Log { get; set; }
        public string ConnectionId { get; set; }

        public SendLogNotificationApiDto(UserLogDto log, string connectionId) 
        {
            Log = log;
            ConnectionId = connectionId;
        }

    }
}
