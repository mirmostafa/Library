using System.IO;
using System.Web;

namespace Mohammad.Web.Api.Tools
{
    public static class DownloadHelper
    {
        public static void StreamDownload(string filepath)
        {
            var filename = Path.GetFileName(filepath);
            using (Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bytesToRead = stream.Length;
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                while (bytesToRead > 0)
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        var buffer = new byte[10000];
                        var length = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        bytesToRead -= length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
            }
        }
    }
}