using System.Diagnostics.CodeAnalysis;

namespace Library.Security.Claims;

public sealed record ClaimInfo([DisallowNull] in string Type, in string? Value);