using System;
using Mohammad.Specialized;

namespace Mohammad.EventsArgs
{
    public class DownloadCompletedEventArgs : YoutubeDownloaderEventArgs
    {
        public Exception Error { get; private set; }
        public bool Cancelled { get; private set; }

        public DownloadCompletedEventArgs(YoutubeVideoInfo video, Exception error, bool cancelled)
            : base(video)
        {
            this.Error = error;
            this.Cancelled = cancelled;
        }
    }

    public class TotalBytesToReceiveChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public long TotalBytesToReceive { get; private set; }

        public TotalBytesToReceiveChangedEventArgs(YoutubeVideoInfo video, long totalBytesToReceive)
            : base(video) { this.TotalBytesToReceive = totalBytesToReceive; }
    }

    public class BytesReceivedChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public long BytesReceived { get; private set; }

        public BytesReceivedChangedEventArgs(YoutubeVideoInfo video, long bytesReceived)
            : base(video) { this.BytesReceived = bytesReceived; }
    }

    public class ProgressChangedEventArgs : YoutubeDownloaderEventArgs
    {
        public int Progess { get; private set; }

        public ProgressChangedEventArgs(YoutubeVideoInfo video, int progess)
            : base(video) { this.Progess = progess; }
    }

    public abstract class YoutubeDownloaderEventArgs : EventArgs
    {
        public YoutubeVideoInfo Video { get; private set; }
        protected YoutubeDownloaderEventArgs(YoutubeVideoInfo video) { this.Video = video; }
    }
}