using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace JWLibrary.Web {

    public class TransactionAttribute : Attribute, IAsyncActionFilter {

        //make sure filter marked as not reusable
        private readonly TransactionScopeOption _transactionScopeOption;

        public TransactionAttribute(TransactionScopeOption transactionScopeOption) {
            _transactionScopeOption = transactionScopeOption;
        }

        public bool IsReusable => false;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            using (var transactionScope = new TransactionScope(_transactionScopeOption, TimeSpan.FromSeconds(5),
                TransactionScopeAsyncFlowOption.Enabled)) {
                var action = await next();
                if (action.Exception == null)
                    transactionScope.Complete();
                else
                    Console.WriteLine(action.Exception.Message);
            }
        }
    }
}