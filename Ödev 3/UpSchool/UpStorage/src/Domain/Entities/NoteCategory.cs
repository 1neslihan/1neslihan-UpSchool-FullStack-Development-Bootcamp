using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    //N-N relationships cross table
    public class NoteCategory
    {
        
        public Guid NoteId { get; set; }
        public Note Note{ get; set;}

        public Guid CategoryId { get; set; }
        public Category Category { get; set;}

        

    }
}
