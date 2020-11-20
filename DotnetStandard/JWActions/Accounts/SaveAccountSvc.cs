using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using LiteDB;
using LiteDbFlex;
using ServiceExample.Data;

namespace ServiceExample.Accounts {
    public class SaveAccountSvc : AccountServiceBase<SaveAccountSvc, SaveAccountSvc.Validator, Account, bool>, ISaveAccountSvc {
        Account _exists = null;
        IGetAccountSvc _getAccountSvc;
        public SaveAccountSvc(IGetAccountSvc getAccountSvc) {
            this._getAccountSvc = getAccountSvc;
        }
        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetAccountSvc>(this._getAccountSvc);
            executor.SetRequest(o => o.Request = this.Request)
                .AddFilter(o => o.Request.jIsNotNull())
                .OnExecuted(o => {
                    this._exists = o.Result;
                });

            if (this._exists.jIsNull()) return false;
            return true;
        }

        public override void Execute() {
            if (_exists.jIsNotNull()) {
                _exists.UserId = this.Request.UserId;
                _exists.Passwd = this.Request.Passwd;

                var result = LiteDbFlexerManager.Instance.Value.Create<Account>()
                    .BeginTrans()
                    .Update(_exists)
                    .Commit()
                    .GetResult<BsonValue>();

                this.Result = (int) result > 0;
            }
            else {
                var result = LiteDbFlexerManager.Instance.Value.Create<Account>()
                    .BeginTrans()
                    .Insert(this.Request)
                    .Commit()
                    .GetResult<BsonValue>();
                this.Result = (int)result > 0;
            }
        }

        public class Validator : AbstractValidator<SaveAccountSvc> {
            public Validator() {

            }
        }
    }
}
