﻿using static TestConApp.Downloader;

namespace TestConApp;

public sealed class Downloader(in Uri downloadUri,
                  in string destinationPath,
                  in string? destinationFile = null,
                  in int? threadCount = null,
                  in (Action<Downloader.Report> Main, Func<Action<Downloader.Report>> SubCreator)? reports = null,
                  in CancellationTokenSource? cancellationToken = null)
{
    private readonly Uri _downloadUri = downloadUri;
    private readonly int _threadCount = threadCount is null or <= 0 ? Environment.ProcessorCount : threadCount.Value;
    private readonly string _destinationPath = destinationPath;
    private readonly string _destinationFile = destinationFile ?? ExtractFileName(downloadUri);
    private readonly CancellationTokenSource _cancellationToken = cancellationToken ?? new CancellationTokenSource();
    private readonly (Action<Report> Main, Func<Action<Report>> SubCreator) _reports = reports ?? (_ => { }, () => _ => { });

    private static string ExtractFileName(in Uri downloadUri)
        => downloadUri.Segments.Last();

    public Task<Result> StartAsync()
    {
        this._reports.Main.Report(-1, -1, "Starting");

        this._reports.Main.Report(-1, -1, "Done");
        return Task.FromResult(new Result());
    }
    public static Task<Result> DownloadAsync(in Uri downloadUri,
                      in string destinationPath,
                      in string? destinationFile = null,
                      in int? threadCount = null,
                      in (Action<Report> Main, Func<Action<Report>> SubCreator)? reports = null,
                      in CancellationTokenSource? cancellationToken = null)
        => new Downloader(downloadUri, destinationPath, destinationFile, threadCount, reports, cancellationToken).StartAsync();
    public void Cancel()
    {
        if (this._cancellationToken.IsCancellationRequested)
        {
            this._cancellationToken.Cancel();
        }
    }

    public record Report(int Current, int Max, string Description);
    public record Result();
}

internal static class ReportExtension
{
    public static void Report(this Action<Report>? report, int current, int max, string description)
        => report?.Invoke(new(current, max, description));
}