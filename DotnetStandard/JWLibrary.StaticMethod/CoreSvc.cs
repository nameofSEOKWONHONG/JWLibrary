using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JWLibrary.StaticMethod {
    public interface ICoreSvcBase {

    }

    public interface ICoreSvc<TRequest, TResult> : ICoreSvcBase {
        Task<bool> OnExecuting(IContext context);
        Task<TResult> OnExecute(IContext context);
        Task<bool> OnExecuted(IContext context);
    }

    public interface IContext {

    }

    public class Context : IContext {
    }


    public class ErrorState {
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class CoreSvcBase : ICoreSvcBase {
        protected bool State { get; set; }
    }

    public class CoreSvc<TOwner, TRequest, TResult> : CoreSvcBase, ICoreSvc<TRequest, TResult>
        where TOwner : class
        where TRequest : class, new() {

        public TOwner Owner { get; set; }
        protected TRequest Request { get; set; }
        protected TResult Result { get; set; }

        private IContext Context = null;

        private List<PipeLine> _pipeLines = null;
        public CoreSvc() {
            Context = new Context();
            Request = new TRequest();
            _pipeLines = new List<PipeLine>();
        }

        public virtual Task<bool> OnExecuting(IContext context) {
            return Task.FromResult<bool>(true);
        }

        public virtual Task<TResult> OnExecute(IContext context) {
            return Task.FromResult<TResult>(Result);
        }

        public virtual Task<bool> OnExecuted(IContext context) {
            return Task.FromResult<bool>(true);
        }

        protected virtual PipeLine CreatePipeLine(TransactionScopeOption transactionScopeOption) {
            var newPipeline = new PipeLine(transactionScopeOption);
            _pipeLines.Add(newPipeline);
            return newPipeline;
        }
    }

    public interface IRequest {

    }
    public interface IFilter {

    }

    public class PipeLine {
        private TransactionScopeOption _transactionScopeOption;
        private ICoreSvcBase _svc;
        private IRequest _request;

        public PipeLine(TransactionScopeOption transactionScopeOption) { _transactionScopeOption = transactionScopeOption; }

        public PipeLine Create(TransactionScopeOption transactionScopeOption) {
            _transactionScopeOption = transactionScopeOption;
            return this;
        }
        public PipeLine Registry<TSvc>(string pipelineName = null)
            where TSvc : ICoreSvcBase {
            var instance = DependencyInjection.Instances.Where(m => m.GetType().GetInterface(typeof(TSvc).Name).jIsNotNull()).First();
            if(instance.jIsNotNull()) {
                _svc = instance;
            }
            return this;
        }
        public PipeLine Mapping(IRequest request) {
            _request = request;
            return this;
        }
        public PipeLine AddFilter(IFilter filter) {
            return this;
        }

        public void OnExecuted(Func<ICoreSvcBase, bool> func) {
            var result = func((TSvc)_svc);
        }
    }

    public interface ITestSvc : ICoreSvc<TestRequstModel, int> { }

    public class DependencyInjection {
        public static List<CoreSvcBase> Instances = new List<CoreSvcBase>() {
            new TestSvc()
        };
    }

    public class TestRequstModel {

    }

    public class MainSvc : CoreSvc<MainSvc, TestRequstModel, int> {
        public override Task<bool> OnExecuted(IContext context) {
            return base.OnExecuted(context);
        }

        public override Task<int> OnExecute(IContext context) {
            var pipe = this.CreatePipeLine(System.Transactions.TransactionScopeOption.Suppress);
            pipe.Registry<ITestSvc>()
                .OnExecuted(o => {
                    return true;
                });
            return base.OnExecute(context);
        }

        public override Task<bool> OnExecuting(IContext context) {
            return base.OnExecuting(context);
        }
    }


    public class TestSvc : CoreSvc<TestSvc, TestRequstModel, int>, ITestSvc {
        public override Task<bool> OnExecuted(IContext context) {
            return base.OnExecuted(context);
        }

        public override Task<int> OnExecute(IContext context) {
            this.Result = 1;
            return base.OnExecute(context);
        }

        public override Task<bool> OnExecuting(IContext context) {
            return base.OnExecuting(context);
        }
    }
}
