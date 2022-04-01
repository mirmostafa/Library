namespace Library.Data.SqlServer.Dynamics.EventsArgs;

public class GettingItemsEventArgs<TSqlObject> : EventArgs
{
    public IEnumerable<TSqlObject> Items { get; set; }
}
