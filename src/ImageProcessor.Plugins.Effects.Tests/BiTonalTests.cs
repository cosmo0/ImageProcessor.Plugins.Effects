namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class BiTonalTests : BaseTest
    {
        [Fact]
        public void Different_dark_color_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    BiTonal processor = new BiTonal();
                    processor.DynamicParameter = new BiTonalParameters() { DarkColor = Color.Black };

                    BiTonal processor2 = new BiTonal();
                    processor2.DynamicParameter = new BiTonalParameters() { DarkColor = Color.DarkCyan };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_bitonal_dark_color2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_light_color_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    BiTonal processor = new BiTonal();
                    processor.DynamicParameter = new BiTonalParameters() { LightColor = Color.White };

                    BiTonal processor2 = new BiTonal();
                    processor2.DynamicParameter = new BiTonalParameters() { LightColor = Color.Cyan };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_bitonal_light_color2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_threshold_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    BiTonal processor = new BiTonal();
                    processor.DynamicParameter = new BiTonalParameters() { Threshold = 155 };

                    BiTonal processor2 = new BiTonal();
                    processor2.DynamicParameter = new BiTonalParameters() { Threshold = 200 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_bitonal_threshold2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public override void Image_is_processed()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    BiTonal processor = new BiTonal();
                    processor.DynamicParameter = new BiTonalParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_bitonal_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}