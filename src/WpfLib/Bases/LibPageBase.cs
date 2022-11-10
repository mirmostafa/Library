using Library.Wpf.Markers;
using Library.Wpf.Windows.Input.Commands;

namespace Library.Wpf.Bases;

public class LibPageBase : Page, ISupportAsyncDataBinding
{
    private CommandController? _commandManager;

    private bool _isBounded = false;

    /// <summary>
    /// Occurs when [binding data].
    /// </summary>
    public event EventHandler? BindingData;

    public LibPageBase()
        => this.Loaded += this.LibPage_LoadedAsync;

    /// <summary>
    /// Gets the command manager.
    /// </summary>
    /// <value>
    /// The command manager.
    /// </value>
    protected CommandController CommandManager => this._commandManager ??= new(this);

    /// <summary>
    /// Gets a value indicating whether this <see cref="LibPage"/> is initializing.
    /// </summary>
    /// <value>
    ///   <c>true</c> if initializing; otherwise, <c>false</c>.
    /// </value>
    protected bool Initializing { get; private set; }

    /// <summary>
    /// Starts the initialization process for this element.
    /// </summary>
    public override void BeginInit()
    {
        if (this.Initializing)
        {
            return;
        }

        this.Initializing = true;
        base.BeginInit();
    }

    /// <summary>
    /// Binds the data asynchronously.
    /// </summary>
    public async Task BindDataAsync()
    {
        if (ControlHelper.IsDesignTime())
        {
            return;
        }

        try
        {
            this.BeginInit();
            this.BindingData?.Invoke(this, EventArgs.Empty);
            await this.OnBindDataAsync();
        }
        catch
        {
            throw;
        }
        finally
        {
            this.EndInit();
        }
    }

    /// <summary>
    /// Indicates that the initialization process for the element is complete.
    /// </summary>
    public override void EndInit()
    {
        if (!this.Initializing)
        {
            return;
        }

        this.Initializing = false;
        base.EndInit();
    }

    public Task RebindDataAsync()
        => this.BindDataAsync();

    /// <summary>
    /// Called when [bind data asynchronous].
    /// </summary>
    protected virtual Task OnBindDataAsync()
        => Task.CompletedTask;

    protected override void OnInitialized(EventArgs e)
        => base.OnInitialized(e);

    private async Task FirstBind()
    {
        if (this._isBounded)
        {
            return;
        }

        await this.BindDataAsync();
        this._isBounded = true;
    }

    private async void LibPage_LoadedAsync(object sender, RoutedEventArgs e)
        => await this.FirstBind();
}