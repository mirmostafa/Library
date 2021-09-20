using System;
using System.Collections.Generic;
using Library.Helpers;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal partial class Program
{
    private static ILogger _logger = null!;

    public static void Main()
    {
        var ali = new Person("Ali", 5);
        var reza = ali with { Name = "Reza" };

        var ppl = new List<Person>() { ali };

        //var people = ppl.AsReadOnly();
        var people = ppl.ToReadOnlyList();
        Display(people);
        Console.WriteLine("===");

        ppl.Add(reza);
        Display(people);
    }

    private static void Display(IEnumerable<Person> people)
    {
        foreach (var person in people)
        {
            Console.WriteLine(person.Name);
        }
    }

    //private static void SourceGeneratorTest()
    //{
    //    var dto = new Models.ProductDto { };

    //    var vm = new AutoNotifyTestModel();
    //    var text = vm.Text;
    //    _logger.LogInformation($"Text = {text}");
    //    var count = vm.Count;
    //    _logger.LogInformation($"Count = {count}");
    //    vm.PropertyChanged += (o, e) => _logger.LogInformation($"Property {e.PropertyName} was changed");
    //    vm.Text = "abc";
    //    vm.Count = 123;
    //}
}