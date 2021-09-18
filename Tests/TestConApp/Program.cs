using System;
using Microsoft.Extensions.Logging;

namespace TestConApp;

internal partial class Program
{
    private static ILogger _logger = null!;

    public static void Main()
    {
        //Console.Out
    }

    //private static void ProgressBarTest()
    //{
    //    Console.Write("Doing... ");
    //    using (var progress = new ConsoleProgressBar())
    //    {
    //        const int max = 200;
    //        for (var i = 0; i <= max; i++)
    //        {
    //            progress.Report(i, max);
    //            Thread.Sleep(20);
    //        }
    //    }
    //    Console.WriteLine("Done.");
    //}

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