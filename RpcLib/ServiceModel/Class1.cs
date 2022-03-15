namespace Library.Rpc.ServiceModel;

public static class Exceptioner
{
    public static void Throw(Exception ex) =>
        throw new FaultException(ex.GetBaseException().Message);

    public static void Throw<TFualt>(Func<TFualt> creator, string reason) =>
        throw new FaultException<TFualt>(creator(), new(reason));

    public static void Throw<TFault>(string reason) where TFault : new() =>
        Throw(() => new TFault(), reason);

    public static void Throw<TFault>(string message, string reason) =>
        Throw(() => typeof(TFault).GetConstructor(Array.Empty<Type>())!.Invoke(new object[] { message }), reason);
}
public partial class ServiceClient<TContract>
{
    public static TContract GetChannel(string uri) =>
        GetChannel(uri, DefaultBinding);

    public static TContract GetChannel(string uri, Binding binding) =>
        new ServiceClient<TContract>(uri, binding).GetChannel();

    public static TContract GetChannel(string uri, object callbackObject) =>
        GetChannel(uri, DefaultBinding, callbackObject);

    public static TContract GetChannel(string uri, Binding binding, object callbackObject) =>
        new ServiceClient<TContract>(uri, binding, callbackObject).GetChannel();
}

/// <summary>
///     A utility class to get a service from a host
/// </summary>
/// <typeparam name="TContract">The service contract, usually an interface</typeparam>
public partial class ServiceClient<TContract> : ServiceBase
{
    public ServiceClient(string uri)
        : this(uri, DefaultBinding)
    {
    }

    public ServiceClient(string uri, object? callbackObject = null)
        : this(uri, DefaultBinding, callbackObject)
    {
    }

    public ServiceClient(string uri, Binding binding, object? callbackObject = null)
    {
        this.Binding = binding;
        this.Endpoint = new EndpointAddress(uri);
        this.ChannelFactory = callbackObject == null
            ? new ChannelFactory<TContract>(this.Binding, this.Endpoint)
            : new DuplexChannelFactory<TContract>(callbackObject, this.Binding, this.Endpoint);
    }

    public EndpointAddress Endpoint { get; private set; }

    public ChannelFactory<TContract> ChannelFactory { get; private set; }

    public Binding Binding { get; private set; }

    public TContract GetChannel() => this.ChannelFactory.CreateChannel();
}

/// <summary>
///     A utility class to create a host for a service
/// </summary>
/// <typeparam name="TContract">The service contract, usually an interface</typeparam>
/// <typeparam name="TService">The service, which implements the contract</typeparam>
public class ServiceHost<TContract, TService> : ServiceHost
    where TService : class
{
    public ServiceHost(string uri, Binding? binding = null, object? singletonServiceInstance = null)
        : base(typeof(TContract), typeof(TService), uri, binding, singletonServiceInstance)
    {
    }
}

/// <summary>
///     A utility class to create a host for a service
/// </summary>
public class ServiceHost : ServiceBase, IDisposable
{
    private static readonly object _eventDisposed = new();
    private readonly List<ServiceEndpoint> _endpoints = new();
    private readonly System.ServiceModel.ServiceHost _host;
    private EventHandlerList _events;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceHost&lt;TContract, TService&gt;" /> class.
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="contractType"></param>
    /// <param name="singletonServiceInstance">The instance of the hosted service.</param>
    /// <param name="binding">The binding protocol</param>
    /// <param name="uri">The service's uri</param>
    public ServiceHost(Type contractType, Type serviceType, string? uri = null, Binding? binding = null, object? singletonServiceInstance = null)
    {
        this.ServiceType = serviceType;
        this.ContractType = contractType;
        this._host = singletonServiceInstance == null
            ? new System.ServiceModel.ServiceHost(serviceType, new Uri(uri ?? throw new($"{nameof(uri)} must have value.")))
            : new System.ServiceModel.ServiceHost(singletonServiceInstance, new Uri(uri ?? throw new($"{nameof(uri)} must have value.")));
        if (uri.IsNullOrEmpty())
        {
            return;
        }

        if (binding == null)
        {
            binding = DefaultBinding;
        }

        this.AddServiceEndpoint(binding, uri);
    }

    /// <summary>
    /// </summary>
    public Type ServiceType { get; private set; }

    /// <summary>
    /// </summary>
    public Type ContractType { get; private set; }

    /// <summary>
    ///     Get the uri, which the service is being up.
    /// </summary>
    public IEnumerable<string> Addresses => this._endpoints.Select(ep => ep.Address.ToString());

    /// <summary>
    ///     Gets the list of event handlers that are attached to this component.
    /// </summary>
    protected EventHandlerList Events => this._events ??= new EventHandlerList();

    #region IDisposable Members
    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    /// <summary>
    ///     Adds the service endpoint.
    /// </summary>
    /// <param name="binding">The binding.</param>
    /// <param name="uri">The URI.</param>
    public void AddServiceEndpoint(Binding binding, string uri)
    {
        this._endpoints.Add(this._host.AddServiceEndpoint(this.ContractType, binding, uri));
        if (binding is WSHttpBinding)
        {
            var wsEnabledbehavior = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpGetUrl = new Uri(uri)
            };
            this._host.Description.Behaviors.Add(wsEnabledbehavior);
        }
    }

    /// <summary>
    ///     Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing">
    ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            lock (this)
            {
                if (this._host != null)
                {
                    this.Close();
                    ((IDisposable)this._host).Dispose();
                }
                if (this._events != null)
                {
                    ((EventHandler?)this._events[_eventDisposed])?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    /// <summary>
    ///     Causes a communication object to transition from the created state to the opened state
    /// </summary>
    public void Open() => this._host.Open();

    /// <summary>
    ///     Causes a communication object to transition from the current state to the closed state
    /// </summary>
    public void Close() => this._host.Close();

    /// <summary>
    ///     Adds a event handler to listen to the Disposed event on the component.
    /// </summary>
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
    public event EventHandler Disposed
    {
        add => this.Events.AddHandler(_eventDisposed, value);
        remove => this.Events.RemoveHandler(_eventDisposed, value);
    }
}

public abstract class ServiceBase
{
    /// <summary>
    ///     A simple default binding = NetTcpBinding
    /// </summary>
    public static NetTcpBinding DefaultBinding =>
        new()
        {
            Security = { Mode = SecurityMode.None }
        };

    /// <summary>
    ///     Gets the basic HTTP binding.
    /// </summary>
    /// <value>The basic HTTP binding.</value>
    public static BasicHttpBinding BasicHttpBinding =>
        new()
        {
            Security = { Mode = BasicHttpSecurityMode.None }
        };

    /// <summary>
    ///     Gets the ws HTTP binding.
    /// </summary>
    /// <value>The ws HTTP binding.</value>
    public static WSHttpBinding WsHttpBinding =>
        new()
        {
            Security = { Mode = SecurityMode.None }
        };
}