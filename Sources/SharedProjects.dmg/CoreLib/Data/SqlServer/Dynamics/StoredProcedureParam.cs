namespace Mohammad.Data.SqlServer.Dynamics
{
    public class StoredProcedureParam : SqlObject<StoredProcedureParam, StoredProcedure>
    {
        public string DefaultValue { get; set; }
        public long Id { get; set; }
        public int Length { get; set; }
        public int NumericPrecision { get; set; }
        public string SqlDataType { get; set; }

        public StoredProcedureParam(StoredProcedure owner, string name, string connectionstring)
            : base(owner, name, connectionstring) { }
    }
}