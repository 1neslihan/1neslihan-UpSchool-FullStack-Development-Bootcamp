using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpSchool.Domain.Utilities
{
    public class LocalDb : ILocalDB
    {
        public LocalDb()
        {
            IPs=new List<string>();
        }

        public List<string> IPs { get; set; }
    }
}
