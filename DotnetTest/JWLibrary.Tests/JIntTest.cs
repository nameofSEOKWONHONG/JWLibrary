using Jint;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Tests {
    
    public class JIntTest {
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void JIntTest1() {
            var engine = new Engine();
            var add = new Engine()
                .Execute("function add(a, b) { return a + b; }")
                .GetValue("add")
                ;

            var value = add.Invoke(1, 2); // -> 3
            Assert.AreEqual(3, value.AsNumber());
        }
    }
}
