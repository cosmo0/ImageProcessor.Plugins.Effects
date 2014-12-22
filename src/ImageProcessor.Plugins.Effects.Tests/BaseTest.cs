namespace ImageProcessor.Plugins.Effects.Tests
{
    using System.IO;

    public class BaseTest
    {
        protected readonly string[] images;
        
        public BaseTest()
        {
            string path = Path.Combine(Path.GetDirectoryName(typeof(ColorBalanceTests).Assembly.Location), "img");
            this.images = new string[]
            {
                Path.Combine(path, "1.jpg"),
                Path.Combine(path, "2.jpg"),
                Path.Combine(path, "3.jpg"),
                Path.Combine(path, "4.jpg"),
                Path.Combine(path, "5.jpg")
            };
        }
    }
}