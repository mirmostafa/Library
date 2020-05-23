#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Library40.Data.Internals.BusinessTools.EventsArgs;
using Library40.Helpers;

namespace Library40.Data.Internals.BusinessTools
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BusinessEntity<TEntity> : BusinessEntityBase
		where TEntity : class
	{
		// Events
		public virtual string DeletePremissionKey
		{
			get { return string.Empty; }
		}

		public virtual string InsertPremissionKey
		{
			get { return string.Empty; }
		}

		public virtual string SelectPremissionKey
		{
			get { return string.Empty; }
		}

		public virtual string UpdatePremissionKey
		{
			get { return string.Empty; }
		}

		public event EventHandler<EntityActed<TEntity>> Deleted;

		public event EventHandler<EntityActed<IEnumerable<long>>> DeletedByIds;

		public event EventHandler<EntityActing<TEntity>> Deleting;

		public event EventHandler<EntityActing<IEnumerable<long>>> DeletingByIds;

		public event EventHandler<EntityActed<TEntity>> Error;

		public event EventHandler<EntityActed<TEntity>> Filled;

		public event EventHandler<EntityActed<TEntity>> GotNew;

		public event EventHandler<EntityActed<TEntity>> Inserted;

		public event EventHandler<EntityActing<TEntity>> Inserting;

		public event EventHandler<EntityActed<TEntity>> Updated;

		public event EventHandler<EntityActing<TEntity>> Updating;

		public event EventHandler<EntityValidating<TEntity>> Validated;

		public event EventHandler<EntityValidating<IEnumerable<long>>> ValidatedByIds;

		public event EventHandler<EntityValidating<TEntity>> Validating;

		public event EventHandler<EntityValidating<IEnumerable<long>>> ValidatingByIds;

		// Methods

		protected virtual void CheckPermission(string premissionKey)
		{
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public virtual void Delete(TEntity entity)
		{
			this.Delete(entity, true);
		}

		public virtual void Delete(TEntity entity, bool submitChanges)
		{
			this.ExceptionHandling.Reset();
			this.CheckPermission(this.DeletePremissionKey);
			try
			{
				if (this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Delete)))
				{
					this.OnValidated(new EntityActed<TEntity>(entity));
					if (!this.OnDeleting(new EntityActing<TEntity>(entity)))
						this.DeleteCore(entity, submitChanges);
					this.OnDeleted(new EntityActed<TEntity>(entity));
				}
			}
			catch (Exception ex)
			{
				this.ExceptionHandling.HandleException(ex);
				this.OnError(new EntityActed<TEntity>(entity));
			}
		}

		[DataObjectMethod(DataObjectMethodType.Delete)]
		public virtual void DeleteByIds(IEnumerable<long> ids)
		{
			this.DeleteByIds(ids, true);
		}

		public virtual void DeleteByIds(IEnumerable<long> ids, bool submitChanges)
		{
			this.ExceptionHandling.Reset();
			try
			{
				if (this.OnValidatingByIds(new EntityValidatingByIds(ids, EntityAction.Delete)))
				{
					this.OnValidatedByIds(new EntityActed<IEnumerable<long>>(ids));
					if (!this.OnDeletingByIds(new EntityActing<IEnumerable<long>>(ids)))
						this.DeleteByIdsCore(ids, submitChanges);
					this.OnDeletedByIds(new EntityActed<IEnumerable<long>>(ids));
				}
			}
			catch (Exception ex)
			{
				this.ExceptionHandling.HandleException(ex);
				this.OnError(new EntityActed<IEnumerable<long>>(ids));
			}
		}

		protected abstract void DeleteByIdsCore(IEnumerable<long> ids, bool submitChanges);
		protected abstract void DeleteCore(TEntity entity, bool submitChanges);

		public TEntity Fill(TEntity entity)
		{
			this.ExceptionHandling.Reset();
			try
			{
				var result = this.FillCore(entity);
				this.OnFilled(new EntityActed<TEntity>(result));
				return result;
			}
			catch (Exception ex)
			{
				this.OnError(new EntityActed<TEntity>(entity));
				this.ExceptionHandling.HandleException(ex);
				return default(TEntity);
			}
		}

		protected abstract TEntity FillCore(TEntity entity);

		public TEntity GetNew()
		{
			this.ExceptionHandling.Reset();
			try
			{
				var result = this.GetNewCore();
				this.OnGotNew(new EntityActed<TEntity>(result));
				return result;
			}
			catch (Exception ex)
			{
				this.OnError(new EntityActed<TEntity>(default(TEntity)));
				this.ExceptionHandling.HandleException(ex);
				return default(TEntity);
			}
		}

		protected abstract TEntity GetNewCore();

		[DataObjectMethod(DataObjectMethodType.Insert)]
		public void Insert(TEntity entity)
		{
			this.Insert(entity, true);
		}

		public void Insert(TEntity entity, bool submitChanges)
		{
			this.ExceptionHandling.Reset();
			this.CheckPermission(this.InsertPremissionKey);
			try
			{
				if (this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Insert)))
				{
					this.OnValidated(new EntityActed<TEntity>(entity));
					if (!this.OnInserting(new EntityActing<TEntity>(entity)))
						this.InsertCore(entity, submitChanges);
					this.OnInserted(new EntityActed<TEntity>(entity));
				}
			}
			catch (Exception ex)
			{
				this.ExceptionHandling.HandleException(ex);
				this.OnError(new EntityActed<TEntity>(entity));
			}
		}

		protected abstract void InsertCore(TEntity entity, bool submitChanges);

		protected virtual void OnDeleted(EntityActed<TEntity> e)
		{
			this.Deleted.Raise(this, e);
		}

		protected virtual void OnDeletedByIds(EntityActed<IEnumerable<long>> e)
		{
			this.DeletedByIds.Raise(this, e);
		}

		protected virtual bool OnDeleting(EntityActing<TEntity> e)
		{
			this.Deleting.Raise(this, e);
			return e.Handled;
		}

		protected virtual bool OnDeletingByIds(EntityActing<IEnumerable<long>> e)
		{
			this.DeletingByIds.Raise(this, e);
			return e.Handled;
		}

		protected virtual void OnError(EntityActed<IEnumerable<long>> e)
		{
			this.Error.Raise(this, e);
		}

		protected virtual void OnError(EntityActed<TEntity> e)
		{
			this.Error.Raise(this, e);
		}

		protected virtual void OnFilled(EntityActed<TEntity> e)
		{
			this.Filled.Raise(this, e);
		}

		protected virtual void OnGotNew(EntityActed<TEntity> e)
		{
			this.GotNew.Raise(this, e);
		}

		protected virtual void OnInserted(EntityActed<TEntity> e)
		{
			this.Inserted.Raise(this, e);
		}

		protected virtual bool OnInserting(EntityActing<TEntity> e)
		{
			this.Inserting.Raise(this, e);
			return e.Handled;
		}

		protected virtual void OnUpdated(EntityActed<TEntity> e)
		{
			this.Updated.Raise(this, e);
		}

		protected virtual bool OnUpdating(EntityActing<TEntity> e)
		{
			this.Updating.Raise(this, e);
			return e.Handled;
		}

		protected virtual void OnValidated(EntityActed<TEntity> e)
		{
			this.Validated.Raise(this, e);
		}

		protected virtual void OnValidatedByIds(EntityActed<IEnumerable<long>> e)
		{
			this.ValidatedByIds.Raise(this, e);
		}

		protected virtual bool OnValidating(EntityValidating<TEntity> e)
		{
			this.Validating.Raise(this, e);
			return !e.Handled;
		}

		protected virtual bool OnValidatingByIds(EntityValidatingByIds e)
		{
			this.ValidatingByIds.Raise(this, e);
			return !e.Handled;
		}

		[DataObjectMethod(DataObjectMethodType.Select)]
		public virtual IEnumerable<TEntity> Select()
		{
			this.ExceptionHandling.Reset();
			this.CheckPermission(this.SelectPremissionKey);
			return this.SelectCore();
		}

		protected abstract IEnumerable<TEntity> SelectCore();

		public virtual void SaveChanges()
		{
			this.SaveChangesCore();
		}

		protected abstract void SaveChangesCore();

		[DataObjectMethod(DataObjectMethodType.Update)]
		public virtual void Update(TEntity entity)
		{
			this.Update(entity, true);
		}

		[DataObjectMethod(DataObjectMethodType.Update)]
		public virtual void Update(TEntity entity, bool submitChanges)
		{
			this.ExceptionHandling.Reset();
			this.CheckPermission(this.UpdatePremissionKey);
			try
			{
				if (this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Update)))
				{
					this.OnValidated(new EntityActed<TEntity>(entity));
					if (!this.OnUpdating(new EntityActing<TEntity>(entity)))
						this.UpdateCore(entity, submitChanges);
					this.OnUpdated(new EntityActed<TEntity>(entity));
				}
			}
			catch (Exception ex)
			{
				this.ExceptionHandling.HandleException(ex);
				this.OnError(new EntityActed<TEntity>(entity));
			}
		}

		protected abstract void UpdateCore(TEntity entity, bool submitChanges);

		// Properties
	}
}