using System.CodeDom;

namespace Library.CodeGeneration.Models;

public struct MethodInfo(
    in string name,
    in string? returnType = null,
    in MemberAttributes? accessModifier = null,
    in IEnumerable<MethodArgument>? arguments = null,
    in bool isPartial = false) : IMemberInfo, IEquatable<MethodInfo>
{
    public MemberAttributes? AccessModifier { get; } = accessModifier;
    public IEnumerable<MethodArgument>? Arguments { get; } = arguments;
    public string? Body { get; set; } = null;
    public bool IsPartial { get; } = isPartial;
    public string Name { get; } = name;
    public string? ReturnType { get; } = returnType;

    public static bool operator !=(MethodInfo left, MethodInfo right) => !(left == right);

    public static bool operator ==(MethodInfo left, MethodInfo right) => left.Equals(right);

    public override bool Equals(object? obj)
        => obj is MethodInfo info && this.Equals(info);

    public readonly bool Equals(MethodInfo other)
        => this.Name == other.Name && EqualityComparer<IEnumerable<MethodArgument>?>.Default.Equals(this.Arguments, other.Arguments);

    public override readonly int GetHashCode()
        => HashCode.Combine(this.Name, this.Arguments);
}