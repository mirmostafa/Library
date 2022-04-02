namespace Library.Data.SqlServer.Dynamics.Collections;

public class StoredProcedures : SqlObjects<StoredProcedure>
{
    internal StoredProcedures(IEnumerable<StoredProcedure> items)
        : base(items)
    {
    }
}
