using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Note:EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }

        #region Added by Neslihan
        public ICollection<NoteCategory> NoteCategories { get; set; }
        #endregion
    }
}
