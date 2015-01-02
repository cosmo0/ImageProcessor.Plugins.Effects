namespace ImageProcessor.Plugins.Effects.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class StainedGlassTests : BaseTest
    {
        [Fact]
        public void Different_edge_color_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = true,
                        EdgesColor = Color.Black,
                        EdgesThreshold = 155,
                        Factor = 4,
                        Size = 10
                    };

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = true,
                        EdgesColor = Color.White,
                        EdgesThreshold = 155,
                        Factor = 4,
                        Size = 10
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_stainedglass_edge_color2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_edge_threshold_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = true,
                        EdgesColor = Color.Black,
                        EdgesThreshold = 100,
                        Factor = 4,
                        Size = 10
                    };

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = true,
                        EdgesColor = Color.Black,
                        EdgesThreshold = 200,
                        Factor = 4,
                        Size = 10
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_stainedglass_edge_threshold2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_edge_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 4,
                        Size = 10
                    };

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = true,
                        EdgesColor = Color.Black,
                        EdgesThreshold = 155,
                        Factor = 4,
                        Size = 10
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_stainedglass_edge2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_factor_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 4,
                        Size = 10
                    };

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 10,
                        Size = 10
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_stainedglass_factor2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_formula_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Manhattan,
                        Edges = false,
                        Factor = 4,
                        Size = 10
                    };

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
                    result2.Save(string.Format("{0}/{1}_stainedglass_formula2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

                    // assert
                    result.Equals(result2).Should().BeFalse("because different parameters should yield different images");
                }
            }
        }

        [Fact]
        public void Different_size_values_yield_different_images()
        {
            foreach (string file in this.images)
            {
                // arrange
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(file);

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 4,
                        Size = 10
                    };

                    StainedGlass processor2 = new StainedGlass();
                    processor2.DynamicParameter = new StainedGlassParameters()
                    {
                        DistanceFormula = StainedGlassParameters.Formula.Euclidean,
                        Edges = false,
                        Factor = 4,
                        Size = 5
                    };

                    // act
                    Bitmap result = new Bitmap(processor.ProcessImage(factory));
                    Bitmap result2 = new Bitmap(processor2.ProcessImage(factory));
                    result2.Save(string.Format("{0}/{1}_stainedglass_size2.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));

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

                    StainedGlass processor = new StainedGlass();
                    processor.DynamicParameter = new StainedGlassParameters();

                    // act
                    Action act = () =>
                    {
                        Image img = processor.ProcessImage(factory);
                        img.Save(string.Format("{0}/{1}_stainedglass_default.jpg", Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    };

                    // assert
                    act.ShouldNotThrow("because the image should have been processed without error");
                }
            }
        }
    }
}