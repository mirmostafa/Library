using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Mohammad.Security.Principals
{
    public class CaptchaImage
    {
        public string Text { get; set; }
        public Size ImageSize { get; set; }
        public Font Font { get; set; }
        public CaptchaImage() { }

        public CaptchaImage(string text, Size imageSize, Font font)
        {
            this.Text = text;
            this.ImageSize = imageSize;
            this.Font = font;
        }

        public Bitmap GenerateImage() { return GenerateImage(this.Text, this.ImageSize, this.Font); }

        public static Bitmap GenerateImage(string text, Size imageSize, Font font)
        {
            var random = new Random();
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb);

            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, imageSize.Width, imageSize.Height);

            var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            graphics.FillRectangle(hatchBrush, rect);

            SizeF sizeF;
            float fontSize = rect.Height + 1;
            Font fontF;
            do
            {
                fontSize--;
                fontF = new Font(font.FontFamily.Name, fontSize, FontStyle.Bold);
                sizeF = graphics.MeasureString(text, fontF);
            } while (sizeF.Width > rect.Width);

            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};

            var path = new GraphicsPath();
            path.AddString(text, fontF.FontFamily, (int) fontF.Style, fontF.Size, rect, format);
            const float v = 4F;
            PointF[] points =
            {
                new PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                new PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                new PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                new PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
            };
            var matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            graphics.FillPath(hatchBrush, path);

            var max = Math.Max(rect.Width, rect.Height);
            for (var i = 0; i < (int) (rect.Width * rect.Height / 30F); i++)
            {
                var x = random.Next(rect.Width);
                var y = random.Next(rect.Height);
                var w = random.Next(max / 50);
                var h = random.Next(max / 50);
                graphics.FillEllipse(hatchBrush, x, y, w, h);
            }

            fontF.Dispose();
            hatchBrush.Dispose();
            graphics.Dispose();

            return bitmap;
        }
    }
}