using JWLibrary.StaticMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace JCoreSvc {
    public class PipeLine {
        private TransactionScopeOption _transactionScopeOption;
        private ICoreSvcBase _svc;
        private IRequest _request;

        public PipeLine(TransactionScopeOption transactionScopeOption) { _transactionScopeOption = transactionScopeOption; }

        public PipeLine CreatePipeline(TransactionScopeOption transactionScopeOption) {
            _transactionScopeOption = transactionScopeOption;
            return this;
        }
        public PipeLine Registry<TSvc>(string pipelineName = null)
            where TSvc : ICoreSvcBase {
            //var instance = DependencyInjection.Instances.Where(m => m.GetType().GetInterface(typeof(TSvc).Name).jIsNotNull()).First();
            //if (instance.jIsNotNull()) {
            //    _svc = instance;
            //}
            return this;
        }
        public PipeLine Mapping(IRequest request) {
            _request = request;
            return this;
        }
        public PipeLine AddFilter(Func<IFilter, bool> func) {
            return this;
        }

        public void OnExecuted(Func<ICoreSvcBase, bool> func) {
            var result = func(_svc);
        }
    }
}
