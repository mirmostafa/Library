#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.Specialized;

namespace Mohammad.EventsArgs
{
    public class DownloadCompletedEventArgs : YoutubeDownloaderEventArgs
    {
        public Exception Error { get; }
        public bool Cancelled { get; }

        public DownloadCompletedEventArgs(YoutubeVideoInfo video, Exception error, bool cancelled)
            : base(video)
        {
            this.Error = error;
            this.Cancelled = cancelled;
        }
    }

    public class TotalBytesToReceiveChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public long TotalBytesToReceive { get; }

        public TotalBytesToReceiveChangedEventArgs(YoutubeVideoInfo video, long totalBytesToReceive)
            : base(video) => this.TotalBytesToReceive = totalBytesToReceive;
    }

    public class BytesReceivedChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public long BytesReceived { get; }

        public BytesReceivedChangedEventArgs(YoutubeVideoInfo video, long bytesReceived)
            : base(video) => this.BytesReceived = bytesReceived;
    }

    public class ProgressChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public int Progess { get; }

        public ProgressChangedEventArgs(YoutubeVideoInfo video, int progess)
            : base(video) => this.Progess = progess;
    }

    public abstract class YoutubeDownloaderEventArgs : EventArgs
    {
        public YoutubeVideoInfo Video { get; }
        protected YoutubeDownloaderEventArgs(YoutubeVideoInfo video) => this.Video = video;
    }
}