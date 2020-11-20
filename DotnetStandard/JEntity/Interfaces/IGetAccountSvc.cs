using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskService;
using ServiceExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceExample.Data {
    public interface IGetAccountByIdSvc : ISvcBase<RequestDto<int>, Account> { }
    public interface IGetAccountSvc : ISvcBase<Account, Account> { }
    public interface IGetAccountsSvc : ISvcBase<PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>> { }
    public interface ISaveAccountSvc : ISvcBase<Account, bool> { }
    public interface IDeleteAccountSvc : ISvcBase<RequestDto<int>, bool> { }
}
