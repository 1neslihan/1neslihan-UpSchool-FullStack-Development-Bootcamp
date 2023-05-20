using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpSchool.Domain.Entities
{
    public class User
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LasName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
