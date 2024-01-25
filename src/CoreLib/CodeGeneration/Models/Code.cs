﻿using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Dynamic;
using Library.Interfaces;
using Library.Results;
using Library.Validations;

namespace Library.CodeGeneration.Models;

[Fluent]
[Immutable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class Code([DisallowNull] in string name, [DisallowNull] in Language language, [DisallowNull] in string statement, in bool isPartial = false, in string? fileName = null) :
    IEquatable<Code>,
    IEmpty<Code>,
    IComparable<Code>
{
    private static Code? _empty;

    /// <summary>
    /// Constructor for the <see cref="Code"/> with parameters.
    /// </summary>
    /// <param name="name">Name of the code.</param>
    /// <param name="language">Language of the code.</param>
    /// <param name="statement">Statement of the code.</param>
    /// <param name="isPartial">Whether the code is partial or not.</param>
    /// <param name="fileName">File name of the code.</param>
    /// <returns>An instance of the <see cref="Code"/>.</returns>
    public Code(in (string Name, Language Language, string Statement, bool IsPartial) data)
        : this(data.Name, data.Language, data.Statement, data.IsPartial) { }

    /// <summary>
    /// Constructor for the <see cref="Code"/> with parameters.
    /// </summary>
    /// <param name="name">Name of the code.</param>
    /// <param name="language">Language of the code.</param>
    /// <param name="statement">Statement of the code.</param>
    /// <param name="isPartial">Whether the code is partial or not.</param>
    /// <param name="fileName">File name of the code.</param>
    /// <returns>An instance of the <see cref="Code"/>.</returns>
    public Code(in (string Name, Language Language, string Statement, bool IsPartial, string? FileName) data)
        : this(data.Name, data.Language, data.Statement, data.IsPartial, data.FileName) { }

    /// <summary>
    /// Constructor for the <see cref="Code"/> with a copy of the original Code instance.
    /// </summary>
    /// <param name="original">Original Code instance to copy.</param>
    public Code(Code original)
        : this(original.Name, original.Language, original.Statement, original.IsPartial, original.FileName)
    {
    }

    /// <summary>
    /// Represents an empty instance of <see cref="Code"/> class.
    /// </summary>
    public static Code Empty { get; } = _empty ??= new(string.Empty, Languages.None, string.Empty);

    public dynamic ExtraProperties { get; } = new Expando();

    /// <summary>
    /// Gets the file name of the code. If the file name is null or empty, generate a new file name
    /// based on the code's name, language, and partial status.
    /// </summary>
    public string FileName => this._FileName.IfNullOrEmpty(GenerateFileName(this.Name, this.Language, this.IsPartial));

    /// <summary>
    /// Gets whether the code is partial or not.
    /// </summary>
    public bool IsPartial { get; } = isPartial;

    /// <summary>
    /// Gets the language of the code.
    /// </summary>
    public Language Language { get; } = language;

    /// <summary>
    /// Gets the name of the code.
    /// </summary>
    public string Name { get; } = name.ArgumentNotNull();

    /// <summary>
    /// Gets or sets the statement of the code.
    /// </summary>
    public string Statement { get; init; } = statement.ArgumentNotNull();

    private string? _FileName { get; init; } = fileName;

    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial) codeData) =>
        new(codeData);

    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial, string FileName) codeData) =>
        new(codeData.Name, codeData.language, codeData.Statement, codeData.IsPartial, codeData.FileName);

    public static implicit operator string?(in Code? code) =>
        code?.Statement;

    public static Code New(in string name, in Language language, in string statement, in bool isPartial = false, in string? fileName = null) =>
        new(name, language, statement, isPartial, fileName);

    public static Code NewEmpty() =>
            new(string.Empty, Languages.None, string.Empty);

    public static bool operator !=(Code? left, Code? right) =>
        !(left == right);

    public static bool operator <(Code left, Code right) => left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(Code left, Code right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator ==(Code? left, Code? right) =>
                left?.Equals(right) ?? (right is null);

    public static bool operator >(Code left, Code right) => left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(Code left, Code right) => left is null ? right is null : left.CompareTo(right) >= 0;

    public int CompareTo(Code? other) =>
        other is null ? 1 : other.Name.CompareTo(this.Name);

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

public static class SourceCodeHelpers
{
    public static Codes GatherAll(this IEnumerable<Codes> codes) =>
        new(codes.SelectAll());

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Code? code) =>
        code is null || code.Equals(Code.Empty);

    public static Codes ToCodes(this IEnumerable<Code> codes) =>
        new(codes);

    public static Codes ToCodes(this IEnumerable<Codes> codes) =>
         new(codes);

    [return: NotNull]
    public static Codes ToCodes(this Code code) =>
        new(EnumerableHelper.ToEnumerable(code));

    public static Result<Codes> ToCodesResult(this Result<Code> code) =>
        Result<Codes>.From(code.ArgumentNotNull(), code.Value.ToCodes());

    public static Code WithStatement(this Code code, [DisallowNull] string statement) =>
        new(code) { Statement = statement };
}