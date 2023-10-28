using Library.CodeGeneration;
using Library.Validations;

namespace Library.Helpers.CodeGen;

public static class TypeMemberNameHelper
{
    public static string FixVariableName(in string memberName)
    {
        //Create an array of illegal characters
        var illegalChars = new[] { "!", "#", "%", "^", "&", "*", "(", ")", "-", "+", "/", "\\", " " };

        //Replace all illegal characters with underscores
        var result = memberName.ArgumentNotNull().Trim().ReplaceAll(illegalChars, "_");

        //Detect language keywords
        if (LanguageKeywords.Keywords.Contains(result))
        {
            result = $"@{result}";
        }

        return result;
    }

    public static string ToArgName(in string name)
    {
        var buffer = Initialize(name);
        return $"{buffer[Range.EndAt(1)].ToLower()}{buffer[1..]}";
    }

    public static string ToFieldName(in string name)
    {
        var buffer = Initialize(name);
        return $"_{buffer[Range.EndAt(1)].ToLower()}{buffer[1..]}";
    }

    public static string ToPropName(in string name)
    {
        var buffer = Initialize(name);
        return $"{buffer[Range.EndAt(1)].ToUpper()}{buffer[1..]}";
    }

    private static string Initialize(in string name)
    {
        var result = name.ArgumentNotNull().Trim().TrimStart('_');
        // Support interfaces
        if (result.Length > 1 && result[0] is 'i' or 'I' && char.IsUpper(result[1]))
        {
            result = result[1..];
        }

        return FixVariableName(result);
    }
}