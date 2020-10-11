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
    public class GetAccountsSvc : AccountSvcBase<RequestDto<Account>, IEnumerable<Account>>, IGetAccountsSvc {
        public override IEnumerable<Account> Executed() {
            var file = ACCOUNT_FILE.jToAppPath();
            using (var db = JDataBase.Resolve<ILiteDatabase>(file)) {
                IEnumerable<Account> accounts = null;

                accounts = db.jGetCollection<Account>(ACCOUNT_DB)
                    .jGetAll().jToList();
                return accounts;
            }
        }
    }
}
