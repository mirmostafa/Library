


using System;

namespace Mohammad.Globalization.Helpers
{
    public static class Localization
    {
        private static ILocalizer _localizer;

        public static ILocalizer Localizer
        {
            get => _localizer ?? throw new NotImplementedException();
            set => _localizer = value;
        }

        public static string ToLocalString(this PersianDateTime dateTime) => Localizer.ToString(dateTime);
        public static string ToLocalString(this DateTime dateTime) => Localizer.ToString(dateTime);
    }
}