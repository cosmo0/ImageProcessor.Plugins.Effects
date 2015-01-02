namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class AsciiTests : BaseTest
    {
        [Fact]
        public void Different_character_count_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Ascii processor = new Ascii();
                    processor.DynamicParameter = new AsciiParameters() { CharacterCount = 20 };

                    Ascii processor2 = new Ascii();
                    processor2.DynamicParameter = new AsciiParameters() { CharacterCount = 30 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_ascii_character_count2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_font_size_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Ascii processor = new Ascii();
                    processor.DynamicParameter = new AsciiParameters() { FontSize = 3 };

                    Ascii processor2 = new Ascii();
                    processor2.DynamicParameter = new AsciiParameters() { FontSize = 10 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_ascii_fontsize2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_pixel_per_character_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Ascii processor = new Ascii();
                    processor.DynamicParameter = new AsciiParameters() { PixelPerCharacter = 2 };

                    Ascii processor2 = new Ascii();
                    processor2.DynamicParameter = new AsciiParameters() { PixelPerCharacter = 5 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_ascii_pixel_per_character2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_zoom_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    Ascii processor = new Ascii();
                    processor.DynamicParameter = new AsciiParameters() { Zoom = 100 };

                    Ascii processor2 = new Ascii();
                    processor2.DynamicParameter = new AsciiParameters() { Zoom = 50 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_ascii_zoom2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

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

                    Ascii processor = new Ascii();
                    processor.DynamicParameter = new AsciiParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_ascii_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}