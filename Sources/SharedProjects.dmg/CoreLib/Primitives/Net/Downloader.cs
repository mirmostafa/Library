using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Mohammad.Threading;

namespace Mohammad.Net
{
    public static class Downloader
    {
        public static async Task<string> DownloadPageAsyncTask(string page)
        {
            var request = WebRequest.Create(page);
            return await request.BeginGetResponse(null, null).AsAwaiter(ar =>
            {
                using (var responce = request.EndGetResponse(ar))
                {
                    if (responce == null)
                        return null;
                    var reader = new StreamReader(responce.GetResponseStream());
                    return reader.ReadToEnd();
                }
            });
        }

        public static AsyncResultAwaiter<TResult> AsAwaiter<TResult>(this IAsyncResult asyncResult, Func<IAsyncResult, TResult> getResult)
            => new AsyncResultAwaiter<TResult>(asyncResult, getResult);

        public static void DownloadFile(string uri, string fileName)
        {
            using (var client = new WebClient())
                client.DownloadFile(uri, fileName);
        }

        public static async void DownloadFileAsync(string uri, string fileName)
        {
            using (var client = new WebClient())
                await client.DownloadFileTaskAsync(uri, fileName);
        }
    }
}