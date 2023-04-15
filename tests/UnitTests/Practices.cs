using Xunit.Abstractions;

namespace UnitTests;

public readonly record struct Location(double Latitude, double Longitude);

public class __ObservationTests
{
    private readonly ITestOutputHelper _output;

    public __ObservationTests(ITestOutputHelper output)
    {
        this._output = output;
        
    }

    [Fact]
    public void _01_Test()
    {
        var console = ConsoleOutputRedirector.CreateAndStart(this._output.WriteLine);

        try
        {
            // Define a provider and two observers.
            var provider = new LocationTracker();
            var reporter1 = new LocationReporter("FixedGPS");
            reporter1.Subscribe(provider);
            var reporter2 = new LocationReporter("MobileGPS");
            reporter2.Subscribe(provider);

            provider.TrackLocation(new Location(47.6456, -122.1312));
            reporter1.Unsubscribe();
            provider.TrackLocation(new Location(47.6677, -122.1199));
            provider.TrackLocation(null);
            provider.EndTransmission();
        }
        finally
        {
            console.StopCapturing();
        }
    }
}

public class LocationTracker : IObservable<Location>
{
    private readonly List<IObserver<Location>> _observers;

    public LocationTracker()
        => this._observers = new List<IObserver<Location>>();

    public void EndTransmission()
    {
        foreach (var observer in this._observers.ToArray())
        {
            if (this._observers.Contains(observer))
            {
                observer.OnCompleted();
            }
        }

        this._observers.Clear();
    }

    public IDisposable Subscribe(IObserver<Location> observer)
    {
        if (!this._observers.Contains(observer))
        {
            this._observers.Add(observer);
        }

        return new Unsubscriber(this._observers, observer);
    }

    public void TrackLocation(Location? loc)
    {
        foreach (var observer in this._observers)
        {
            if (!loc.HasValue)
            {
                observer.OnError(new LocationUnknownException());
            }
            else
            {
                observer.OnNext(loc.Value);
            }
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly IObserver<Location> _observer;
        private readonly List<IObserver<Location>> _observers;

        public Unsubscriber(List<IObserver<Location>> observers, IObserver<Location> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (this._observer != null && this._observers.Contains(this._observer))
            {
                _ = this._observers.Remove(this._observer);
            }
        }
    }
}

public class LocationUnknownException : Exception
{
    internal LocationUnknownException()
    { }
}

public class LocationReporter : IObserver<Location>
{
    private IDisposable? _unsubscriber;

    public LocationReporter(string name)
        => this.Name = name;

    public string Name { get; }

    public virtual void OnCompleted()
    {
        Console.WriteLine("The Location Tracker has completed transmitting data to {0}.", this.Name);
        this.Unsubscribe();
    }

    public virtual void OnError(Exception e)
        => Console.WriteLine("{0}: The location cannot be determined.", this.Name);

    public virtual void OnNext(Location value)
        => Console.WriteLine("{2}: The current location is {0}, {1}", value.Latitude, value.Longitude, this.Name);

    public virtual void Subscribe(IObservable<Location> provider)
    {
        if (provider != null)
        {
            this._unsubscriber = provider.Subscribe(this);
        }
    }

    public virtual void Unsubscribe()
        => this._unsubscriber?.Dispose();
}