using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using JWLibrary.Utils.Files;
using JWLibrary.Core;
using System.Linq;
using System.IO;

namespace JWLibrary.Tests {
    public class FileUniqueIdTest {
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void CreateUniqueId() {
            var filePath = @"D:\JWLibrary\DotNetFramework\JWLibrary.Core\FileIdentify\FileUniqueId.cs";
            var before = filePath.jToFileUniqueId();
            var lines = filePath.jReadLines();
            var savelines = new JList<string>();
            lines.jForEach(item => {
                if (item.Contains("//")) {
                    item += "//test";
                }
                savelines.Add(item);
                return true;
            });

            File.WriteAllLines(filePath, savelines);
            var after = filePath.jToFileUniqueId();

            Assert.AreNotEqual(before, after);
        }
    }
}
