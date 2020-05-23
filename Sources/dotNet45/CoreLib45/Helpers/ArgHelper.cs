#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

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
        internal static void AssertBiggerThan(int arg, int min, string argName)
        {
            if (arg <= min)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The argument {0}(={1}) cannot be lass than {2}", argName, arg, min));
        }

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
    }
}