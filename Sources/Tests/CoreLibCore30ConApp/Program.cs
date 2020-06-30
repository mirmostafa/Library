using System;
using Mohammad.Globalization;
using Mohammad.Helpers;

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

            var (year, month, day, hour, minute, second, millisecond) = PersianDateTime.Now;
        }
    }
}