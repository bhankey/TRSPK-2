using System;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace LucasKanade
{
    class Program
    {
        static void Main(string[] args)
        {
            var splitter = new VideoSplitterV2("./video/movementFirst.mp4");
            splitter.Parse();
        }
    }
}