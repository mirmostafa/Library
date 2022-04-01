using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Defaults;

public class DynamicRoleRequirement : IAuthorizationRequirement
{
}

public class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        => throw new NotImplementedException();
}
