using JWLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterHelper pHelper = new ParameterHelper(
                new KeyValuePair<string, object>("Id", "1"));
            Console.WriteLine(pHelper.ToJson());
            Console.ReadLine();
        }
    }
}
