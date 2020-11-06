using System.Collections.Generic;
using System.Threading.Tasks;
using JWLibrary.ApiCore.Base;
using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskService;
using JWService.Accounts;
using JWService.Data;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.ApiCore.Controllers
{
    public class MemberController : JControllerBase<MemberController>
    {
        public MemberController(ILogger<MemberController> logger) : base(logger)
        {
        }

        [HttpPost]
        public async Task<bool> SaveMember([FromBody] Account account)
        {
            using (var svc = ServiceFactory.CreateService<ISaveAccountSvc, SaveAccountSvc, Account, bool>())
            {
                return await svc.SetRequest(account)
                    .ExecuteAsync();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<PagingResultDto<IEnumerable<Account>>> GetMembers(
            [FromQuery] PagingRequestDto<Account> pagingRequestDto)
        {
            using (var svc =
                CreateAction<IGetAccountsSvc, GetAccountsSvc, PagingRequestDto<Account>,
                    PagingResultDto<IEnumerable<Account>>>())
            {
                return await svc.SetRequest(pagingRequestDto)
                    .SetFilter(r => r.Page > 0)
                    .ExecuteAsync();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IAccount> GetMember(string userId, string passwd)
        {
            var svc = CreateAction<IGetAccountSvc, GetAccountSvc, Account, Account>();
            return await svc.SetRequest(new Account
            {
                UserId = userId,
                Passwd = passwd
            }).ExecuteAsync();
        }

        [Authorize]
        [HttpDelete]
        public async Task<bool> DeleteMember(int id)
        {
            var svc = CreateAction<IDeleteAccountSvc, DeleteAccountSvc, RequestDto<int>, bool>();
            return await svc.SetRequest(new RequestDto<int> {RequestDto = id})
                .ExecuteAsync();
        }
    }
}