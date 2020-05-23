#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Library40.Win.Helpers
{
	public static class ImageHelper
	{
		public static IEnumerable<Color> GetPixels(this Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			return bitmap.GetPixels(0, 0, bitmap.Width, bitmap.Height);
		}

		public static IEnumerable<Color> GetPixels(this Bitmap bitmap, int x, int y, int width, int height)
		{
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			return from x1 in Enumerable.Range(x, width) from y1 in Enumerable.Range(y, height) orderby y1, x1 select bitmap.GetPixel(x1, y1);
		}

		public static Bitmap Crop(this Bitmap bitmap, Rectangle rect)
		{
			var croppedBitmap = new Bitmap(rect.Width, rect.Height, bitmap.PixelFormat);

			using (var gfx = Graphics.FromImage(croppedBitmap))
				gfx.DrawImage(bitmap, 0, 0, rect, GraphicsUnit.Pixel);

			return croppedBitmap;
		}

		public static Bitmap Resize(this Bitmap bitmap, double ratio, InterpolationMode mode)
		{
			var width = (int)(bitmap.Width * ratio);
			var height = (int)(bitmap.Height * ratio);
			return bitmap.Resize(width, height, mode);
		}

		public static Bitmap Resize(this Bitmap bitmap, int width, int height, InterpolationMode mode)
		{
			var resizedBitmap = new Bitmap(width, height, bitmap.PixelFormat);

			using (var gfx = Graphics.FromImage(resizedBitmap))
			{
				gfx.InterpolationMode = mode;
				gfx.DrawImage(bitmap, 0, 0, width, height);
			}

			return resizedBitmap;
		}

		public static byte[] ToBytes(this Image image, ImageFormat format)
		{
			if (image == null)
				throw new ArgumentNullException("image");
			if (format == null)
				throw new ArgumentNullException("format");

			using (var stream = new MemoryStream())
			{
				image.Save(stream, format);
				return stream.ToArray();
			}
		}

		public static ImageCodecInfo GetImageCodecInfo(this ImageFormat imageFormat)
		{
			if (imageFormat == null)
				throw new ArgumentNullException("imageFormat");

			return ImageCodecInfo.GetImageEncoders().FirstOrDefault(i => i.Clsid == imageFormat.Guid);
		}

		public static Rectangle GetBounds(this Image image)
		{
			return new Rectangle(0, 0, image.Width, image.Height);
		}

		public static Rectangle Surround(this Point p, int distance)
		{
			return new Rectangle(p.X - distance, p.Y - distance, distance * 2, distance * 2);
		}
	}
}