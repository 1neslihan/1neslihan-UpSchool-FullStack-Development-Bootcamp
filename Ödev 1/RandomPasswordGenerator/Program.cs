using System.Linq;
using System.Runtime.CompilerServices;

namespace RandomPasswordGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Characters c = new Characters();
            c.getAnswers();
            Console.ReadKey();
        }
    }
}