using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Authorization;

public class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        => throw new NotImplementedException();
}
