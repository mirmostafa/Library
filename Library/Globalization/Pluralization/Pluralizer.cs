using System.Globalization;
using System.Text.RegularExpressions;
using Library.Globalization.Pluralization.Rules;

namespace Library.Globalization.Pluralization;
public static class Pluralizer
{
    private static readonly Dictionary<Regex, string> _PluralRules = PluralRules.GetRules();
    private static readonly Dictionary<Regex, string> _SingularRules = SingularRules.GetRules();
    private static readonly List<string> _Uncountables = Uncountables.GetUncountables();
    private static readonly Dictionary<string, string> _IrregularPlurals = IrregularRules.GetIrregularPlurals();
    private static readonly Dictionary<string, string> _IrregularSingles = IrregularRules.GetIrregularSingulars();
    private static readonly Regex _ReplacementRegex = new("\\$(\\d{1,2})");

    public static string Pluralize(string word) => Transform(word, _IrregularSingles, _IrregularPlurals, _PluralRules);

    public static string Singularize(string word) => Transform(word, _IrregularPlurals, _IrregularSingles, _SingularRules);

    internal static string RestoreCase(string originalWord, string newWord)
    {
        // Tokens are an exact match.
        if (originalWord == newWord)
        {
            return newWord;
        }

        // Upper cased words. E.g. "HELLO".
        if (originalWord == originalWord.ToUpper(CultureInfo.InvariantCulture))
        {
            return newWord.ToUpper(CultureInfo.InvariantCulture);
        }

        // Title cased words. E.g. "Title".
        if (originalWord[0] == char.ToUpper(originalWord[0], CultureInfo.InvariantCulture))
        {
            return char.ToUpper(newWord[0], CultureInfo.InvariantCulture) + newWord[1..];
        }

        // Lower cased words. E.g. "test".
        return newWord.ToLower(CultureInfo.InvariantCulture);
    }

    internal static string ApplyRules(string token, string originalWord, Dictionary<Regex, string> rules)
    {
        // Empty string or doesn't need fixing.
        if (string.IsNullOrEmpty(token) || _Uncountables.Contains(token))
        {
            return RestoreCase(originalWord, token);
        }

        var length = rules.Count;

        // Iterate over the sanitization rules and use the first one to match.
        while (length-- > 0)
        {
            var rule = rules.ElementAt(length);

            // If the rule passes, return the replacement.
            if (rule.Key.IsMatch(originalWord))
            {
                var match = rule.Key.Match(originalWord);
                var matchString = match.Groups[0].Value;
                return string.IsNullOrWhiteSpace(matchString)
                    ? rule.Key.Replace(originalWord, GetReplaceMethod(originalWord[match.Index - 1].ToString(), rule.Value), 1)
                    : rule.Key.Replace(originalWord, GetReplaceMethod(matchString, rule.Value), 1);
            }
        }

        return originalWord;
    }

    private static MatchEvaluator GetReplaceMethod(string originalWord, string replacement) => match => RestoreCase(originalWord, _ReplacementRegex.Replace(replacement, m => match.Groups[Convert.ToInt32(m.Groups[1].Value, CultureInfo.InvariantCulture)].Value));

    private static string Transform(string word, Dictionary<string, string> replacables, Dictionary<string, string> keepables, Dictionary<Regex, string> rules)
    {
        var token = word.ArgumentNotNull().ToLower(CultureInfo.InvariantCulture);
        return keepables.ContainsKey(token)
            ? RestoreCase(word, token)
            : replacables.ContainsKey(token) ? RestoreCase(word, replacables[token]) : ApplyRules(token, word, rules);
    }
}
