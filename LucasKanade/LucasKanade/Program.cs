using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
            frame1[0, 0] = new Rgb24(0, 0, 0);

            Console.WriteLine(frame2[0, 0].ToString());
        }

        static void MainAlgoCycle()
        {
            var splitter = new VideoSplitter("./video/1.mp4");

            splitter.LoadVideo();
            
            if (!splitter.TryGetNextFrame(out var frame1))
            {
                throw new ArgumentException("too short video");
            }

            int i = 0;
            while (splitter.TryGetNextFrame(out var frame2))
            {
                var imageMatrix = ImageUtils.ToGrayScale(frame1);
                var imageMatrix2 = ImageUtils.ToGrayScale(frame2);
                
                var res = LucasKanade.GetOpticalFlow(imageMatrix, imageMatrix2);

                ImageUtils.DrawVectorsOnImage(frame1, res, LucasKanade.GetBoxSize());
            
                frame1.SaveAsPng($"./splitted/{i++}.png");
                
                frame1.Dispose();

                frame1 = frame2;
            }
        }

        static void Main(string[] args)
        {
            MainAlgoCycle();
            // // "C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\movementFirst\\image-0.jpeg
            // var imageFirst =
            //     Image.Load<Rgb24>(
            //         "C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\movementFirst\\photo_1.jpg");
            // var imageSecond =
            //     Image.Load<Rgb24>
            //     ( // C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\mostSimpleExample\\sample1-18.png
            //         "C:\\Users\\Sergey\\RiderProjects\\TRSPK-2\\LucasKanade\\photos\\movementFirst\\photo_2.jpg");
            //
            // var imageMatrix = ImageUtils.ToGrayScale(imageFirst);
            // var imageMatrix2 = ImageUtils.ToGrayScale(imageSecond);
            //
            // var res = LucasKanade.GetOpticalFlow(imageMatrix, imageMatrix2);
            //
            // ImageUtils.DrawVectorsOnImage(imageFirst, res, LucasKanade.GetBoxSize());
            //
            // imageFirst.SaveAsPng("./lol.png");


        }
    }
}