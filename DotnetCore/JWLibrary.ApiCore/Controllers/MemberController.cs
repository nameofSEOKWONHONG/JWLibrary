namespace JWLibrary.ApiCore.Controllers {
    using JWLibrary.ApiCore.Base;
    using JWLibrary.ApiCore.Config;
    using JWLibrary.Core;
    using JWLibrary.Core.Data;
    using JWLibrary.Pattern.TaskAction;
    using JWService.Accounts;
    using JWService.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MemberController : JControllerBase<MemberController> {
        public MemberController(ILogger<MemberController> logger) : base(logger) {
        }

        
        [HttpPost]
        public async Task<int> SaveMember([FromBody] Account account) {
            var result = 0;
            using (var svc = ServiceFactory.CreateService<ISaveAccountSvc, SaveAccountSvc, Account, int>()) {
                svc.Action.Request = account;
                result = await svc.ExecuteCoreAsync();
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Account>> GetMembers() {
            IEnumerable<Account> accounts = null;
            var svc = base.CreateAction<IGetAccountsSvc, GetAccountsSvc, RequestDto<Account>, IEnumerable<Account>>();
            svc.Action.Request = new RequestDto<Account>();
            accounts = await svc.ExecuteCoreAsync();
            return accounts;
        }

        [Authorize]
        [HttpGet]
        public async Task<Account> GetMember(string userId, string passwd) {
            Account result = null;
            var svc = base.CreateAction<IGetAccountSvc, GetAccountSvc, Account, IAccount>();
            svc.Action.Request = new Account() {
                UserId = userId,
                Passwd = passwd
            };
            result = await svc.ExecuteCoreAsync() as Account;
            return result;            
        }

        [Authorize]
        [HttpDelete]
        public async Task<bool> DeleteMember(int id) {
            var svc = base.CreateAction<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            svc.Action.Request = new RequestDto<int>(){ Dto = id};
            return await svc.ExecuteCoreAsync();
        }
    }
}
