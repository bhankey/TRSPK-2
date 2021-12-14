using System;
using System.Collections.Generic;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace LucasKanade
{
    // Чтобы этот класс заработал надо убить кучу времени, ибо доки к либам очень плохие на C#.
    // Необходимо, чтобы были установлены нужные пакеты и наличие папки /runtimes/win-x64/native
    // (P.S в доке про эту папка ничего нет, слава животворящему дебагу)
    // Если что-то не работает, то поставьте сборку только для x64
    
    // TODO можно доработать класс для работы с потоком
    public class VideoSplitter
    {
        private string _pathToVideo;

        private MediaFile _file;

        private bool _isLoaded = false;
        public VideoSplitter(string pathToVideo)
        {
            _pathToVideo = pathToVideo;
        }

        private byte[] byteBuffer;
        ~VideoSplitter()
        {
            if (_isLoaded)
            {
                _file.Dispose();
            }
        }
        
        public void Clean()
        {
            _file.Dispose();
            _isLoaded = false;
        }

        public System.Drawing.Size GetImageSize()
        {
            return _file.Video.Info.FrameSize;
        }

        public void LoadVideo()
        {
            _file = MediaFile.Open(_pathToVideo);
            var size = _file.Video.Info.FrameSize;
            byteBuffer = new byte[ImageData.EstimateStride(size.Width, ImagePixelFormat.Bgr24) * size.Height];
            
            _isLoaded = true;
        }

        public bool TryGetNextFrame(out Image<Rgb24> bitmap)
        {
            bitmap = default;
            if (!_isLoaded)
            {
                return false;
            }

            var ok = _file.Video.TryGetNextFrame(out var imageData);

            if (!ok)
            {
                return false;
            }

            bitmap = ToBitmap(imageData);

            return true;
        }
        
        public bool TryGetNextFrameBuffered(out Image<Rgb24> bitmap)
        {
            bitmap = default;
            if (!_isLoaded)
            {
                return false;
            }

            var ok = _file.Video.TryGetNextFrame(byteBuffer);

            if (!ok)
            {
                return false;
            }

            var size = _file.Video.Info.FrameSize;
            bitmap = ToBitmap(byteBuffer, size.Width, size.Height);

            return true;
        }
        public List<Image<Rgb24>> GetAllFrames()
        {
            if (!_isLoaded)
            {
                throw new Exception("Video is not loaded");
            }
            
            var images = new List<Image<Rgb24>>();
            while (_file.Video.TryGetNextFrame(out var imageData))
            {
                var image = ToBitmap(imageData);
                images.Add(image);
            }

            return images;
        }
        
        private static Image<Rgb24> ToBitmap(byte[] imageData, int width, int height)
        {
            return Image.LoadPixelData<Rgb24>(imageData, width, height);
        }
        private static Image<Rgb24> ToBitmap(ImageData imageData)
        {
            return Image.LoadPixelData<Rgb24>(imageData.Data, imageData.ImageSize.Width, imageData.ImageSize.Height);
        }
    }
}
