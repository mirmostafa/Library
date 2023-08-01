using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Dynamic;
using Library.Validations;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Code(in string name, in Language language, in string statement, in bool isPartial = false, in string? fileName = null) : IEquatable<Code>
{
    public static readonly Code Empty = new(string.Empty, Languages.None, string.Empty);
    protected readonly dynamic ExtraProperties = new Expando();

    /// <summary>
    /// Constructor for Code class with parameters.
    /// </summary>
    /// <param name="Name">Name of the code.</param>
    /// <param name="Language">Language of the code.</param>
    /// <param name="Statement">Statement of the code.</param>
    /// <param name="IsPartial">Whether the code is partial or not.</param>
    /// <returns>An instance of Code class.</returns>
    public Code(in (string Name, Language Language, string Statement, bool IsPartial) data)
        : this(data.Name, data.Language, data.Statement, data.IsPartial) { }

    public Code(in (string Name, Language Language, string Statement, bool IsPartial, string? FileName) data)
        : this(data.Name, data.Language, data.Statement, data.IsPartial, data.FileName) { }

    public Code(Code original)
        : this(original.Name, original.Language, original.Statement, original.IsPartial, original.FileName)
    {
    }

    public string FileName => this._FileName.IfNullOrEmpty(GenerateFileName(this.Name, this.Language, this.IsPartial));

    public bool IsPartial { get; } = isPartial;

    public Language Language { get; } = language;

    public string Name { get; } = name.ArgumentNotNull().ToString();

    public string Statement { get; init; } = statement.ArgumentNotNull().ToString();

    private string? _FileName { get; init; } = fileName;

    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial) codeData) =>
        new(codeData);

    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial, string FileName) codeData) =>
        new(codeData.Name, codeData.language, codeData.Statement, codeData.IsPartial, codeData.FileName);

    public static implicit operator string?(in Code? code) =>
        code?.Statement;

    public static bool operator !=(Code? left, Code? right) =>
        !(left == right);

    public static bool operator ==(Code? left, Code? right) =>
        left?.Equals(right) ?? (right is null);

    public static Code ToCode(in string name, in Language language, in string statement, in bool isPartial, in string? fileName) =>
        new(name, language, statement, isPartial, fileName);

    public void Deconstruct(out string name, out string statement) =>
        (name, statement) = (this.Name, this.Statement);

    public void Deconstruct(out string name, out string statement, out bool isPartial) =>
        (name, statement, isPartial) = (this.Name, this.Statement, this.IsPartial);

    public void Deconstruct(out string name, out Language language, out string statement, out bool isPartial) =>
        (name, language, statement, isPartial) = (this.Name, this.Language, this.Statement, this.IsPartial);

    public void Deconstruct(out string name, out Language language, out string statement, out bool isPartial, out string fileName) =>
        (name, language, statement, isPartial, fileName) = (this.Name, this.Language, this.Statement, this.IsPartial, this.FileName);

    public virtual bool Equals(Code? other) =>
        this.GetHashCode() == other?.GetHashCode();

    public bool Equals(string name) =>
        this.Name == name;

    public override bool Equals(object? obj) =>
        this.Equals(obj as Code);

    public override int GetHashCode() =>
        HashCode.Combine(this.Name, this.Language);

    public override string ToString() =>
        this.Name;

    private static string GenerateFileName(string name, Language language, bool isPartial) =>
        Path.ChangeExtension(isPartial ? $"{name}.partial.tmp" : name, language.FileExtension);

    private string GetDebuggerDisplay() =>
        this.Name;
}

public static class CodeStatementHelpers
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this Code? code) =>
        code is null || code.Equals(Code.Empty);

    public static Codes ToCodes(this IEnumerable<Code> codes) =>
        new(codes);

    public static Codes GatherAll(this IEnumerable<Codes> codes) =>
        new(codes.SelectAll());

    public static Code WithStatement(this Code code, [DisallowNull] string statement) =>
        new(code) { Statement = statement };

}