using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using JWLibrary.ApiCore.Base;
using JWLibrary.ApiCore.Config;
using JWService.Accounts;
using JWService.Data;
using JWService.Data.Models;
using LiteDbFlex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JWLibrary.ApiCore.Controllers
{
    public class AuthorizeController : JControllerBase<AuthorizeController>
    {
        public AuthorizeController(ILogger<AuthorizeController> logger)
            : base(logger)
        {

        }

        [HttpPost]
        public async Task<string> GetToken([FromBody] Account account)
        {
            var exists = GetCache<string>(account.Id + account.Passwd);
            if (exists != null) return exists.Data;

            var jwtTokenService = new JWTTokenService();
            var resultAccount = await CreateAction<IGetAccountSvc,
                    GetAccountSvc,
                    Account,
                    Account>()
                .SetFilter(r => !r.UserId.Contains("h"))
                .SetRequest(account)
                .ExecuteAsync();
            var jwtToken = jwtTokenService.GenerateJwtToken(resultAccount.Id);
            var cacheInfo = new CacheInfo<string>
            {
                CacheName = account.Id + account.Passwd,
                Data = jwtToken,
                SetTime = DateTime.Now,
                Interval = 5
            };
            SetCache(cacheInfo);

            return jwtToken;
        }
    }
}