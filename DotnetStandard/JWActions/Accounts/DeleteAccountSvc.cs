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
    public class DeleteAccountSvc : AccountSvcBase<RequestDto<int>, bool>, IDeleteAccountSvc {
        public override async Task<bool> PreExecute() {
            using (var action = ServiceFactory.CreateService<IGetAccountByIdSvc, GetAccountByIdSvc, RequestDto<int>, Account>()) {
                action.SetRequest(this.Request);
                var exists = await action.ExecuteCoreAsync();
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
