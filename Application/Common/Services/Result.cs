using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services;

public class Result<T>
{
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public IDictionary<string, string[]>? Errors { get; private set; }

    private Result(bool success, T? data, IDictionary<string, string[]>? errors)
    {
        Success = success;
        Data = data;
        Errors = errors;
    }

    public static Result<T> SuccessResult(T data) => new Result<T>(true, data, null);
    public static Result<T> FailureResult(IDictionary<string, string[]> errors) => new Result<T>(false, default, errors);
}
