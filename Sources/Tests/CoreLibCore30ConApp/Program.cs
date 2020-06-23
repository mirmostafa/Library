using System;
using Mohammad.Globalization;

namespace CoreLibCore30ConApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter you name: ");
            var name = Console.ReadLine();

            if (name is { Length: var length })
            {
                Console.WriteLine(length);
            }

            var a = PersianDateTime.Now;
            Console.WriteLine(a);
        }

        private static bool? Get() => null;
    }
}