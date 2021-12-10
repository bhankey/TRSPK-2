using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LucasKanade
{
    public static class ImageUtils
    {
        
        public static double[,] ToGrayScale1(Image<Rgb24> image)
        {
            image.Mutate(x => x.Grayscale());

            var grayImage = new double[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    grayImage[i, j] = image[i, j].R;
                }
            }

            return grayImage;
        } 
        
        public static double[,] ToGrayScale(Image<Rgb24> image)
        {
            var grayImage = new double[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    grayImage[i, j] = (image[i, j].R + image[i, j].G + image[i, j].B) / 3.0;
                }
            }

            return grayImage;
        }

        public static float LineScale = 1;
        public static float LineWidth = 1;
        public static void DrawVectorsOnImage(Image<Rgb24> image, List<List<double[]>> vector, int boxSize) 
        {
            image.Mutate(imageContext =>
            {
                for (int x = 0, opticalFlowX = 0; x + LucasKanade.GetBoxSize() < image.Width; x += LucasKanade.GetBoxSize(), opticalFlowX++)
                {
                    for (int y = 0, opticalFlowY = 0;y + LucasKanade.GetBoxSize() < image.Height; y += LucasKanade.GetBoxSize(), opticalFlowY++)
                    {
                        if ((float) (vector[opticalFlowX][opticalFlowY][0]) == 0.0 ||
                            (float) (vector[opticalFlowX][opticalFlowY][1]) == 0.0)
                        {
                            continue;
                        }
                        
                        var points = new PointF[2];
                        points[0] = new PointF(
                            x: x  + LucasKanade.GetBoxSize() / 2,
                            y: y +  LucasKanade.GetBoxSize() / 2
                        );
                        points[1] = new PointF(
                            x: (float) (points[0].X + vector[opticalFlowX][opticalFlowY][0] * LineScale),
                            y: (float) (points[0].Y + vector[opticalFlowX][opticalFlowY][1] * LineScale)
                        );
                        
                        var lineColor = Color.FromRgb(
                            r: (byte) 255,
                            g: (byte) 0,
                            b: (byte) 0);
                        

                        var linePen = new Pen(lineColor, LineWidth);

                        imageContext.DrawLines(linePen, points);
                    }
                }
            });
        }
        
    }
}