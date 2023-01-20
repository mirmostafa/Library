using Library.Wpf.Markers;

namespace Library.Wpf.Windows.Controls;

public abstract class AsyncDataBindUserControl : UserControlBase, ISupportAsyncDataBinding
{
    private bool _isFirstBinding = true;

    /// <summary>
    /// Occurs when [binding data].
    /// </summary>
    public event EventHandler? BindingData;

    /// <summary>
    /// Gets or sets a value indicating whether [should bind data on data context change].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [should bind data on data context change]; otherwise, <c>false</c>.
    /// </value>
    public bool ShouldBindDataOnDataContextChange { get; set; } = true;

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
        => this.Initializing = true;

    /// <summary>
    /// Binds the data asynchronously.
    /// </summary>
    public Task BindDataAsync()
    {
        if (ControlHelper.IsDesignTime())
        {
            return Task.CompletedTask;
        }

        try
        {
            this.BeginInit();
            BindingData?.Invoke(this, EventArgs.Empty);
            return this.OnBindDataAsync(_isFirstBinding);
        }
        finally
        {
            _isFirstBinding = false;
            this.EndInit();
        }
    }

    /// <summary>
    /// Indicates that the initialization process for the element is complete.
    /// </summary>
    public override void EndInit()
        => this.Initializing = false;

    public Task RebindDataAsync()
        => this.BindDataAsync();

    protected abstract Task OnBindDataAsync(bool isFirstBinding);

    /// <summary>
    /// Raises the <see cref="FrameworkElement.Initialized">Initialized</see> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized">IsInitialized</see> is set to <span class="keyword"><span class="languageSpecificText"><span class="cs">true</span><span class="vb">True</span><span class="cpp">true</span></span></span><span class="nu"><span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span> internally.
    /// </summary>
    /// <param name="e">The <see cref="RoutedEventArgs">RoutedEventArgs</see> that contains the event data.</param>
    protected override void OnInitialized(EventArgs e)
    {
        this.DataContextChanged += this.UserControlBase_DataContextChanged;
        this.Loaded += this.UserControlBase_Loaded;
        base.OnInitialized(e);
    }

    /// <summary>
    /// Handles the DataContextChanged event of the LibUserControl control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private async void UserControlBase_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (!this.Initializing && this.ShouldBindDataOnDataContextChange)
        {
            await this.BindDataAsync();
        }
    }

    private async void UserControlBase_Loaded(object sender, RoutedEventArgs e)
            => await this.BindDataAsync();
}