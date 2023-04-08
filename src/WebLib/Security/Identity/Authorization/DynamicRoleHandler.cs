using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Authorization;

public sealed class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        => throw new NotImplementedException();
}
