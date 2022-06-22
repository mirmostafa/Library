namespace Library.Data.SqlServer.Dynamics.Collections;

public class StoredProcedureParams : SqlObjects<StoredProcedureParam>
{
    internal StoredProcedureParams(IEnumerable<StoredProcedureParam> items)
        : base(items)
    {
    }
}
