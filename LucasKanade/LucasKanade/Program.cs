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
            
            var opticalFlow = new OpticalFlow("./video/1.mp4");

            opticalFlow.Open();
            var i = 0;
            var watch = Stopwatch.StartNew();
            while (opticalFlow.TryGetNextOpticalFlowFrame(out var frame))
            {
                Console.WriteLine($"FrameAll things time {watch.ElapsedMilliseconds}");
                
                frame.SaveAsPng($"./splitted/{i++}.png");
                
                watch.Restart();
            }
        }

        static void Main(string[] args)
        {
            MainAlgoCycle();
        }
    }
}