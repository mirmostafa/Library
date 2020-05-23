using System;

namespace TestConsole45
{
    partial class App
    {
        protected override void Execute()
        {
            Console.WriteLine(1);
#line hidden
            Console.WriteLine(2);
#line default
            Console.WriteLine(3);
        }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}