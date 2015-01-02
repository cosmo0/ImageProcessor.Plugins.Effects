namespace ImageProcessor.Plugins.Effects.Tests
{
    using System.IO;

    public class BaseTest
    {
        protected readonly string[] images;

        private readonly string path = Path.Combine(Path.GetDirectoryName(typeof(ColorBalanceTests).Assembly.Location), "img");

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

        public string ImagePath(string fileName)
        {
            return Path.Combine(this.path, fileName);
        }
    }
}