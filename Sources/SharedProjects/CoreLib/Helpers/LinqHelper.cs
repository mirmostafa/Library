using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using static Mohammad.Helpers.CodeHelper;


namespace Mohammad.Helpers
{
    public static class LinqHelper
    {
        public static ConflictMode? DefaultConflictMode = null;
        public static SqlDataReader AsDataReader(this DataContext db, IQueryable query) => query.AsDataReader(db);

        public static SqlDataReader AsDataReader(this IQueryable query, DataContext db)
        {
            using (var command = query.AsSqlCommand(db))
            {
                command.Connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static SqlCommand AsSqlCommand(this IQueryable query, DataContext db) => db.GetCommand(query) as SqlCommand;

        public static bool CanRequestNotifications() => HasException(() => new SqlClientPermission(PermissionState.Unrestricted).Demand());

        public static void DeleteEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            Catch(() =>
                {
                    Catch(() => db.GetTable<TEntity>().Attach(entity, false));
                    db.Refresh(RefreshMode.KeepCurrentValues, entity);
                    db.GetTable<TEntity>().DeleteOnSubmit(entity);
                    if (submitChanges)
                        db.SubmitChanges();
                },
                handling: exceptionHandling,
                throwException: true);
        }

        public static Table<TEntity> GetAll<TEntity>(this DataContext db, ExceptionHandling exceptionHandling)
            where TEntity : class => CatchFunc(db.GetTable<TEntity>, handling: exceptionHandling, throwException: true);

        public static string GetSqlStatement(this IQueryable query, DataContext db) => db.GetCommand(query).CommandText;

        public static void InsertEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            Catch(() =>
                {
                    db.GetTable<TEntity>().InsertOnSubmit(entity);
                    if (submitChanges)
                        db.SaveChanges();
                },
                handling: exceptionHandling,
                throwException: true);
        }

        private static void SaveChanges(this DataContext db, ConflictMode? failureMode = null)
        {
            if (failureMode.HasValue)
                db.SubmitChanges(failureMode.Value);
            else
            {
                if (DefaultConflictMode.HasValue)
                    db.SubmitChanges(DefaultConflictMode.Value);
                else
                    db.SubmitChanges();
            }
        }

        public static DataTable ToDataTable(this IQueryable query, DataContext db)
        {
            var result = new DataTable();
            result.Load(query.AsDataReader(db));
            return result;
        }

        public static void UpdateEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            lock (db)
            {
                Catch(() => Attach(db, entity));
                Catch(() =>
                    {
                        db.Refresh(RefreshMode.KeepCurrentValues, entity);
                        if (submitChanges)
                            db.SubmitChanges();
                    },
                    handling: exceptionHandling,
                    throwException: true);
            }
        }

        public static void Attach<TEntity>(this DataContext db, TEntity entity, bool asModified = true)
            where TEntity : class
        {
            db.GetTable<TEntity>().Attach(entity, asModified);
        }

        public static void Attach<TEntity>(this DataContext db, TEntity entity, TEntity original)
            where TEntity : class
        {
            db.GetTable<TEntity>().Attach(entity, original);
        }
    }
}