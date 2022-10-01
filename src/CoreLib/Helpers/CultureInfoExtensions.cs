using System.Globalization;

namespace Library.Helpers;

public static class CultureInfoHelper
{
    /// <summary>
    /// The weekday/weekend state for a given day.
    /// </summary>
    public enum WeekdayState
    {
        /// <summary>
        /// A work day.
        /// </summary>
        Workday,

        /// <summary>
        /// A weekend.
        /// </summary>
        Weekend,

        /// <summary>
        /// Morning is a workday, afternoon is the start of the weekend.
        /// </summary>
        WorkdayMorning
    }

    /// <summary>
    /// Returns the English version of the country name. Extracted from the CultureInfo.EnglishName.
    /// </summary>
    /// <param name="ci">The CultureInfo this object.</param>
    /// <returns>The English version of the country name.</returns>
    public static string GetCountryEnglishName(this CultureInfo ci)
    {
        var parts = ci.EnglishName.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            return ci.EnglishName;
        }

        parts = parts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        return parts[^1].Trim();
    }

    /// <summary>
    /// Returns the English version of the language name. Extracted from the CultureInfo.EnglishName.
    /// </summary>
    /// <param name="ci">The CultureInfo this object.</param>
    /// <returns>The English version of the language name.</returns>
    public static string GetLanguageEnglishName(this CultureInfo ci)
        => ci.EnglishName.Split(new[] { '(' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

    /// <summary>
    /// Return if the passed in day of the week is a weekend.
    ///
    /// note: state pulled from http://en.wikipedia.org/wiki/Workweek_and_weekend
    /// </summary>
    /// <param name="ci">The CultureInfo this object.</param>
    /// <param name="day">The Day of the week to return the stat of.</param>
    /// <returns>The weekday/weekend state of the passed in day of the week.</returns>
    public static WeekdayState GetWeekdayState(this CultureInfo ci, DayOfWeek day)
        => GetCountryAbbreviation(ci) switch
        {
            "DZ" // Algeria
         or "BH" // Bahrain
         or "BD" // Bangladesh
         or "EG" // Egypt
         or "IQ" // Iraq
         or "IL" // Israel
         or "JO" // Jordan
         or "KW" // Kuwait
         or "LY" // Libya
                 // Northern Malaysia (only in the states of Kelantan, Terengganu, and Kedah)
         or "MV" // Maldives
         or "MR" // Mauritania
         or "NP" // Nepal
         or "OM" // Oman
         or "QA" // Qatar
         or "SA" // Saudi Arabia
         or "SD" // Sudan
         or "SY" // Syria
         or "AE" // U.A.E.
         or "YE" // Yemen
              => day is DayOfWeek.Thursday or DayOfWeek.Friday
                     ? WeekdayState.Weekend
                     : WeekdayState.Workday,
            "AF" // Afghanistan
         or "IR" when day == DayOfWeek.Thursday // Iran
              => WeekdayState.WorkdayMorning,
            "IR" // Iran
              => day == DayOfWeek.Friday ? WeekdayState.Weekend : WeekdayState.Workday,
            "BN" // Brunei Darussalam
              => day is DayOfWeek.Friday or DayOfWeek.Sunday
                        ? WeekdayState.Weekend
                        : WeekdayState.Workday,
            "MX" // Mexico
         or "TH" when day == DayOfWeek.Saturday // Thailand
              => WeekdayState.WorkdayMorning,
            "TH" // Thailand
              => day is DayOfWeek.Saturday or DayOfWeek.Sunday
                        ? WeekdayState.Weekend
                        : WeekdayState.Workday,
            _ => day is DayOfWeek.Saturday or DayOfWeek.Sunday ? WeekdayState.Weekend : WeekdayState.Workday
        };

    private static string GetCountryAbbreviation(CultureInfo ci)
        => ci.Name.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[^1];
}