using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Immutable]
public readonly struct ConstructorArgument(in TypePath type, in string name, in string? dataMemberName = null, in bool isProperty = false)
{
    public string? DataMemberName { get; } = dataMemberName;
    public bool IsProperty { get; } = isProperty;
    public string Name { get; } = name;
    public TypePath Type { get; } = type;
}

[Immutable]
public readonly struct GenerateCodeResult(in Code? main, in Code? partial)
{
    public Code? Main { get; } = main;
    public Code? Partial { get; } = partial;

    public void Deconstruct(out Code? main, out Code? partial)
        => (main, partial) = (this.Main, this.Partial);
}

/// <summary>
/// This class is used to hold parameters for code generation.
/// </summary>
[Immutable]
public record GenerateCodesParameters(
    /// <summary>
    /// A flag indicating whether to generate the main code.
    /// </summary>
    in bool GenerateMainCode = true,

    /// <summary>
    /// A flag indicating whether to generate the partial code.
    /// </summary>
    in bool GeneratePartialCode = true,

    /// <summary>
    /// A flag indicating whether to generate the UI code.
    /// </summary>
    in bool GenerateUiCode = true,

    /// <summary>
    /// The name of the backend file.
    /// </summary>
    in string? BackendFileName = null,

    /// <summary>
    /// The name of the frontend file.
    /// </summary>
    in string? FrontFileName = null)
{
    /// <summary>
    /// Copy constructor. Creates a new instance of GenerateCodesParameters with the same values as the original.
    /// </summary>
    public GenerateCodesParameters(GenerateCodesParameters original) =>
        (GenerateMainCode, GeneratePartialCode, GenerateUiCode, BackendFileName, FrontFileName) = original;

    /// <summary>
    /// Factory method to create a new instance of GenerateCodesParameters with all flags set to true.
    /// </summary>
    public static GenerateCodesParameters FullCode() =>
        new(true, true, true);

    /// <summary>
    /// Deconstructs the object into its full parameters.
    /// </summary>
    public void Deconstruct(out bool generateMainCode, out bool generatePartialCode, out bool generateUiCode, out string? backendFileName, out string? frontFileName) =>
        (generateMainCode, generatePartialCode, generateUiCode, backendFileName, frontFileName) = (this.GenerateMainCode, this.GeneratePartialCode, this.GenerateUiCode, this.BackendFileName, this.FrontFileName);

    /// <summary>
    /// Deconstructs the object into its code generation parameters.
    /// </summary>
    public void Deconstruct(out bool generateMainCode, out bool generatePartialCode, out bool generateUiCode) =>
        (generateMainCode, generatePartialCode, generateUiCode) = (this.GenerateMainCode, this.GeneratePartialCode, this.GenerateUiCode);

    /// <summary>
    /// Deconstructs the object into its file name parameters.
    /// </summary>
    public void Deconstruct(out string? backendFileName, out string? frontFileName) =>
        (backendFileName, frontFileName) = (this.BackendFileName, this.FrontFileName);
}


[Immutable]
public readonly struct MethodArgument(in TypePath type, in string name) : IEquatable<MethodArgument>
{
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public static bool operator !=(MethodArgument left, MethodArgument right)
        => !(left == right);

    public static bool operator ==(MethodArgument left, MethodArgument right)
        => left.Equals(right);

    public void Deconstruct(out TypePath type, out string name)
        => (type, name) = (this.Type, this.Name);

    public readonly bool Equals(MethodArgument other)
            => EqualityComparer<TypePath>.Default.Equals(this.Type, other.Type) && EqualityComparer<string>.Default.Equals(this.Name, other.Name);

    public override bool Equals(object? obj)
        => obj is MethodArgument argument && this.Equals(argument);

    public override int GetHashCode()
        => HashCode.Combine(this.Name.GetHashCode(), this.Type.GetHashCode());
}

[Immutable]
public readonly struct PropertyAccessor(in bool has = true, in bool? isPrivate = null, in string? code = null)
{
    public string? Code { get; } = code;
    public bool Has { get; } = has;
    public bool? IsPrivate { get; } = isPrivate;
}

public static class ModelsExtensions
{
    public static void Deconstruct(this MethodArgument argument, out TypePath type, out string name)
        => (type, name) = (argument.Type, argument.Name);
}