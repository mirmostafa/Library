﻿using System.Diagnostics;

using Library.DesignPatterns.Markers;
using Library.Validations;

namespace Library.CodeGeneration.Models;

public interface ICode<TCode> : IEquatable<TCode>
    where TCode : ICode<TCode>
{
    string FileName { get; }
    string Name { get; init; }
    string Statement { get; init; }

    void Deconstruct(out string name, out string statement);

    void Deconstruct(out string name, out string statement, out bool isPartial);
    void Deconstruct(out string name, out Language language, out string statement, out bool isPartial);
    void Deconstruct(out string name, out string statement);
    void Deconstruct(out string name, out Language language, out string statement);
    void Deconstruct(out string name, out string statement, out bool isPartial);
    void Deconstruct(out string name, out Language language, out string statement, out bool isPartial);
    string ToString();
}

[Immutable]
[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public readonly record struct Code : IEquatable<Code>, ICode<Code>
{
    public static readonly Code Empty = new(string.Empty, Languages.None, string.Empty);

    public Code(in string name, in Language language, in string statement, in bool isPartial = false)
    {
        this.Name = name.As<object>().ArgumentNotNull(nameof(name)).ToString()!;
        this.Statement = statement.As<object>().ArgumentNotNull(nameof(statement)).ToString()!;
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
    public string Name { get; init; }
    public string Statement { get; init; }
    public Language Language { get; init; }
    public bool IsPartial { get; init; }
    public string FileName => this._FileName.IfNullOrEmpty(GenerateFileName(this.Name, this.Language, this.IsPartial));

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

public static class CodeStatementHelpers
{
    public static Codes ToCodes(this IEnumerable<Code> codes)
        => new(codes);
}

public readonly record struct BlazorCode : ICode<BlazorCode>
{
    public string FileName { get; }
    public string Name { get; init; }
    public string Statement { get; init; }

    public void Deconstruct(out string name, out string statement) => throw new NotImplementedException();
    public void Deconstruct(out string name, out string statement, out bool isPartial) => throw new NotImplementedException();
    public void Deconstruct(out string name, out Language language, out string statement, out bool isPartial) => throw new NotImplementedException();
    public void Deconstruct(out string name, out Language language, out string statement) => throw new NotImplementedException();
}