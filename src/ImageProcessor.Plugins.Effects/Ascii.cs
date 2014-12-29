namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Text;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Plugins.Effects.Imaging;
    using ImageProcessor.Plugins.Effects.Text;
    using ImageProcessor.Processors;

    /// <summary>
    /// ASCII-style filter processor
    /// http://softwarebydefault.com/2013/07/14/image-ascii-art/
    /// </summary>
    public class Ascii : IGraphicsProcessor
    {
        private static string colorCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Gets or sets the processor parameter
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image through the filter
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = new Bitmap(factory.Image);
            AsciiParameters parameters = this.DynamicParameter;
            int pixelBlockSize = parameters.PixelPerCharacter;

            try
            {
                BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                    sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                StringBuilder asciiArt = new StringBuilder();

                int offset = 0;

                int rows = sourceBitmap.Height / pixelBlockSize;
                int columns = sourceBitmap.Width / pixelBlockSize;

                if (parameters.CharacterCount > 0)
                {
                    colorCharacters = GenerateRandomString(parameters.CharacterCount);
                }

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        int avgBlue = 0;
                        int avgGreen = 0;
                        int avgRed = 0;

                        for (int pY = 0; pY < pixelBlockSize; pY++)
                        {
                            for (int pX = 0; pX < pixelBlockSize; pX++)
                            {
                                offset = y * pixelBlockSize * sourceData.Stride + x * pixelBlockSize * 4;

                                offset += pY * sourceData.Stride;
                                offset += pX * 4;

                                avgBlue += pixelBuffer[offset];
                                avgGreen += pixelBuffer[offset + 1];
                                avgRed += pixelBuffer[offset + 2];
                            }
                        }

                        avgBlue = avgBlue / (pixelBlockSize * pixelBlockSize);
                        avgGreen = avgGreen / (pixelBlockSize * pixelBlockSize);
                        avgRed = avgRed / (pixelBlockSize * pixelBlockSize);

                        asciiArt.Append(GetColorCharacter(avgBlue, avgGreen, avgRed));
                    }

                    asciiArt.Append("\r\n");
                }

                Font textFont = new System.Drawing.Font("Courier New", parameters.FontSize, FontStyle.Bold | FontStyle.Italic);
                sourceBitmap = asciiArt.ToString().TextToImage(textFont, 1);
            }
            catch (Exception ex)
            {
                if (sourceBitmap != null)
                {
                    sourceBitmap.Dispose();
                }

                throw new ImageProcessingException("Error processing image with " + this.GetType().Name, ex);
            }

            return sourceBitmap;
        }

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="maxSize">The size of the string</param>
        /// <returns>A random string</returns>
        private static string GenerateRandomString(int maxSize)
        {
            StringBuilder stringBuilder = new StringBuilder(maxSize);
            Random randomChar = new Random();

            for (int k = 0; k < maxSize; k++)
            {
                char charValue = (char)(Math.Floor(255 * randomChar.NextDouble() * 4));

                if (stringBuilder.ToString().IndexOf(charValue) != -1)
                {
                    charValue = (char)(Math.Floor((byte)charValue * randomChar.NextDouble()));
                }

                if (Char.IsControl(charValue) == false &&
                    Char.IsPunctuation(charValue) == false &&
                    stringBuilder.ToString().IndexOf(charValue) == -1)
                {
                    stringBuilder.Append(charValue);

                    randomChar = new Random((int)((byte)charValue * (k + 1) + DateTime.Now.Ticks));
                }
                else
                {
                    randomChar = new Random((int)((byte)charValue * (k + 1) + DateTime.UtcNow.Ticks));
                    k -= 1;
                }
            }

            return stringBuilder.ToString().RandomStringSort();
        }

        /// <summary>
        /// Gets a character assigned for a color
        /// </summary>
        /// <param name="blue">The blue level</param>
        /// <param name="green">The green level</param>
        /// <param name="red">The red level</param>
        /// <returns>The character</returns>
        private static string GetColorCharacter(int blue, int green, int red)
        {
            int intensity = (blue + green + red) / 3 * (colorCharacters.Length - 1) / 255;

            string colorChar = colorCharacters.Substring(intensity, 1).ToUpper();
            colorChar += colorChar.ToLower();

            return colorChar;
        }
    }
}