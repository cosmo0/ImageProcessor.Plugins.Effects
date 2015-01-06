namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ComicsTests : BaseTest
    {
        [Fact]
        public void Different_smoothings_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Comics processor = new Comics();
                    processor.DynamicParameter = new ComicsParameters() { Smoothing = Imaging.SmoothingFilterType.Mean5x5 };

                    Comics processor2 = new Comics();
                    processor2.DynamicParameter = new ComicsParameters() { Smoothing = Imaging.SmoothingFilterType.None };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_comics_smoothing2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_thresholds_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Comics processor = new Comics();
                    processor.DynamicParameter = new ComicsParameters() { Threshold = 100 };

                    Comics processor2 = new Comics();
                    processor2.DynamicParameter = new ComicsParameters() { Threshold = 150 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_comics_threshold2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

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

                    Comics processor = new Comics();
                    processor.DynamicParameter = new ComicsParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_comics_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}