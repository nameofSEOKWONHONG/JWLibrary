using JCoreSvc;
using System;

using JWLibrary.StaticMethod;
using System.Threading.Tasks;

namespace JCoreSvcTest {
    class Program {
        static async Task Main(string[] args) {
            //Setup();
            //CreateInstanceRun();
            //CreateManagerRun();
            var action = new ActionTest();
            var result = await action.Run();
            Console.WriteLine(result);
        }

        //static ITestSvc testSvc = null;
        //static IContext context = null;
        //static CoreSvcManager<TestSvc, TestModel, int, TestSvc.Validator> manager = null;
        //static void Setup() {
        //    manager = new CoreSvcManager<TestSvc, TestModel, int, TestSvc.Validator>();
        //    testSvc = new TestSvc();
        //    context = new Context();
        //}

        //static async void CreateInstanceRun() {
        //    var executing = await testSvc.OnExecuting(context);
        //    var execute = await testSvc.OnExecute(context);
        //    var executed = await testSvc.OnExecuted(context);
        //    var isRequestNull = testSvc.Request.jIsNull();
        //    var isResultNull = testSvc.Result.jIsNull();
        //}

        //static async void CreateManagerRun() {
        //    manager.Owner.SvcName = "test";
        //    var validatorIsNotNull = manager.Validator.jIsNotNull();
        //    await manager.Owner.OnExecuting(context);
        //    await manager.Owner.OnExecute(context);
        //    await manager.Owner.OnExecuted(context);

        //    Console.WriteLine(manager.Owner.SvcName);
        //}
    }
}
