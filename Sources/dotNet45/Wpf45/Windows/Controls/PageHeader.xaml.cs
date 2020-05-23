using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Mohammad.Helpers;
using Mohammad.Wpf.Windows.Media;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for PageHeader.xaml
    /// </summary>
    public partial class PageHeader
    {
        private bool _AutoVisible;
        private bool _Loading;

        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register("Owner",
            typeof(IDescriptiveObject),
            typeof(PageHeader),
            new PropertyMetadata(default(IDescriptiveObject)));

        public IDescriptiveObject Owner
        {
            get { return (IDescriptiveObject) this.GetValue(OwnerProperty); }
            set
            {
                this.SetValue(OwnerProperty, value);
                this.OnOwnerChanged();
                this.OnPropertyChanged();
            }
        }

        public string AppTitle
        {
            get { return this.AppTitleBlock.Text; }
            set
            {
                if (this.UseWindows8TextingStyle)
                    value = value.ToUpper();
                this.AppTitleBlock.Text = value;
            }
        }

        public string PageTitle
        {
            get { return this.PageTitleBlock.Text; }
            set
            {
                if (this.AutoVisible)
                    this.Visibility = value.IsNullOrEmpty() ? Visibility.Collapsed : Visibility.Visible;
                if (value.IsNullOrEmpty())
                    return;
                if (this.UseWindows8TextingStyle)
                    value = value.ToLower();
                if (!this._Loading)
                    this.Animate();
                this.PageTitleBlock.Text = value;
                this.PageTitleBlurBlock.Text = value;
            }
        }

        public bool AutoVisible
        {
            get { return this._AutoVisible; }
            set
            {
                if (value.Equals(this._AutoVisible))
                    return;
                this._AutoVisible = value;
                this.OnPropertyChanged();
            }
        }

        public string PageDescription { get { return this.PageDescriptionBlock.Text; } set { this.PageDescriptionBlock.Text = value; } }

        public ImageSource LogoSource
        {
            get { return this.PageLogoImage.Source; }
            set
            {
                this.PageLogoImage.Source = value;
                if (value != null)
                    this.PageLogoImage.Visibility = Visibility.Visible;
            }
        }

        public bool UseWindows8TextingStyle { get; set; }
        public PageHeaderAnimation Animation { get; set; } = PageHeaderAnimation.Simple;

        public PageHeader()
        {
            this.UseWindows8TextingStyle = true;
            this._Loading = true;
            this.InitializeComponent();
        }

        private async Task<bool> Animate()
        {
            switch (this.Animation)
            {
                case PageHeaderAnimation.Simple:
                    Animations.FadeIn(this.PageTitleBlock, 200);
                    Animations.FadeIn(this.PageTitleBlurBlock);
                    return true;
                case PageHeaderAnimation.Navigation:
                    this.AppTitleBlock.Visibility = Visibility.Hidden;
                    this.PageTitleBlock.Visibility = Visibility.Hidden;
                    this.PageDescriptionBlock.Visibility = Visibility.Hidden;
                    await Task.Delay(300);
                    this.PageTitleBlock.Visibility = Visibility.Visible;
                    Animations.SkewIn(this.PageTitleBlock);
                    await Task.Delay(1000);
                    Animations.FadeIn(this.PageTitleBlurBlock);
                    //Animations.FadeIn(this.PageTitleBlurBlock);
                    this.AppTitleBlock.Visibility = Visibility.Visible;
                    Animations.FadeIn(this.AppTitleBlock);
                    this.PageDescriptionBlock.Visibility = Visibility.Visible;
                    Animations.FadeIn(this.PageDescriptionBlock);
                    return true;
                case PageHeaderAnimation.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnOwnerChanged()
        {
            this.Owner.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != "Title" && e.PropertyName != "Description")
                    return;
                var p = sender.As<IDescriptiveObject>();
                this.PageTitle = p.Title;
                this.PageDescription = p.Description;
            };
            this.PageTitle = this.Owner.Title;
            this.PageDescription = this.Owner.Description;
        }

        private async void PageHeade_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Owner != null)
            {
                if (this.AppTitle.IsNullOrEmpty())
                    if (ApplicationHelper.ProductTitle.IsNullOrEmpty() && ApplicationHelper.ApplicationTitle != null)
                        this.AppTitle = $"{ApplicationHelper.ProductTitle} - {ApplicationHelper.ApplicationTitle}";
                    else if (ApplicationHelper.ProductTitle.IsNullOrEmpty())
                        this.AppTitle = ApplicationHelper.ProductTitle;
                    else if (ApplicationHelper.ApplicationTitle.IsNullOrEmpty())
                        this.AppTitle = ApplicationHelper.ApplicationTitle;

                if (this.PageTitle.IsNullOrEmpty())
                    this.PageTitle = this.Owner.Title;
                if (this.PageDescription.IsNullOrEmpty())
                    this.PageDescription = this.Owner.Description ?? ApplicationHelper.Description;
            }
            if (!await this.Animate())
            {
                this.PageTitleBlock.Visibility = Visibility.Visible;
                this.PageTitleBlurBlock.Visibility = Visibility.Visible;
                this.AppTitleBlock.Visibility = Visibility.Visible;
                this.PageDescriptionBlock.Visibility = Visibility.Visible;
            }
            this._Loading = false;
        }

        private void PageHeader_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.Owner.As<Window>()?.DragMove();
        }
    }

    public enum PageHeaderAnimation
    {
        Simple,
        Navigation,
        None
    }
}