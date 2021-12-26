using System;
using System.Collections.Generic;
using System.Diagnostics;
using FFMediaToolkit;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;

namespace LucasKanade
{
    class Program
    {
        static void MainAlgoCycle(string videoPath, string outputDirectory)
        {
            Configuration.Default.MemoryAllocator = ArrayPoolMemoryAllocator.CreateWithAggressivePooling();

            using (var splitter = new VideoSplitter(videoPath))
            {
                try
                {
                    splitter.LoadVideo();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to open video by path {videoPath} or load ffmpeg");
                    throw;
                }
                

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

                    try
                    {
                        result.SaveAsPng($"{outputDirectory}\\{i++}.png");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("unable to save picture by path");
                    }

                    Console.WriteLine($"Process frame time - {watch.ElapsedMilliseconds} #{i}");

                    frame = nextFrame;

                    watch.Restart();
                }
            }
        }

        static void Main(string[] args)
        {
            
            var inMemoryConfigSettings = new Dictionary<string, string>()
            {
                {"paths:video", "./video/seq.gif"},
                {"paths:ffmpeg", "./runtimes/win-x64/native"},
                {"paths:output", "./splitted"}
            };
            
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfigSettings)
                .AddYamlFile($"{Environment.CurrentDirectory}\\config\\config.yaml", optional: true)
                .Build();

            var paths = config.GetSection("paths");

            var output = paths.GetSection("output").Value;
            var videoPath = paths.GetSection("video").Value;

            FFmpegLoader.FFmpegPath = paths.GetSection("ffmpeg").Value;
            
            MainAlgoCycle(videoPath, output);
        }
    }
}