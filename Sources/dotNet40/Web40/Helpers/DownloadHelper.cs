#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Web;

namespace Library40.Web.Helpers
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
						var buffer = new Byte[10000];
						var length = stream.Read(buffer, 0, 10000);
						HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
						HttpContext.Current.Response.Flush();
						bytesToRead = bytesToRead - length;
					}
					else
						bytesToRead = -1;
			}
		}
	}
}