using System.Security.Claims;

using Library.Validations;

using Microsoft.AspNetCore.Identity;

namespace Library.Helpers;

public static class IdentityHelper
{
    public static Claim? ClaimByKey(this IList<Claim> claimList, [DisallowNull] string type) =>
        claimList.FirstOrDefault(x => x.Type == type);

    public static async Task<IdentityResult> CreateAsync<TIdentityUser>(this UserManager<TIdentityUser> manager, [DisallowNull] TIdentityUser user, [DisallowNull] string password)
        where TIdentityUser : class
    {
        // Validate the inputs
        Check.MustBeArgumentNotNull(manager);
        Check.MustBeArgumentNotNull(user);
        Check.MustBeArgumentNotNull(password);

        // Assuming userManager is an instance of UserManager<InfraIdentityUser> that has been
        // injected previously.
        var result = await manager.CreateAsync(user, password);

        // Return the creation result
        return result;
    }

    public static async Task<Claim?> GetClaimByType<TIdentityUser>(this UserManager<TIdentityUser> manager, TIdentityUser user, string type)
        where TIdentityUser : class
    {
        Check.MustBeArgumentNotNull(manager);
        Check.MustBeArgumentNotNull(user);
        Check.MustBeArgumentNotNull(type);

        var claims = await manager.GetClaimsAsync(user);
        var result = claims.ClaimByKey(type);
        return result;
    }

    public static async Task<string?> GetClaimValueByType<TIdentityUser>(this UserManager<TIdentityUser> manager, TIdentityUser user, string type)
            where TIdentityUser : class
    {
        var claim = await GetClaimByType(manager, user, type);
        return claim?.Value;
    }

    public static async Task<IdentityResult> SetClaimAsync<TIdentityUser>(this UserManager<TIdentityUser> userManager, TIdentityUser user, string type, string value)
        where TIdentityUser : class
    {
        Check.MustBeArgumentNotNull(userManager);

        var claims = await userManager.GetClaimsAsync(user);
        var oldClaim = claims.FirstOrDefault(x => x.Type == type);
        var newClaim = new Claim(type, value);
        return oldClaim != null
            ? await userManager.ReplaceClaimAsync(user, oldClaim, newClaim)
            : await userManager.AddClaimAsync(user, newClaim);
    }

    public static Results.Result ToResult(this IdentityResult identityResult, string? successMessage = null, string? errorMessage = null) =>
            identityResult.ArgumentNotNull().Succeeded
            ? Results.Result.Success(message: successMessage)
            : Results.Result.Fail(message: errorMessage);

    public static Results.Result<TValue?> ToResult<TValue>(
        this IdentityResult identityResult,
        TValue? value,
        int errorCode = -1,
        string? successMessage = null,
        string? errorMessage = null) =>
        identityResult.ArgumentNotNull().Succeeded
            ? Results.Result.Success(value, message: successMessage)
            : Results.Result.Fail<TValue>();

    public static async Task<Results.Result> ToResultAsync(this Task<IdentityResult> identityResult, string? successMessage = null, string? errorMessage = null) =>
        ToResult(await identityResult, successMessage, errorMessage);

    public static IEnumerable<(object Code, object Error)> ToResultErrors(this IEnumerable<IdentityError> identityErrors) =>
        identityErrors.Select(error => ((object)error.Code, (object)error.Description));

    public static string? ValueByKey(this IList<Claim> claimList, [DisallowNull] string type) =>
        ClaimByKey(claimList, type)?.Value;
}