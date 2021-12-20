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
            
            var opticalFlow = new OpticalFlow(".\\video\\seq.gif");

            opticalFlow.Open();
            var i = 0;
            var watch = Stopwatch.StartNew();
            while (opticalFlow.TryGetNextOpticalFlowFrame(out var frame))
            {

                frame.SaveAsPng($"./splitted/{i++}.png");

                Console.WriteLine($"FrameAll things time {watch.ElapsedMilliseconds} {i}");
                watch.Restart();
            }
        }

        static void Main(string[] args)
        {
           MainAlgoCycle();
            
           // Convolution.TestConv();
        }
    }
}