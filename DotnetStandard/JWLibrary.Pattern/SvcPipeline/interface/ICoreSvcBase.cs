using System.Threading.Tasks;

namespace JCoreSvc {

    public interface ICoreSvcBase {

        Task<bool> OnExecuting(IContext context);

        Task<bool> OnExecuted(IContext context);
    }

    public interface ICoreSvc<TRequest, TResult> : ICoreSvcBase {
        TRequest Request { get; set; }
        TResult Result { get; set; }

        Task<TResult> OnExecute(IContext context);
    }

    public interface ICoreSvcOwner<TOwner, TRequst, TResult> : ICoreSvc<TRequst, TResult> {
        TOwner Owner { get; set; }
    }
}