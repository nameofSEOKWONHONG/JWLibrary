using JWLibrary.Pattern.TaskService;

namespace JWService.Accounts {
    public abstract class AccountSvcBase<TRequest, TResult> : SvcBase<TRequest, TResult>
        where TRequest : class {

        public override TResult Executed() {
            return default(TResult);
        }

        public override bool PostExecute() {
            return true;
        }

        public override bool PreExecute() {
            return true;
        }
    }
}
