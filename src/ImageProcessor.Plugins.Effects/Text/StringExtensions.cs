namespace ImageProcessor.Plugins.Effects.Text
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;

    public static class StringExtensions
    {
        /// <summary>
        /// Randomly sorts a string
        /// </summary>
        /// <param name="stringValue">The string to sort</param>
        /// <returns>The sorted string</returns>
        public static string RandomStringSort(this string stringValue)
        {
            char[] charArray = stringValue.ToCharArray();

            Random randomIndex = new Random((byte)charArray[0]);
            int iterator = charArray.Length;

            while (iterator > 1)
            {
                iterator -= 1;
                int nextIndex = randomIndex.Next(iterator + 1);
                char nextValue = charArray[nextIndex];
                charArray[nextIndex] = charArray[iterator];
                charArray[iterator] = nextValue;
            }

            return new string(charArray);
        }

        /// <summary>
        /// Converts text to an image
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="font">The font to use</param>
        /// <param name="factor">The zoom factor</param>
        /// <returns>The generated image</returns>
        public static Bitmap TextToImage(this string text, Font font, float factor)
        {
            Bitmap textBitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(textBitmap);

            int width = (int)Math.Ceiling(graphics.MeasureString(text, font).Width * factor);
            int height = (int)Math.Ceiling(graphics.MeasureString(text, font).Height * factor);

            graphics.Dispose();

            textBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            graphics = Graphics.FromImage(textBitmap);
            graphics.Clear(Color.Black);

            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            graphics.ScaleTransform(factor, factor);
            graphics.DrawString(text, font, Brushes.White, new PointF(0, 0));

            graphics.Flush();
            graphics.Dispose();

            return textBitmap;
        }
    }
}