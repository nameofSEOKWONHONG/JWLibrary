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
            using (var svc = ServiceFactory.CreateService<ISaveAccountSvc, SaveAccountSvc, Account, int>()) {
                return await svc.SetRequest(account)
                .ExecuteCoreAsync();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Account>> GetMembers() {
            var svc = base.CreateAction<IGetAccountsSvc, GetAccountsSvc, RequestDto<Account>, IEnumerable<Account>>();
            return await svc.SetRequest(new RequestDto<Account>())
            .ExecuteCoreAsync();
        }

        [Authorize]
        [HttpGet]
        public async Task<IAccount> GetMember(string userId, string passwd) {
            var svc = base.CreateAction<IGetAccountSvc, GetAccountSvc, Account, IAccount>();
            return await svc.SetRequest(new Account() {
                UserId = userId,
                Passwd = passwd
            }).ExecuteCoreAsync();
        }

        [Authorize]
        [HttpDelete]
        public async Task<bool> DeleteMember(int id) {
            var svc = base.CreateAction<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            return await svc.SetRequest(new RequestDto<int>(){ Dto = id})
            .ExecuteCoreAsync();
        }
    }
}
