namespace ImageProcessor.Plugins.Effects.Tests
{
    using System.IO;
    using Xunit;

    /// <summary>
    /// The base test class
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// The images list
        /// </summary>
        protected readonly string[] images;

        /// <summary>
        /// The path to the current DLL
        /// </summary>
        private readonly string path = Path.Combine(Path.GetDirectoryName(typeof(BaseTest).Assembly.Location), "img");

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class
        /// </summary>
        public BaseTest()
        {
            this.images = new string[]
            {
                this.ImagePath("1.jpg"),
                this.ImagePath("2.jpg"),
                this.ImagePath("3.jpg"),
                this.ImagePath("4.jpg"),
                this.ImagePath("5.jpg")
            };
        }

        [Fact]
        public abstract void Image_is_processed();

        /// <summary>
        /// Builds a full path to the specified image
        /// </summary>
        /// <param name="fileName">The image file name</param>
        /// <returns>The full path to the image</returns>
        public string ImagePath(string fileName)
        {
            return Path.Combine(this.path, fileName);
        }
    }
}