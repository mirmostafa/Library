#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;
using Mohammad.Win32.Natives.IfacesEnumsStructsClasses;

namespace Mohammad.Helpers
{
    public static partial class DateTimeHelper
    {
        public static DateTime ToDateTime(this FILETIME filetime) => DateTime.FromFileTime(filetime.ToLong());
        public static long ToLong(this FILETIME filetime) => ((long)filetime.dwHighDateTime << 32) + filetime.dwLowDateTime;
    }
}