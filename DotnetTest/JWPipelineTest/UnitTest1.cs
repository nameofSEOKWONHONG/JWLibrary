using JWLibrary.StaticMethod;
using NUnit.Framework;
using System.Threading.Tasks;

namespace JWPipelineTest {
    public class Tests {
        MainSvc _mainSvc = null;
        [SetUp]
        public void Setup() {
            _mainSvc = new MainSvc();
        }

        [Test]
        public void Test1() {
            var context = new Context();
            var modelValidate = 0;
            var isExecuting = _mainSvc.OnExecuting(context).Result;
            if (isExecuting.jIsTrue()) {
                _mainSvc.OnExecute(context);
                _mainSvc.OnExecuted(context);
            }
            Assert.Pass();
        }
    }




}