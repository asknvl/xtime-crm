﻿using crm.Models.creatives;
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

namespace crm.Models.uniq
{
    public class Uniqalizer : IUniqalizer
    {
        #region vars
        Random random = new Random();
        CancellationTokenSource cts;
        #endregion
        public Uniqalizer()
        {
        }

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
            FFmpeg.SetExecutablesPath(codecdir);

            var prgrsconv = new Progress<ProgressInfo>((p) =>
            {
                int intp = (int)(p.DownloadedBytes * 100.0d / p.TotalBytes);
                progress?.Invoke(intp);
                Debug.WriteLine(p.DownloadedBytes + " " + p.TotalBytes + " " + intp);
            });

            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, codecdir, prgrsconv);
        }
        public async Task Uniqalize(ICreative creative, int n, string outputdir) {

            string inputPath = Path.GetFullPath(creative.LocalPath);

            string outputFolderPath = Path.Combine(outputdir, creative.GEO.Code, creative.Name);
            if (!Directory.Exists(outputFolderPath))
                Directory.CreateDirectory(outputFolderPath);

            DirectoryInfo directoryInfo = new DirectoryInfo(outputFolderPath);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            cts = new CancellationTokenSource();

            for (int i = 0; i < n; i++)
            {
                var outputPath = Path.Combine(outputFolderPath, $"NEW_UNIQ_{i + 1}.mp4");
                
                IMediaInfo info = await FFmpeg.GetMediaInfo(inputPath);
                IVideoStream videoStream = info.VideoStreams.First();
                IStream audioStream = info.AudioStreams.FirstOrDefault()?.SetChannels(2);

                long origBitRate = videoStream.Bitrate;
                long bitrate = getBitRate(origBitRate);

                try
                {

                    var conversion = FFmpeg.Conversions.New();
                    conversion.OnProgress += (s, a) => {
                        UniqalizeProgessUpdateEvent?.Invoke(a.Percent);
                    };

                    IConversionResult conversionResult = await conversion
                        .AddStream(videoStream, audioStream)
                        .SetOutput(outputPath)                        
                        .AddParameter($"-b:v {bitrate} -bufsize {bitrate}")
                        .Start(cts.Token);

                } catch (Exception ex) {

                    deleteFiles(outputFolderPath);                

                } finally
                {
                    UniqalizeProgessUpdateEvent.Invoke(0);
                }
            }
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
