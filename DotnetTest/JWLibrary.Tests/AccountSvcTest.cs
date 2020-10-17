using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskService;
using JWService.Accounts;
using JWService.Data;
using JWService.Data.Models;
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
            }).ExecuteAsync().Result;
            Assert.Greater(result, 0);
        }

        [Test]
        public void GetAllTest() {
            //using (var svc = ServiceFactory.CreateService<IGetAccountsSvc, GetAccountsSvc, RequestDto<Account>, IEnumerable<Account>>()) {
            //    var result = svc.SetRequest(new RequestDto<Account>())
            //    .ExecuteAsync().Result;
            //    Assert.Greater(result.jCount(), 0);
            //}
        }

        [Test]
        public void GetTest() {
            using var svc = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, Account>();
            var result = svc.SetRequest(new Account() {
                Id = 1,
                HashId = "",
                UserId = "test",
                Passwd = "test"
            }).ExecuteAsync().Result;
            Assert.NotNull(result);
        }

        [Test]
        public void DeleteTest() {
            using var svc = ServiceFactory.CreateService<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            var result = svc.SetRequest(new RequestDto<int>() { RequestDto = 1 })
            .ExecuteAsync().Result;
            Assert.IsTrue(result);
        }
    }
}
