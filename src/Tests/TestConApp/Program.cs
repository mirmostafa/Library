using Library.CodeGeneration.HtmlGeneration;

namespace TestConApp;

internal partial class Program
{
    private static void Main()
    {
        var getEditButton = (int id) => HtmlElement.New("button").AddAttribute("type", "button").SetInnerHtml("Edit");
        var getDeleteButton = (int id) => HtmlElement.New("button").AddAttribute("type", "button").SetInnerHtml("Delete");
        var data = new Schema[]
        {
            new(1, "Ali", 5, getEditButton(1), getDeleteButton(1)), new(2, "Reza", 15, getEditButton(2), getDeleteButton(2)), new(3, "Mohammad", 25, getEditButton(3), getDeleteButton(3))
        };
        var table = TableElement.CreateTable(new TableHeader[]
        {
            new(nameof(Schema.Name), "nam"), new(nameof(Schema.Age), "sen"), new(nameof(Schema.Actions), "Actions")
        }, data, 1, "100%");
        WriteLine(table);
    }

    public record Schema(long id, string Name, int Age, params IHtmlElementInfo[] Actions);
}