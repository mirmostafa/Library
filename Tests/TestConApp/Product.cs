//using Library.SourceGenerator.Contracts;

//namespace TestConApp;

//[GenerateDto]
//public class Product
//{
//    public long Id { get; set; }
//    public string Name { get; set; }
//    public string Description { get; set; }
//}

//public partial class AutoNotifyTestModel
//{
//    [AutoNotify]
//    private string _Text = "private field text";

//    [AutoNotify(PropertyName = "Count")]
//    private int _Amount = 5;
//}

//partial class FluentCodeTest
//{
//    //[FluentProp]
//    public string? Name { get; set; }
    
//    //[FluentProp]
//    public int Age { get; set; }
    
//    public string? Address { get; set; }
//}