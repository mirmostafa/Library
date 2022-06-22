using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Library.Helpers;

public static class ImageHelper
{
    /// <summary>
    /// تهیه تصویر بند انگشتی از یک تصویر
    /// بنداگشتی با حداکثر اندازه ۹۶ در ۹۶ تولید می شود
    /// اگر تصویر کوچکتر از ۹۶ باشد به ۹۶ می رسد و اگر بزرگتر باشد به این اندازه کوچک می شود
    /// </summary>
    /// <param name="imageData"></param>
    /// <returns></returns>
    public static byte[] ConvertToThumbnail(byte[] imageData, double maxWidth = 96, double maxHeight = 96)
    {
        Image image = new Bitmap(new MemoryStream(imageData));

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

    public static byte[] GetThumbnail(byte[] value, int thumbWidth = 96)
    {
        var image = Image.FromStream(new MemoryStream(value));
        var thumbValue = value;
        if (image.Height > thumbWidth || image.Width > thumbWidth)
        {
            if (image.Height >= image.Width)
            {
                var d = (double)image.Height / image.Width;

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
                var d = (double)image.Width / image.Height;

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
            var myEncoder = System.Drawing.Imaging.Encoder.Quality;
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

public static class ImageUtilities
{
    /// <summary>
    /// A quick lookup for getting image encoders
    /// </summary>
    private static Dictionary<string, ImageCodecInfo>? _encoders;

    /// <summary>
    /// A quick lookup for getting image encoders
    /// </summary>
    public static Dictionary<string, ImageCodecInfo> Encoders
    {
        //get accessor that creates the dictionary on demand
        get
        {
            //if the quick lookup isn't initialised, initialise it
            if (_encoders == null)
            {
                _encoders = new Dictionary<string, ImageCodecInfo>();
            }

            //if there are no codecs, try loading them
            if (_encoders.Count == 0)
            {
                //get all the codecs
                foreach (var codec in ImageCodecInfo.GetImageEncoders().Compact())
                {
                    if (codec.MimeType?.ToLower() is { } c)
                    {
                        //add each codec to the quick lookup
                        _encoders.Add(c, codec);
                    }
                }
            }

            //return the lookup
            return _encoders;
        }
    }

    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    public static Bitmap ResizeImage(Image image, int width, int height)
    {
        //a holder for the result
        var result = new Bitmap(width, height);
        //set the resolutions the same to avoid cropping due to resolution differences
        result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //use a graphics object to draw the resized image into the bitmap
        using (var graphics = Graphics.FromImage(result))
        {
            //set the resize quality modes to high quality
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //draw the image into the target bitmap
            graphics.DrawImage(image, 0, 0, result.Width, result.Height);
        }

        //return the resulting bitmap
        return result;
    }

    /// <summary>
    /// Saves an image as a jpeg image, with the given quality
    /// </summary>
    /// <param name="path">Path to which the image would be saved.</param>
    /// <param name="image"></param>
    /// <param name="quality">An integer from 0 to 100, with 100 being the
    /// highest quality</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// An invalid value was entered for image quality.
    /// </exception>
    public static void SaveJpeg(Stream path, Image image, int quality)
    {
        //ensure the quality is within the correct range
        if (quality is < 0 or > 100)
        {
            //create the error message
            var error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
            //throw a helpful exception
            throw new ArgumentOutOfRangeException(error);
        }

        //create an encoder parameter for the image quality
        var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //get the jpeg codec
        var jpegCodec = GetEncoderInfo("image/jpeg")!;

        //create a collection of all parameters that we will pass to the encoder
        var encoderParams = new EncoderParameters(1);
        //set the quality parameter for the codec
        encoderParams.Param[0] = qualityParam;
        //save the image using the codec and the parameters
        image.Save(path, jpegCodec, encoderParams);
    }

    /// <summary>
    /// Returns the image codec with the given mime type
    /// </summary>
    public static ImageCodecInfo? GetEncoderInfo(string mimeType)
    {
        //do a case insensitive search for the mime type
        var lookupKey = mimeType.ToLower();

        //the codec to return, default to null
        ImageCodecInfo? foundCodec = null;

        //if we have the encoder, get it to return
        if (Encoders.ContainsKey(lookupKey))
        {
            //pull the codec from the lookup
            foundCodec = Encoders[lookupKey];
        }

        return foundCodec;
    }
}