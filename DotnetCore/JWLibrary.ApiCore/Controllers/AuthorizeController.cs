using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.ApiCore.Base;
using JWLibrary.ApiCore.Config;
using JWLibrary.Core.Data;
using JWLibrary.Web;
using JWService.Accounts;
using JWService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                                IAccount>()
                                .SetRequest(account)
                                .ExecuteCoreAsync();
            return jwtTokenService.GenerateJwtToken(resultAccount.Id);
        }
    }
}
