using FluentValidation;
using JWLibrary.Core;
using JWService.Data.Models;
using ServiceExample.Data;

namespace ServiceExample.Accounts {
    public class GetAccountByIdSvc : AccountServiceBase<GetAccountByIdSvc, GetAccountByIdSvc.GetAccountByIdSvcValidator, RequestDto<int>, Account>,
        IGetAccountByIdSvc {

        public override void Execute() {
            this.Result = LiteDbFlex.LiteDbSafeFlexer<Account>.Instance.Value.Execute<Account>(litedb => {
                return litedb.Get(this.Request.Dto)
                    .GetResult<Account>();
            });
        }

        public class GetAccountByIdSvcValidator : AbstractValidator<GetAccountByIdSvc> {
            public GetAccountByIdSvcValidator() {

            }
        }
    }
}
