using System.Text;

using Library.Data.SqlServer.Dynamics;
using Library.Helpers;
using Library.ProgressiveOperations;
using Library.Validations;

namespace ConAppTest;

[Obsolete("Not done yet.", true)]
public class DatabaseScriptGenerator
{
    private readonly Database _database;

    public DatabaseScriptGenerator(string connectionString, string? databaseName = null)
        => this._database = Database.GetDatabase(connectionString, databaseName).NotNull();

    public async Task<string> GenerateDatabaseSchemaAsync(IEnumerable<string>? objects, bool generateCreateScript = true, bool generateDropScript = false, bool useGo = true, IMultistepProgress? progress = null)
    {
        await Task.CompletedTask;

        StringBuilder result = new();
        Action addGo = useGo ? () => result.AppendLine("GO") : () => { };

        progress?.Report("Initializing");
        initializeDatabase();

        progress?.Report("Getting the list of objects from Database.", 1, 0);
        var dbItems = getDatabaseObjects();

        progress?.Report("Generating.", 1, 1);
        var count = dbItems.Count;
        generate();

        return result.ToString();

        void initializeDatabase()
        {
            var dbOptions = new[]
            {
                "ANSI_NULL_DEFAULT OFF",
                "ANSI_NULLS OFF",
                "ANSI_PADDING OFF",
                "ANSI_WARNINGS OFF",
                "ARITHABORT OFF",
                "AUTO_CLOSE ON",
                "AUTO_SHRINK OFF",
                "AUTO_UPDATE_STATISTICS ON",
                "CURSOR_CLOSE_ON_COMMIT OFF",
                "CURSOR_DEFAULT  GLOBAL",
                "CONCAT_NULL_YIELDS_NULL OFF",
                "NUMERIC_ROUNDABORT OFF",
                "QUOTED_IDENTIFIER OFF",
                "RECURSIVE_TRIGGERS OFF",
                "DISABLE_BROKER",
                "AUTO_UPDATE_STATISTICS_ASYNC OFF",
                "DATE_CORRELATION_OPTIMIZATION OFF",
                "TRUSTWORTHY OFF",
                "ALLOW_SNAPSHOT_ISOLATION OFF",
                "PARAMETERIZATION SIMPLE",
                "READ_COMMITTED_SNAPSHOT OFF",
                "HONOR_BROKER_PRIORITY OFF",
                "RECOVERY FULL",
                " MULTI_USER",
                "PAGE_VERIFY CHECKSUM",
                "DB_CHAINING OFF",
                "FILESTREAM( NON_TRANSACTED_ACCESS = OFF )",
                "TARGET_RECOVERY_TIME = 60 SECONDS",
                "DELAYED_DURABILITY = DISABLED",
                "ACCELERATED_DATABASE_RECOVERY = OFF",
                "QUERY_STORE = OFF",
            };

            _ = result.AppendLine("useGo");
            addGo();
            _ = result.AppendLine($"CREATE DATABASE [{this._database.Name}]");
            _ = dbOptions.ForEachEager(x => result.AppendLine($"ALTER DATABASE [{this._database.Name}] SET {x}"));
            addGo();
            _ = result.AppendLine($"USE [{this._database.Name}]");
            addGo();
        }

        IReadOnlyList<Table> getDatabaseObjects()
        {
            var dbObjects = this._database.Tables.AsEnumerable();
            if (objects is not null)
            {
                dbObjects = dbObjects.Where(x => x.Name.IsInRange(true, objects.ToArray()));
            }
            var dbItems = dbObjects.ToReadOnlyList();
            return dbItems;
        }

        void generate()
        {
            if (dbItems.Any())
            {
                for (var index = 0; index < dbItems.Count; index++)
                {
                    var dbObject = dbItems[index];
                }
            }
        }
    }
}