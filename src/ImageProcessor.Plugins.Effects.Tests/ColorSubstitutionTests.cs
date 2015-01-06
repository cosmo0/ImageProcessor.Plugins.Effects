namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ColorSubstitutionTests : BaseTest
    {
        [Fact]
        public void Different_replacement_colors_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ColorSubstitution processor = new ColorSubstitution();
                    processor.DynamicParameter = new ColorSubstitutionParameters()
                    {
                        ReplaceWith = Color.Black,
                        Threshold = 150
                    };

                    ColorSubstitution processor2 = new ColorSubstitution();
                    processor2.DynamicParameter = new ColorSubstitutionParameters()
                    {
                        ReplaceWith = Color.Red,
                        Threshold = 150
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_substitution_toreplace2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_source_colors_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ColorSubstitution processor = new ColorSubstitution();
                    processor.DynamicParameter = new ColorSubstitutionParameters()
                    {
                        ToReplace = Color.White,
                        Threshold = 150
                    };

                    ColorSubstitution processor2 = new ColorSubstitution();
                    processor2.DynamicParameter = new ColorSubstitutionParameters()
                    {
                        ToReplace = Color.Blue,
                        Threshold = 150
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_substitution_toreplace2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

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

                    ColorSubstitution processor = new ColorSubstitution();
                    processor.DynamicParameter = new ColorSubstitutionParameters() { Threshold = 50 };

                    ColorSubstitution processor2 = new ColorSubstitution();
                    processor2.DynamicParameter = new ColorSubstitutionParameters() { Threshold = 150 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_substitution_threshold2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);

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

                    ColorSubstitution processor = new ColorSubstitution();
                    processor.DynamicParameter = new ColorSubstitutionParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_substitution_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ImageFormat.Jpeg);
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}