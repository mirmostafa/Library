using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Library.Validations;

namespace Library.Security.Claims;

public interface IClaim
{
    string Type { get; }
    string? Value { get; }
}

public readonly record struct ClaimInfo([DisallowNull] in string Type, in string? Value) : IClaim, IEquatable<ClaimInfo>, IEquatable<Claim>
{
    public void Deconstruct(out string type, out string? value) =>
        (type, value) = (this.Type, this.Value);
    public static implicit operator (string Type, string? Value)(ClaimInfo claim) =>
        (claim.Type, claim.Value);

    public static bool operator ==(ClaimInfo? left, Claim? right) =>
        (left, right) switch
        {
            (null, null) => true,
            (not null, null) => false,
            (null, not null) => false,
            _ => (left.Value.Type, left.Value.Value) == (right.Type, right.Value)
        };
    public static bool operator !=(ClaimInfo? left, Claim? right) =>
        !(left == right);

    public static bool operator ==(Claim? left, ClaimInfo? right) =>
        right == left;
    public static bool operator !=(Claim? left, ClaimInfo? right) =>
        !(left == right);

    public override string ToString() =>
        FormatString(this);
    public static string FormatString((string Type, string? Value) claim) =>
        $"{claim.Type}:'{claim.Value}'";

    public static ClaimInfo FromClaim(in Claim claim) =>
        new(claim.ArgumentNotNull().Type, claim.Value);
    public Claim ToClaim() =>
        new(this.Type, this.Value ?? string.Empty);

    public bool Equals(Claim? other) =>
        this == other;
}