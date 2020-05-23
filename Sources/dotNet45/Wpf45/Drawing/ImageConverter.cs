using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mohammad.Compression;
using Mohammad.Exceptions;
using Mohammad.Wpf.Helpers;

namespace Mohammad.Wpf.Drawing
{
    public static class ImageConverter
    {
        public static byte[] ConvertToBinary(BitmapSource source, ImageEncoding encoding)
        {
            var memStream = new MemoryStream();
            var encoder = GetProperEncoder(encoding);
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memStream);
            return memStream.GetBuffer();
        }

        public static byte[] ConvertToBinary(BitmapSource source)
        {
            var image = source as BitmapImage;
            if (image != null)
            {
                //find encoding from StreamSource as MemoryStream 
                var bitmapImage = image;
                if (!(bitmapImage.StreamSource is MemoryStream))
                    throw new Exception("undefined format type");
                var stream = (MemoryStream) bitmapImage.StreamSource;
                var bytes = stream.ToArray();
                byte[] uncompressed;
                var encoding = GetImageEncoding(bytes, out uncompressed);
                return ConvertToBinary(image, encoding);
            }
            var metadata = source.Metadata as BitmapMetadata;
            if (metadata != null)
                //find encoding from Metadate format
                return ConvertToBinary(source, GetImageEncoding(metadata.Format));
            if (source.Format == PixelFormats.BlackWhite)
                //set BMP Encoding for BlackAndWhite formats
                return ConvertToBinary(source, ImageEncoding.Bmp);
            throw new Exception("undefined format type");
        }

        public static byte[] ConvertToBinary(ImageSource source, ImageEncoding encoding)
        {
            var bitmapSource = source as BitmapSource;
            if (bitmapSource != null)
                return ConvertToBinary(bitmapSource, encoding);
            throw new ArgumentException($"{nameof(bitmapSource)} must be a BitmapSource");
        }

        public static ImageSource LoadImage(string path, bool isResource, bool canThrowException)
        {
            if (!string.IsNullOrEmpty(path))
                try
                {
                    if (isResource)
                        return ApplicationHelper.GetImageSource(path);
                    var bmImage = new BitmapImage();
                    bmImage.BeginInit();
                    var fullPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + path.Replace("/", "\\");
                    bmImage.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bmImage.EndInit();
                    return bmImage;
                }
                catch (Exception)
                {
                    if (canThrowException)
                        throw;
                    return null;
                }
            return null;
        }

        public static bool TryConvert(byte[] source, ImageEncoding encoding, out BitmapImage bitmapImage)
        {
            try
            {
                bitmapImage = ConvertToBitmapImage(source, encoding);
                return true;
            }
            catch (FileFormatException) {}
            catch (NotSupportedException) {}
            bitmapImage = null;
            return false;
        }

        public static BitmapSource ConvertToBitmapSource(byte[] bytes)
        {
            byte[] uncompressed;
            var encoding = GetImageEncoding(bytes, out uncompressed);
            return ConvertToBitmapSource(uncompressed, encoding);
        }

        public static BitmapSource ConvertToBitmapSource(byte[] bytes, ImageEncoding encoding)
        {
            Stream imageStreamSource = new MemoryStream(bytes, false);
            imageStreamSource.Seek(0, SeekOrigin.Begin);
            var decoder = GetProperDecoder(encoding, imageStreamSource);
            BitmapSource bitmapSource = decoder.Frames[0];
            imageStreamSource.Close();
            return bitmapSource;
        }

        private static BitmapSource ConvertToBitmapSource(Bitmap image)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static BitmapSource ConvertToBitmapSource(string imagePath)
        {
            var imageStreamSource = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = new byte[imageStreamSource.Length];
            imageStreamSource.Seek(0, SeekOrigin.Begin);
            imageStreamSource.Read(result, 0, (int) imageStreamSource.Length);
            imageStreamSource.Seek(0, SeekOrigin.Begin);
            byte[] uncompressed;
            var decoder = GetProperDecoder(GetImageEncoding(result, out uncompressed), imageStreamSource);
            //new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];
            return bitmapSource;
        }

        public static BitmapImage ConvertToBitmapImage(byte[] bytes, bool isBlackWhite)
        {
            byte[] unCompressedBytes;
            var encoding = GetImageEncoding(bytes, out unCompressedBytes);
            return ConvertToBitmapImage(unCompressedBytes, encoding, isBlackWhite);
        }

        public static BitmapImage ConvertToBitmapImage(byte[] bytes, ImageEncoding encoding) { return ConvertToBitmapImage(bytes, encoding, false); }

