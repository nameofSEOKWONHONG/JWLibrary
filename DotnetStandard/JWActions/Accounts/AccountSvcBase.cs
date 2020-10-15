using JWLibrary.Core;
using JWLibrary.Core.Data;
using JWLibrary.Pattern.TaskAction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace JWService.Accounts {
    public abstract class AccountSvcBase<TRequest, TResult> : SvcBase<TRequest, TResult> 
        where TRequest : class {
        protected readonly string ACCOUNT_FILE = typeof(Account).jGetAttributeValue((DBFileNameAttribute dn) => dn.FileName);
        protected readonly string ACCOUNT_DB = typeof(Account).jGetAttributeValue((TableAttribute ta) => ta.Name);

        public override TResult Executed() {
            return default(TResult);
        }

        public override async Task<bool> PostExecute() {
            return await Task.FromResult(true);
        }

        public override async Task<bool> PreExecute() {
            return await Task.FromResult(true);
        }
    }
}
