using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskAction;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Data {
    public interface IGetAccountByIdSvc : ISvcBase<RequestDto<int>, Account> { }
    public interface IGetAccountSvc : ISvcBase<Account, IAccount> { }
    public interface IGetAccountsSvc : ISvcBase<RequestDto<Account>, IEnumerable<Account>> { }
    public interface ISaveAccountSvc : ISvcBase<Account, int> { }
    public interface IDeleteAccountSvc : ISvcBase<RequestDto<int>, bool> { }
}
