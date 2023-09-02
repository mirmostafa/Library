using Library.Exceptions.Validations;
using Library.Helpers.Models;
using Library.Validations;

namespace Library.Helpers.CodeGen;

public static class TypeMemberNameHelper
{
    public static string FixVariableName(in string? memberName, bool checkNullability = true)
    {
        //Check if the memberName is null
        if (memberName is null)
        {
            //If checkNullability is true, throw a ValidationException
            //Otherwise, return an empty string
            return checkNullability ? throw new ValidationException("Cannot be empty") : string.Empty;
        }

        //Trim the memberName and replace spaces with underscores
        var result = memberName.Trim().Replace(" ", "_");

        //Create an array of illegal characters
        var illegalChars = new[] { "$", "!", "#", "@", "%", "^", "&", "*", "(", ")", "-", "+", "/", "\\", " " };

        //Replace all illegal characters with underscores
        return result.ReplaceAll(illegalChars, "_");
    }

    /// <summary>
    /// Combines the given nameSpace and name with a dot.
    /// </summary>
    /// <param name="nameSpace">The namespace.</param>
    /// <param name="name">The name.</param>
    /// <returns>The combined name.</returns>
    public static string GetFullName(string? nameSpace, string? name)
        => CombineWithDot(nameSpace, name);

    /// <summary>
    /// Gets the name of the type from the full name.
    /// </summary>
    /// <param name="fullName">The full name of the type.</param>
    /// <returns>The name of the type.</returns>
    public static string GetName(in string? fullName)
    {
        if (fullName.IsNullOrEmpty())
        {
            return string.Empty;
        }

        string result;
        if (!fullName.Contains('.'))
        {
            result = fullName;
        }
        else if (!fullName.Contains('<'))
        {
            result = fullName.Split('.').Last();
        }
        else
        {
            var generics = fullName.Split('<');
            result = null!;
            foreach (var generic in generics)
            {
                if (result.IsNullOrEmpty())
                {
                    result = generic.Split(".").Last();
                }
                else
                {
                    result += $"<{generic.Split(".").Last()}";
                }
            }
        }
        return result;
    }

    public static string GetNameSpace(in string? fullName)
    {
        var m = (fullName?.Contains('[') ?? false) ? fullName[..fullName.IndexOf("[")] : fullName;
        return (m?.Contains('.') ?? false) ? m[..m.LastIndexOf(".")] : string.Empty;
    }

    public static IEnumerable<string> GetNameSpaces(string fullName)
    {
        //var (classFullData, genericParamsFullData) = GetFullData(fullName);
        //var result = EnumerableHelper.ToEnumerable(classFullData.NameSpace)
        //    .AddRangeImmuted(genericParamsFullData.Select(gpf => gpf.NameSpace)).Compact();
        //return result;
        var items = fullName.Remove(">").Split('<');
        foreach (var item in items)
        {
            if (item.Contains('.'))
            {
                yield return item[..item.LastIndexOf('.')];
            }
        }
    }

    public static string ToArgName(in string name)
    {
        var buffer = name.ArgumentNotNull(nameof(name)).Trim().TrimStart('_');
        return $"{buffer[Range.EndAt(1)].ToLower()}{buffer[1..]}";
    }

    public static string ToFieldName(in string name)
    {
        var buffer = name.ArgumentNotNull(nameof(name)).Trim().TrimStart('_').TrimStart('I');
        return $"_{buffer[Range.EndAt(1)].ToLower()}{buffer[1..]}";
    }

    public static string ToPropName(in string name)
    {
        var propName = name.ArgumentNotNull(nameof(name)).StartsWith("_") ? name[1..] : name;
        return $"{propName[Range.EndAt(1)].ToUpper()}{propName[1..]}";
    }

    public static string ValidateName(in string? memberName, bool checkNullability = true)
    {
        (bool IsValid, string? ErrorContent) result;
        if (memberName is null)
        {
            if (checkNullability)
            {
                result = new(false, "Cannot be empty");
            }
            else
            {
                return string.Empty;
            }
        }
        else
        {
            result = memberName.StartsWithAny(Enumerable.Range(0, 9).Select(x => x.ToString()))
                ? new(false, "Illegal character.")
                : memberName.Contains(' ')
                            ? new(false, "Illegal character.")
                            : memberName.StartsWithAny("$", "!", "#", "@", "%", "^", "&", "*", "(", ")", "-", "+", "/", "\\")
                                        ? new(false, "Illegal character.")
                                        : ((bool IsValid, string? ErrorContent))(true, null);
        }
        return result.IsValid ? memberName! : throw new ValidationException(result.ErrorContent!);
    }

    private static string CombineWithDot(params string?[] parts)
    {
        var names = parts.Select(part => FixVariableName(part, false)).Compact();
        var builder = new StringBuilder();
        foreach (var name in names)
        {
            _ = builder.Append($"{name}.");
        }
        var result = builder.ToString();
        if (result?.Length > 0)
        {
            result = result[0..^1];
        }

        return result ?? string.Empty;
    }

    private static (MemberNameInfo ClassFullData, IEnumerable<MemberNameInfo> GenericParamsFullData) GetFullData(in string fullName)
    {
        var buffer = fullName.ArgumentNotNull(nameof(fullName));
        var genericParamsFullData = new List<MemberNameInfo>();
        if (buffer.Contains('<'))
        {
            var genericParamString = buffer.GetPhrase(0, '<', '>')!;
            var genericParams = genericParamString.Split(",");
            genericParamsFullData.AddRange(genericParams.Select(gp => new MemberNameInfo(GetNameSpace(gp), getName(gp), gp.EndsWith("?"))));
            buffer = buffer.Remove(genericParamString).Remove("<").Remove(">")!;
        }
        var classFullData = new MemberNameInfo(GetNameSpace(buffer), getName(buffer), buffer.EndsWith("?"));

        return (classFullData, genericParamsFullData);

        static string getName(in string s)
        {
            var m = s.Contains('[') ? s[..s.IndexOf("[")] : s;
            return (m.Contains('.') ? s[(m.LastIndexOf(".") + 1)..] : s).Remove("?")!;
        }
    }
}