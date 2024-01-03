using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Types;
using Library.Validations;

namespace Library.CodeGeneration.Models;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
[Immutable]
public readonly record struct Language([DisallowNull]in string Name, in string? FileExtension = null) : IEquatable<Language>
{
    public override string ToString() =>
        this.Name;

    private string GetDebuggerDisplay() =>
        this.ToString();
}