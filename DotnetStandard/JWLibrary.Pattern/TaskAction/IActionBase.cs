using FluentValidation;

namespace JWLibrary.Pattern.TaskAction {

    public interface IActionBase<TRequest, TResult> {
        TRequest Request { set; }
        TResult Result { get; set; }

        bool PreExecute();

        TResult Executed();

        bool PostExecute();
    }
}