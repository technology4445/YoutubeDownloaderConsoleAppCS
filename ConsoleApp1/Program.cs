using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;

class Program
{
    static async Task Main()
    {
        // Replace "VIDEO_URL" with the URL of the YouTube video you want to download
        //string videoUrl = "https://www.youtube.com/watch?v=fxDIyALaOI0";
        Console.WriteLine("Enter Url...");
        string videoUrl = Console.ReadLine().ToString();

        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(videoUrl);

        if (video != null)
        {
            var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var streamInfo = streamInfoSet.GetMuxedStreams().FirstOrDefault();

            if (streamInfo != null)
            {
                var videoStream = await youtube.Videos.Streams.GetAsync(streamInfo);
                Console.WriteLine("enter output");

                string o = Console.ReadLine();
                string outputFilePath = Path.Combine(o);
                //string outputFilePath = Path.Combine(o, $"{video.Title}.{videoStream.Container.Name}");

                using (var fileStream = File.Create(outputFilePath))
                {
                    await videoStream.CopyToAsync(fileStream);
                    Console.WriteLine($"Video downloaded to: {outputFilePath}");
                }
            }
            else
            {
                Console.WriteLine("No suitable video stream found.");
            }
        }
        else
        {
            Console.WriteLine("Video not found.");
        }
    }
}
