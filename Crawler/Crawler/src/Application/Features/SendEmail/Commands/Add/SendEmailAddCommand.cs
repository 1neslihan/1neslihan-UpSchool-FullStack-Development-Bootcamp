using Application.Common.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SendEmail.Commands.Add
{
    public class SendEmailAddCommand:IRequest<SendEmailAddDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
    }
}
