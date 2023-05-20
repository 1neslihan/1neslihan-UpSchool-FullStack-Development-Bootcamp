using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpSchool.Domain.Utilities
{
    public class IPHelper : IIPHelper
    {
        public string GetCurrentIPAddress()
        {
            return GetIpAddress();
        }

        public List<string> GetAllIPAddresses()
        {
            throw new NotImplementedException();
        }

        public static string GetIpAddress()
        {
            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        return ip.ToString();
            //    }
            //}
            return string.Empty;
        }
    }
}
