//using System.ComponentModel;

namespace Library.Data.SqlServer.ObjectModel;

//[AttributeUsage(AttributeTargets.Property)]
//public sealed class SqlFieldAttribute : SqlObjectAttribute
//{
//    public bool IsIdentity { get; set; }
//}

//[AttributeUsage(AttributeTargets.All)]
//public sealed class SqlIgnoreAttribute : Attribute
//{
//}

//[EditorBrowsable(EditorBrowsableState.Never)]
//public abstract class SqlObjectAttribute : Attribute
//{
//    public string? Name { get; set; }
//}

//[AttributeUsage(AttributeTargets.Class)]
//public sealed class SqlTableAttribute : SqlObjectAttribute
//{
//}

[AttributeUsage(AttributeTargets.Property)]
public sealed class SqlAutoIcreamentalAttribute : Attribute
{
}