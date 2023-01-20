using Library.Results;
using Library.Validations;
using Microsoft.AspNetCore.Identity;

namespace Library.Helpers;

public static class IdentityHelper
{
    public static Result ToResult(this IdentityResult identityResult, string? successMessage = null, string? errorMessage = null)
    {
        if (identityResult.ArgumentNotNull().Succeeded)
        {
            return Result.CreateSuccess(message: successMessage);
        }
        var result = Result.CreateFail(message: errorMessage);
        List<(object, object)> errors = new();
        foreach (var error in identityResult.Errors)
        {
            errors.Add((error.Code, error.Description));
        }
        return Result.CreateFail(message: errorMessage, errors: errors);
    }

    public static Result<TValue?> ToResult<TValue>(this IdentityResult identityResult,
                                                  TValue value,
                                                  int errorCode = -1,
                                                  string? successMessage = null,
                                                  string? errorMessage = null)
    {
        if (identityResult.ArgumentNotNull().Succeeded)
        {
            return Result<TValue>.CreateSuccess(value, message: successMessage);
        }
        List<(object, object)> errors = new();
        foreach (var error in identityResult.Errors)
        {
            errors.Add((error.Code, error.Description));
        }
        var result = Result<TValue>.CreateFail(message: errorMessage, errors: errors);
        return result;
    }
}