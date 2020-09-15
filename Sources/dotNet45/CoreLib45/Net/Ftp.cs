// Created on     2018/07/23

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mohammad.DesignPatterns.ExceptionHandlingPattern;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Logging;
using Mohammad.Threading.Tasks;

namespace Mohammad.Net
{
    public class Ftp : IExceptionHandlerContainer, ILoggerContainer
    {
        private const int BUFFER_SIZE = 2048;
        private readonly string _Host;
        private readonly string _Pass;
        private readonly string _User;
        private ExceptionHandling _ExceptionHandling;

        public Ftp(string hostIp, string userName, string password, ExceptionHandling exceptionHandling = null)
        {
            this._Host = (hostIp ?? string.Empty).Normalize();
            this._User = (userName ?? string.Empty).Normalize();
            this._Pass = (password ?? string.Empty).Normalize();
            this.ExceptionHandling = exceptionHandling;
        }

        public bool UsePassive { get; set; }

        public ExceptionHandling ExceptionHandling
        {
            get => this._ExceptionHandling ?? (this._ExceptionHandling = new ExceptionHandling {RaiseExceptions = true});
            private set => this._ExceptionHandling = value;
        }

        public ILogger Logger { get; set; }
        public object DefaultLogSender => "Ftp";

        public static Ftp CreateInstance(string hostIp, string userName, string password, ExceptionHandling exceptionHandling = null) => new Ftp(hostIp,
            userName,
            password,
            exceptionHandling);

        public static void Validate(string hostIp, string userName, string password, ExceptionHandling exceptionHandling = null) => new Ftp(hostIp,
                userName,
                password,
                exceptionHandling)
            .Validate();

        public static bool IsValid(string hostIp, string userName, string password, ExceptionHandling exceptionHandling = null) => new Ftp(hostIp,
                userName,
                password,
                exceptionHandling)
            .IsValid();

        public static void Upload(string remoteFile,
            string localFile,
            string hostIp,
            string userName,
            string password,
            ExceptionHandling exceptionHandling = null)
            => new Ftp(hostIp, userName, password, exceptionHandling).Upload(remoteFile, localFile);

        public static void Download(string remoteFile,
            string localFile,
            string hostIp,
            string userName,
            string password,
            ExceptionHandling exceptionHandling = null)
            => new Ftp(hostIp, userName, password, exceptionHandling).Download(remoteFile, localFile);

        public bool TryUpload(string ftpPath, string patch, int retryCount = 3, Action<Ftp, int> onTrying = null, int delay = 50)
        {
            this.ExceptionHandling.RaiseExceptions = false;
            var tryCount = 0;
            while (tryCount++ < retryCount)
            {
                onTrying?.Invoke(this, tryCount);
                this.UsePassive = !this.UsePassive;
                Debug.WriteLine($"Trying to upload {Path.GetFileName(patch)} for {tryCount} time.");
                if (this.Upload(ftpPath, patch))
                {
                    return true;
                }

                Thread.Sleep(delay);
            }

            return false;
        }

        public async Task<bool> TryUploadAsync(string ftpPath, string patch, int retryCount = 3, Action<Ftp, int> onTrying = null, int delay = 50) =>
            await Async.Run(() => this.TryUpload(ftpPath, patch, retryCount, onTrying, delay));

        public void Validate() => this.Run(".", WebRequestMethods.Ftp.ListDirectory);

        public bool IsValid() => CodeHelper.Catch(this.Validate) == null;

        public async Task DownloadAsync(string remoteFile, string localFile) =>
            await Async.Run(() => this.Download(remoteFile, localFile));

        public void Download(string remoteFile, string localFile)
        {
            this.Log($"Starting to download from '{remoteFile}' to '{localFile}'");
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(this._Host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(this._User, this._Pass);
                ftpRequest.UseBinary = true;
                //ftpRequest.UsePassive = true;
                ftpRequest.UsePassive = false;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = ftpResponse.GetResponseStream();
                var localFileStream = new FileStream(localFile, FileMode.Create);
                var byteBuffer = new byte[BUFFER_SIZE];
                if (ftpStream != null)
                {
                    var bytesRead = ftpStream.Read(byteBuffer, 0, BUFFER_SIZE);
                    try
                    {
                        while (bytesRead > 0)
                        {
                            this.Log($"{bytesRead} bytes read from {remoteFile} to {localFile}.");
                            this.OnBytesRead(new ItemActedEventArgs<int>(bytesRead));
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = ftpStream.Read(byteBuffer, 0, BUFFER_SIZE);
                        }

                        this.Log($"read from {remoteFile} to {localFile} is completed..");
                    }
                    catch (Exception ex)
                    {
                        this.Log($"Exception while reading: {Environment.NewLine}{ex.GetBaseException().Message}");
                        this.ExceptionHandling.HandleException(this, ex);
                    }
                }

                localFileStream.Close();
                ftpStream?.Close();
                ftpResponse.Close();
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex);
            }
        }

