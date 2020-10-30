using JWLibrary.Version;
using JWLibrary.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Tests {
    public class ConfigSettingTest {
        [Test]
        public void Test1() {
            ConfigManager configManager = new ConfigManager();
            var configInfo = configManager.GetInitialConfig("DB_VERSION");
            Console.WriteLine("log : {0} - {1} ms",
                            TestContext.CurrentContext.Test.Name,
                            configInfo.jToString());
            Assert.Greater(configInfo.OwnerNowVersion, 0);
        }
    }
}
