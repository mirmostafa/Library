using System.Diagnostics.CodeAnalysis;

namespace Library.Security.Claims;

public sealed record ClaimInfo([DisallowNull] in string Type, in string? Value)
{
    public void Deconstruct(out string type, out string? value)
        => (type, value) = (this.Type, this.Value);

    public static implicit operator (string Type, string? Value)(ClaimInfo claim) =>
        (claim.Type, claim.Value);

    public override string ToString() =>
        FormatString(this);

    public static string FormatString((string Type, string? Value) claim) =>
        $"{claim.Type}:'{claim.Value ?? "None"}'";
}