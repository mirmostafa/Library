#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using Library40.Data.Internals.BusinessTools;
using Library40.Helpers;

namespace Library40.Data.EntityFramework.BusinessTools
{
	public abstract class BusinessEntityOnEf<TEntity, TDateContext> : BusinessEntity<TEntity>, IDisposable
		where TEntity : EntityObject, new()
		where TDateContext : ObjectContext
	{
		protected abstract TDateContext DataContext { get; }

		#region IDisposable Members
		public void Dispose()
		{
			var dataContext = this.DataContext;
			if (dataContext != null)
				dataContext.Dispose();
		}
		#endregion

		protected override void DeleteCore(TEntity entity, bool submitChanges)
		{
			this.DataContext.DeleteEntity(entity, submitChanges);
		}

		protected override TEntity FillCore(TEntity entity)
		{
			throw new NotImplementedException();
		}

		protected override TEntity GetNewCore()
		{
			return Activator.CreateInstance<TEntity>();
		}

		protected override void InsertCore(TEntity entity, bool submitChanges)
		{
			this.DataContext.InsertEntity(entity, submitChanges);
		}

		protected override IEnumerable<TEntity> SelectCore()
		{
			return this.DataContext.GetAll<TEntity>();
		}

		protected override void SaveChangesCore()
		{
			this.DataContext.SaveChanges();
		}

		protected override void UpdateCore(TEntity entity, bool submitChanges)
		{
			this.DataContext.UpdateEntity(entity, submitChanges);
		}
	}
}