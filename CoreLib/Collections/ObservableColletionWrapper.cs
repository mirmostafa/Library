﻿using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Library.Collections;

public class ObservableColletionWrapper<T> : FluentListBase<T, ObservableColletionWrapper<T>>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyPropertyChanging
{
    protected ObservableColletionWrapper(IList<T> list) : base(list)
    {
    }

    protected ObservableColletionWrapper(IEnumerable<T> list) : base(list)
    {
    }

    protected ObservableColletionWrapper(int capacity) : base(capacity)
    {
    }

    protected ObservableColletionWrapper()
    {
    }

    public new T this[int index]
    {
        get => base[index];
        set
        {
            this.OnCountPropertyChanging();
            base[index] = value;
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, value);
            this.OnCountPropertyChanged();
        }
    }

    //public int Count => this._set.Count;

    //public bool IsReadOnly => this._set.IsReadOnly;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    public new void Add(T item)
    {
        this.OnCountPropertyChanging();
        base.Add(item);
        this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        this.OnCountPropertyChanged();
    }

    public new void Clear()
    {
        if (this.Count != 0)
        {
            this.OnCountPropertyChanging();
            var oldItems = this.ToList();
            base.Clear();
            this.OnCollectionChanged(ObservableHashSetSingletons._noItems, oldItems);
            this.OnCountPropertyChanged();
        }
    }

    public new bool Contains(T item) => base.Contains(item).Result;
    public new void CopyTo(T[] array, int arrayIndex) => base.CopyTo(array, arrayIndex);
    public new IEnumerator<T> GetEnumerator() => base.GetEnumerator();
    public new int IndexOf(T item) => base.IndexOf(item).Result;
    public new void Insert(int index, T item)
    {
        this.OnCountPropertyChanging();
        base.Insert(index, item);
        this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        this.OnCountPropertyChanged();
    }
    public new bool Remove(T item)
    {
        if (!this.Contains(item))
        {
            return false;
        }

        this.OnCountPropertyChanging();
        base.Remove(item);
        this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
        this.OnCountPropertyChanged();
        return true;
    }

    public new void RemoveAt(int index)
    {
        this.OnCountPropertyChanging();
        base.RemoveAt(index);
        this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, index);
        this.OnCountPropertyChanged();
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this).GetEnumerator();

    private void OnCountPropertyChanging() => this.OnPropertyChanging(ObservableHashSetSingletons._countPropertyChanging);

    private void OnCollectionChanged(NotifyCollectionChangedAction action, object? item) => this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item));

    private void OnCollectionChanged(IList newItems, IList oldItems) => this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems));

    protected virtual void OnPropertyChanging(PropertyChangingEventArgs e) => this.PropertyChanging?.Invoke(this, e);

    private void OnCountPropertyChanged() => this.OnPropertyChanged(ObservableHashSetSingletons._countPropertyChanged);

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => this.CollectionChanged?.Invoke(this, e);

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => this.PropertyChanged?.Invoke(this, e);
}

internal static class ObservableHashSetSingletons
{
    public static readonly PropertyChangedEventArgs _countPropertyChanged = new("Count");

    public static readonly PropertyChangingEventArgs _countPropertyChanging = new("Count");

    public static readonly object[] _noItems = Array.Empty<object>();
}