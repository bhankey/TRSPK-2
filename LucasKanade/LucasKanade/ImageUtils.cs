using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LucasKanade
{
    public static class ImageUtils
    { 
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
        
        public static void ToGrayScale(Image<Rgb24> image, double[,] buffer)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    buffer[i, j] = (image[i, j].R + image[i, j].G + image[i, j].B) / 3.0;
                }
            }
        }

        public static double LineScale = 1;
        public static float LineWidth = 1;
        public static void DrawVectorsOnImage(Image<Rgb24> image, List<List<double[]>> vector, int boxSize, double threshold) 
        {
            image.Mutate(imageContext =>
            {
                var points = new PointF[2];
                var square = new PointF[4];
                for (int x = 0, opticalFlowX = 0; x + boxSize < image.Width; x += boxSize, opticalFlowX++)
                {
                    for (int y = 0, opticalFlowY = 0;y + boxSize < image.Height; y += boxSize, opticalFlowY++)
                    {
                        if ((float) Math.Abs(vector[opticalFlowX][opticalFlowY][0]) <= threshold ||
                            (float) Math.Abs(vector[opticalFlowX][opticalFlowY][1]) <= threshold ||
                            (float) Math.Abs(vector[opticalFlowX][opticalFlowY][0]) >= 1000 ||
                            (float) Math.Abs(vector[opticalFlowX][opticalFlowY][1]) >= 1000)
                        {
                            continue;
                        }

                        var centerX = x + boxSize / 2;
                        var centerY = y + boxSize / 2;

                        
                        points[0] = new PointF(
                            x: centerX,
                            y: centerY
                        );
                        points[1] = new PointF(
                            x: (float) (points[0].X + vector[opticalFlowX][opticalFlowY][0] * LineScale),
                            y: (float) (points[0].Y + vector[opticalFlowX][opticalFlowY][1] * LineScale)
                        );
                        
                        var lineColor = Color.FromRgb(
                            r: (byte) 255,
                            g: (byte) 0,
                            b: (byte) 0);

                        square[0] = new PointF(
                            x: centerX + 1,
                            y: centerY + 1);
                        square[1] = new PointF(
                            x: centerX + 1,
                            y: centerY - 1);
                        square[2] = new PointF(
                            x: centerX - 1,
                            y: centerY - 1 );
                        square[3] = new PointF(
                            x: centerX - 1,
                            y: centerY + 1);
                        
                        
                        var linePen = new Pen(lineColor, LineWidth);

              
                        
                        imageContext.DrawLines(linePen, points);
                        imageContext.DrawLines(linePen, square);
                    }
                }
            });
        }
        
    }
}