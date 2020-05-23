#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System;

namespace Mohammad.Globalization.Helpers
{
    public static class Localization
    {
        public static ILocalizer Localizer                                    { get; set; }
        public static string     ToLocalString(this PersianDateTime dateTime) => Localizer.ToString(dateTime);
        public static string     ToLocalString(this DateTime        dateTime) => Localizer.ToString(dateTime);
    }
}