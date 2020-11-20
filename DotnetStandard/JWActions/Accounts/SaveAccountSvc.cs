using JWLibrary.Core;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using ServiceExample.Data;
using ServiceExample.Data.Models;
using LiteDB;
using LiteDbFlex;

namespace ServiceExample.Accounts {
    public class SaveAccountSvc : AccountServiceBase<Account, bool>, ISaveAccountSvc {
        Account _exists = null;
        public override bool PreExecute() {
            using var action = ServiceFactory.CreateService<IGetAccountSvc, GetAccountSvc, Account, Account>();
            action.SetRequest(this.Request);
            _exists = action.ExecuteAsync().GetAwaiter().GetResult();

            return true;
        }

        public override bool Executed() {
            if (_exists.jIsNotNull())
            {
                _exists.UserId = this.Request.UserId;
                _exists.Passwd = this.Request.Passwd;

                var result = LiteDbFlexerManager.Instance.Value.Create<Account>()
                    .BeginTrans()
                    .Update(_exists)
                    .Commit()
                    .GetResult<BsonValue>();

                return (int) result > 0;
            }
            else
            {
                var result = LiteDbFlexerManager.Instance.Value.Create<Account>()
                    .BeginTrans()
                    .Insert(this.Request)
                    .Commit()
                    .GetResult<BsonValue>();
                return (int)result > 0;
            }
        }
    }
}
