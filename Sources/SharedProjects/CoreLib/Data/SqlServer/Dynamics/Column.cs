using System;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Column : SqlObject<Column, Table>, IEquatable<Column>
    {
        public bool IsIdentity { get; set; }
        public bool IsForeignKey { get; set; }
        public ForeignKeyInfo ForeignKeyInfo { get; set; }
        public string CollationName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public int MaxLength { get; set; }
        public int Position { get; set; }
        public int Precision { get; set; }

        public Column(Table owner, string name, string connectionstring)
            : base(owner, name, connectionstring) { }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((Column) obj);
        }

        public override int GetHashCode() { return this.Name?.GetHashCode() ?? 0; }
        public static bool operator ==(Column left, Column right) { return Equals(left, right); }
        public static bool operator !=(Column left, Column right) { return !Equals(left, right); }

        public bool Equals(Column other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || string.Equals(this.Name, other.Name);
        }
    }
}