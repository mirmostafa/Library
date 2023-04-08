using Library.EventsArgs;
using Library.Interfaces;
using Library.Validations;

namespace Library.DesignPatterns.Behavioral.Observation;

public sealed class ObservationRepository : IObservableRepository, IObserverRepository
{
    private readonly List<RegisterObserver> _Observers = new();
    private readonly List<RegisterObservable> _Observables = new();

    private void Subject_PropertyChanged(object? sender, ItemActedEventArgs<NotifyPropertyChanged> e)
    {
        if (e?.Item is null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        foreach (var (observer, propertyType, onPropertyChanged, propertyName, typeOfObserable) in this._Observers)
        {
            if (typeOfObserable is not null && typeOfObserable != e.Item.Sender?.GetType())
            {
                continue;
            }
            if (propertyType is not null && propertyType != e.Item.PropertyType)
            {
                continue;
            }
            if (propertyName is not null && propertyName != e.Item.PropertyName)
            {
                continue;
            }

            onPropertyChanged?.Invoke(e.Item);
        }
    }

    public void Add(in RegisterObservable observable)
    {
        this._Observables.Add(observable.ArgumentNotNull(nameof(observable)));
        observable.Observable.Changed += this.Subject_PropertyChanged;
    }
    public void Add(in RegisterObserver observer)
        => this._Observers.Add(observer);
}
