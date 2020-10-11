namespace JWLibrary.ApiCore.Controllers {
    using JWLibrary.ApiCore.Base;
    using JWLibrary.Core.Data;
    using JWLibrary.Pattern.TaskAction;
    using JWService.Accounts;
    using JWService.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class MemberController : JControllerBase<MemberController> {
        public MemberController(ILogger<MemberController> logger) : base(logger) {
        }

        [Route("/SaveMember")]
        [HttpPost]
        public async Task<int> SaveMember([FromBody] Account account) {
            var result = 0;
            using (var svc = ServiceFactory.CreateService<ISaveAccountSvc, SaveAccountSvc, Account, int>()) {
                svc.Action.Request = account;
                result = await svc.ExecuteCoreAsync();
            }
            return result;
        }
    }
}
