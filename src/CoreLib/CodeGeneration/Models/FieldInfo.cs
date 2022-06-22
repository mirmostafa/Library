using System.CodeDom;

namespace Library.CodeGeneration.Models;

//! Old way !
////public readonly struct FieldInfo : IMemberInfo, ISupportPartial, IEquatable<FieldInfo>
////{
////    public FieldInfo(
////        in string type,
////        in string name,
////        in string? comment = null,
////        in MemberAttributes? accessModifier = null,
////        in bool isReadOnly = false,
////        in bool isPartial = false)
////    {
////        this.Type = type;
////        this.Name = name;
////        this.Comment = comment;
////        this.AccessModifier = accessModifier;
////        this.IsReadOnly = isReadOnly;
////        this.IsPartial = isPartial;
////    }

////    public string Type { get; init; }
////    public string Name { get; init; }
////    public string? Comment { get; init; }
////    public MemberAttributes? AccessModifier { get; init; }
////    public bool IsReadOnly { get; init; }
////    public bool IsPartial { get; init; }

////    public override bool Equals(object? obj) => obj is FieldInfo info && this.Equals(info);
////    public bool Equals(FieldInfo other) => this.Name == other.Name;
////    public override int GetHashCode() => HashCode.Combine(this.Name);

////    public static bool operator ==(FieldInfo left, FieldInfo right) => left.Equals(right);
////    public static bool operator !=(FieldInfo left, FieldInfo right) => !(left == right);
////}
public record struct FieldInfo(
        in string Type,
        in string Name,
        in string? Comment = null,
        in MemberAttributes? AccessModifier = null,
        in bool IsReadOnly = false,
        in bool IsPartial = false) : IMemberInfo, ISupportPartial;

