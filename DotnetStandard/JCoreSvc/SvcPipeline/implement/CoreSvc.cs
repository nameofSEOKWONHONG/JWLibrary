using JWLibrary.StaticMethod;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JCoreSvc {
    public abstract class CoreSvcBase {
        protected bool State { get; set; }

        public abstract Task<bool> OnExecuted(IContext context);

        public abstract Task<bool> OnExecuting(IContext context);
    }

    public abstract class CoreSvc<TOwner, TRequest, TResult> : CoreSvcBase
        where TOwner : class
        where TRequest : class, new() {

        public TOwner Owner { get; set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }

        private List<PipeLine> _pipeLines = null;

        public CoreSvc() {
            _pipeLines = new List<PipeLine>();
        }

        public CoreSvc(TRequest request) {
            this.Request = request;
        }

        public override Task<bool> OnExecuting(IContext context) {
            return Task.FromResult<bool>(true);
        }

        public abstract Task<TResult> OnExecute(IContext context);

        public override Task<bool> OnExecuted(IContext context) {
            return Task.FromResult<bool>(true);
        }

        protected virtual PipeLine CreatePipeLine(TransactionScopeOption transactionScopeOption) {
            var newPipeline = new PipeLine(transactionScopeOption);
            _pipeLines.Add(newPipeline);
            return newPipeline;
        }
    }

    public class CoreSvcOwner<TOwner, TRequest, TResult> : CoreSvc<TOwner, TRequest, TResult>, IDisposable
        where TOwner : class
        where TRequest : class, new() {
        new public TRequest Request { get { return base.Request; } set { base.Request = value; } }

        public override Task<TResult> OnExecute(IContext context) {
            return Task.FromResult(base.Result);
        }

        public void Dispose() {

        }
    }

    public interface IValidator { }
    public class ValidatorBase : IValidator {

    }

    public class CoreSvcManager<TClass, TRequest, TResult, TValidator>
        where TClass : class, ICoreSvcOwner<TClass, TRequest, TResult>, new()
        where TRequest : class, new()
        where TValidator : class, IValidator, new() {

        public TClass Owner { get; set; }
        public TRequest Request { get; set; }
        public TValidator Validator { get; set; }

        public CoreSvcManager() {
            Owner = new TClass();
            Owner.Owner = Owner;
            Owner.Request = Owner.Request.jIfNull(x => new TRequest());
            this.Validator = new TValidator();
        }
    }
}
