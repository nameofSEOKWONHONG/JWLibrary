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
using LiteDbFlex;

namespace JWService.Accounts {
    public class GetAccountSvc : AccountSvcBase<Account, Account>, IGetAccountSvc {
        public override Account Executed()
        {
            var account = LiteDbFlexerManager.Instance.Value.Create<Account>()
                .Get(m => m.UserId == this.Request.UserId &&
                          m.Passwd == this.Request.Passwd)
                .GetResult<Account>();
            return account;
        }
    }
}
