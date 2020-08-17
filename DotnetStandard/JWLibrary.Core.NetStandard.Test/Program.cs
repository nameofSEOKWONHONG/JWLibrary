using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.Core.NetStandard;

namespace JWLibrary.Core.NetStandard.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(StringEnum.GetStringValue(Test.test1));
            Console.WriteLine(DateTime.Now.ConvertDateToString(ConvertFormat.YYYYMMDD));
            Console.ReadLine();
        }

        public enum Test
        {
            [StringValue("a")]
            test1,
            [StringValue("b")]
            test2
        }
    }


}
