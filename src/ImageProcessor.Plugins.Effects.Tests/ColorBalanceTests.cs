namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ColorBalanceTests : BaseTest
    {
        [Fact]
        public void ColorBalance_processes_image()
        {
            foreach (string file in this.images)
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
                        img.Save(string.Format("{0}/{1}_colorbalance.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }

        [Fact]
        public void Different_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ColorBalance processor = new ColorBalance();
                    processor.DynamicParameter = new ColorBalanceParameters();

                    ColorBalance processor2 = new ColorBalance();
                    processor2.DynamicParameter = new ColorBalanceParameters()
                    {
                        Blue = 155,
                        Green = 155,
                        Red = 155
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_colorbalance2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }
    }
}