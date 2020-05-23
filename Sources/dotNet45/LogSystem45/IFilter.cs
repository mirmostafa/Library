using System.IO;

namespace Mohammad.Logging
{
    public interface IFilter
    {
        TextWriter Out { get; set; }
    }
}