using System.ComponentModel;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.Validation;

namespace Mohammad.Data.BusinessTools
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BusinessEntityBase<TBusinessEntity>
        where TBusinessEntity : BusinessEntityBase<TBusinessEntity>, new()
    {
        private ExceptionHandling _ExceptionHandling;
        private static TBusinessEntity _Instance;

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = this.OnExceptionHandlingRequired());
            protected set => this._ExceptionHandling = value;
        }

        protected Validator Validator { get; private set; }

        public static TBusinessEntity Instance
        {
            get => _Instance ?? (_Instance = new TBusinessEntity());
            protected set => _Instance = value;
        }

        protected BusinessEntityBase() { this.Initialize(); }

        private void Initialize()
        {
            this.OnInitializing();
            this.Validator = new Validator(this.ExceptionHandling);
        }

        protected virtual void OnInitializing() { }
        protected virtual ExceptionHandling OnExceptionHandlingRequired() => new ExceptionHandling {RaiseExceptions = true};
    }
}