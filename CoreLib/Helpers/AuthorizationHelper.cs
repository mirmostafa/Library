using System.Diagnostics.CodeAnalysis;
using Library.Validations;
using Microsoft.AspNetCore.Authorization;

namespace Library.Helpers;

public static class AuthorizationHelper
{
    public static AuthorizationOptions AddPolicy([DisallowNull] this AuthorizationOptions options, params (string Name, Action<AuthorizationPolicyBuilder> ConfigurePolicy)[] policies)
    {
        Check.IfArgumentNotNull(options);
        Check.IfArgumentNotNull(policies);
        foreach (var (name, configurePolicy) in policies)
        {
            options.ArgumentNotNull().AddPolicy(name, configurePolicy);
        }
        return options;
    }
}
