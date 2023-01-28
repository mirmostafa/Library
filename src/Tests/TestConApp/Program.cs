using System.Net;

using Library.Coding;
using Library.Helpers;
using Library.Helpers.Models;
using Library.Net;

namespace ConAppTest;

internal partial class Program
{
    private static void Main(string[] args)
    {
        //var result = OldDownloader.Download("http://dejanstojanovic.net/media/215073/optimize-jpg.zip", @"c:\temp\");

        //WriteLine($"Location: {result.FilePath}");
        //WriteLine($"Size: {result.Size}bytes");
        //WriteLine($"Time taken: {result.TimeTaken.Milliseconds}ms");
        //WriteLine($"Parallel: {result.ParallelDownloads}");

        //var sizeTask = RemoteFile.GetFileSizeAsync(@"https://aspb16.cdn.asset.aparat.com/aparat-video/c62ac0f6959b9c2a932379549bac41c120708902-240p.mp4?wmsAuthSign=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0b2tlbiI6Ijk0YTE2ZGM2NDZmZmI0NjNkNGYyYmU4MTZjYjM2Y2MxIiwiZXhwIjoxNjc0ODU3ODA0LCJpc3MiOiJTYWJhIElkZWEgR1NJRyJ9.uULyakihsaXzdOIT8JOSe4f6McWL_T3FPgvOxcZ-C5k");
        var sizeTask = RemoteFile.GetFileSizeAsync(@"D:\adaptive-icon-monochrome.png");
        var size = sizeTask.Result.ToStandardMetricScale();
        WriteLine(size);
    }
}