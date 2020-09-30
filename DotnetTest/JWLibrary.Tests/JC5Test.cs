using JWLibrary.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWLibrary.Tests {
    public class JC5Test {
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void JListTest() {
            var list = new JList<JC5TestModel>();
            Enumerable.Range(1, 5000).jForEach(i => {
                list.Add(new JC5TestModel() { Id = i, IdName = i.ToString() });
            });

            list.jWhere(m => m.Id == 3000).jFirst();
            Assert.AreEqual(1, list[0].Id);
        }

        [Test]
        public void JLKListTest() {
            var list = new JLKList<JC5TestModel>();
            Enumerable.Range(1, 5000).jForEach(i => {
                list.Add(new JC5TestModel() { Id = i, IdName = i.ToString() });
            });

            list.jWhere(m => m.Id == 3000).jFirst();
        }

        [Test]
        public void ListTest() {
            var list = new List<JC5TestModel>();
            Enumerable.Range(1, 5000).jForEach(i => {
                list.Add(new JC5TestModel() { Id = i, IdName = i.ToString() });
            });

            list.jWhere(m => m.Id == 3000).jFirst();
        }
    }

    public class JC5TestModel {
        public int Id { get; set; }
        public string IdName { get; set; }
    }
}
