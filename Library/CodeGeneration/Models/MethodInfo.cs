using System.CodeDom;

namespace Library.CodeGeneration.Models;

public struct MethodInfo : IMemberInfo, IEquatable<MethodInfo>
{
    public MethodInfo(
        in string name,
        in string? returnType = null,
        in MemberAttributes? accessModifier = null,
        in IEnumerable<MethodArgument>? arguments = null,
        in bool isPartial = false)
    {
        this.Name = name;
        this.ReturnType = returnType;
        this.AccessModifier = accessModifier;
        this.Arguments = arguments;
        this.IsPartial = isPartial;
        this.Body = null;
    }
    public string? Body { get; set; }
    public string Name { get; }
    public string? ReturnType { get; }
    public MemberAttributes? AccessModifier { get; }
    public IEnumerable<MethodArgument>? Arguments { get; }
    public bool IsPartial { get; }

    public override bool Equals(object? obj) => obj is MethodInfo info && this.Equals(info);
    public bool Equals(MethodInfo other)
        => this.Name == other.Name && EqualityComparer<IEnumerable<MethodArgument>?>.Default.Equals(this.Arguments, other.Arguments);
    public override int GetHashCode() => HashCode.Combine(this.Name, this.Arguments);

    public static bool operator ==(MethodInfo left, MethodInfo right) => left.Equals(right);
    public static bool operator !=(MethodInfo left, MethodInfo right) => !(left == right);
}
