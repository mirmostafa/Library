using System;
using Mohammad.Specialized;

namespace Mohammad.EventsArgs
{
    public class DownloadCompletedEventArgs : YoutubeDownloaderEventArgs
    {
        public DownloadCompletedEventArgs(YoutubeVideoInfo video, Exception error, bool cancelled)
            : base(video)
        {
            this.Error = error;
            this.Cancelled = cancelled;
        }

        public Exception Error { get; }
        public bool Cancelled { get; }
    }

    public class TotalBytesToReceiveChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public TotalBytesToReceiveChangedEventArgs(YoutubeVideoInfo video, long totalBytesToReceive)
            : base(video) => this.TotalBytesToReceive = totalBytesToReceive;

        public long TotalBytesToReceive { get; }
    }

    public class BytesReceivedChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public BytesReceivedChangedEventArgs(YoutubeVideoInfo video, long bytesReceived)
            : base(video) => this.BytesReceived = bytesReceived;

        public long BytesReceived { get; }
    }

    public class ProgressChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public ProgressChangedEventArgs(YoutubeVideoInfo video, int progess)
            : base(video) => this.Progess = progess;

        public int Progess { get; }
    }

    public abstract class YoutubeDownloaderEventArgs : EventArgs
    {
        protected YoutubeDownloaderEventArgs(YoutubeVideoInfo video) => this.Video = video;
        public YoutubeVideoInfo Video { get; }
    }
}