using System.CodeDom;
using Library.Coding.Models;

namespace Library.CodeGeneration.Models
{
    public interface INameSpace
    {
        public string FullName { get; set; }
        public IList<ICodeGenType> CodeGenTypes { get; }
        public IList<string> UsingNameSpaces { get; }
    }

    public interface ICodeGenType : ISupportPartial
    {
        string Name { get; set; }
        string NameSpace { get; set; }
        IList<string> UsingNameSpaces { get; }
        IList<string> BaseTypes { get; }
        IList<IMemberInfo> Members { get; }
        MemberAttributes? AccessModifier { get; set; }
        InhertaceKind InhertaceKind { get; set; }
    }
    public interface IMemberInfo
    {
        string Name { get; }
    }
    public interface ISupportPartial
    {
        public bool IsPartial { get; }
    }
    public interface ISupportComment
    {
        public string Comment { get; }
    }
    //public enum AccessModifier
    //{
    //    None,
    //    Private,
    //    Protected,
    //    Internal,
    //    InternalProtected,
    //    Public
    //}

    public enum InhertaceKind
    {
        None,
        Abstract,
        Sealed,
        New
    }

    public struct FieldInfo : IMemberInfo, ISupportPartial, IEquatable<FieldInfo>
    {
        public FieldInfo(
            in string type,
            in string name,
            in string? comment = null,
            in MemberAttributes? accessModifier = null,
            in bool isReadOnly = false,
            in bool isPartial = false)
        {
            this.Type = type;
            this.Name = name;
            this.Comment = comment;
            this.AccessModifier = accessModifier;
            this.IsReadOnly = isReadOnly;
            this.IsPartial = isPartial;
        }

        public string Type { get; init; }
        public string Name { get; init; }
        public string? Comment { get; init; }
        public MemberAttributes? AccessModifier { get; init; }
        public bool IsReadOnly { get; init; }
        public bool IsPartial { get; init; }

        public override bool Equals(object? obj) => obj is FieldInfo info && this.Equals(info);
        public bool Equals(FieldInfo other) => this.Name == other.Name;
        public override int GetHashCode() => HashCode.Combine(this.Name);

        public static bool operator ==(FieldInfo left, FieldInfo right) => left.Equals(right);
        public static bool operator !=(FieldInfo left, FieldInfo right) => !(left == right);
    }

    public struct PropertyInfo : IMemberInfo, IEquatable<PropertyInfo>
    {
        public PropertyInfo(
            in string type,
            in string name,
            in MemberAttributes? accessModifier = null,
            in PropertyAccessor? getter = null,
            in PropertyAccessor? setter = null)
        {
            this.Name = name;
            this.Type = type;
            this.AccessModifier = accessModifier;
            this.Getter = getter;
            this.Setter = setter;
            this.HasBackingField = false;
            this.Value = null;
            this.Comment = null;
            this.InitCode = null;
            this.IsNullable = false;
            this.BackingFieldName = null;
            this.ShouldCreateBackingField = null;
            this.Attributes = Enumerable.Empty<string>();
        }

        public MemberAttributes? AccessModifier { get; init; }
        public string Type { get; init; }
        public bool IsNullable { get; init; }
        public string Name { get; init; }
        public object? Value { get; set; }

        public IEnumerable<string>? Attributes { get; init; }
        public PropertyAccessor? Setter { get; init; }
        public PropertyAccessor? Getter { get; init; }
        public string? InitCode { get; init; }

        public bool HasBackingField { get; init; }
        public string? BackingFieldName { get; init; }
        public bool? ShouldCreateBackingField { get; init; }

        public string? Comment { get; init; }

        public override bool Equals(object? obj) => obj is PropertyInfo info && this.Equals(info);
        public bool Equals(PropertyInfo other) => this.Name == other.Name;
        public override int GetHashCode() => HashCode.Combine(this.Name);

        public static bool operator ==(PropertyInfo left, PropertyInfo right) => left.Equals(right);
        public static bool operator !=(PropertyInfo left, PropertyInfo right) => !(left == right);
    }

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

    public interface ICodeGenProvider
    {
        Codes GenerateBehindCode(in INameSpace nameSpace, in GenerateCodesParameters arguments);
    }

    public sealed record GenerateCodeResult(in Code? Main, in Code? Partial);
    public sealed record MethodArgument(in string Type, in string Name);
    public sealed record GenerateCodesParameters(in bool GenerateMainCode = false, in bool GeneratePartialCode = true, bool GenerateUiCode = false);

    public sealed record ConstrcutorArgument(string Type, string Name, string? DataMemberName = null, bool IsPropery = false);
    public sealed record PropertyAccessor(bool Has = true, bool? IsPrivate = null, string? Code = null);
}
