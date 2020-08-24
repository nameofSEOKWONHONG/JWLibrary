using JCoreSvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JCoreSvcTest {
    public interface ITestSvc : ICoreSvc<TestModel, int> { }

    public class TestSvc : CoreSvcOwner<TestSvc, TestModel, int>, ITestSvc {
        public string SvcName { get; set; }

        public override Task<int> OnExecute(IContext context) {
            return Task.FromResult<int>(0);
        }

        public override Task<bool> OnExecuted(IContext context) {
            return base.OnExecuted(context);
        }

        public override Task<bool> OnExecuting(IContext context) {
            return base.OnExecuting(context);
        }

        public class Validator : ValidatorBase {
            public Validator() {
                Console.WriteLine("constructor TestSvc Validator");
            }
        }
    }

    public class TestModel {
        public string Name { get; set; }
    }
}
