using JCoreSvc;
using System.Transactions;

namespace JWLibrary.Pattern {

    public class Request<TDto>
        where TDto : class, new() {
    }

    public class SvcBase : ISvcBase {
    }

    public interface ISvcBase {
    }

    public class SvcCore<TOwner, TRequest, TReturn> : SvcBase, ISvcCore
        where TOwner : class
        where TRequest : class, new() {
        protected TOwner Owner { get; set; }
        protected TRequest Request { get; set; }

        public SvcCore() {
            Request = new TRequest();
        }
    }

    public interface ISvcCore : ISvcBase {
    }

    public class SvcImpl<TOwner, TRequst, TReturn> : SvcCore<TOwner, TRequst, TReturn>
        where TOwner : class
        where TRequst : class, new() {
        private PipeLine _pipeLine = null;

        public SvcImpl() {
            this.Setup();
        }

        private void Setup() {
        }

        protected PipeLine CreatePipeLine(TransactionScopeOption transactionScopeOption) {
            _pipeLine = new PipeLine(transactionScopeOption);
            return _pipeLine;
        }
    }

    //public class Service : SvcImpl<Service, Request<TestReqestDto>, int> {
    //    public void Run() {
    //        var pipe = this.CreatePipeLine(TransactionScopeOption.Suppress);
    //        pipe.Registry<>
    //    }
    //}

    //public class Program {
    //    public void Main() {
    //        SvcImpl svcImpl = new SvcImpl();

    //    }
    //}
}