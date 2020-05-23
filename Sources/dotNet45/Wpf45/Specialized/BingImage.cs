using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mohammad.Specialized;
using Mohammad.Threading.Tasks;

namespace Mohammad.Wpf.Specialized
{
    /// <summary>
    ///     Provides an attached property determining the current Bing image and assigning it to an image or imagebrush.
    /// </summary>
    public static class BingImage
    {
        public static readonly DependencyProperty UseBingImageProperty = DependencyProperty.RegisterAttached("UseBingImage",
            typeof(bool),
            typeof(BingService),
            new PropertyMetadata(OnUseBingImageChanged));

        private static BitmapImage _CachedBingImage;

        private static async void OnUseBingImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool) e.NewValue;
            var image = o as Image;
            var imageBrush = o as ImageBrush;

            if (!newValue || image == null && imageBrush == null)
                return;

            if (_CachedBingImage == null)
            {
                var url = await Async.Run(BingService.GetCurrentBingImageUrl);
                if (url != null)
                    _CachedBingImage = new BitmapImage(url);
            }

            if (_CachedBingImage != null)
                if (image != null)
                    image.Source = _CachedBingImage;
                else
                    imageBrush.ImageSource = _CachedBingImage;
        }

        public static bool GetUseBingImage(DependencyObject o) { return (bool) o.GetValue(UseBingImageProperty); }
        public static void SetUseBingImage(DependencyObject o, bool value) { o.SetValue(UseBingImageProperty, value); }

        public static async Task<BitmapImage> GetBingImageAsync()
        {
            if (_CachedBingImage == null)
            {
                var url = await Async.Run(BingService.GetCurrentBingImageUrl);
                if (url != null)
                {
                    //_CachedBingImage = new BitmapImage(url);
                    _CachedBingImage = new BitmapImage();
                    _CachedBingImage.BeginInit();
                    _CachedBingImage.UriSource = url;
                    _CachedBingImage.EndInit();
                }
            }
            return _CachedBingImage;
        }

        public static ImageSource GetBingImage() { return GetBingImageAsync().Result; }
    }
}