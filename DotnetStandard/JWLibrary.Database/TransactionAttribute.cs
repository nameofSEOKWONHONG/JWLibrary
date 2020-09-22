using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JWLibrary.Database {
    public class TransactionAttribute : Attribute, IAsyncActionFilter {
        //make sure filter marked as not reusable
        private TransactionScopeOption _transactionScopeOption;
        public TransactionAttribute(TransactionScopeOption transactionScopeOption) {
            this._transactionScopeOption = transactionScopeOption;
        }
        public bool IsReusable => false;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            using (var transactionScope = new TransactionScope()) {
                var action = await next();
                if(action.Exception == null) {
                    transactionScope.Complete();
                }
            }
        }
    }
}
