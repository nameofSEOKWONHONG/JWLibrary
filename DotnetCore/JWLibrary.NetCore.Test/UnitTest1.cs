using System;
using Xunit;

namespace JWLibrary.NetCore.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            CLIHelper helper = new CLIHelper();
            helper.ExecuteCommand("", "", "dir", false);

        }
    }
}
