using System.Diagnostics.CodeAnalysis;
using Library.Dynamic;

namespace Library.Helpers;

public static class Deconstructs
{
    //public static void Deconstruct<TKey, TValue>([DisallowNull] this Dictionary<TKey, TValue> dic, out IEnumerable<(TKey Key, TValue Value)> items)
    //    where TKey : notnull => items = dic.ArgumentNotNull(nameof(dic)).Select(kv => (kv.Key, kv.Value));
}

public static class ExpandoHelper
{
    public static object? GetByPropName([DisallowNull] this Expando expando, [DisallowNull] string propName)
        => expando.ArgumentNotNull(nameof(expando))[propName.ArgumentNotNull(nameof(propName))];

    public static object? GetByPropName([DisallowNull] dynamic expando, [DisallowNull] string propName)
        => Check.Is<Expando>(expando, nameof(expando))[propName.ArgumentNotNull(nameof(propName))];
}