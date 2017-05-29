using JWLibrary.NetCore;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str;
            CLIHelper helper = new CLIHelper();            
            helper.ExecuteCommand("cmd", "/c dir", out str);
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}