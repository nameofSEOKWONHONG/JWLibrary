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
    public class GetAccountSvc : AccountSvcBase<Account, IAccount>, IGetAccountSvc, IAccountSvc {
        public override IAccount Executed() {
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var account = db.jGetCollection<Account>()
                    .jGet(x => x.UserId == this.Request.UserId && x.Passwd == this.Request.Passwd);
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