        public async Task<bool> UploadAsync(string remoteFile, string localFile) => await Async.Run(() => this.Upload(remoteFile, localFile));

        public bool Upload(string remoteFile, string localFile)
        {
            this.ExceptionHandling.Reset();
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(this._Host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(this._User, this._Pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.UsePassive;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                var ftpStream = ftpRequest.GetRequestStream();
                var localFileStream = new FileStream(localFile, FileMode.Open);
                var byteBuffer = new byte[BUFFER_SIZE];
                var bytesSent = localFileStream.Read(byteBuffer, 0, BUFFER_SIZE);
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, BUFFER_SIZE);
                    }

                    return true;
                }
                finally
                {
                    localFileStream.Close();
                    ftpStream?.Close();
                }
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex, $"Exception on uploading file{Path.GetFileName(localFile)}");

                return false;
            }
        }

        public void Delete(string deleteFile) => this.Run(deleteFile, WebRequestMethods.Ftp.DeleteFile);

        public void Rename(string currentFileNameAndPath, string newFileName)
            => this.Run(currentFileNameAndPath, WebRequestMethods.Ftp.Rename, null, initFtpWebRequest: req => req.RenameTo = newFileName);

        public void CreateDirectory(string newDirectory) => this.Run(newDirectory, WebRequestMethods.Ftp.MakeDirectory);

        public string GetFileCreatedDateTime(string fileName) => this.Run(fileName, WebRequestMethods.Ftp.GetDateTimestamp, reader => reader.ReadToEnd());

        public string GetFileSize(string fileName) => this.Run(fileName,
            WebRequestMethods.Ftp.GetFileSize,
            reader =>
            {
                string fileInfo = null;
                while (reader.Peek() != -1)
                {
                    fileInfo = reader.ReadToEnd();
                }

                return fileInfo;
            });

        public string[] DirectoryListSimple(string directory) => this.Run(directory,
            WebRequestMethods.Ftp.ListDirectory,
            reader =>
            {
                string directoryRaw = null;
                while (reader.Peek() != -1)
                {
                    directoryRaw += reader.ReadLine() + "|";
                }

                var directoryList = directoryRaw?.Split("|".ToCharArray());
                return directoryList;
            });

        public string[] DirectoryListDetailed(string directory) => this.Run(directory,
            WebRequestMethods.Ftp.ListDirectoryDetails,
            reader =>
            {
                string directoryRaw = null;
                while (reader.Peek() != -1)
                {
                    directoryRaw += reader.ReadLine() + "|";
                }

                var directoryList = directoryRaw.Split("|".ToCharArray());
                return directoryList;
            },
            resultOnException: new[] {""});

        public override string ToString() => $"{nameof(this._Host)}: {this._Host}, {nameof(this._User)}: {this._User}";

        protected virtual void OnBytesRead(ItemActedEventArgs<int> e) => this.Downloading?.Invoke(this, e);

        private void Log(string log) => this.Logger?.Info(log);

        private void Run(string path,
            string method,
            Action<StreamReader> action = null,
            Action disposer = null,
            Action<FtpWebRequest> initFtpWebRequest = null)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(this._Host + "/" + path);
                ftpRequest.Credentials = new NetworkCredential(this._User, this._Pass);
                ftpRequest.UseBinary = true;
                //ftpRequest.UsePassive = true;
                ftpRequest.UsePassive = false;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = method;
                initFtpWebRequest?.Invoke(ftpRequest);
                using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                using (var ftpStream = ftpResponse.GetResponseStream())
                using (var ftpReader = new StreamReader(ftpStream))
                {
                    action?.Invoke(ftpReader);
                }
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex);
            }
            finally
            {
                disposer?.Invoke();
            }
        }

        private TResult Run<TResult>(string path,
            string method,
            Func<StreamReader, TResult> action,
            Action disposer = null,
            TResult resultOnException = default)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create(this._Host + "/" + path);
                ftpRequest.Credentials = new NetworkCredential(this._User, this._Pass);
                ftpRequest.UseBinary = true;
                //ftpRequest.UsePassive = true;
                ftpRequest.UsePassive = false;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = method;
                using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                using (var ftpStream = ftpResponse.GetResponseStream())
                using (var ftpReader = new StreamReader(ftpStream))
                {
                    return action(ftpReader);
                }
            }
            catch (Exception ex)
            {
                this.ExceptionHandling.HandleException(this, ex);
                return resultOnException;
            }
            finally
            {
                disposer?.Invoke();
            }
        }

        public event EventHandler<ItemActedEventArgs<int>> Downloading;
    }
}