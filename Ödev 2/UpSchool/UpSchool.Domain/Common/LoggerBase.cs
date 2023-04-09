using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Common;
namespace UpSchool.Domain.Common
{
    public abstract class LoggerBase
    {
        private readonly string _apiurl;
        public LoggerBase(string titanicFluteUrl)
        {
            _apiurl = titanicFluteUrl;
        }

        public virtual void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(_apiurl);
        }
    }
}
