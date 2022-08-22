
using Library.Helpers;
using Library.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        
    }
}

namespace Models
{
    internal class Person
    {
        public required string? FirstLast { get; init; }
        public required string? FirstName { get; init; }
    }
}