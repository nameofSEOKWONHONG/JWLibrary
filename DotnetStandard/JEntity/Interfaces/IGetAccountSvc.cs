using JWLibrary.Core;
using JWLibrary.ServiceExecutor;
using JWService.Data.Models;
using System.Collections.Generic;

namespace ServiceExample.Data {
    public interface IGetAccountByIdSvc : IServiceExecutor<RequestDto<int>, Account> { }
    public interface IGetAccountSvc : IServiceExecutor<Account, Account> { }
    public interface IGetAccountsSvc : IServiceExecutor<PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>> { }
    public interface ISaveAccountSvc : IServiceExecutor<Account, bool> { }
    public interface IDeleteAccountSvc : IServiceExecutor<RequestDto<int>, bool> { }
}
