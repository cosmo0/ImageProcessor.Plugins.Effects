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
            foreach (string file in Directory.GetFiles(@".\img"))
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
                            img.Save(string.Format(@".\img\{0}_result.jpg", Path.GetFileNameWithoutExtension(file)));
                        };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}