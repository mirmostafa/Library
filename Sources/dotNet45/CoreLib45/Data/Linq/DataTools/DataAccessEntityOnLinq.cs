#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Mohammad.Data.BusinessTools;
using Mohammad.Data.BusinessTools.EventsArgs;
using Mohammad.Data.Linq.DataTools.Internals;
using Mohammad.DesignPatterns.Behavioral;
using Mohammad.Helpers;

namespace Mohammad.Data.Linq.DataTools
{
    [Obsolete("Please use 'Mohammad.Data.Linq.Data.DataAccessTools.DataAccessEntityOnLinq' instead.")]
    public abstract class DataAccessEntityOnLinq<TEntity, TDateContext, TBusinessEntity> : BusinessEntity<TEntity, TBusinessEntity>
        where TEntity : class, new() where TDateContext : DataContext where TBusinessEntity : BusinessEntity<TEntity, TBusinessEntity>, new()
    {
        #region Fields

        private TDateContext _Db;

        #endregion

        protected TDateContext Db
        {
            get
            {
                if (this.UseStaticDataContext)
                {
                    if (Memento.Get("StaticDb") != null)
                    {
                        return Memento.Get<TDateContext>("StaticDb");
                    }

                    this.ExceptionHandling.Reset();
                    try
                    {
                        return Memento.Set("StaticDb", this.GetDb());
                    }
                    catch (Exception ex)
                    {
                        this.ExceptionHandling.HandleException(ex);
                        CodeHelper.Break();
                        return null;
                    }
                }

                if (this._Db != null)
                {
                    return this._Db;
                }

                this.ExceptionHandling.Reset();
                try
                {
                    return this._Db = this.GetDb();
                }
                catch (Exception ex)
                {
                    this.ExceptionHandling.HandleException(ex);
                    CodeHelper.Break();
                    return null;
                }
            }
        }

        protected bool UseStaticDataContext { get; set; }

        protected DataAccessEntityOnLinq()
        {
            this.ExceptionHandling.Reset();
            try
            {
                this.Db.Log = new LinqLogger();
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(ex);
            }
        }

        public void Attach(TEntity entity, bool asModified = true)
        {
            this.OnAttaching(entity, asModified);
        }

        public void Attach(TEntity entity, TEntity original)
        {
            this.OnAttaching(entity, original);
        }

        public void Refresh(TEntity entity, RefreshMode refreshMode = RefreshMode.OverwriteCurrentValues)
        {
            this.Db.Refresh(refreshMode, entity);
        }

        public void Reset()
        {
            this._Db?.Dispose();
            this._Db = null;
            Instance = null;
            GC.Collect();
        }

        protected override void DeleteCore(TEntity entity, bool submitChanges)
        {
            this.Db.DeleteEntity(entity, submitChanges, this.ExceptionHandling);
        }

        protected override TEntity FillCore(TEntity entity) => throw new NotImplementedException();
        protected abstract TDateContext GetDb();
        protected override TEntity GetNewCore() => Activator.CreateInstance<TEntity>();

        protected override void InsertCore(TEntity entity, bool submitChanges)
        {
            this.Db.InsertEntity(entity, submitChanges, this.ExceptionHandling);
        }

        protected virtual void OnAttaching(TEntity entity, bool asModified)
        {
            this.Db.Attach(entity, asModified);
        }

        protected virtual void OnAttaching(TEntity entity, TEntity original)
        {
            this.Db.Attach(entity, original);
        }

        protected override void SaveChangesCore()
        {
            Exception ex;
            var changes = this.Db.GetChangeSet();
            foreach (var entity in changes.Inserts.Cast<TEntity>())
            {
                this.ExceptionHandling.Reset();
                try
                {
                    if (!this.OnValidating(new EntityValidating<TEntity>(entity, EntityAction.Insert)))
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    ex = exception;
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
                    {
                        return;
                    }
                }
                catch (Exception exception)
                {
                    ex = exception;
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
                    {
                        return;
                    }
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

        protected override IEnumerable<TEntity> SelectCore()
        {
            lock (this)
            {
                var result = this.Db.GetAll<TEntity>(this.ExceptionHandling);
                if (this.IsLazy)
                {
                    return result;
                }

                return result.ToList();
            }
        }

        protected override void UpdateCore(TEntity entity, bool submitChanges)
        {
            this.Db.UpdateEntity(entity, submitChanges, this.ExceptionHandling);
        }
    }
}