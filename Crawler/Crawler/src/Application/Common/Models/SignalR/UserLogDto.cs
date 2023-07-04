using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.SignalR
{
    public class UserLogDto
    {
        public UserLogDto(string message)
        {
            Message=message;
            SendOn=DateTimeOffset.Now;
        }

        public string Message { get; set; }
        public DateTimeOffset SendOn { get; set; }

    }
}
