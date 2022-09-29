using crm.Models.creatives;
using crm.Models.storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

using Mono.Unix.Native;
using System.Runtime.InteropServices;

namespace crm.Models.uniq
{
    public class Uniqalizer : IUniqalizer
    {
        #region vars
        Random random = new Random();
        CancellationTokenSource cts;
        #endregion        

        #region private
        long getBitRate(long origBitRate) {
            Int64 lo = (Int64)(origBitRate * 0.4);
            Int64 hi = (Int64)(origBitRate * 0.7);
            return random.NextInt64(lo, hi);
        }
        #endregion

        #region helpers
        void deleteFiles(string dir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
        #endregion

        #region public
        public static async Task Init(string codecdir, Action<int> progress)
        {

            var bins = Directory.GetFiles(codecdir);
            FFmpeg.SetExecutablesPath(codecdir);

            if (bins != null && bins.Length > 0)
                return;            

            var prgrsconv = new Progress<ProgressInfo>((p) =>
            {
                int intp = (int)(p.DownloadedBytes * 100.0d / p.TotalBytes);
                progress?.Invoke(intp);
                Debug.WriteLine(p.DownloadedBytes + " " + p.TotalBytes + " " + intp);
            });

            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, codecdir, prgrsconv);

            bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (isMacOSX)
            {
                string ffmpeg = Path.Combine(codecdir, "ffmpeg");
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "chmod",
                    Arguments = "+x " + "\"" + ffmpeg + "\""
                };

                Process process = new Process() { StartInfo = startInfo };
                process.Start();

                string ffprobe = Path.Combine(codecdir, "ffprobe");
                startInfo = new ProcessStartInfo()
                {
                    FileName = "chmod",
                    Arguments = "+x " + "\"" + ffprobe + "\""
                };

                process = new Process() { StartInfo = startInfo };
                process.Start();
            }

            //Syscall.chmod("ffmpeg", FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IXUSR);
            //Syscall.chmod("ffprobe", FilePermissions.S_IRUSR | FilePermissions.S_IWUSR | FilePermissions.S_IXUSR);
        }

        private async Task uniqalize(string inputPath, string outputFolderPath, int n, bool erase)
        {
            if (erase)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(outputFolderPath);
                foreach (var file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            cts = new CancellationTokenSource();

            for (int i = 0; i < n; i++)
            {
                string s = Guid.NewGuid().ToString();
                var outputPath = Path.Combine(outputFolderPath, $"{i}_{s}.mp4");

                IMediaInfo info = await FFmpeg.GetMediaInfo(inputPath);
                IVideoStream videoStream = info.VideoStreams.First();
                IStream audioStream = info.AudioStreams.FirstOrDefault()?.SetChannels(2);

                long origBitRate = videoStream.Bitrate;
                long bitrate = getBitRate(origBitRate);

                try
                {
                    var conversion = FFmpeg.Conversions.New();

                    //var accelerator = new HardwareAccelerator();
                    //conversion.UseHardwareAcceleration(accelerator, VideoCodec.h264, VideoCodec.h264);

                    //conversion.OnProgress += (s, a) => {
                    //    UniqalizeProgessUpdateEvent?.Invoke(a.Percent);
                    //};

                    IConversionResult conversionResult = await conversion
                        .AddStream(videoStream, audioStream)
                        .SetOutput(outputPath)
                        //.AddParameter($"-b:v {bitrate} -bufsize {bitrate} -preset:v faster")
                        .AddParameter($"-b:v {bitrate} -bufsize {bitrate} -preset:v veryfast")
                        .Start(cts.Token);

                    UniqalizeProgessUpdateEvent?.Invoke((i + 1) * 100 / n);

                } catch (Exception ex)
                {                    
                    deleteFiles(outputFolderPath);
                    UniqalizeProgessUpdateEvent?.Invoke(0);
                    throw new Exception(ex.Message);                    

                } finally
                {
                    //UniqalizeProgessUpdateEvent?.Invoke(0);
                }
            }

            UniqalizeProgessUpdateEvent?.Invoke(0);
        }

        public async Task Uniqalize(string inputPath, int n, string outpurdir) {
            inputPath = Path.GetFullPath(inputPath);
            if (!Directory.Exists(outpurdir))
                Directory.CreateDirectory(outpurdir);
            await uniqalize(inputPath, outpurdir, n, false);
        }

        public async Task Uniqalize(ICreative creative, int n, string outputdir) {

            string inputPath = Path.GetFullPath(creative.LocalPath);

            string outputFolderPath = Path.Combine(outputdir, creative.ServerDirectory.dir, creative.Name);
            if (!Directory.Exists(outputFolderPath))
                Directory.CreateDirectory(outputFolderPath);

            await uniqalize(inputPath, outputFolderPath, n, true);
        }

        public void Cancel()
        {
            cts?.Cancel();            
        }
        #endregion

        #region callbacks
        public event Action<int> UniqalizeProgessUpdateEvent;
        #endregion
    }
}
