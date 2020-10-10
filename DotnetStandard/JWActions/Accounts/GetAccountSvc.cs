using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskAction;
using JWService.Data;
using JWService.WeatherForecast;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JWService.Accounts {
    public class GetAccountSvc : SvcBase<Account, IAccount>, IGetAccountSvc, IAccountSvc {
        private readonly string ACCOUNT_FILE = @"./account.db";
        private readonly string ACCOUNT_DB = typeof(Account).jGetAttributeValue((TableAttribute ta) => ta.Name);
        public override IAccount Executed() {
            var file = ACCOUNT_FILE.jToAppPath();
            using (var db = JDataBase.Resolve<ILiteDatabase>(file)) {
                var account = db.jGetCollection<Account>(ACCOUNT_DB)
                    .jGet(x => x.UserId == this.Request.UserId && x.Passwd == this.Request.Passwd);

                if(account.jIsNull()) {
                    this.Request.HashId = this.Request.Id.GetHashCode().ToString();
                    var col = db.jBeginTrans().jGetCollection<Account>(ACCOUNT_DB);
                    account.Id = col.jInsert(this.Request);
                }
                return account;
            }
        }

        public IAccount GetAccount(Account account) {
            this.Request = account;
            return this.Executed();
        }

        public override bool PostExecute() {
            return true;
        }

        public override bool PreExecute() {
            return true;
        }
    }
}
