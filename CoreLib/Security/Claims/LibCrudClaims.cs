using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Claims;

public static class LibCrudClaims
{
    public const string READ_CLAIM_TYPE = "ReadClaim";
    public const string CREATE_CLAIM_TYPE = "CreateClaim";
    public const string UPDATE_CLAIM_TYPE = "UpdateClaim";
    public const string DELETE_CLAIM_TYPE = "DeleteClaim";
        
    public static readonly Claim Read = new(READ_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly Claim Create = new(CREATE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly Claim Update = new(UPDATE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);
    public static readonly Claim Delete = new(DELETE_CLAIM_TYPE, LibClaimDefaultValues.VALID_CLAIM_VALUE);

    public static IEnumerable<string> GetClaimTypes()
    {
        yield return READ_CLAIM_TYPE;
        yield return CREATE_CLAIM_TYPE;
        yield return UPDATE_CLAIM_TYPE;
        yield return DELETE_CLAIM_TYPE;
    }

    public static IEnumerable<Claim> GetClaims()
    {
        yield return Read;
        yield return Create;
        yield return Update;
        yield return Delete;
    }
}

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
