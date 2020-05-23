using System;
using System.ComponentModel;

namespace Mohammad.Data.SqlServer.ObjectModel
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SqlObjectAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public sealed class SqlTableAttribute : SqlObjectAttribute {}

    public sealed class SqlFieldAttribute : SqlObjectAttribute
    {
        public bool IsIdentity { get; set; }
    }

    public sealed class SqlIgnoreAttribute : Attribute {}

    public sealed class SqlAutoIcreamentalAttribute : Attribute {}
}