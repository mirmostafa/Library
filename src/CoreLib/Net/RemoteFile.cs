using System.Net;

using Library.Validations;

namespace Library.Net;

public sealed class RemoteFile
{
    public RemoteFile([DisallowNull] string fullPath)
        : this(new Uri(fullPath))
    {
    }

    public RemoteFile([DisallowNull] Uri fileUri)
        => this.FileUri = fileUri.Check().ArgumentNotNull();

    public Uri FileUri { get; }

    public static Task<long> GetFileSizeAsync(string filePath) 
        => GetFileSizeAsync(new Uri(filePath));

    public static async Task<long> GetFileSizeAsync(Uri fileUrl)
    {
        var webRequest = WebRequest.Create(fileUrl);
        webRequest.Method = "HEAD";
        using var webResponse = await webRequest.GetResponseAsync();
        return long.Parse(webResponse.Headers.Get("Content-Length")!);
    }

    public Task<long> GetFileSizeAsync()
        => GetFileSizeAsync(this.FileUri);
}