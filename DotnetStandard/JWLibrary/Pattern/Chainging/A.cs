//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using System.Diagnostics;
//using System.Linq.Expressions;
//using System.Text;
//using FluentValidation;
//using JWLibrary.Core;

//namespace JWLibrary.Pattern.Chainging {

//    public interface IAccountService : IServiceBase<AccountRequest, AccountResult>
//    {

//    }

//    public class AccountService : ServiceBase<AccountRequest, AccountResult>, IAccountService {
//        public AccountService()
//        {

//        }

//        public class Validator : AbstractValidator<AccountService> {
//            public Validator()
//            {

//            }
//        }
//    }

//    public class AccountRequest
//    {
//    }

//    public class AccountResult
//    {
//    }

//    public class Test123
//    {
//        public void Run()
//        {
//            IAccountService svc = new AccountService();
//            var result = ServiceManager<IAccountService, AccountRequest, AccountResult>.AddIService(svc)
//                .SetRequest(new AccountRequest())
//                .SetFilter(m => m != null)
//                .SetFilter(m => m != null)
//                .OnExecute();

//        }
//    }
//}
