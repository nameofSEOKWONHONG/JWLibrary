using System.Threading.Tasks;
using FluentValidation;

namespace JWLibrary.Pattern.TaskAction {

    public interface ISvcBase<TRequest, TResult> {
        TRequest Request { set; }
        TResult Result { get; set; }

        Task<bool> PreExecute();

        TResult Executed();

        Task<bool> PostExecute();
    }
}