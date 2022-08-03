using Library.Data.SqlServer.Dynamics;
using Library.Helpers;
using Library.Validations;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        Write("Please paste Connection String: ");
        var cs = ReadLine();
        var db = Database.GetDatabase(cs).NotNull();
        _ = db.Tables.ForEachEager(WriteLine);
    }
}