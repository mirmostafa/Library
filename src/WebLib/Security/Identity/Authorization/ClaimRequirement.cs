using System.Security.Claims;

using Library.Security.Claims;
using Library.Validations;
using Library.Web.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace Library.Security.Identity.Authorization;
public sealed class ClaimRequirement : IAuthorizationRequirement
{
    public ClaimRequirement([DisallowNull] in ClaimInfo claim)
        : this(claim.ToClaim()) { }
    public ClaimRequirement([DisallowNull] in Claim claim)
        => this.Claim = claim.ArgumentNotNull();
    public ClaimRequirement([DisallowNull] in string claimType, in string claimValue = LibClaimDefaultValues.VALID_CLAIM_VALUE)
        : this(new Claim(claimType.NotNull(), claimValue)) { }

    public Claim Claim { get; }
}