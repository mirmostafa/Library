using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Data.SqlServer;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;

namespace Mohammad.Data.Ado.BusinessTools
{
    public abstract class BusinessEntityOnAdo<TEntity> : IExceptionHandlerContainer
    {
        private ExceptionHandling _ExceptionHandling;
        protected string ConnectionString => this.Sql.ConnectionString;
        protected Sql Sql { get; }
        protected string IdentityColumnName { get; set; } = "Id";
        protected virtual bool IsLazy { get; set; } = true;
        protected BusinessEntityOnAdo(string connectionString) { this.Sql = new Sql(connectionString); }
        protected BusinessEntityOnAdo(Sql sql) { this.Sql = sql; }

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

        protected virtual IEnumerable<TEntity> SelectCore()
        {
            lock (this)
            {
                var result = this.Sql.Select(SqlStatementBuilder.CreateSelect<TEntity>(), this.CreateNew);
                return this.IsLazy ? result : result.ToList();
            }
        }

        protected virtual void InsertCore(TEntity entity) { this.Sql.ExecuteNonQuery(this.CreateInsertValueStatement(entity)); }
        protected virtual void UpdateCore(TEntity entity) { this.Sql.ExecuteNonQuery(this.CreateUpdateStatement(entity)); }
        protected virtual void DeleteCore(TEntity entity) { this.Sql.ExecuteNonQuery(this.CreateDeleteCoreStatement(entity)); }
        protected virtual string CreateInsertValueStatement(TEntity entity) { return SqlStatementBuilder.CreateInsertValue(entity); }
        protected virtual string CreateUpdateStatement(TEntity entity) { return SqlStatementBuilder.CreateUpdate(entity, false, new[] {this.IdentityColumnName}); }
        protected virtual string CreateDeleteCoreStatement(TEntity entity) { return SqlStatementBuilder.CreateDelete(entity); }
        protected virtual TEntity CreateNew() { return (TEntity) typeof(TEntity).GetConstructor(null).Invoke(null); }

        public IEnumerable<TEntity> SelectAll()
        {
            IEnumerable<TEntity> entities = null;
            this.ExecuteAndHandleExceptions(() => { entities = this.SelectCore(); });
            return entities;
        }

        public void Insert(TEntity entity) { this.ExecuteAndHandleExceptions(() => this.InsertCore(entity)); }
        public void Update(TEntity entity) { this.ExecuteAndHandleExceptions(() => this.UpdateCore(entity)); }
        public void Delete(TEntity entity) { this.ExecuteAndHandleExceptions(() => this.DeleteCore(entity)); }

        public ExceptionHandling ExceptionHandling
        {
            get { return this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling()); }
            protected set { this._ExceptionHandling = value; }
        }
    }
}