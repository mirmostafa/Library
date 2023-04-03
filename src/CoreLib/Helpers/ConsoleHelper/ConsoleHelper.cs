using System.Collections;

namespace Library.Helpers.ConsoleHelper;

public static class ConsoleHelper
{
    public static T DumpLine<T>( T t)
    {
        if (t == null)
        {
            System.Console.WriteLine();
            return t;
        }
        if (t is IEnumerable items)
        {
            foreach (var item in items)
            {
                _ = DumpLine(item);
                System.Console.WriteLine();
            }
            return t;
        }

        var props = ObjectHelper.GetProperties(t);
        var maxLength = props.Select(x => x.Name.Length).Max() + 1;
        foreach (var (name, value) in props)
        {
            _ = WriteLine($"{StringHelper.FixSize(name, maxLength)}: {value}");
        }

        return t;
    }

    public static T WriteLine<T>(T t)
    {
        if (t is not string)
        {
            if (t is IEnumerable items)
            {
                foreach (var item in items)
                {
                    _ = WriteLine(item);
                }
                return t;
            }
        }

        System.Console.WriteLine(t);
        return t;
    }
}