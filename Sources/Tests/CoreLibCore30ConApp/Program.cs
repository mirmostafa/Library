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

            var a = PersianDateTime.Now;
            var p4 = ImmutableTypeInitializer<Person>.New().CtorParam("Name", "Ali").CtorParam("Age", 5).Build();
        }
    }
}