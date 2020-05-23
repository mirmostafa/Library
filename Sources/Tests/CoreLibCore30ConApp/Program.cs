using System;
using System.Dynamic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace CoreLibCore30ConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter you name: ");
            string? name = Console.ReadLine();
            
            // Pattern matching
            if (name is {Length: var length})
            {
                Console.WriteLine(length);
            }

            bool? b = Get();
            if (b.HasValue && b.Value)
            {
            }
        }

        private static bool? Get() => null;
    }
}
