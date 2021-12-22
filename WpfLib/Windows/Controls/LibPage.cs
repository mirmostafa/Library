using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Library.Wpf.Helpers;
using Library.Wpf.Windows.Input.Commands;

namespace Library.Wpf.Windows.Controls
{
    public class LibPage : Page, ISupportAsyncDataBinding
    {
        private CommandController _CommandManager;

        public LibPage()
        {
            this.DataContextChanged += this.LibUserControl_DataContextChanged;
            this.Loaded += this.LibPage_LoadedAsync;
        }

        private async void LibPage_LoadedAsync(object sender, RoutedEventArgs e) => 
            await this.BindDataAsync();

        /// <summary>
        /// Gets the command manager.
        /// </summary>
        /// <value>
        /// The command manager.
        /// </value>
        protected CommandController CommandManager => this._CommandManager ??= new(this);

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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized">Initialized</see> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized">IsInitialized</see> is set to <span class="keyword"><span class="languageSpecificText"><span class="cs">true</span><span class="vb">True</span><span class="cpp">true</span></span></span><span class="nu"><span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span> internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs">RoutedEventArgs</see> that contains the event data.</param>
        protected override async void OnInitialized(EventArgs e)
            => base.OnInitialized(e);

        /// <summary>
        /// Handles the DataContextChanged event of the LibUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private async void LibUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Rebinds the data.
        /// </summary>
        public void RebindData()
            => this.RebindDataContext();

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
    }
}