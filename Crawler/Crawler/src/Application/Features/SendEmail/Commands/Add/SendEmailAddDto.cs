using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SendEmail.Commands.Add
{
    public class SendEmailAddDto
    {
        public SendEmailAddDto(string firstName, string lastName, string email, string link)
        {
            FirstName=firstName;
            LastName=lastName;
            Email=email;
            Link=link;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
    }
}
