using FluentValidation;
using JWLibrary.ServiceExecutor;

namespace ServiceExample.Accounts {
    public abstract class AccountServiceBase<TOwner, TValidator, TRequest, TResult> : ServiceExecutor<TOwner, TValidator, TRequest, TResult>
        where TOwner : AccountServiceBase<TOwner, TValidator, TRequest, TResult>
        where TValidator : AbstractValidator<TOwner>, new() {
        public AccountServiceBase() : base() {
        }
    }
}
