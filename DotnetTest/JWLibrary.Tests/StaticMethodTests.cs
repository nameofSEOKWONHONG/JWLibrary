using NUnit.Framework;
using System.Collections.Generic;
using JWLibrary.StaticMethod;

namespace JWLibrary.Tests
{
    public class StaticMethodTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void JIfNullTest()
        {
            List<string> list = null;

            list = list.jIfNull(x => x = new List<string>());

            Assert.NotNull(list);
        }

        [Test]
        public void JIfNotNullTest()
        {
            string v = null;

            v = v.jIfNotNull(x => x, "10");

            Assert.AreEqual("10", v);
        }

        [Test]
        public void StringEmptyTest()
        {
            string v = string.Empty;

            v = v.jIfNotNull(x => x, "10");

            Assert.AreEqual("10", v);
        }
    }
}