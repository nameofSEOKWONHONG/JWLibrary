using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskAction;
using JWService.Accounts;
using JWService.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Tests {
    
    public class AccountSvcTest {
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void InsertTest() {
            using var svc = ServiceFactory.CreateService<ISaveAccountSvc, SaveAccountSvc, Account, int>();
            svc.Action.Request = new Account() {
                Id = 0,
                HashId = "",
                UserId = "test",
                Passwd = "test"
            };
            var result = svc.ExecuteCoreAsync().Result;
            Assert.Greater(result, 0);
        }

        [Test]
        public void GetAllTest() {
            using (var svc = ServiceFactory.CreateService<IGetAccountsSvc, GetAccountsSvc, RequestDto<Account>, IEnumerable<Account>>()) {
                svc.Action.Request = new RequestDto<Account>();
                var result = svc.ExecuteCoreAsync().Result;
                Assert.Greater(result.jCount(), 0);
            }
        }

        [Test]
        public void GetTest() {
            using var svc = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, IAccount>();
            svc.Action.Request = new Account() {
                Id = 1,
                HashId = "",
                UserId = "test",
                Passwd = "test"
            };
            var result = svc.ExecuteCoreAsync().Result;
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteTest() {
            using var svc = ServiceFactory.CreateService<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            svc.Action.Request = new RequestDto<int>() { Dto = 1 };
            var result = svc.ExecuteCoreAsync().Result;
            Assert.IsTrue(result);
        }
    }
}
