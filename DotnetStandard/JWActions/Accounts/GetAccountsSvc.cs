using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskService;
using JWService.Data;
using JWService.Data.Models;
using LiteDB;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWService.Accounts {
    public class GetAccountsSvc : AccountSvcBase<PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>>, IGetAccountsSvc {
        public override PagingResultDto<IEnumerable<Account>> Executed() {
            using (var db = JDataBase.Resolve<ILiteDatabase, Account>()) {
                var col = db.jGetCollection<Account>();
                var query = col.Query();
                var request = this.Request.RequestDto.jIfNull(x => x = new Account());
                if(request.Id > 0) {
                    query = query.Where(m => m.Id >= request.Id);
                }
                if(request.UserId.jIsNullOrEmpty().jIsFalse()) {
                    query = query.Where(m => m.UserId == request.UserId);
                }
                var accounts = query.Limit(this.Request.Size).Offset((this.Request.PageNumber - 1) * this.Request.Page).ToList();

                var result = this.Request.Adapt<PagingResultDto<IEnumerable<Account>>>();
                result.TotalCount = col.Count();
                result.ResultDto = accounts;
                //var result = new PagingResultDto<IEnumerable<Account>>() {
                //    Page = this.Request.Page,
                //    Size = this.Request.Size,
                //    PageNumber = this.Request.PageNumber,
                //    TotalPageNumber = col.Count(),
                //    ResultDto = accounts
                //};
                return result;
            }
        }
    }
}
