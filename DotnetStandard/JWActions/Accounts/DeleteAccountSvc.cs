using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using JWService.Data;
using JWService.Data.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWService.Accounts {
    public class DeleteAccountSvc : AccountSvcBase<RequestDto<int>, bool>, IDeleteAccountSvc {
        public override bool PreExecute() {
            using (var action = ServiceFactory.CreateService<IGetAccountByIdSvc, GetAccountByIdSvc, RequestDto<int>, Account>()) {
                action.SetRequest(this.Request);
                var exists = action.ExecuteAsync().GetAwaiter().GetResult();
                return exists.jIsNotNull(); 
            }
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
    }
}
