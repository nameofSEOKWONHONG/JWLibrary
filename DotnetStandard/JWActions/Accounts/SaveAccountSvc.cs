using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskAction;
using JWService.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWService.Accounts {
    public class SaveAccountSvc : AccountSvcBase<Account, int>, ISaveAccountSvc {
        Account _exists = null;
        public override bool PreExecute() {
            using(var action = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, IAccount>()) {
                action.SetRequest(this.Request);
                _exists = action.ExecuteCoreAsync().GetAwaiter().GetResult().jCast<Account>();
            }

            return true;
        }

        public override int Executed() {
            int accountId = 0;
            if (_exists.jIsNotNull()) {
                using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                    var tran = db.jBeginTrans();
                    var col = tran.jGetCollection<Account>();
                    _exists.UserId = this.Request.UserId;
                    _exists.Passwd = this.Request.Passwd;
                    if(col.jUpdate(_exists).jIsTrue()) {
                        accountId = _exists.Id;
                        tran.jCommit();
                    }
                }
            }
            else {
                using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                    var tran = db.jBeginTrans();
                    var col = tran.jGetCollection<Account>();
                    accountId = col.jInsert(this.Request);
                    tran.Commit();
                }
            }

            return accountId;
        }
    }
}
