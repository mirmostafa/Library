using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Properties;
using ProgressChangedEventArgs = Mohammad.EventsArgs.ProgressChangedEventArgs;

namespace Mohammad.Specialized
{
    public class YoutubeVideoInfo : INotifyPropertyChanged
    {
        private string _Author;
        private string _FileName;
        private string _Id;
        private long _Length;
        private List<YoutubeVideoFormat> _SupportedFormats;
        private string _ThumbnailUrl;
        private string _Title;
        private long _ViewCount;

        public string Author
        {
            get { return this._Author; }
            internal set
            {
                if (value == this._Author)
                    return;
                this._Author = value;
                this.OnPropertyChanged();
            }
        }

        public string FileName
        {
            get { return this._FileName; }
            internal set
            {
                if (this._FileName == value)
                    return;
                this._FileName = value;
                this.OnPropertyChanged();
            }
        }

        public List<YoutubeVideoFormat> SupportedFormats
        {
            get { return this._SupportedFormats; }
            internal set
            {
                if (Equals(value, this._SupportedFormats))
                    return;
                this._SupportedFormats = value;
                this.OnPropertyChanged();
            }
        }

        public string Id
        {
            get { return this._Id; }
            set
            {
                if (value == this._Id)
                    return;
                this._Id = value;
                this.OnPropertyChanged();
            }
        }

        public long Length
        {
            get { return this._Length; }
            internal set
            {
                if (value == this._Length)
                    return;
                this._Length = value;
                this.OnPropertyChanged();
            }
        }

        public string ThumbnailUrl
        {
            get { return this._ThumbnailUrl; }
            internal set
            {
                if (value == this._ThumbnailUrl)
                    return;
                this._ThumbnailUrl = value;
                this.OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return this._Title; }
            internal set
            {
                if (value == this._Title)
                    return;
                this._Title = value;
                this.OnPropertyChanged();
            }
        }

        internal string Url { get; set; }

        public long ViewCount
        {
            get { return this._ViewCount; }
            internal set
            {
                if (value == this._ViewCount)
                    return;
                this._ViewCount = value;
                this.OnPropertyChanged();
            }
        }

