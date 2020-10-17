using JWLibrary.ApiCore.Base;
using JWLibrary.ApiCore.Config;
using JWService.Accounts;
using JWService.Data;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    public class AuthorizeController : JControllerBase<AuthorizeController> {
        public AuthorizeController(Microsoft.Extensions.Logging.ILogger<AuthorizeController> logger) : base(logger) {
        }

        [HttpPost]
        public async Task<string> GetToken([FromBody] Account account) {
            JWTTokenService jwtTokenService = new JWTTokenService();
            var resultAccount = await base.CreateAction<IGetAccountSvc,
                                GetAccountSvc,
                                Account,
                                Account>()
                                .SetFilter(r => !r.UserId.Contains("h"))
                                .SetRequest(account)
                                .ExecuteAsync();
            return jwtTokenService.GenerateJwtToken(resultAccount.Id);
        }
    }
}
