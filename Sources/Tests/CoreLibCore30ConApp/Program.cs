using System;

namespace CoreLibCore30ConApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Enter you name: ");
            var name = Console.ReadLine();

            // Pattern matching
            if (name is {Length: var length})
            {
                Console.WriteLine(length);
            }

            var b = Get();
            if (b.HasValue && b.Value)
            {
            }
        }

        private static bool? Get() => null;
    }
}