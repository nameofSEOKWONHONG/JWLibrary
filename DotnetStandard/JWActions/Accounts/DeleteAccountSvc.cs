using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskAction;
using JWService.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Accounts {
    public class DeleteAccountSvc : AccountSvcBase<RequestDto<int>, bool>, IDeleteAccountSvc {
        public override bool PreExecute() {
            using (var action = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, IAccount>()) {
                action.Action.Request = new Account() {
                    Id = this.Request.Dto
                };
                var exists = action.Action.Executed();
                return exists.jIsNotNull(); 
            }
        }

        public override bool Executed() {
            var result = false;
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var tran = db.jBeginTrans();
                var col = tran.jGetCollection<Account>();
                result = col.jDelete(this.Request.Dto);
                tran.Commit();
            }
            return result;
        }
    }
}
