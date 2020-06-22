using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Helpers
{
    public static class ImageHelpers
    {
        /// <summary>
        ///     Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>
        ///     The resized image.
        /// </returns>
        public static Image ResizeImage(Image image, int width, int height, InterpolationMode mode = InterpolationMode.Default)
        {
            var resizedBitmap = new Bitmap(width, height, image.PixelFormat);

            var gfx = Graphics.FromImage(resizedBitmap);
            gfx.InterpolationMode = mode;
            gfx.DrawImage(image, 0, 0, width, height);

            return resizedBitmap;
        }

        /// <summary>
        ///     Gets the new size by size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static (int Width, int Height) GetNewSizeBySize(int width, int height, int size)
        {
            if (width < height)
            {
                var scale = Convert.ToDouble(height) / Convert.ToDouble(width);
                return (Convert.ToInt32(size / scale), size);
            }
            else
            {
                var scale = Convert.ToDouble(width) / Convert.ToDouble(height);
                return (size, Convert.ToInt32(size / scale));
            }
        }

        /// <summary>
        ///     Gets the new size by size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static (int Width, int Height) GetNewSizeBySize(Image image, int size) => GetNewSizeBySize(image.Width, image.Height, size);

        /// <summary>
        ///     Gets the resizer by size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Func<int, (int Width, int Height)> GetResizerBySize(int width, int height) => width < height
            ? (Func<int, (int Width, int Height)>)(size =>
            {
                var scale = Convert.ToDouble(height) / Convert.ToDouble(width);
                return (Convert.ToInt32(size / scale), size);
            })
            : (size =>
            {
                var scale = Convert.ToDouble(width) / Convert.ToDouble(height);
                return (size, Convert.ToInt32(size / scale));
            });

        /// <summary>
        ///     Gets the resizer by size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static Func<int, (int Width, int Height)> GetResizerBySize(Image image) => GetResizerBySize(image.Width, image.Height);

        public static void ResizeAndSaveImage(string filePath, int size)
        {
            using (var image = Image.FromFile(filePath))
                ResizeAndSaveImage(image, size, filePath);
        }

        /// <summary>
        ///     Resizes the and saves image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="path">The path.</param>
        public static void ResizeAndSaveImage(Image image, int width, int height, string path)
        {
            using (var newImage = ResizeImage(image, width, height))
                newImage.Save(path);
        }

        /// <summary>
        ///     Resizes the and saves image.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="path">The path.</param>
        public static void ResizeAndSaveImage(Stream stream, int width, int height, string path)
        {
            ResizeAndSaveImage(Image.FromStream(stream), width, height, path);
        }

        /// <summary>
        ///     Resizes the and saves image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        public static void ResizeAndSaveImage(Image image, int size, string path)
        {
            var newSize = GetNewSizeBySize(image, size);
            ResizeAndSaveImage(image, newSize.Width, newSize.Height, path);
        }

        /// <summary>
        ///     Resizes the and saves image.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        public static void ResizeAndSaveImage(Stream stream, int size, string path)
        {
            ResizeAndSaveImage(Image.FromStream(stream), size, path);
        }
    }
}