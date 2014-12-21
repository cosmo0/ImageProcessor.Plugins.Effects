﻿namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using Xunit;
    using FluentAssertions;

    public class ColorBalanceTests
    {
        private readonly string[] images;

        public ColorBalanceTests()
        {
            string path = Path.Combine(Path.GetDirectoryName(typeof(ColorBalanceTests).Assembly.Location), "img");
            images = new string[] { Path.Combine(path, "1.jpg") };
        }

        [Fact]
        public void ColorBalance_processes_image()
        {
            foreach (string file in images)
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
                        img.Save(string.Format("{0}/{1}_result.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }

        [Fact]
        public void Different_values_yield_different_images()
        {
            foreach (string file in images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    ColorBalance processor = new ColorBalance();
                    processor.DynamicParameter = new ColorBalanceParameters();

                    ColorBalance processor2 = new ColorBalance();
                    processor2.DynamicParameter = new ColorBalanceParameters() { Blue = 155, Green = 155, Red = 155 };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_result2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }
    }
}