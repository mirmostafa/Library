using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Claims;

public static class LibCrudPolicies
{
    private const string FULL_ACCESS_POLICY_NAME = nameof(FullAccessPolicy);
    public static (string Name, Action<AuthorizationPolicyBuilder> ConfigurePolicy) FullAccessPolicy
    {
        get
        {
            void configurePolicy(AuthorizationPolicyBuilder policy) => LibCrudClaims.GetClaimTypes().ForEach(x => policy.RequireClaim(x)).Build();
            return (FULL_ACCESS_POLICY_NAME, configurePolicy);
        }
    }

    private const string ADMIN_OR_FULL_ACCESS_POLICY_NAME = nameof(AdminOrFullAccessPolicy);
    public static (string Name, Action<AuthorizationPolicyBuilder> ConfigurePolicy) AdminOrFullAccessPolicy
    {
        get
        {
            void configurePolicy(AuthorizationPolicyBuilder policy) => policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.IsInRole("SuperAdmin") || LibCrudClaims.GetClaimTypes().All(x => context.User.HasClaim(x, LibClaimDefaultValues.VALID_CLAIM_VALUE)));
            return (ADMIN_OR_FULL_ACCESS_POLICY_NAME, configurePolicy);
        }
    }
}
