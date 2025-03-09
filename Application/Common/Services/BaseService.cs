using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services;

public class BaseService
{
    protected Result<T> GetFailureResult<T>(FluentValidation.Results.ValidationResult validationResult)
    {
        return Result<T>.FailureResult(
           validationResult.Errors
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray())
       );
    }
}
