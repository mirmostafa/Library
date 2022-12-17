using System.CodeDom;

namespace Library.CodeGeneration.Models;

public record struct FieldInfo(
        in string Type,
        in string Name,
        in string? Comment = null,
        in MemberAttributes? AccessModifier = null,
        in bool IsReadOnly = false,
        in bool IsPartial = false) : IMemberInfo, ISupportPartial;

