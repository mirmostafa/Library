using System;
using System.Collections.Generic;
using Library.Helpers;

namespace TestConApp;

internal partial class Program
{
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
