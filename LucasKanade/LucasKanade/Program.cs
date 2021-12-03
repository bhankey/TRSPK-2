using System;
using SixLabors.ImageSharp.PixelFormats;

namespace LucasKanade
{
    class Program
    {
        // ExampleOfWorkingWithSplitter на релизе удалить
        static void ExampleOfWorkingWithSplitter()
        {
            var splitter = new VideoSplitter("./video/movementFirst.mp4");
            
            splitter.LoadVideo();
            
            // Так можем в цикле получать кадры
            if (!splitter.TryGetNextFrame(out var frame1))
            {
                Console.WriteLine("Не удалось получить первый кадр");
            }

            if (!splitter.TryGetNextFrame(out var frame2))
            {
                Console.WriteLine("Не удалось получить второй кадр");
            }

            // Так можно обращаться к конкретному пикселю кадру в формате rgb
            frame1[0, 0] = new Bgr24(0, 0, 0);
            
            Console.WriteLine(frame2[0, 0].ToString());
        }
        static void Main(string[] args)
        {
           // ExampleOfWorkingWithSplitter();

            var f = new double[2][];
            for (int i = 0; i < 2; i++)
            {
                f[i] = new[]{2.0, 2.0};
            }

            f[0][0] = 4;
            f[0][1] = 3;
            f[1][0] = 3;
            f[1][1] = 2;

            var a = MatrixOperation.MatrixInverse(f);
        }
    }
}