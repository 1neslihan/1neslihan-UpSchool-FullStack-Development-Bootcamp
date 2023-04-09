using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account:EntityBase<Guid>
    {
        
        public string Title { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Url { get; set; }
        public bool IsFavourite { get; set; }
        public string UserId { get; set; }
        public ICollection<AccountCategory> AccountCategories { get; set; }
    }
}
