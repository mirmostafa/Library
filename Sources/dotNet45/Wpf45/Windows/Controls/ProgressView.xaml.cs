using System.Windows;
using System.Windows.Media;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.ProgressiveOperations;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for ProgressView.xaml
    /// </summary>
    public partial class ProgressView
    {
        private MultiStepOperation _Operation;

        public static readonly DependencyProperty CurrentOperationProgressBarMaxValueProperty = DependencyProperty.Register("CurrentOperationProgressBarMaxValue",
            typeof(long),
            typeof(ProgressView),
            new PropertyMetadata(default(long)));

        public long CurrentOperationProgressBarMaxValue
        {
            get { return (long) this.GetValue(CurrentOperationProgressBarMaxValueProperty); }
            set { this.SetValue(CurrentOperationProgressBarMaxValueProperty, value); }
        }

        public static readonly DependencyProperty CurrentOperationProgressBarValueProperty = DependencyProperty.Register("CurrentOperationProgressBarValue",
            typeof(long),
            typeof(ProgressView),
            new PropertyMetadata(default(long)));

        public long CurrentOperationProgressBarValue
        {
            get { return (long) this.GetValue(CurrentOperationProgressBarValueProperty); }
            set { this.SetValue(CurrentOperationProgressBarValueProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(ProgressView),
            new PropertyMetadata(default(string)));

        public string Header { get { return (string) this.GetValue(HeaderProperty); } set { this.SetValue(HeaderProperty, value); } }

        public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle",
            typeof(Style),
            typeof(ProgressView),
            new PropertyMetadata(default(Style)));

        public Style HeaderStyle { get { return (Style) this.GetValue(HeaderStyleProperty); } set { this.SetValue(HeaderStyleProperty, value); } }

        public static readonly DependencyProperty IsCurrentOperationIndeterminateProperty = DependencyProperty.Register("IsCurrentOperationIndeterminate",
            typeof(bool),
            typeof(ProgressView),
            new PropertyMetadata(default(bool)));

        public bool IsCurrentOperationIndeterminate
        {
            get { return (bool) this.GetValue(IsCurrentOperationIndeterminateProperty); }
            set { this.SetValue(IsCurrentOperationIndeterminateProperty, value); }
        }

        public static readonly DependencyProperty IsMainOperationIndeterminateProperty = DependencyProperty.Register("IsMainOperationIndeterminate",
            typeof(bool),
            typeof(ProgressView),
            new PropertyMetadata(default(bool)));

        public bool IsMainOperationIndeterminate
        {
            get { return (bool) this.GetValue(IsMainOperationIndeterminateProperty); }
            set { this.SetValue(IsMainOperationIndeterminateProperty, value); }
        }

        public static readonly DependencyProperty MainOperationProgressBarMaxValueProperty = DependencyProperty.Register("MainOperationProgressBarMaxValue",
            typeof(int),
            typeof(ProgressView),
            new PropertyMetadata(default(int)));

        public int MainOperationProgressBarMaxValue
        {
            get { return (int) this.GetValue(MainOperationProgressBarMaxValueProperty); }
            set { this.SetValue(MainOperationProgressBarMaxValueProperty, value); }
        }

        public static readonly DependencyProperty MainOperationProgressBarValueProperty = DependencyProperty.Register("MainOperationProgressBarValue",
            typeof(int),
            typeof(ProgressView),
            new PropertyMetadata(default(int)));

        public int MainOperationProgressBarValue
        {
            get { return (int) this.GetValue(MainOperationProgressBarValueProperty); }
            set { this.SetValue(MainOperationProgressBarValueProperty, value); }
        }

        public static readonly DependencyProperty ProgressBarColorProperty = DependencyProperty.Register("ProgressBarColor",
            typeof(Brush),
            typeof(ProgressView),
            new PropertyMetadata(Brushes.Green));

        public Brush ProgressBarColor { get { return (Brush) this.GetValue(ProgressBarColorProperty); } set { this.SetValue(ProgressBarColorProperty, value); } }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status",
            typeof(string),
            typeof(ProgressView),
            new PropertyMetadata(default(string)));

        public string Status { get { return (string) this.GetValue(StatusProperty); } set { this.SetValue(StatusProperty, value); } }

        public static readonly DependencyProperty StatusStyleProperty = DependencyProperty.Register("StatusStyle",
            typeof(Style),
            typeof(ProgressView),
            new PropertyMetadata(default(Style)));

        public Style StatusStyle { get { return (Style) this.GetValue(StatusStyleProperty); } set { this.SetValue(StatusStyleProperty, value); } }

        public static readonly DependencyProperty ThumbnailProperty = DependencyProperty.Register("Thumbnail",
            typeof(object),
            typeof(ProgressView),
            new PropertyMetadata(default(object)));

        public object Thumbnail { get { return this.GetValue(ThumbnailProperty); } set { this.SetValue(ThumbnailProperty, value); } }

        public MultiStepOperation Operation
        {
            get { return this._Operation; }
            set
            {
                if (this._Operation != null)
                {
                    this._Operation.MainOperationEnded -= this.Operation_OnMainOperationEnded;
                    this._Operation.MainOperationStepIncreasing -= this.Operation_OnMainOperationStepIncreasing;
                    this._Operation.CurrentOperationStepIncreased -= this.Operation_OnCurrentOperationStepIncreased;
                    this._Operation.MainOperationStarted -= this.Operation_MainOperationStarted;
                }
                this._Operation = value;
                this._Operation.MainOperationEnded += this.Operation_OnMainOperationEnded;
                this._Operation.MainOperationStepIncreasing += this.Operation_OnMainOperationStepIncreasing;
                this._Operation.CurrentOperationStepIncreased += this.Operation_OnCurrentOperationStepIncreased;
                this._Operation.MainOperationStarted += this.Operation_MainOperationStarted;
            }
        }

        public string DoneStatus { get; set; } = "Done.";

        public ProgressView()
        {
            this.InitializeComponent();
            this.StatusStyle = this.FindResource("LowlightBlock").As<Style>();
            this.CurrentOperationProgressBarValue = 0;
        }

        private void Operation_MainOperationStarted(object sender, MultiStepStartedLogEventArgs e) { this.MainOperationProgressBarMaxValue = e.Max.ToInt(); }

        private void Operation_OnCurrentOperationStepIncreased(object sender, MultiStepLogEventArgs e)
        {
            this.CurrentOperationProgressBarValue = e.Step.ToInt();
            this.CurrentOperationProgressBarMaxValue = e.Max.ToInt();
        }

        private void Operation_OnMainOperationStepIncreasing(object e1, MultiStepLogEventArgs e2)
        {
            this.Status = (e2.Log ?? string.Empty).ToString();
            this.MainOperationProgressBarValue = e2.Step.ToInt();
        }

        private void Operation_OnMainOperationEnded(object _, MultiStepEndedLogEventArgs __) { this.Status = this.DoneStatus; }
    }
}