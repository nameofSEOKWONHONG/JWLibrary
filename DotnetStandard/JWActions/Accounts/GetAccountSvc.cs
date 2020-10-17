using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using JWService.Data;
using JWService.Data.Models;
using JWService.WeatherForecast;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JWService.Accounts {
    public class GetAccountSvc : AccountSvcBase<Account, Account>, IGetAccountSvc {
        public override Account Executed() {
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var account = db.jGetCollection<Account>()
                    .jGet(x => x.UserId == this.Request.UserId && x.Passwd == this.Request.Passwd);
                return account;
            }
        }
    }
}
