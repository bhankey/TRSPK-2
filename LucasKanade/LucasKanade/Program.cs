using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LucasKanade
{
    class Program
    {
        static void MainAlgoCycle()
        {
            var splitter = new VideoSplitter("./video/1.mp4");

            splitter.LoadVideo();

            var size = splitter.GetImageSize();

            if (!splitter.TryGetNextFrameBuffered(out var frame1))
            {
                throw new ArgumentException("too short video");
            }

            var algo = new LucasKanade(frame1.Height, frame1.Width); 

            int i = 0;

            var imageMatrix = new double[frame1.Width, frame1.Height];
            var imageMatrix2 = new double[frame1.Width, frame1.Height];
            while (splitter.TryGetNextFrameBuffered(out var frame2))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                
                ImageUtils.ToGrayScaleBuffered(frame1, imageMatrix);
                ImageUtils.ToGrayScaleBuffered(frame2, imageMatrix2);

                Console.WriteLine($"time of graying {watch.ElapsedMilliseconds}ms");
                watch.Restart();
                // 3.701 - 81 кадр
                var res = algo.GetOpticalFlow(imageMatrix, imageMatrix2);

                Console.WriteLine($"time of algo {watch.ElapsedMilliseconds}ms");
                watch.Restart();

                ImageUtils.DrawVectorsOnImage(frame1, res, algo.BoxSize);
                Console.WriteLine($"time of drawing {watch.ElapsedMilliseconds}ms");
                
                watch.Restart();
                frame1.SaveAsPng($"./splitted/{i++}.png");
                
                Console.WriteLine($"time of saving {watch.ElapsedMilliseconds}ms\n");

                frame1.Dispose();

                frame1 = frame2;
            }
        }

        static void Main(string[] args)
        {
            MainAlgoCycle();
        }
    }
}