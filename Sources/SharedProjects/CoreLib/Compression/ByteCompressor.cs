using System.IO;
using System.IO.Compression;

namespace Mohammad.Compression
{
    public static class ByteCompressor
    {
        public static byte[] Compress(byte[] bytes)
        {
            if (bytes == null)
                return null;
            using (var result = new MemoryStream())
            using (var gZip = new GZipStream(result, CompressionMode.Compress))
            {
                gZip.Write(bytes, 0, bytes.Length);
                gZip.Close();
                return result.ToArray();
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            if (bytes == null)
                return null;
            using (var result = new MemoryStream())
            using (var gZip = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            {
                var buffer = new byte[1024];
                var count = 1024;
                while (count == 1024)
                {
                    count = gZip.Read(buffer, 0, 1024);
                    result.Write(buffer, 0, count);
                }
                return result.ToArray();
            }
        }
    }
}