using System;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace LucasKanade
{
    public class VideoSplitter
    {
        private string _pathToVideo;

        private string _pathToOutPut;

        public VideoSplitter(string pathToVideo, string pathToOutPut = "./tmp")
        {
            _pathToVideo = pathToVideo;
            _pathToOutPut = pathToOutPut;
        }

        public void Parse()
        {
            using (var engine = new Engine())
            {
                var video = new MediaFile {Filename = _pathToVideo};

                engine.GetMetadata(video);
                var milliSecondPerCadre = video.Metadata.VideoData.Fps / 1000;
                var i = 0d;
                var counter = 0;
                while (i < video.Metadata.Duration.TotalMilliseconds)
                {
                    var options = new ConversionOptions {Seek = TimeSpan.FromMilliseconds(i)};
                    var outputFile = new MediaFile
                        {Filename = string.Format("{0}\\image-{1}.jpeg", _pathToOutPut, counter)};
                    engine.GetThumbnail(video, outputFile, options);
                    i += milliSecondPerCadre;
                    ++counter;
                }
            }
        }
    }
}