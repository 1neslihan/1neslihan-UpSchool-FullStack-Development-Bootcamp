using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersDto
    {
        public GetAllUsersDto(string firstName, string lastName, string email)
        {
            FirstName=firstName;
            LastName=lastName;
            Email=email;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
