using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.TaskAction {
    public interface IActionBase<TResult> {
        bool PreExecute();
        TResult Executed();
        bool PostExecute();

    }
}
