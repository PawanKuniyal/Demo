using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.StoredProcedures.Services;

namespace HelpaTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            HelperService services = new HelperService();

            var res = services.GetHomeHelpers(100, "77.22178310000004", "28.68627380000001");
            Console.WriteLine("Cococ");
            Console.ReadKey();
        }
    }
}
