using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpSchool.Domain.Utilities
{
    public interface IIPHelper
    {
        string GetCurrentIPAddress();

        List<string> GetAllIPAddresses();
    }
}
