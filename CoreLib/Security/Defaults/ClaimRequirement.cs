using System.Security.Claims;
using Library.Security.Claims;
using Library.Validations;
using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Defaults;

public sealed class ClaimRequirement : IAuthorizationRequirement
{
    public ClaimRequirement([DisallowNull] in Claim claim)
        => this.Claim = claim.ArgumentNotNull();
    public ClaimRequirement([DisallowNull] in string claimType, in string claimValue = LibClaimDefaultValues.VALID_CLAIM_VALUE)
        : this(new(claimType, claimValue)) { }

    public Claim Claim { get; }
}

public sealed class ClaimRequirementHandler : AuthorizationHandler<ClaimRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
    {
        if (context.User.HasClaim(requirement.Claim.Type, requirement.Claim.Value))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}