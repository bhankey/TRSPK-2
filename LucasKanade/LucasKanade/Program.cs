using System;

namespace LucasKanade
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = new VideoSplitter("video/movementFirst.mp4", "photos/movementFirst");

            v.Parse(500);
        }
    }
}