namespace Library.Helpers
{
    public static class NumberHelper
    {
        public static string ToString(this int? number, string format, int defaultValue = 0)
            => (number ?? defaultValue).ToString(format);
    }
}