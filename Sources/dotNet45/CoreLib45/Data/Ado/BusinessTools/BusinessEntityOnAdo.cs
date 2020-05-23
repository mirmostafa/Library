#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Data.SqlServer;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;

namespace Mohammad.Data.Ado.BusinessTools
{
    public abstract class BusinessEntityOnAdo<TEntity> : IExceptionHandlerContainer
    {
        #region Fields

        private ExceptionHandling _ExceptionHandling;

        #endregion

        protected BusinessEntityOnAdo(string connectionString) => this.Sql = new Sql(connectionString);
        protected BusinessEntityOnAdo(Sql    sql) => this.Sql = sql;

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling());
            protected set => this._ExceptionHandling = value;
        }

        protected         string ConnectionString   => this.Sql.ConnectionString;
        protected         string IdentityColumnName { get; set; } = "Id";
        protected virtual bool   IsLazy             { get; set; } = true;
        protected         Sql    Sql                { get; }

        protected virtual string CreateDeleteCoreStatement(TEntity entity) => SqlStatementBuilder.CreateDelete(entity);

        protected virtual string  CreateInsertValueStatement(TEntity entity) => SqlStatementBuilder.CreateInsertValue(entity);
        protected virtual TEntity CreateNew()                                => (TEntity) typeof(TEntity).GetConstructor(null).Invoke(null);

        protected virtual string CreateUpdateStatement(TEntity entity) =>
            SqlStatementBuilder.CreateUpdate(entity, false, new[] {this.IdentityColumnName});

        public void Delete(TEntity entity)
        {
            this.ExecuteAndHandleExceptions(() => this.DeleteCore(entity));
        }

        protected virtual void DeleteCore(TEntity entity)
        {
            this.Sql.ExecuteNonQuery(this.CreateDeleteCoreStatement(entity));
        }

        protected virtual bool ExecuteAndHandleExceptions(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex);
                return false;
            }
        }

        public void Insert(TEntity entity)
        {
            this.ExecuteAndHandleExceptions(() => this.InsertCore(entity));
        }

        protected virtual void InsertCore(TEntity entity)
        {
            this.Sql.ExecuteNonQuery(this.CreateInsertValueStatement(entity));
        }

        public IEnumerable<TEntity> SelectAll()
        {
            IEnumerable<TEntity> entities = null;
            this.ExecuteAndHandleExceptions(() => { entities = this.SelectCore(); });
            return entities;
        }

        protected virtual IEnumerable<TEntity> SelectCore()
        {
            lock (this)
            {
                var result = this.Sql.Select(SqlStatementBuilder.CreateSelect<TEntity>(), this.CreateNew);
                return this.IsLazy ? result : result.ToList();
            }
        }

        public void Update(TEntity entity)
        {
            this.ExecuteAndHandleExceptions(() => this.UpdateCore(entity));
        }

        protected virtual void UpdateCore(TEntity entity)
        {
            this.Sql.ExecuteNonQuery(this.CreateUpdateStatement(entity));
        }
    }
}