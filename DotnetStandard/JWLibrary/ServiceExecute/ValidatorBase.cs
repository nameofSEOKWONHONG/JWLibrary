using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public class ValidatorBase<T, TV>
        where T : class
        where TV : AbstractValidator<T>, new() {
        public TV TValidator { get; private set; }

        public ValidatorBase() {
            TValidator = new TV();
        }
    }
}
