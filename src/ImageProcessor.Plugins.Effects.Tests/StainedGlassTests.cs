namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    class StainedGlassTests : BaseTest
    {
        [Fact]
        public void Different_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters();

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 4,
                        Size = 10
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_result2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Image_is_processed()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_result.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}