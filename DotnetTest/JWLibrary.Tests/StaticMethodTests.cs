using JWLibrary.StaticMethod;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace JWLibrary.Tests {
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

            List<string> list = new List<string>();
        }

        [Test]
        public void JWhereTest() {
            List<string> list = null;

            var result = list.jWhere(m => m == "").jToList();

            Assert.NotNull(result);
        }

        [Test]
        public void JDataReaderTest() {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));

            var row1 = table.NewRow();
            row1["Id"] = 1;
            row1["Name"] = "test1";

            var row2 = table.NewRow();
            row2["Id"] = 2;
            row2["Name"] = "Test2";

            table.Rows.Add(row1);
            table.Rows.Add(row2);
            

            using (DataTableReader reader = table.CreateDataReader()) {
                do {
                    IDataReader reader1 = (IDataReader)reader;
                    if (!reader.HasRows) {
                        Console.WriteLine("Empty DataTableReader");
                    } else {
                        var model = reader1.jToObject<TestModel>();
                        Assert.NotNull(model);
                    }
                    Console.WriteLine("========================");
                } while (reader.NextResult());
            }
        }

        [Test]
        public void JIsNumberTest() {
            Assert.IsTrue("123".jIsNumber());
            Assert.IsFalse("a123".jIsNumber());
        }

        [Test]
        public void JIsAlphabetTest() {
            //Assert.IsTrue("asb".jIsAlphabetOnly());
            //Assert.IsFalse("asb«—±€".jIsAlphabetOnly());
            //Assert.IsTrue("absdf1123".jIsAlphabetAndNumber());
            //Assert.IsTrue("1123asdf".jIsAlphabetAndNumber());
            //Assert.IsTrue("asdfasdf".jIsAlphabetAndNumber());
            Assert.IsTrue("123.123".jIsNumeric());
            Assert.IsFalse("123,123".jIsNumeric());
        }
    }

    public static class MockReader{
        public static void SetupDataReader(this Mock<IDataReader> dataReaderMock, IList<string> columnNames, ICollection collection) {
            var queue = new Queue(collection);

            dataReaderMock
                .Setup(x => x.Read())
                .Returns(() => queue.Count > 0)
                .Callback(() => {
                    if (queue.Count > 0) {
                        var row = queue.Dequeue();
                        foreach (var columnName in columnNames) {
                            var columnValue = row.GetType().GetProperty(columnName).GetValue(row);
                            dataReaderMock
                                .Setup(x => x[columnNames.IndexOf(columnName)])
                                .Returns(columnValue);
                            dataReaderMock
                                .Setup(x => x[columnName])
                                .Returns(columnValue);
                        }
                    }
                });
        }
    }

    public class TestModel {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}