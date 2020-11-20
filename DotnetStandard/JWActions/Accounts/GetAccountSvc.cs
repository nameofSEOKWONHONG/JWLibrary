using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using ServiceExample.Data;
using ServiceExample.Data.Models;
using ServiceExample.WeatherForecast;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using LiteDbFlex;

namespace ServiceExample.Accounts {
    public class GetAccountSvc : AccountServiceBase<Account, Account>, IGetAccountSvc {
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
