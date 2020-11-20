using System;

namespace JWLibrary.ServiceExecutor {
    public interface IServiceBase : IDisposable {
        bool PreExecute();
        void Execute();
        void PostExecute();
        void Validate();
    }
}
