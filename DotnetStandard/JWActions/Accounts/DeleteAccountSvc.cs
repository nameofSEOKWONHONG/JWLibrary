using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using ServiceExample.Data;
using ServiceExample.Data.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JWLibrary.ServiceExecutor;
using FluentValidation;

namespace ServiceExample.Accounts {
    public class DeleteAccountSvc : AccountServiceBase<DeleteAccountSvc, DeleteAccountSvc.DeleteAccountServiceValidator, RequestDto<int>, bool>,
        IDeleteAccountSvc {
        private IGetAccountByIdSvc _getAccountByIdSvc;
        public DeleteAccountSvc(IGetAccountByIdSvc getAccountByIdSvc) {
            _getAccountByIdSvc = getAccountByIdSvc;
        }
        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetAccountByIdSvc>(_getAccountByIdSvc);
            //using (var action = ServiceFactory.CreateService<IGetAccountByIdSvc, GetAccountByIdSvc, RequestDto<int>, Account>()) {
            //    action.SetRequest(this.Request);
            //    var exists = action.ExecuteAsync().GetAwaiter().GetResult();
            //    return exists.jIsNotNull();
            //}
        }

        public override bool Executed() {
            var result = false;
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var tran = db.jBeginTrans();
                var col = tran.jGetCollection<Account>();
                result = col.jDelete(this.Request.RequestDto);
                tran.Commit();
            }
            return result;
        }

        public class DeleteAccountServiceValidator : AbstractValidator<DeleteAccountSvc> {
            public DeleteAccountServiceValidator() {
                RuleFor(o => o.req)
            }
        }
    }
}
