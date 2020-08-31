using JWLibrary.Pattern;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JCoreSvcTest {
    class ActionTest {
        public async Task<int> Run() {
            using (var action = ActionFactory.CreateAction<ITestAction, TestAction, TestReqestDto, int>()) {
                action.Request = new TestReqestDto() {

                };
                var result = await action.ExecuteCore();
                return result;
            }
        }
    }

    public interface ITestAction : IActionBase<int> { }

    public class TestAction : ActionBase<TestReqestDto, int>, ITestAction {
        public override bool PostExecute() {
            return true;
        }

        public override bool PreExecute() {
            return true;
        }

        public override int Executed() {
            return 0;
        }
    }

    public class TestReqestDto {

    }
}
