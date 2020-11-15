using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceFilter<TRequest, TResult> {
        IServiceFilter<TRequest, TResult> AddFilter(Func<TRequest, bool> func);
        IServiceExecutor<TRequest, TResult> Validate<TOwner, TValidator>() where TOwner : BaseService<TRequest, TResult>
            where TValidator : IValidator<TOwner>, new();
    }
}
