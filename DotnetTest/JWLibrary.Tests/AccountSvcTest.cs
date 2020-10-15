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
            var result = svc.SetRequest(new Account() {
                Id = 0,
                HashId = "",
                UserId = "test",
                Passwd = "test"
            }).ExecuteCoreAsync().Result;
            Assert.Greater(result, 0);
        }

        [Test]
        public void GetAllTest() {
            using (var svc = ServiceFactory.CreateService<IGetAccountsSvc, GetAccountsSvc, RequestDto<Account>, IEnumerable<Account>>()) {
                var result = svc.SetRequest(new RequestDto<Account>())
                .ExecuteCoreAsync().Result;
                Assert.Greater(result.jCount(), 0);
            }
        }

        [Test]
        public void GetTest() {
            using var svc = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, IAccount>();
            var result = svc.SetRequest(new Account() {
                Id = 1,
                HashId = "",
                UserId = "test",
                Passwd = "test"
            }).ExecuteCoreAsync().Result;
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteTest() {
            using var svc = ServiceFactory.CreateService<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            var result = svc.SetRequest(new RequestDto<int>() { Dto = 1 })
            .ExecuteCoreAsync().Result;
            Assert.IsTrue(result);
        }
    }
}
