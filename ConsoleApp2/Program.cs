using JWLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterHelper p = new ParameterHelper(new{ Key = "", Value = "" }, new { Key = "1", Value = "2"});
            ParameterHelper pHelper = new ParameterHelper(
                new KeyValuePair<string, object>("Id", "1"));
            Console.WriteLine(pHelper.ToJson());
            Console.ReadLine();
        }
    }

    
}
