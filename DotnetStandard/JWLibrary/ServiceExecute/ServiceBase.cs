using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public abstract class ServiceBase<TOwner> : IServiceBase {
        public abstract void Dispose();

        public abstract void Execute();

        public abstract void PostExecute();

        public abstract bool PreExecute();
        public abstract void Validate();
    }
}
