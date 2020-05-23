#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.ComponentModel;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Validation;

namespace Mohammad.Data.BusinessTools
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BusinessEntityBase<TBusinessEntity>
        where TBusinessEntity : BusinessEntityBase<TBusinessEntity>, new()
    {
        #region Fields

        private static TBusinessEntity   _Instance;
        private        ExceptionHandling _ExceptionHandling;

        #endregion

        protected BusinessEntityBase()
        {
            this.Initialize();
        }

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = this.OnExceptionHandlingRequired());
            protected set => this._ExceptionHandling = value;
        }

        public static TBusinessEntity Instance
        {
            get => _Instance ?? (_Instance = new TBusinessEntity());
            protected set => _Instance = value;
        }

        protected Validator Validator { get; private set; }

        private void Initialize()
        {
            this.OnInitializing();
            this.Validator = new Validator(this.ExceptionHandling);
        }

        protected virtual ExceptionHandling OnExceptionHandlingRequired() => new ExceptionHandling
        {
            RaiseExceptions = true
        };

        protected virtual void OnInitializing()
        {
        }
    }
}