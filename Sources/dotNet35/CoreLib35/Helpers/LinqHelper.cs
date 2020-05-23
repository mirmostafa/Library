#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using Library35.ExceptionHandlingPattern;

namespace Library35.Helpers
{
	public static class LinqHelper
	{
		// Fields
		private static ExceptionHandling<Exception> _ExceptionHandling;

		public static ExceptionHandling<Exception> ExceptionHandling
		{
			get { return _ExceptionHandling ?? (_ExceptionHandling = new ExceptionHandling<Exception>()); }
			set { _ExceptionHandling = value; }
		}

		// Methods
		public static SqlDataReader AsDataReader(this DataContext db, IQueryable query)
		{
			return query.AsDataReader(db);
		}

		public static SqlDataReader AsDataReader(this IQueryable query, DataContext db)
		{
			using (var command = query.AsSqlCommand(db))
			{
				command.Connection.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
		}

		public static SqlCommand AsSqlCommand(this IQueryable query, DataContext db)
		{
			return (db.GetCommand(query) as SqlCommand);
		}

		public static bool CanRequestNotifications()
		{
			try
			{
				new SqlClientPermission(PermissionState.Unrestricted).Demand();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static void DeleteEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true) where TEntity : class
		{
			ExceptionHandling.Reset();
			try
			{
				try
				{
					db.GetTable<TEntity>().Attach(entity, false);
				}
				catch
				{
				}
				db.Refresh(RefreshMode.KeepCurrentValues, entity);
				db.GetTable<TEntity>().DeleteOnSubmit(entity);
				if (submitChanges)
					db.SubmitChanges();
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleException(ex);
			}
		}

		public static Table<TEntity> GetAll<TEntity>(this DataContext db) where TEntity : class
		{
			ExceptionHandling.Reset();
			try
			{
				return db.GetTable<TEntity>();
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleException(ex);
				return null;
			}
		}

		public static string GetSqlStatement(this IQueryable query, DataContext db)
		{
			return db.GetCommand(query).CommandText;
		}

		public static void InsertEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true) where TEntity : class
		{
			ExceptionHandling.Reset();
			try
			{
				db.GetTable<TEntity>().InsertOnSubmit(entity);
				if (submitChanges)
					db.SubmitChanges();
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleException(ex);
			}
		}

		public static DataTable ToDataTable(this IQueryable query, DataContext db)
		{
			var result = new DataTable();
			result.Load(query.AsDataReader(db));
			return result;
		}

		public static void UpdateEntity<TEntity>(this DataContext db, TEntity entity, bool submitChanges = true) where TEntity : class
		{
			ExceptionHandling.Reset();
			try
			{
				try
				{
					db.GetTable<TEntity>().Attach(entity);
				}
				catch
				{
				}
				db.Refresh(RefreshMode.KeepCurrentValues, entity);
				if (submitChanges)
					db.SubmitChanges();
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleException(ex);
			}
		}

		// Properties
	}
}