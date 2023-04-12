using Library.Helpers.Models;

var progressBar = new ConsoleProgressBar(100);
var reporter = new Progress<int>(x => progressBar.Report(x));//.Create(x => progressBar.Report(x));
Run(reporter);

WriteLine("Done.");

static void Run(IProgress<int> reporter)
{
    for (var i = 0; i < 100; i++)
    {
        reporter.Report(i);
        Thread.Sleep(100);
    }
}