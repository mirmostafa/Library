namespace TestConApp;

internal class Program
{
    public static void Main()
    {
        var dto = new Models.ProductDto { };

        var vm = new AutoNotifyTestModel();
        var text = vm.Text;
        Console.WriteLine($"Text = {text}");
        var count = vm.Count;
        Console.WriteLine($"Count = {count}");
        vm.PropertyChanged += (o, e) => Console.WriteLine($"Property {e.PropertyName} was changed");
        vm.Text = "abc";
        vm.Count = 123;
    }
}