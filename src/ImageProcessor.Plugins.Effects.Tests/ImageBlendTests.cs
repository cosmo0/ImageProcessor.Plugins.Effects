namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ImageBlendTests : BaseTest
    {
        [Fact]
        public  override void Image_is_processed()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ImageBlend processor = new ImageBlend();
                    processor.DynamicParameter = new ImageBlendParameters { Overlay = this.ImagePath("blend.jpg") };

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_blend_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}