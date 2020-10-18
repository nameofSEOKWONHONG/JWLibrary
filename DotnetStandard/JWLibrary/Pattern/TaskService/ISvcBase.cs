﻿using System;
using System.Threading.Tasks;
using FluentValidation;
using JWLibrary.Core;

namespace JWLibrary.Pattern.TaskService {

    public interface ISvcBase<TRequest, TResult> {
        TRequest Request { set; }
        TResult Result { get; set; }

        JList<Func<TRequest, bool>> Filters { get; set; }

        bool PreExecute();

        TResult Executed();

        bool PostExecute();
    }
}