using System;
using System.Globalization;

namespace Mohammad.Helpers
{
    /// <summary>
    ///     A utility to do some common tasks about method
    ///     arguments
    /// </summary>
    public static class ArgHelper
    {
        public static void AssertNotNull<T>(T arg, string argName)
            where T : class
        {
            switch (arg)
            {
                case null:
                    throw new ArgumentNullException(argName);
                case string _ when arg.ToString().IsNullOrEmpty():
                    throw new ArgumentNullException(argName);
            }
        }

        internal static void AssertBiggerThan(int arg, int min, string argName)
        {
            if (arg <= min)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(={1}) cannot be lass than {2}", argName, arg, min));
            }
        }
    }
}