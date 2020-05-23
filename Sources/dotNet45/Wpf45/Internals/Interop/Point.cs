using System.Runtime.InteropServices;

namespace Mohammad.Wpf.Internals.Interop
{
    /// <summary>
    ///     Win API struct providing coordinates for a single point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }
}