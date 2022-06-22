namespace Library.Data.SqlServer.Dynamics;

public class StoredProcedureParam : SqlObject<StoredProcedureParam, StoredProcedure>
{
    public StoredProcedureParam(StoredProcedure owner, string name, string connectionString)
        : base(owner, name, connectionString: connectionString)
    {
    }

    public string DefaultValue { get; set; }
    public long Id { get; set; }
    public int Length { get; set; }
    public int NumericPrecision { get; set; }
    public string SqlDataType { get; set; }
}
