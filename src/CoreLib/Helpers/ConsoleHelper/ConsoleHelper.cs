using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helpers.ConsoleHelper;
public static class ConsoleHelper
{
    public static T WriteLine<T>(this T t)
    {
        System.Console.WriteLine(t);
        return t;
    }
}
