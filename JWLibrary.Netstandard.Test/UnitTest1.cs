using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JWLibrary.Helpers;

namespace JWLibrary.Netstandard.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ParameterHelper helper = new ParameterHelper(
                () => 
            );

            var result = helper.ToGetParameter();

            if (!result.Contains("Id") && !result.Contains("1"))
                Assert.Fail();
               
        }
    }
}
