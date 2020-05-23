using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Mohammad.Wpf.Windows.Controls
{
    public class GifImage : Image
    {
        private Int32Animation _Animation;
        private GifBitmapDecoder _GifDecoder;
        private bool _IsInitialized;

        public static readonly DependencyProperty FrameIndexProperty = DependencyProperty.Register("FrameIndex",
            typeof(int),
            typeof(GifImage),
            new UIPropertyMetadata(0, ChangingFrameIndex));

        public static readonly DependencyProperty AutoStartProperty = DependencyProperty.Register("AutoStart",
            typeof(bool),
            typeof(GifImage),
            new UIPropertyMetadata(false, AutoStartPropertyChanged));

        private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
                (sender as GifImage).StartAnimation();
        }

        /// <summary>
        ///     Defines whether the animation starts on it's own
        /// </summary>
        public bool AutoStart { get { return (bool) this.GetValue(AutoStartProperty); } set { this.SetValue(AutoStartProperty, value); } }

        private static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            var gifImage = obj as GifImage;
            gifImage.Source = gifImage._GifDecoder.Frames[(int) ev.NewValue];
        }

        public int FrameIndex { get { return (int) this.GetValue(FrameIndexProperty); } set { this.SetValue(FrameIndexProperty, value); } }

        public static readonly DependencyProperty GifSourceProperty = DependencyProperty.Register("GifSource",
            typeof(string),
            typeof(GifImage),
            new UIPropertyMetadata(string.Empty, GifSourcePropertyChanged));

        private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { (sender as GifImage).Initialize(); }
        public string GifSource { get { return (string) this.GetValue(GifSourceProperty); } set { this.SetValue(GifSourceProperty, value); } }
        static GifImage() { VisibilityProperty.OverrideMetadata(typeof(GifImage), new FrameworkPropertyMetadata(VisibilityPropertyChanged)); }

        private void Initialize()
        {
            this._GifDecoder = new GifBitmapDecoder(new Uri("pack://application:,,," + this.GifSource),
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            this._Animation = new Int32Animation(0,
                                  this._GifDecoder.Frames.Count - 1,
                                  new Duration(new TimeSpan(0,
                                      0,
                                      0,
                                      this._GifDecoder.Frames.Count / 10,
                                      (int) ((this._GifDecoder.Frames.Count / 10.0 - this._GifDecoder.Frames.Count / 10) * 1000))))
                              {
                                  RepeatBehavior =
                                      RepeatBehavior.Forever
                              };
            this.Source = this._GifDecoder.Frames[0];

            this._IsInitialized = true;
        }

        private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Visibility) e.NewValue == Visibility.Visible)
                ((GifImage) sender).StartAnimation();
            else
                ((GifImage) sender).StopAnimation();
        }

        /// <summary>
        ///     Starts the animation
        /// </summary>
        public void StartAnimation()
        {
            if (!this._IsInitialized)
                this.Initialize();

            this.BeginAnimation(FrameIndexProperty, this._Animation);
        }

        /// <summary>
        ///     Stops the animation
        /// </summary>
        public void StopAnimation()
        {
            this.BeginAnimation(FrameIndexProperty, null);
        }
    }
}