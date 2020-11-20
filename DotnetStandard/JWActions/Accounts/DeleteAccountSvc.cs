using FluentValidation;
using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using ServiceExample.Data;

namespace ServiceExample.Accounts {
    public class DeleteAccountSvc : AccountServiceBase<DeleteAccountSvc, DeleteAccountSvc.DeleteAccountServiceValidator, RequestDto<int>, bool>,
        IDeleteAccountSvc {
        private IGetAccountByIdSvc _getAccountByIdSvc;
        public DeleteAccountSvc(IGetAccountByIdSvc getAccountByIdSvc) {
            _getAccountByIdSvc = getAccountByIdSvc;
        }

        public override bool PreExecute() {
            var result = false;
            using var executor = new ServiceExecutorManager<IGetAccountByIdSvc>(_getAccountByIdSvc);
            executor.SetRequest(o => o.Request = this.Request)
                .OnExecuted(o => {
                    result = o.Result.jIsNotNull();
                });

            return result;
        }

        public override void Execute() {
            this.Result = LiteDbFlex.LiteDbSafeFlexer<Account>.Instance.Value.Execute(litedb => {
                return litedb.BeginTrans()
                    .Delete(m => m.Id == this.Request.Dto)
                    .GetResult<int>() > 0;
            });
        }

        public class DeleteAccountServiceValidator : AbstractValidator<DeleteAccountSvc> {
            public DeleteAccountServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}
