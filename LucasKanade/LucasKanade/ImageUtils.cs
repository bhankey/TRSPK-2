﻿using System;
using System.Collections.Generic;
using SixLabors.Fonts;
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
        
        public static void ToGrayScale(in Image<Rgb24> image, double[,] buffer)
        {
            MatrixOperation.SetEmpty(buffer);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    buffer[i, j] = (image[i, j].R + image[i, j].G + image[i, j].B) / 3.0;
                }
            }
        }

        public static double LineScale = 10 ;
        public static float LineWidth = 1;
        public static void DrawVectorsOnImage(Image<Rgb24> image, List<FlowPoints> vectors, int boxSize, double threshold) 
        {
            image.Mutate(imageContext =>
                {
                    var points = new PointF[2];
                    var square = new PointF[4];
                    foreach (var vector in vectors)
                    {
                        points[0] = new PointF(
                            x: vector.X,
                            y: vector.Y
                        );

                        points[1] = new PointF(
                            x: (float) (vector.X + vector.XDirection * LineScale),
                            y: (float) (vector.Y + vector.YDirection * LineScale)
                        );

                        var lineColor = Color.FromRgb(
                            r: (byte) 255,
                            g: (byte) 0,
                            b: (byte) 0);

                        square[0] = new PointF(
                            x: vector.X + 1,
                            y: vector.Y + 1);
                        square[1] = new PointF(
                            x: vector.X + 1,
                            y: vector.Y - 1);
                        square[2] = new PointF(
                            x: vector.X - 1,
                            y: vector.Y - 1);
                        square[3] = new PointF(
                            x: vector.X - 1,
                            y: vector.Y + 1);


                        var linePen = new Pen(lineColor, LineWidth);


                        imageContext.DrawLines(linePen, points);
                        imageContext.DrawLines(linePen, square);
                    }

                }
            );
        }
        
    }
}