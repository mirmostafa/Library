using Library.Security.Claims;
using Library.Security.Identity.Authorization;
using Library.Validations;

using Microsoft.AspNetCore.Authorization;

namespace Library.Web.Helpers;

public static class AuthorizationHelper
{
    public static AuthorizationOptions AddCrudRequirementPolicies([DisallowNull] this AuthorizationOptions options)
    {
        Check.MustBeArgumentNotNull(options);
        foreach (var claim in LibCrudClaims.GetClaims())
        {
            options.AddPolicy(claim.Type, policy => policy.Requirements.Add(new ClaimRequirement(claim)));
        }
        return options;
    }

    public static AuthorizationOptions AddPolicies([DisallowNull] this AuthorizationOptions options, params (string Name, Action<AuthorizationPolicyBuilder> ConfigurePolicy)[] policies)
    {
        Check.MustBeArgumentNotNull(options);
        Check.MustBeArgumentNotNull(policies);

        foreach (var (name, configurePolicy) in policies)
        {
            options.AddPolicy(name, configurePolicy);
        }
        return options;
    }
}