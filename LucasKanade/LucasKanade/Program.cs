using System;
using SixLabors.ImageSharp;
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
            var imageFirst = Image.Load<Rgb24>("C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\mostSimpleExample\\sample1-17.png");
            var imageSecond =
                Image.Load<Rgb24>
                (
                    "C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\mostSimpleExample\\sample1-18.png");

            var imageMatrix = ImageUtils.ToGrayScale(imageFirst);
            var imageMatrix2 = ImageUtils.ToGrayScale(imageSecond);

            var res = LucasKanade.GetOpticalFlow(imageMatrix, imageMatrix2);

            Console.WriteLine("lol");
        }
    }
}