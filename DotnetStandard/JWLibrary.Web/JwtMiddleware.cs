using JWLibrary.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Web {
    /// <summary>
    /// mvc : https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
    /// api : https://jasonwatmore.com/post/2020/07/21/aspnet-core-3-create-and-validate-jwt-tokens-use-custom-jwt-middleware
    /// </summary>
    public class JwtMiddleware {
        private readonly RequestDelegate _next;
        private readonly JWTSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JWTSettings> appSettings) {
            _next = next;
            _jwtSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, DataContext dataContext) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, DataContext dataContext, string token) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach account to context on successful jwt validation
                context.Items["Account"] = await dataContext.Accounts.FindAsync(accountId);
            } catch {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }

    public class JWTTokenService {
        public string GenerateJwtToken(int accountId) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim("id", accountId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            try {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return account id from JWT token if validation successful
                return accountId;
            } catch {
                // return null if validation fails
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context) {
            var account = (Account)context.HttpContext.Items["Account"];
            if (account == null) {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
