//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace JCoreSvc {
//    public interface ITestSvc : ICoreSvc<TestRequstModel, int> { }
//    public class TestRequstModel {
//    }

//    public class MainSvc : CoreSvc<MainSvc, TestRequstModel, int> {
//        public override Task<bool> OnExecuted(IContext context) {
//            return base.OnExecuted(context);
//        }

//        public override Task<int> OnExecute(IContext context) {
//            var pipe = this.CreatePipeLine(System.Transactions.TransactionScopeOption.Suppress);
//            pipe.Registry<ITestSvc>()
//                .OnExecuted(o => {
//                    return true;
//                });
//            return base.OnExecute(context);
//        }

//        public override Task<bool> OnExecuting(IContext context) {
//            return base.OnExecuting(context);
//        }
//    }

//    public class TestSvc : CoreSvc<TestSvc, TestRequstModel, int>, ITestSvc {
//        public override Task<bool> OnExecuted(IContext context) {
//            return base.OnExecuted(context);
//        }

//        public override Task<int> OnExecute(IContext context) {
//            this.Result = 1;
//            return base.OnExecute(context);
//        }

//        public override Task<bool> OnExecuting(IContext context) {
//            return base.OnExecuting(context);
//        }
//    }
//}