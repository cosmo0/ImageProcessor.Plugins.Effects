namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class ComicsTests : BaseTest
    {
        [Fact]
        public void Image_is_processed()
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
                        img.Save(string.Format("{0}/{1}_comics_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}