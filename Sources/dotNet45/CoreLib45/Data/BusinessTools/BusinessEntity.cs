#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Mohammad.Data.BusinessTools.EventsArgs;
using Mohammad.Helpers;

namespace Mohammad.Data.BusinessTools
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BusinessEntity<TEntity, TBusinessEntity> : BusinessEntityBase<TBusinessEntity>
        where TEntity : class where TBusinessEntity : BusinessEntity<TEntity, TBusinessEntity>, new()
    {
        public virtual string DeletePremissionKey => string.Empty;
        public virtual string InsertPremissionKey => string.Empty;
        public virtual string SelectPremissionKey => string.Empty;
        public virtual string UpdatePremissionKey => string.Empty;
        protected virtual bool IsLazy { get; set; } = true;

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public virtual void Delete(TEntity entity)
        {
            this.Delete(entity, true);
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public virtual void DeleteByIds(IEnumerable<long> ids)
        {
            this.DeleteByIds(ids, true);
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void Insert(TEntity entity)
        {
            this.Insert(entity, true);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual IEnumerable<TEntity> Select()
        {
            this.ExceptionHandling.Reset();
            this.CheckPermission(this.SelectPremissionKey);
            return this.SelectCore();
        }

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
                if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Update)))
                {
                    return;
                }

                this.OnValidated(new EntityActed<TEntity>(entity));
                if (!this.OnUpdating(new EntityActing<TEntity>(entity)))
                {
                    this.UpdateCore(entity, submitChanges);
                }

                this.OnUpdated(new EntityActed<TEntity>(entity));
            }
            catch (Exception ex)
            {
                this.OnError(new EntityActed<TEntity>(entity));
                this.ExceptionHandling.HandleException(ex);
            }
        }

        public virtual void Delete(TEntity entity, bool submitChanges)
        {
            this.ExceptionHandling.Reset();
            this.CheckPermission(this.DeletePremissionKey);
            try
            {
                if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Delete)))
                {
                    return;
                }

                this.OnValidated(new EntityActed<TEntity>(entity));
                if (!this.OnDeleting(new EntityActing<TEntity>(entity)))
                {
                    this.DeleteCore(entity, submitChanges);
                }

                this.OnDeleted(new EntityActed<TEntity>(entity));
            }
            catch (Exception ex)
            {
                this.OnError(new EntityActed<TEntity>(entity));
                this.ExceptionHandling.HandleException(ex);
            }
        }

        public virtual void DeleteByIds(IEnumerable<long> ids, bool submitChanges)
        {
            this.ExceptionHandling.Reset();
            try
            {
                if (!this.OnValidatingByIds(new EntityValidatingByIds(ids, EntityAction.Delete)))
                {
                    return;
                }

                this.OnValidatedByIds(new EntityActed<IEnumerable<long>>(ids));
                if (!this.OnDeletingByIds(new EntityActing<IEnumerable<long>>(ids)))
                {
                    this.DeleteByIdsCore(ids, submitChanges);
                }

                this.OnDeletedByIds(new EntityActed<IEnumerable<long>>(ids));
            }
            catch (Exception ex)
            {
                this.OnError(new EntityActed<IEnumerable<long>>(ids));
                this.ExceptionHandling.HandleException(ex);
            }
        }

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
                return default;
            }
        }

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
                this.OnError(new EntityActed<TEntity>(default));
                this.ExceptionHandling.HandleException(ex);
                return default;
            }
        }

        public void Insert(TEntity entity, bool submitChanges)
        {
            this.ExceptionHandling.Reset();
            this.CheckPermission(this.InsertPremissionKey);
            try
            {
                if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Insert)))
                {
                    return;
                }

                this.OnValidated(new EntityActed<TEntity>(entity));
                if (!this.OnInserting(new EntityActing<TEntity>(entity)))
                {
                    this.InsertCore(entity, submitChanges);
                }

                this.OnInserted(new EntityActed<TEntity>(entity));
            }
            catch (Exception ex)
            {
                this.OnError(new EntityActed<TEntity>(entity));
                this.ExceptionHandling.HandleException(ex);
            }
        }

        public virtual void SaveChanges()
        {
            this.SaveChangesCore();
        }

        protected virtual void CheckPermission(string premissionKey)
        {
        }

        protected virtual void DeleteByIdsCore(IEnumerable<long> ids, bool submitChanges) => throw new NotImplementedException();
        protected abstract void DeleteCore(TEntity entity, bool submitChanges);

        protected abstract TEntity FillCore(TEntity entity);

        protected abstract TEntity GetNewCore();

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

        protected abstract void SaveChangesCore();

        protected abstract IEnumerable<TEntity> SelectCore();

        protected abstract void UpdateCore(TEntity entity, bool submitChanges);
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
    }
}