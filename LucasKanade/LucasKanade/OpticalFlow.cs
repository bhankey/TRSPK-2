using System;
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LucasKanade
{
    
    public struct FlowPoints
    {
        public int X;
        public int Y;
        public double XDirection;
        public double YDirection;

        public FlowPoints(int x, int y, double xDirection, double yDirection)
        {
            X = x;
            Y = y;
            XDirection = xDirection;
            YDirection = yDirection;
        }
    }
    
    public class OpticalFlow
    {
        private double _threshold = 0.05; // TODO
        
        private LucasKanade _lucasKanade;

        private int _width;
        private int _height;

        private double[,] _firstImageBuffer;
        private double[,] _secondImageBuffer;

        public OpticalFlow(int height, int width)
        {
            _width = width;
            _height = height;
            
            _firstImageBuffer = new double[width, height];
            _secondImageBuffer = new double[width, height];
        }

        public void Open()
        {
            _lucasKanade = new LucasKanade(_height,_width, (int)Registry.Get("box_size"));
        }

        public void TryGetNextOpticalFlowFrame(in Image<Rgb24> frame, in Image<Rgb24> nextFrame, out Image<Rgb24> resultingFrame) 
        {
            resultingFrame = default;

            if (_width != frame.Width || _width != nextFrame.Width ||
                _height != frame.Height || _height != nextFrame.Height)
            {
                throw new ArgumentException("Images must be same size");
            }

            ImageUtils.ToGrayScale(frame, _firstImageBuffer);
            ImageUtils.ToGrayScale(nextFrame, _secondImageBuffer);
            
            var res = _lucasKanade.GetOpticalFlow(
                _firstImageBuffer,
                _secondImageBuffer,
                _threshold, 
                (int)Registry.Get("interval_between_points"),
                (int)Registry.Get("convolution_coefficient")
                );

            resultingFrame = frame.Clone();
            
            ImageUtils.DrawVectorsOnImage(resultingFrame, res, _lucasKanade.BoxSize, _threshold);
        }
    }
}

