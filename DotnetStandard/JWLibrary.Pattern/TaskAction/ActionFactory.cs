using JWLibrary.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace JWLibrary.Pattern.TaskAction {
    public class ActionFactory {
        public static TaskAction<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : IActionBase<TRequest, TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class {
            return new TaskAction<TIAction, TAction, TRequest, TResult>();
        }
    }
}
