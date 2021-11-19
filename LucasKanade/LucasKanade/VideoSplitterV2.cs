using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LucasKanade
{
    public class VideoSplitterV2
    {
        private string _pathToVideo;

        private string _pathToOutPut;

        public VideoSplitterV2(string pathToVideo, string pathToOutPut = "./tmp")
        {
            _pathToVideo = pathToVideo;
            _pathToOutPut = pathToOutPut;
        }

        public void Parse()
        {
            int i = 0;
            var file = MediaFile.Open(@".\video\movementFirst.mp4");
            while (file.Video.TryGetNextFrame(out var imageData))
            {
                var bitMap = ToBitmap(imageData);
                // See the #Usage details for example .ToBitmap() implementation
                // The .Save() method may be different depending on your graphics library
            }
        }
        
        private static Image<Bgr24> ToBitmap(ImageData imageData)
        {
            return Image.LoadPixelData<Bgr24>(imageData.Data, imageData.ImageSize.Width, imageData.ImageSize.Height);
        }
    }
}