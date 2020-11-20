using FluentValidation;
using JWLibrary.Core;
using JWLibrary.Database;
using JWService.Data.Models;
using Mapster;
using ServiceExample.Data;
using System.Collections.Generic;

namespace ServiceExample.Accounts {
    public class GetAccountsSvc : AccountServiceBase<GetAccountsSvc, GetAccountsSvc.GetAccountsSvcValidator, PagingRequestDto<Account>, PagingResultDto<IEnumerable<Account>>>,
        IGetAccountsSvc {

        public override void Execute() {
            using (var db = LiteDbFlex.LiteDbResolver.Resolve<Account>()) {
                var col = db.jGetCollection<Account>();
                var query = col.Query();
                var request = this.Request.Dto.jIfNull(x => x = new Account());
                if(request.Id > 0) {
                    query = query.Where(m => m.Id >= request.Id);
                }
                if(request.UserId.jIsNullOrEmpty().jIsFalse()) {
                    query = query.Where(m => m.UserId == request.UserId);
                }
                var accounts = query.Limit(this.Request.Size).Offset((this.Request.Page - 1) * this.Request.Page).ToList();

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
                this.Result = result;
            }
        }

        public class GetAccountsSvcValidator : AbstractValidator<GetAccountsSvc> {
            public GetAccountsSvcValidator() {

            }
        }
    }
}
