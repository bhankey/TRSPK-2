using System;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using SixLabors.ImageSharp;

namespace LucasKanade
{
    class Program
    {
        static void Main(string[] args)
        {
            var splitter = new VideoSplitter("./video/movementFirst.mp4");
            
            splitter.LoadVideo();
            for (int i = 0; i < 500; i++)
            {
                _ = splitter.TryGetNextFrame(out var frame);
                frame.SaveAsPng($"C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\test{i}.png");
            }

            
        }
    }
}