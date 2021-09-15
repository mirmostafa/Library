
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

public static class ImageHelper
{
    /// <summary>
    /// تهیه تصویر بند انگشتی از یک تصویر
    /// بنداگشتی با حداکثر اندازه ۹۶ در ۹۶ تولید می شود
    /// اگر تصویر کوچکتر از ۹۶ باشد به ۹۶ می رسد و اگر بزرگتر باشد به این اندازه کوچک می شود
    /// </summary>
    /// <param name="imageData"></param>
    /// <returns></returns>
    public static byte[] ConvertToThumbnail(this byte[] imageData)
    {
        Image image = new Bitmap(new MemoryStream(imageData));

        const double maxWidth = 96;
        const double maxHeight = 96;

        var aspectRatio = image.Width / image.Height;
        double scaleFactor = 0;
        double imageHeight = image.Height;
        double imageWidth = image.Width;

        scaleFactor = 1 > aspectRatio ? maxHeight / imageHeight : maxWidth / imageWidth;

        var newWidth = (int)(imageWidth * scaleFactor);
        var newHeight = (int)(imageHeight * scaleFactor);
        var img = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);

        using var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);

        return ms.ToArray();
    }

    public static byte[] GetThumbnail(byte[] value, int thumbWidth)
    {
        var image = Image.FromStream(new MemoryStream(value));
        var thumbValue = value;
        if (image.Height > thumbWidth || image.Width > thumbWidth)
        {
            if (image.Height >= image.Width)
            {
                var d = ((double)image.Height) / image.Width;

                var thumbHeight = (int)Math.Round(thumbWidth * d);

                Image thumb = new Bitmap(thumbWidth, thumbHeight);
                using (var grfx = Graphics.FromImage(thumb))
                {
                    grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // necessary setting for proper work with image borders
                    grfx.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    grfx.DrawImage(image, new Rectangle(new Point(0, 0), new Size(thumbWidth, thumbHeight)), new Rectangle(new Point(0, 0), image.Size), GraphicsUnit.Pixel);
                }

                var target = new Bitmap(new Rectangle(0, 0, thumbWidth, thumbWidth).Width, new Rectangle(0, 0, thumbWidth, thumbWidth).Height);

                using (var g = Graphics.FromImage(target))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    var i = 0;
                    if (thumbHeight > thumbWidth)
                    {
                        i = (thumbHeight - thumbWidth) / 2;
                    }

                    g.DrawImage(thumb, new Rectangle(0, 0, target.Width, target.Height), new Rectangle(0, i, thumbWidth, thumbWidth), GraphicsUnit.Pixel);
                }

                using var memoryStream = new MemoryStream();
                target.Save(memoryStream, ImageFormat.Jpeg);
                thumbValue = memoryStream.ToArray();
            }
            else
            {
                var d = ((double)image.Width) / image.Height;

                var thumbHeight = thumbWidth;
                thumbWidth = (int)Math.Round(thumbHeight * d);

                Image thumb = new Bitmap(thumbWidth, thumbHeight);
                using (var grfx = Graphics.FromImage(thumb))
                {
                    grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // necessary setting for proper work with image borders
                    grfx.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    grfx.DrawImage(image, new Rectangle(new Point(0, 0), new Size(thumbWidth, thumbHeight)), new Rectangle(new Point(0, 0), image.Size), GraphicsUnit.Pixel);
                }

                var target = new Bitmap(new Rectangle(0, 0, thumbHeight, thumbHeight).Width, new Rectangle(0, 0, thumbHeight, thumbHeight).Height);

                using (var g = Graphics.FromImage(target))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    var i = 0;
                    if (thumbHeight < thumbWidth)
                    {
                        i = (thumbWidth - thumbHeight) / 2;
                    }

                    g.DrawImage(thumb, new Rectangle(0, 0, target.Width, target.Height), new Rectangle(i, 0, thumbHeight, thumbHeight), GraphicsUnit.Pixel);
                }

                using var memoryStream = new MemoryStream();
                target.Save(memoryStream, ImageFormat.Jpeg);
                thumbValue = memoryStream.ToArray();
            }

        }
        return thumbValue;
    }

    [Obsolete]
    public static byte[] ScaleImage(byte[] signature, int width, int height, Rectangle destRect)
    {
        byte[] scaled;
        using (var scaledSignature = new MemoryStream())
        {
            using (var frame = Image.FromStream(new MemoryStream(signature)))
            {
                using var bitmap = new Bitmap(width, height);
                bitmap.MakeTransparent(Color.Transparent);
                using (var image = Graphics.FromImage(bitmap))
                {
                    image.Clear(Color.Transparent);
                    image.DrawImage(frame, destRect,
                        new Rectangle(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
                    _ = image.Save();
                }
                bitmap.Save(scaledSignature, ImageFormat.Png);
            }
            scaled = scaledSignature.ToArray();
        }
        return scaled;
    }

    public static byte[] ConvertToTransparent(byte[] bytes, Color color)
    {
        byte[] converted;
        using (var convertedSignature = new MemoryStream())
        {
            using (var frame = new Bitmap(new MemoryStream(bytes)))
            {
                frame.MakeTransparent(color);
                frame.Save(convertedSignature, ImageFormat.Png);
            }
            converted = convertedSignature.ToArray();
        }
        return converted;
    }

    public static int GetNumberOfPages(byte[] content)
    {
        using var stream = new MemoryStream(content);
        using var fromStream = Image.FromStream(stream);
        return fromStream.GetFrameCount(FrameDimension.Page);
    }

    public static bool IsImageFile(byte[] content)
    {
        try
        {
            using (var stream = new MemoryStream(content))
            {
                using (Image.FromStream(stream))
                {
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static byte[] GetImagePage(byte[] inputImage, int pageNumber, int? pageDpi, long? quality)
    {
        using var stream = new MemoryStream(inputImage);
        using var image = Image.FromStream(stream);
        int frameCount;
        try
        {
            frameCount = image.GetFrameCount(FrameDimension.Page);
        }
        catch (Exception)
        {

            var dimension = new FrameDimension(image.FrameDimensionsList[0]);
            frameCount = image.GetFrameCount(dimension);

        }
        if (frameCount > 1)
        {
            _ = image.SelectActiveFrame(FrameDimension.Page, pageNumber - 1);
        }
        if (quality == null)
        {
            quality = 100;
        }

        using var ms = new MemoryStream();
        if (quality.Value is < 100 and >= 0)
        {
            var myEncoderParameters = new EncoderParameters(1);
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID 
            // for the Quality parameter category.
            var myEncoder = Encoder.Quality;
            var myEncoderParameter = new EncoderParameter(myEncoder, quality.Value);
            myEncoderParameters.Param[0] = myEncoderParameter;
            if (jpgEncoder is null)
            {
                throw new();
            }

            image.Save(ms, jpgEncoder, myEncoderParameters);
        }
        else
        {
            image.Save(ms, ImageFormat.Jpeg);
        }

        var array = ms.ToArray();
        return array;

    }

    public static ImageCodecInfo? GetEncoder(ImageFormat format)
        => ImageCodecInfo
                    .GetImageDecoders()
                    .FirstOrDefault(codec => codec.FormatID == format.Guid);
}