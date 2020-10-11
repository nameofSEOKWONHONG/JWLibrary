using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWService.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Accounts {
    public class GetAccountByIdSvc : AccountSvcBase<RequestDto<int>, Account>, IGetAccountByIdSvc {
        public override Account Executed() {
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var account = db.jGetCollection<Account>()
                    .jGet(x => x.Id == this.Request.Dto);

                return account;
            }
        }
    }
}
