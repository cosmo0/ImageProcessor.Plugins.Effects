namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using Xunit;
    using FluentAssertions;

    public class ColorBalanceTests
    {
        [Fact]
        public void ColorBalance_processes_image()
        {
            string imgs = Path.Combine(Path.GetDirectoryName(typeof(ColorBalanceTests).Assembly.Location), "img");

            foreach (string file in Directory.GetFiles(imgs))
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ColorBalance processor = new ColorBalance();
                    processor.DynamicParameter = new ColorBalanceParameters();

                    // act
                    Action act = () =>
                        {
                            Image img = processor.ProcessImage(factory);
                            img.Save(string.Format("{0}/{1}_result.jpg", imgs, Path.GetFileNameWithoutExtension(file)));
                        };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}