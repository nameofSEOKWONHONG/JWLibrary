using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using ServiceExample.Data;
using ServiceExample.Data.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceExample.Accounts {
    public class GetAccountByIdSvc : AccountServiceBase<RequestDto<int>, Account>, IGetAccountByIdSvc {
        public override Account Executed() {
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var account = db.jGetCollection<Account>()
                    .jGet(x => x.Id == this.Request.RequestDto);

                return account;
            }
        }
    }
}
