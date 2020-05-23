#region Code Identifications

// Created on     2018/07/25
// Last update on 2018/07/28 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using Mohammad.Data.BusinessTools;
using Mohammad.DesignPatterns.Behavioral;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.DesignPatterns.ExceptionHandlingPattern.Handlers;
using Mohammad.Helpers;

namespace TestConsole45
{
    public abstract class DataAccessEntityOnEf5<TEntity, TDateContext, TBusinessEntity> : BusinessEntity<TEntity, TBusinessEntity>, IDisposable
        where TEntity : EntityObject, new() where TDateContext : ObjectContext where TBusinessEntity : BusinessEntity<TEntity, TBusinessEntity>, new()
    {
        private TDateContext _Db;
        protected bool UseStaticDataContext { get; set; }

        protected TDateContext DataContext
        {
            get
            {
                if (this.UseStaticDataContext)
                {
                    if (Memento.Get("StaticDb") != null)
                        return Memento.Get<TDateContext>("StaticDb");
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
                    return this._Db;
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

        protected override void DeleteCore(TEntity entity, bool submitChanges) { this.DataContext.DeleteEntity(entity, submitChanges, this.ExceptionHandling); }
        protected override TEntity FillCore(TEntity entity) => throw new NotImplementedException();
        protected abstract TDateContext GetDb();
        protected override TEntity GetNewCore() => Activator.CreateInstance<TEntity>();
        protected override void InsertCore(TEntity entity, bool submitChanges) { this.DataContext.InsertEntity(entity, submitChanges, this.ExceptionHandling); }
        protected override IEnumerable<TEntity> SelectCore() => this.DataContext.GetAll<TEntity>();

        protected override void SaveChangesCore()
        {
            CodeHelper.Catch(() => this.DataContext.SaveChanges(), throwException: true, handling: this.ExceptionHandling);
        }

        protected override void UpdateCore(TEntity entity, bool submitChanges) { this.DataContext.UpdateEntity(entity, submitChanges); }

        public void Dispose()
        {
            var dataContext = this.DataContext;
            dataContext?.Dispose();
        }
    }

    public static class EntityFrameworkHelper
    {
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

        public static void DeleteEntity<TEntity>(this ObjectContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            CodeHelper.Catch(() =>
                {
                    CodeHelper.Catch(() => db.CreateObjectSet<TEntity>().Attach(entity));
                    db.Refresh(RefreshMode.ClientWins, entity);
                    db.CreateObjectSet<TEntity>().DeleteObject(entity);
                    if (submitChanges)
                        db.SaveChanges();
                },
                handling: exceptionHandling);
        }

        public static ObjectSet<TEntity> GetAll<TEntity>(this ObjectContext db, ExceptionHandling exceptionHandling = null)
            where TEntity : class => CodeHelper.CatchFunc(db.CreateObjectSet<TEntity>, handling: exceptionHandling);

        public static void InsertEntity<TEntity>(this ObjectContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            CodeHelper.Catch(() =>
                {
                    db.CreateObjectSet<TEntity>().AddObject(entity);
                    if (submitChanges)
                        db.SaveChanges();
                },
                handling: exceptionHandling);
        }

        public static void UpdateEntity<TEntity>(this ObjectContext db, TEntity entity, bool submitChanges = true, ExceptionHandling exceptionHandling = null)
            where TEntity : class
        {
            CodeHelper.Catch(() =>
                {
                    CodeHelper.Catch(() => db.CreateObjectSet<TEntity>().Attach(entity));
                    db.Refresh(RefreshMode.ClientWins, entity);
                    if (submitChanges)
                        db.SaveChanges();
                },
                handling: exceptionHandling);
        }

        public static string GetTableName<TEntityObject>()
            where TEntityObject : EntityObject => typeof(TEntityObject).GetCustomAttributes(typeof(EdmEntityTypeAttribute), false).FirstOrDefault() is
            EdmEntityTypeAttribute edmEntityType
            ? edmEntityType.Name
            : string.Empty;
    }
}