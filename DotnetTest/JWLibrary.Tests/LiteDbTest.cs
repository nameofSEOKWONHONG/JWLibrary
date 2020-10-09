using JWLibrary.Database;
using LiteDB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JWLibrary.Tests {
    public class LiteDbTest {
        //enc file
        //string filename = @"Filename=d:\litedbtest2.db;Password=1234";
        string filename = @"Filename=d:\litedbtest.db";

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void InsertTest() {
            var customer = new Customer() {
                Name = "ghdwldn",
                Phones = new string[] { "8000-0000", "9000-0000" },
                IsActive = true
            };

            using (var db = JDataBase.Resolve<ILiteDatabase>(filename)) {
                var col = db.jGetCollection<Customer>("customers");

                var result = db.jBeginTrans()
                               .jGetCollection<Customer>("customers")
                               .jInsert<Customer>(customer);
                db.jCommit();

                var resultCustomer = col.jGet<Customer>(result);
                Assert.NotNull(resultCustomer);
            }
        }

        [Test]
        public void GetAllTest() {
            var result = JDataBase.Resolve<ILiteDatabase>(filename)
                .jGetCollection<Customer>("customers")
                .jGetAll<Customer>();

            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public void DeleteAllTest() {
            var db = JDataBase.Resolve<ILiteDatabase>(filename);
            db.jGetCollection<Customer>("customers")
              .jDeleteAll();

            var result = db
                .jGetCollection<Customer>("customers")
                .jGetAll();

            Assert.IsEmpty(result);
        }
    }
    public class Customer {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }
}
