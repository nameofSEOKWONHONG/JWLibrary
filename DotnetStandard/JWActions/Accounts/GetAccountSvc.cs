using FluentValidation;
using JWService.Data.Models;
using LiteDbFlex;
using ServiceExample.Data;

namespace ServiceExample.Accounts {
    public class GetAccountSvc : AccountServiceBase<GetAccountSvc, GetAccountSvc.Validator, Account, Account>, IGetAccountSvc {
        public override void Execute()
        {
            var account = LiteDbFlexerManager.Instance.Value.Create<Account>()
                .Get(m => m.UserId == this.Request.UserId &&
                          m.Passwd == this.Request.Passwd)
                .GetResult<Account>();
            this.Result = account;
        }

        public class Validator : AbstractValidator<GetAccountSvc> {
            public Validator() {

            }
        }
    }
}
