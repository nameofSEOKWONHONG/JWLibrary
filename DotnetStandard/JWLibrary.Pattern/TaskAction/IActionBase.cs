using FluentValidation;

namespace JWLibrary.Pattern.TaskAction {

    public interface IActionBase<TRequest, TResult> {
        TRequest Request { set; }

        bool PreExecute();

        TResult Executed();

        bool PostExecute();
    }
}