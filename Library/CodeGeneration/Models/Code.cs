﻿using System.Diagnostics;
using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public record struct Code : IEquatable<Code>
{
    public static readonly Code Default = new("Default", Languages.None, "(No Code)");

    public Code(in string name, in Language language, in string statement, in bool isPartial = false)
    {
        this.Name = name.ArgumentNotNull(nameof(name));
        this.Statement = statement.ArgumentNotNull(nameof(statement));
        this.Language = language;
        this.IsPartial = isPartial;
        this._FileName = null;
    }

    public Code(in (string Name, Language language, string Statement, bool IsPartial) data)
        : this(data.Name, data.language, data.Statement, data.IsPartial) { }

    public void Deconstruct(out string name, out string statement)
        => (name, statement) = (this.Name, this.Statement);

    public void Deconstruct(out string name, out Language language, out string statement)
        => (name, language, statement) = (this.Name, this.Language, this.Statement);

    public void Deconstruct(out string name, out string statement, out bool isPartial)
        => (name, statement, isPartial) = (this.Name, this.Statement, this.IsPartial);

    public void Deconstruct(out string name, out Language language, out string statement, out bool isPartial)
       => (name, language, statement, isPartial) = (this.Name, this.Language, this.Statement, this.IsPartial);

    private static string GenerateFileName(string name, Language language, bool isPartial)
        => Path.ChangeExtension(isPartial ? $"{name}.partial.tmp" : name, language.FileExtension);

    private string? _FileName { get; init; }
    public string FileName => this._FileName.IfNullOrEmpty(GenerateFileName(this.Name, this.Language, this.IsPartial));
    public string Name { get; init; }
    public string Statement { get; init; }
    public Language Language { get; init; }
    public bool IsPartial { get; init; }

    public override int GetHashCode()
        => HashCode.Combine(this.Name);
    public bool Equals(Code other)
        => this.Name == other.Name;
    public bool Equals(string name)
        => this.Name == name;

    public static implicit operator string(in Code code)
        => code.Statement;
    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial) codeData)
        => new(codeData);
    public static implicit operator Code(in (string Name, Language language, string Statement, bool IsPartial, string FileName) codeData)
        => new(codeData.Name, codeData.language, codeData.Statement, codeData.IsPartial) { _FileName = codeData.FileName };

    public static bool operator ==(Code left, string code)
        => left.Equals(code);
    public static bool operator !=(Code left, string code)
        => !(left == code);

    private string GetDebuggerDisplay()
        => this.Name;

    public override string ToString()
        => this.Name;

    public static Code ToCode(in (string Name, Language language, string Statement, bool IsPartial) codeData)
        => codeData;
    public static Code ToCode(in string name, in Language language, in string statement, in bool isPartial)
        => new(name, language, statement, isPartial);
    public static Code ToCode(in string name, in Language language, in string statement, in bool isPartial, in string? fileName)
        => new(name, language, statement, isPartial) { _FileName = fileName };
}

public static class CodeHelpers
{
    public static Codes ToCodes(this IEnumerable<Code> codes)
        => new(codes);
}