        public static BitmapImage ConvertToBitmapImage(byte[] bytes, ImageEncoding encoding, bool isBlackAndWhite)
        {
            try
            {
                var memoryStream = new MemoryStream(bytes);
                var bitmap = new Bitmap(memoryStream);
                return ConvertToBitmapImage(bitmap, encoding, isBlackAndWhite);
            }
            catch (Exception ex)
            {
                throw new LibraryException("Caspian.Banking.Infra.Common.ImageCorruptionException", ex);
            }
        }

        private static BitmapImage ConvertToBitmapImage(Bitmap image, ImageEncoding encoding, bool isBlackAndWhite)
        {
            var bitmapSource = ConvertToBitmapSource(image);
            if (isBlackAndWhite)
                bitmapSource = ConvertToBlackAndWhite(bitmapSource);
            return ConvertToBitmapImage(bitmapSource, encoding);
        }

        public static BitmapImage ConvertToBitmapImage(BitmapSource bitmapSource, ImageEncoding encoding)
        {
            BitmapImage bitmapImage;
            using (var memoryStream = new MemoryStream())
            {
                bitmapImage = new BitmapImage();
                var encoder = GetProperEncoder(encoding);
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memoryStream);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();
                memoryStream.Close();
            }

            return bitmapImage;
        }

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private static extern uint FindMimeFromData(uint pBc, [MarshalAs(UnmanagedType.LPStr)] string pwzUrl, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            uint cbSize, [MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed, uint dwMimeFlags, out uint ppwzMimeOut, uint dwReserverd);

        private static string GetMimeType(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var buffer = new byte[256];
            Array.Copy(bytes, buffer, bytes.Length > 256 ? 256 : bytes.Length);
            try
            {
                uint mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                var mimeTypePtr = new IntPtr(mimetype);
                var mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception)
            {
                return "unknown/unknown";
            }
        }

        private static ImageEncoding GetImageEncoding(byte[] bytes, out byte[] unCompressedBytes)
        {
            var mimeType = GetMimeType(bytes);
            if (mimeType.Contains("zip"))
            {
                bytes = ByteCompressor.Decompress(bytes);
                mimeType = GetMimeType(bytes);
            }
            unCompressedBytes = bytes;
            return GetImageEncoding(mimeType);
        }

        private static ImageEncoding GetImageEncoding(string formatOrMimeType)
        {
            formatOrMimeType = formatOrMimeType.ToLower();
            if (string.IsNullOrEmpty(formatOrMimeType))
                throw new InvalidOperationException("undefined mime type");
            if (formatOrMimeType.Contains("png"))
                return ImageEncoding.Png;
            if (formatOrMimeType.Contains("jpg") || formatOrMimeType.Contains("jpeg"))
                return ImageEncoding.Jpeg;
            if (formatOrMimeType.Contains("tif"))
                return ImageEncoding.Tiff;
            if (formatOrMimeType.Contains("gif"))
                return ImageEncoding.Gif;
            return ImageEncoding.Bmp;
        }

        private static BitmapEncoder GetProperEncoder(ImageEncoding encoding)
        {
            BitmapEncoder encoder;
            if (encoding == ImageEncoding.Jpeg)
                encoder = new JpegBitmapEncoder();
            else if (encoding == ImageEncoding.Png)
                encoder = new PngBitmapEncoder();
            else if (encoding == ImageEncoding.Gif)
                encoder = new GifBitmapEncoder();
            else if (encoding == ImageEncoding.Tiff)
                encoder = new TiffBitmapEncoder();
            else //if (encoding == ImageEncoding.BMP)
                encoder = new BmpBitmapEncoder();
            //else
            //{
            //    throw new NotImplementedException("BitmapEncoder");
            //}
            return encoder;
        }

        private static BitmapDecoder GetProperDecoder(ImageEncoding encoding, Stream imageStreamSource)
        {
            BitmapDecoder decoder;
            if (encoding == ImageEncoding.Jpeg)
                decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            else if (encoding == ImageEncoding.Gif)
                decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            else if (encoding == ImageEncoding.Png)
                decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            else if (encoding == ImageEncoding.Tiff)
                decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            else //if (encoding == ImageEncoding.BMP)
                decoder = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            //else
            //{
            //    throw new NotImplementedException("BitmapEncoder");
            //}
            return decoder;
        }

        public static BitmapSource ConvertToBlackAndWhite(Image image)
        {
            var bitmap = new Bitmap(image);
            var bitmapSource = ConvertToBitmapSource(bitmap);

            return ConvertToBlackAndWhite(bitmapSource);
        }

        public static BitmapSource ConvertToBlackAndWhite(BitmapSource bitmapSource)
        {
            var newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = bitmapSource;
            newFormatedBitmapSource.DestinationFormat = PixelFormats.BlackWhite;
            newFormatedBitmapSource.EndInit();
            return newFormatedBitmapSource;
        }
    }
}