        internal static YoutubeVideoInfo FromVideoInfoPage(string page)
        {
            var values = HttpUtility.ParseQueryString(page);
            var result = new YoutubeVideoInfo
                         {
                             Id = values["video_id"],
                             Title = values["title"],
                             ThumbnailUrl = values["thumbnail_url"],
                             Length = long.Parse(values["length_seconds"]),
                             Author = values["author"],
                             ViewCount = long.Parse(values["view_count"]),
                             SupportedFormats =
                                 values["url_encoded_fmt_stream_map"].Split(',').Select(YoutubeVideoFormat.FromFormatStreamMapItem).ToList()
                         };
            result.FileName = Regex.Replace(result.Title, "[:*?\"<>|\\\\/ \\t]", string.Empty);
            if (string.IsNullOrEmpty(result.FileName))
                result.FileName = result.Id;
            return result;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class YoutubeVideoFormat : INotifyPropertyChanged
    {
        private string _Format;
        private string _Resolution;
        private long _Size;

        public string Format
        {
            get { return this._Format; }
            internal set
            {
                if (value == this._Format)
                    return;
                this._Format = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("OutputFilePath");
            }
        }

        public string Resolution
        {
            get { return this._Resolution; }
            internal set
            {
                if (value == this._Resolution)
                    return;
                this._Resolution = value;
                this.OnPropertyChanged();
            }
        }

        public long Size
        {
            get { return this._Size; }
            internal set
            {
                if (value == this._Size)
                    return;
                this._Size = value;
                this.OnPropertyChanged();
            }
        }

        internal string Url { get; set; }

        internal static YoutubeVideoFormat FromFormatStreamMapItem(string format)
        {
            var values = HttpUtility.ParseQueryString(format);
            if (values["sig"] == null)
                throw new Exception("Encrypted Signature");
            return new YoutubeVideoFormat
                   {
                       Url = string.Format("{0}&fallback_host={1}&signature={2}", values["url"], values["fallback_host"], values["sig"]),
                       Resolution = GetResolution(values["itag"]),
                       Format = GetFormat(values["type"])
                   };
        }

        private static string GetFormat(string type)
        {
            type = type.ToLower();
            if (type.Contains("video/webm"))
                return "WEBM";
            if (type.Contains("video/mp4"))
                return "MP4";
            if (type.Contains("video/x-flv"))
                return "FLV";
            return type.Contains("video/3gpp") ? "3GP" : "Unknown";
        }

        private static string GetResolution(string itag)
        {
            switch (itag)
            {
                case "5":
                    return "240p";

                case "6":
                    return "270p";

                case "13":
                    return "360p";

                case "17":
                    return "144p";

                case "18":
                    return "270p/360p";

                case "22":
                    return "720p";

                case "34":
                    return "360p";

                case "35":
                    return "480p";

                case "36":
                    return "240p";

                case "37":
                    return "1080p";

                case "38":
                    return "3072p";

                case "43":
                    return "360p";

                case "44":
                    return "480p";

                case "45":
                    return "720p";

                case "46":
                    return "1080p";

                case "82":
                    return "360p(3D)";

                case "83":
                    return "240p(3D)";

                case "84":
                    return "720p(3D)";

                case "85":
                    return "520p(3D)";

                case "100":
                    return "360p(3D)";

                case "101":
                    return "360p(3D)";

                case "102":
                    return "720p(3D)";

                case "120":
                    return "720p";

                case "133":
                    return "240p";

                case "134":
                    return "360p";

                case "135":
                    return "480p";

                case "136":
                    return "720p";

                case "137":
                    return "1080p";

                case "139":
                    return "Low";

                case "140":
                    return "Medium";

                case "141":
                    return "High";

                case "160":
                    return "144p";
            }
            return "Unknown";
        }

        public override string ToString() { return string.Format("{0} ({1})", this.Format, this.Resolution); }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class YoutubeDownloader : LoggerContainer, INotifyPropertyChanged, IDisposable
    {
        private readonly WebClient _Client = new WebClient();
        private long _BytesReceived;
        private YoutubeVideoFormat _Format;
        private int _Progress;
        private long _TotalBytesReceived;
        private YoutubeVideoInfo _VideoInfo;

        public YoutubeVideoFormat Format
        {
            get { return this._Format; }
            set
            {
                if (Equals(value, this._Format))
                    return;
                this._Format = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged("OutputFilePath");
            }
        }

        public int Progress
        {
            get { return this._Progress; }
            set
            {
                if (this._Progress == value)
                    return;
                this._Progress = value;
                this.Logger.Debug(
                    string.Format("[{2}%] {0} of {1}", this.BytesReceived.ToMesuranceSystem(), this.TotalBytesReceived.ToMesuranceSystem(), this.Progress),
                    sender: this.VideoInfo.Id);
                this.OnPropertyChanged();
                this.OnProgressChanged();
            }
        }

        public long BytesReceived
        {
            get { return this._BytesReceived; }
            set
            {
                if (this._BytesReceived == value)
                    return;
                this._BytesReceived = value;
                this.Logger.Debug(
                    string.Format("[{2}%] {0} of {1}", this.BytesReceived.ToMesuranceSystem(), this.TotalBytesReceived.ToMesuranceSystem(), this.Progress),
                    sender: this.VideoInfo.Id);
                this.OnPropertyChanged();
                this.OnBytesReceivedChanged();
            }
        }

        public long TotalBytesReceived
        {
            get { return this._TotalBytesReceived; }
            set
            {
                if (this._TotalBytesReceived == value)
                    return;
                this._TotalBytesReceived = value;
                this.Logger.Debug(
                    string.Format("[{2}%] {0} of {1}", this.BytesReceived.ToMesuranceSystem(), this.TotalBytesReceived.ToMesuranceSystem(), this.Progress),
                    sender: this.VideoInfo.Id);
                this.OnPropertyChanged();
                this.OnTotalBytesReceivedChanged();
            }
        }

        public YoutubeVideoInfo VideoInfo
        {
            get { return this._VideoInfo; }
            set
            {
                if (Equals(value, this._VideoInfo))
                    return;
                this._VideoInfo = value;
                if (this.Logger.IsDebugModeEnabled)
                {
                    var sb = new StringBuilder("Information gathered:");
                    sb.AppendLine();
                    foreach (var property in ObjectHelper.ReflectProperties(value, false))
                    {
                        sb.AppendFormat("{0}:\t{1}", property.Key, property.Value);
                        sb.AppendLine();
                    }
                    this.Logger.Debug(sb.ToString(), sender: this.VideoInfo.Id);
                }
                else
                {
                    this.Info(string.Format("Information gathered: {0}", value.Title));
                }
                this.OnPropertyChanged();
                this.OnPropertyChanged("OutputFilePath");
            }
        }

        public YoutubeDownloader(TextWriter writer)
            : this()
        {
            this.Out = writer;
        }

        private YoutubeDownloader()
        {
            this._Client.DownloadProgressChanged += this.Client_OnDownloadProgressChanged;
            this._Client.DownloadFileCompleted += this.Client_OnDownloadFileCompleted;
        }

        public event EventHandler<BytesReceivedChangedEventArgs> BytesReceivedChanged;

        protected virtual void OnBytesReceivedChanged()
        {
            var e = new BytesReceivedChangedEventArgs(this.VideoInfo, this.BytesReceived);
            var handler = this.BytesReceivedChanged;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TotalBytesToReceiveChangedEventArgs> TotalBytesReceivedChanged;

        protected virtual void OnTotalBytesReceivedChanged()
        {
            var e = new TotalBytesToReceiveChangedEventArgs(this.VideoInfo, this.TotalBytesReceived);
            var handler = this.TotalBytesReceivedChanged;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        protected virtual void OnProgressChanged()
        {
            var handler = this.ProgressChanged;
            if (handler != null)
                handler(this, new ProgressChangedEventArgs(this.VideoInfo, this.Progress));
        }

        public void Cancel() { this._Client.CancelAsync(); }

        private void Client_OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                this.Error("Job not completed.", e.Error);
            else
                this.Info(string.Format("Job for {0} is {1}.", this.VideoInfo.Id, e.Cancelled ? "canceled" : "done"));
            this.OnDownloadCompleted(new DownloadCompletedEventArgs(this.VideoInfo, e.Error, e.Cancelled));
        }

        protected virtual void OnDownloadCompleted(DownloadCompletedEventArgs e) { this.DownloadCompleted.Raise(this, e); }

        private void Client_OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Progress = e.ProgressPercentage;
            this.BytesReceived = e.BytesReceived;
            this.TotalBytesReceived = e.TotalBytesToReceive;
        }

        public void Start() { this.Start(Environment.CurrentDirectory); }

        public void Start(string outputDirectory, YoutubeVideoFormat format = null)
        {
            if (outputDirectory == null)
                throw new ArgumentNullException("outputDirectory");
            var youtubeVideoFile = format ?? this.Format;
            if (youtubeVideoFile == null)
                throw new NullReferenceException("Please fill Format type to download.");
            //this._Client.DownloadFileAsync(new Uri(youtubeVideoFile.Url), Path.Combine(outputDirectory, string.Format("{0}.{1}", this.VideoInfo.FileName, this.Format.Format)));
            var uri = new Uri(youtubeVideoFile.Url);
            this.Logger.Debug(string.Format("download url: {0}", uri), sender: this.VideoInfo.Id);
            this._Client.DownloadFileAsync(uri,
                Path.Combine(outputDirectory, string.Format("{0}.{1}", this.VideoInfo.Title.Replace("/", "_").Replace(@"\", "_"), youtubeVideoFile.Format)));
            this.Logger.Debug("Download started.", sender: this.VideoInfo.Id);
        }

        public static YoutubeDownloader FromUrl(string url, TextWriter logWriter = null, bool isDebugModeEnabled = false)
        {
            var videoInfoClient = new WebClient();

            if (logWriter != null)
                logWriter.WriteLine("Gathering video information");
            var v = HttpUtility.ParseQueryString(new Uri(url).Query)["v"];
            var page =
                videoInfoClient.DownloadString(string.Format("http://www.youtube.com/get_video_info?&video_id={0}&el=detailpage&ps=default&eurl=&gl=US&hl=en", v));
            if (logWriter != null)
                logWriter.WriteLine("Parsing video information");
            var video = YoutubeVideoInfo.FromVideoInfoPage(page);
            if (logWriter != null)
                logWriter.WriteLine("Initializing");
            var result = new YoutubeDownloader(logWriter);
            result.Logger.IsDebugModeEnabled = isDebugModeEnabled;
            result.VideoInfo = video;
            result.Format = video.SupportedFormats.FirstOrDefault();
            return result;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            if (this._Client != null)
                this._Client.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}