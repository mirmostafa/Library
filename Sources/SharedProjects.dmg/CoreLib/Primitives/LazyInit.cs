using System;

namespace Mohammad.Bcl
{
    public class LazyInit<T>
        where T : class
    {
        private T _Value;
        public Func<T> Creator { get; set; }
        public LazyInitMode Mode { get; set; }

        public T Value
        {
            get
            {
                switch (this.Mode)
                {
                    case LazyInitMode.FirstGet:
                        return this._Value ?? (this._Value = this.Creator());
                    case LazyInitMode.Immediately:
                        return this._Value;
                    case LazyInitMode.NewPerGet:
                        return this.Creator();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set { this._Value = value; }
        }

        public LazyInit(Func<T> creator, LazyInitMode mode = LazyInitMode.FirstGet)
        {
            this.Creator = creator;
            this.Mode = mode;
            if (this.Mode == LazyInitMode.Immediately)
                this.Value = creator();
        }

        public static implicit operator T(LazyInit<T> lazyInit) => lazyInit.Value;
    }

    public enum LazyInitMode
    {
        FirstGet,
        Immediately,
        NewPerGet
    }
}