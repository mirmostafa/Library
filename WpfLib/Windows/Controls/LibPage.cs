using Library.Wpf.Bases;
using Library.Wpf.Markers;
using Library.Wpf.Windows.Input.Commands;

namespace Library.Wpf.Windows.Controls;

public class LibPage : PageBase, ISupportAsyncDataBinding
{
    private CommandController _commandManager;

    public LibPage() => this.Loaded += this.LibPage_LoadedAsync;

    private async void LibPage_LoadedAsync(object sender, RoutedEventArgs e) =>
        await this.BindDataAsync();

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
    /// Occurs when [binding data].
    /// </summary>
    public event EventHandler? BindingData;

    /// <summary>
    /// Starts the initialization process for this element.
    /// </summary>
    public override void BeginInit()
    {
        this.Initializing = true;
        base.BeginInit();
    }

    /// <summary>
    /// Indicates that the initialization process for the element is complete.
    /// </summary>
    public override void EndInit()
    {
        this.Initializing = false;
        base.EndInit();
    }

    protected override void OnInitialized(EventArgs e)
        => base.OnInitialized(e);

    /// <summary>
    /// Called when [bind data asynchronous].
    /// </summary>
    protected virtual async Task OnBindDataAsync()
        => await Task.CompletedTask;

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

    public Task RebindDataAsync() => this.BindDataAsync();
}
