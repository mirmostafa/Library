using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Mohammad.Collections.ObjectModel
{
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext _SynchronizationContext = SynchronizationContext.Current;
        public AsyncObservableCollection() { }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == this._SynchronizationContext)
                this.RaiseCollectionChanged(e);
            else
                this._SynchronizationContext.Post(this.RaiseCollectionChanged, e);
        }

        private void RaiseCollectionChanged(object param) { base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param); }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == this._SynchronizationContext)
                this.RaisePropertyChanged(e);
            else
                this._SynchronizationContext.Post(this.RaisePropertyChanged, e);
        }

        private void RaisePropertyChanged(object param) { base.OnPropertyChanged((PropertyChangedEventArgs) param); }
    }
}