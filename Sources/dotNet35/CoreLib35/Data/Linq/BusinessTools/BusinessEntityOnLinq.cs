#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Library35.Data.Common.BusinessTools;
using Library35.Data.Common.BusinessTools.EventsArgs;
using Library35.Data.Linq.BusinessTools.Internals;
using Library35.Helpers;

namespace Library35.Data.Linq.BusinessTools
{
	public abstract class BusinessEntityOnLinq<TEntity, TDateContext> : BusinessEntity<TEntity>
		where TEntity : class, new()
		where TDateContext : DataContext
	{
		// Fields
		protected readonly TDateContext Db;

		// Methods
		protected BusinessEntityOnLinq()
		{
			this.ExceptionHandling.Reset();
			try
			{
				this.Db = this.GetDb();
				this.Db.Log = new LinqLogger();
			}
			catch (Exception ex)
			{
				this.ExceptionHandling.HandleException(ex);
			}
		}

		protected override void DeleteCore(TEntity entity, bool submitChanges)
		{
			this.Db.DeleteEntity(entity, submitChanges);
		}

		public override TEntity FillCore(TEntity entity)
		{
			throw new NotImplementedException();
		}

		protected abstract TDateContext GetDb();

		public override TEntity GetNewCore()
		{
			return Activator.CreateInstance<TEntity>();
		}

		protected override void InsertCore(TEntity entity, bool submitChanges)
		{
			this.Db.InsertEntity(entity, submitChanges);
		}

		protected override IEnumerable<TEntity> SelectCore()
		{
			return this.Db.GetAll<TEntity>();
		}

		protected override void SubmitChangesCore()
		{
			Exception ex;
			var changes = this.Db.GetChangeSet();
			foreach (var entity in changes.Inserts.Cast<TEntity>())
			{
				this.ExceptionHandling.Reset();
				try
				{
					if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Insert)))
						return;
				}
				catch (Exception exception1)
				{
					ex = exception1;
					this.ExceptionHandling.HandleException(ex);
					this.OnError(new EntityActed<TEntity>(entity));
				}
			}
			foreach (var entity in changes.Updates.Cast<TEntity>())
			{
				this.ExceptionHandling.Reset();
				try
				{
					if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Update)))
						return;
				}
				catch (Exception exception2)
				{
					ex = exception2;
					this.ExceptionHandling.HandleException(ex);
					this.OnError(new EntityActed<TEntity>(entity));
				}
			}
			foreach (var entity in changes.Deletes.Cast<TEntity>())
			{
				this.ExceptionHandling.Reset();
				try
				{
					if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Delete)))
						return;
				}
				catch (Exception exception3)
				{
					ex = exception3;
					this.ExceptionHandling.HandleException(ex);
					this.OnError(new EntityActed<TEntity>(entity));
				}
			}
			this.Db.SubmitChanges();
		}

		protected override void UpdateCore(TEntity entity, bool submitChanges)
		{
			this.Db.UpdateEntity(entity, submitChanges);
		}
	}
}