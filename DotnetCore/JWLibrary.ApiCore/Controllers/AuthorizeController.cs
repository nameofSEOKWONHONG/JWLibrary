using JWLibrary.ApiCore.Base;
using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWService.Accounts;
using JWService.Data;
using JWService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    public class AuthorizeController : JControllerBase<AuthorizeController> {
        public AuthorizeController(Microsoft.Extensions.Logging.ILogger<AuthorizeController> logger) : base(logger) {
        }

        [HttpPost]
        public async Task<string> GetToken([FromBody] Account account) {
            var exists = base.GetCache<string>(x => x.CacheName == (account.Id.ToString() + account.Passwd));
            if(exists != null) {
                return exists.Data;
            }

            JWTTokenService jwtTokenService = new JWTTokenService();
            var resultAccount = await base.CreateAction<IGetAccountSvc,
                                GetAccountSvc,
                                Account,
                                Account>()
                                .SetFilter(r => !r.UserId.Contains("h"))
                                .SetRequest(account)
                                .ExecuteAsync();
            var jwtToken = jwtTokenService.GenerateJwtToken(resultAccount.Id);
            base.SetCache<string>(new CacheInfo<string>() {
                CacheName = account.Id.ToString() + account.Passwd.ToString(),
                Data = jwtToken,
                SetDateTime = DateTime.Now.ToString(),
                Interval = 5
            });

            return jwtToken;
        }
    }
}
