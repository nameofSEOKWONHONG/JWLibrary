using FluentValidation;
using JWLibrary.ServiceExecutor;

namespace JCoreSvcTest {
    class Program {
        static void Main(string[] args) {
            ITestService service = new TestService();

            var result = string.Empty;
            using (var executor = new ServiceExecutorManager<ITestService>(service)) {
                executor.SetRequest(o => o.Request = null)
                    //.AddFilter(o => o.Request.Length > 0)
                    //.AddFilter(o => o.Request.jIsNotNull())
                    .OnExecuted(o => {
                        result = o.Result;
                    });
            }
        }
    }

    public interface ITestService : IServiceExecutor<string, string> { }

    public class TestService : ServiceExecutor<TestService, TestService.TestServiceValidator, string, string>, ITestService {
        public TestService() {

        }

        public override void Execute() {
            this.Request = "HelloWorld";
            base.Execute();
        }

        public class TestServiceValidator : AbstractValidator<TestService> {
            public TestServiceValidator() {
                RuleFor(o => o.Request).NotNull();
            }
        }
    }
}
