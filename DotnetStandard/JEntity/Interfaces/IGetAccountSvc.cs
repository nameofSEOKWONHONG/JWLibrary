using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskService;
using JWService.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Data {
    public interface IGetAccountByIdSvc : ISvcBase<RequestDto<int>, Account> { }
    public interface IGetAccountSvc : ISvcBase<Account, Account> { }
    public interface IGetAccountsSvc : ISvcBase<PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>> { }
    public interface ISaveAccountSvc : ISvcBase<Account, int> { }
    public interface IDeleteAccountSvc : ISvcBase<RequestDto<int>, bool> { }
}
