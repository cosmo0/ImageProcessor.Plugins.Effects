namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class OilPaintingTests : BaseTest
    {
        [Fact]
        public void Different_filter_size_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    OilPainting processor = new OilPainting();
                    processor.DynamicParameter = new OilPaintingParameters() { FilterSize = 10 };

                    OilPainting processor2 = new OilPainting();
                    processor2.DynamicParameter = new OilPaintingParameters() { FilterSize = 20 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_oil_filtersize2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_levels_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    OilPainting processor = new OilPainting();
                    processor.DynamicParameter = new OilPaintingParameters() { Levels = 10 };

                    OilPainting processor2 = new OilPainting();
                    processor2.DynamicParameter = new OilPaintingParameters() { Levels = 20 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_oil_levels2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

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

                    OilPainting processor = new OilPainting();
                    processor.DynamicParameter = new OilPaintingParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_oil_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}