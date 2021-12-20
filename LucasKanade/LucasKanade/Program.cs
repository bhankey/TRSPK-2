using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;

namespace LucasKanade
{
    class Program
    {
        static void MainAlgoCycle()
        {
            Configuration.Default.MemoryAllocator = ArrayPoolMemoryAllocator.CreateWithAggressivePooling();

            using (var splitter = new VideoSplitter(".\\video\\seq.gif"))
            {
                splitter.LoadVideo();

                var size = splitter.GetImageSize();
                var opticalFlow = new OpticalFlow(size.Height, size.Width);

                opticalFlow.Open();

                var i = 0;

                var watch = Stopwatch.StartNew();

                if (!splitter.TryGetNextFrame(out var frame))
                {
                    throw new ArgumentException("too short video");
                }

                while (splitter.TryGetNextFrame(out var nextFrame))
                {
                    opticalFlow.TryGetNextOpticalFlowFrame(frame, nextFrame, out var result);

                    result.SaveAsPng($"./splitted/{i++}.png");

                    Console.WriteLine($"FrameAll things time {watch.ElapsedMilliseconds} {i}");

                    frame = nextFrame;

                    watch.Restart();
                }
            }
        }

        static void Main(string[] args)
        {
           MainAlgoCycle();
            
           // Convolution.TestConv();
        }
    }
}