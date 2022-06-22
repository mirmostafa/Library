﻿using System.Globalization;
using Library.Exceptions.Validations;
using Library.Validations;

namespace Library.Helpers.CodeGen;

public static class TypeMemberNameHelper
{
    public static string CombineWithDot(params string?[] parts)
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

    public static string FixVariableName(in string? memberName, bool checkNullability = true)
    {
        if (memberName is null)
        {
            return checkNullability ? throw new ValidationException("Cannot be empty") : string.Empty;
        }
        var result = memberName.Trim().Replace(" ", "_");
        var illegarChars = new[] { "$", "!", "#", "@", "%", "^", "&", "*", "(", ")", "-", "+", "/", "\\", " " };
        return result.ReplaceAll(illegarChars, "_");
    }

    public static string GetFullName(string? nameSpace, string? name)
        => CombineWithDot(nameSpace, name);

    public static string GetName(in string? fullName)
    {
        if (fullName.IsNullOrEmpty())
        {
            return string.Empty;
        }

        var (classFullData, genericParamsFullData) = GetFullData(fullName);

        var result = genericParamsFullData.Any()
            ? $"{classFullData.Name}<{genericParamsFullData.Select(gpf => gpf.IsNullable ? $"{gpf.Name}?" : gpf.Name).Merge(", ")}"
            : classFullData.Name;
#warning موقت
        if (result.Any(c => c == '<'))
        {
            result = $"{result}>";
        }
        if (classFullData.IsNullable)
        {
            result = $"{result}?";
        }
        //result = result.Remove("?");
        return result!;
    }

    public static string GetNameSpace(in string? fullName)
    {
        var m = (fullName?.Contains('[') ?? false) ? fullName[..fullName.IndexOf("[")] : fullName;
        return (m?.Contains('.') ?? false) ? m[..m.LastIndexOf(".")] : string.Empty;
    }

    public static IEnumerable<string> GetNameSpaces(in string fullName)
    {
        var (classFullData, genericParamsFullData) = GetFullData(fullName);
        var result = EnumerableHelper
            .AsEnumerableItem(classFullData.NameSpace)
            .AddRangeImmuted(genericParamsFullData.Select(gpf => gpf.NameSpace)).Compact();
        return result;
    }

    public static string ToArgName(in string name)
    {
        var buffer = name.ArgumentNotNull(nameof(name)).Trim().TrimStart('_');
        return $"{buffer[Range.EndAt(1)].ToLower(CultureInfo.InvariantCulture)}{buffer[1..]}";
    }

    public static string ToFieldName(in string name)
    {
        var buffer = name.ArgumentNotNull(nameof(name)).Trim().TrimStart('_').TrimStart('I');
        return $"_{buffer[Range.EndAt(1)].ToLower(CultureInfo.InvariantCulture)}{buffer[1..]}";
    }

    public static string ToPropName(in string name)
    {
        var propName = name.ArgumentNotNull(nameof(name)).StartsWith("_") ? name[1..] : name;
        return $"{propName[Range.EndAt(1)].ToUpper(CultureInfo.InvariantCulture)}{propName[1..]}";
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
        else if (memberName.StartsWithAny(Enumerable.Range(0, 9).Select(x => x.ToString(CultureInfo.InvariantCulture))))
        {
            result = new(false, "Illegal character.");
        }
        else if (memberName.Contains(' '))
        {
            result = new(false, "Illegal character.");
        }
        else if (memberName.StartsWithAny("$", "!", "#", "@", "%", "^", "&", "*", "(", ")", "-", "+", "/", "\\"))
        {
            result = new(false, "Illegal character.");
        }
        else
        {
            result = (true, null);
        }
        return result.IsValid ? memberName! : throw new ValidationException(result.ErrorContent!);
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

public record MemberNameInfo(string NameSpace, string Name, bool IsNullable);