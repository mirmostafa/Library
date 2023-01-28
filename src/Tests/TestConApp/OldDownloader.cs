using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading;

using Library.Coding;
using Library.Helpers;

namespace ConAppTest;

public record struct DownloadResult(in string? FilePath, in int ParallelDownloads, in long Size, in TimeSpan TimeTaken);

internal record struct Range(in long End, in long Start);

public static class OldDownloader
{
    static OldDownloader()
    {
        ServicePointManager.Expect100Continue = false;
        ServicePointManager.DefaultConnectionLimit = 100;
        ServicePointManager.MaxServicePointIdleTime = 1000;
    }

    public static DownloadResult Download(in string fileUrl,
                                          in string destinationFolderPath,
                                          in int numberOfParallelDownloads = 0,
                                          in bool validateSSL = false,
                                          in IProgressiveReport<string>? main = null,
                                          in Func<IProgressiveReport<string>>? subReportCreator = null,
                                          in CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? CancellationToken.None;
        var subCreator = subReportCreator ?? (() => IProgressiveReport<string>.Empty);
        if (!validateSSL)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        main?.Report("Initializing", 1, 3);
        var startTime = DateTime.Now;
        var downloadThreadsCount = handleNumberOfParallelDownloads(numberOfParallelDownloads);
        var tempFilesDictionary = new ConcurrentDictionary<int, string>();
        var result = new DownloadResult
        {
            FilePath = calculateDestinationPath(fileUrl, destinationFolderPath),
            Size = getFileSize(fileUrl)
        };
        if (token.IsCancellationRequested)
        {
            main?.Report("Downloading is cancelled.", 3, 3);
            return result;
        }

        main?.Report("Downloading", 2, 3);
        result.ParallelDownloads = parallelDownload(fileUrl, downloadThreadsCount, tempFilesDictionary, result.Size, token);
        if (token.IsCancellationRequested)
        {
            main?.Report("Downloading is cancelled.", 3, 3);
            return result;
        }

        main?.Report("Merging chunks", 3, 3);
        using var destinationFileStream = createDestinationFile(result.FilePath);
        mergeChunks(destinationFileStream, tempFilesDictionary);
        result.TimeTaken = DateTime.Now.Subtract(startTime);
        if (token.IsCancellationRequested)
        {
            main?.Report("Downloading is cancelled.", 3, 3);
            return result;
        }

        main?.Report("Downloading is done.", 3, 3);
        return result;

        #region Local Methods

        static long getFileSize(in string fileUrl)
        {
            var webRequest = WebRequest.Create(fileUrl);
            webRequest.Method = "HEAD";
            using var webResponse = webRequest.GetResponse();
            return long.Parse(webResponse.Headers.Get("Content-Length")!);
        }

        static FileStream createDestinationFile(in string destinationFilePath)
        {
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }
            if (!Directory.Exists(Path.GetDirectoryName(destinationFilePath)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath)!);
            }

            return File.Create(destinationFilePath);
        }

        static string calculateDestinationPath(in string fileUrl, in string destinationFolderPath)
        {
            var uri = new Uri(fileUrl);
            var destinationFilePath = Path.Combine(destinationFolderPath, uri.Segments.Last());
            return destinationFilePath;
        }

        static int handleNumberOfParallelDownloads(in int numberOfParallelDownloads)
            => numberOfParallelDownloads <= 0 ? Environment.ProcessorCount : numberOfParallelDownloads;

        static List<Range> calculateRanges(in int downloadThreadsCount, in long responseLength)
        {
            var readRanges = new List<Range>();
            for (var chunk = 0; chunk < downloadThreadsCount - 1; chunk++)
            {
                var range = new Range()
                {
                    Start = chunk * (responseLength / downloadThreadsCount),
                    End = ((chunk + 1) * (responseLength / downloadThreadsCount)) - 1
                };
                readRanges.Add(range);
            }

            readRanges.Add(new Range()
            {
                Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                End = responseLength - 1
            });
            return readRanges;
        }

        static int parallelDownload(string fileUrl, in int downloadThreadsCount, ConcurrentDictionary<int, string> tempFilesDictionary, in long responseLength, CancellationToken cancellationToken)
        {
            var parallelDownloads = 0;
            var readRanges = calculateRanges(downloadThreadsCount, responseLength);
            _ = Parallel.ForEach(readRanges, new ParallelOptions { MaxDegreeOfParallelism = downloadThreadsCount, CancellationToken = cancellationToken }, readRange =>
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(fileUrl);
                httpWebRequest.Method = "GET";
                httpWebRequest.AddRange(readRange.Start, readRange.End);
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    var tempFilePath = Path.GetTempFileName();
                    using var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write);
                    httpWebResponse.GetResponseStream().CopyTo(fileStream);
                    _ = tempFilesDictionary.TryAdd(parallelDownloads, tempFilePath);
                }
                parallelDownloads++;
            });
            return parallelDownloads;
        }

        static void mergeChunks(in FileStream destinationStream, in ConcurrentDictionary<int, string> tempFilesDictionary)
        {
            foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Key))
            {
                var tempFileBytes = File.ReadAllBytes(tempFile.Value);
                destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);
                File.Delete(tempFile.Value);
            }
            destinationStream.Flush(true);
        }

        #endregion Local Methods
    }
}