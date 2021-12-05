using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LucasKanade
{
    public static class ImageUtils
    {
        public static double[,] ToGrayScale(Image<Rgb24> image)
        {
            image.Mutate(x => x.Grayscale());

            var grayImage = new double[image.Height, image.Width];

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    grayImage[i, j] = image[i, j].R;
                }
            }

            return grayImage;
        } 
        
    }
}