using Library.Security.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Authorization;
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