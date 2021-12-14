using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LucasKanade
{
    public class OpticalFlow
    {
        private VideoSplitter _splitter;

        private LucasKanade _lucasKanade;

        private Image<Rgb24> _currentFrame;

        private double[,] _firstImageBuffer;
        private double[,] _secondImageBuffer;

        public OpticalFlow(string videoPath)
        {
            var splitter = new VideoSplitter(videoPath);

            splitter.LoadVideo();

            _splitter = splitter;
        }

        public void Open()
        {
            if (!_splitter.TryGetNextFrameBuffered(out _currentFrame))
            {
                throw new ArgumentException("too short video");
            }

            _lucasKanade = new LucasKanade(_currentFrame.Height, _currentFrame.Width);

            _firstImageBuffer = new double[_currentFrame.Width, _currentFrame.Height];
            _secondImageBuffer = new double[_currentFrame.Width, _currentFrame.Height];
        }

        public bool TryGetNextOpticalFlowFrame(out Image<Rgb24> resultingFrame)
        {
            resultingFrame = default;

            if (!_splitter.TryGetNextFrameBuffered(out var frame2))
            {
                return false;
            }

           //  var watch = Stopwatch.StartNew();

            ImageUtils.ToGrayScale(_currentFrame, _firstImageBuffer);
            ImageUtils.ToGrayScale(frame2, _secondImageBuffer);


           // Console.WriteLine($"time of graying {watch.ElapsedMilliseconds}ms");
            
            //watch.Restart();
            
            var res = _lucasKanade.GetOpticalFlow(_firstImageBuffer, _secondImageBuffer);

           // Console.WriteLine($"time of algo {watch.ElapsedMilliseconds}ms\n\n");
           // watch.Restart();

            ImageUtils.DrawVectorsOnImage(_currentFrame, res, _lucasKanade.BoxSize);
            
            resultingFrame = _currentFrame;
            
            _currentFrame = frame2;

            return true;
        }
    }
}

