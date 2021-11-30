using System.Security.Claims;
using Library.Validations;

namespace Library.Helpers;

public static class ClaimHelper
{
    public static Claim New(string claimName) =>
        new(claimName, true.ToString());

    public static Claim New(object claimName) =>
        New(claimName.ArgumentNotNull().ToString()!);
}
