using Library.CodeGeneration.HtmlGeneration;

namespace TestConApp;

internal partial class Program
{
    private static void CreateHtmlTable()
    {
        var getEditButton = (in int id) => HtmlElement.New("button").AddAttribute("type", "button").SetInnerHtml("Edit");
        var getDeleteButton = (in int id) => HtmlElement.New("button").AddAttribute("type", "button").SetInnerHtml("Delete");
        var data = new DataSchema[]
        {
            new(1, "Ali", 5, getEditButton(1), getDeleteButton(1)), new(2, "Reza", 15, getEditButton(2), getDeleteButton(2)), new(3, "Mohammad", 25, getEditButton(3), getDeleteButton(3))
        };
        var table = TableElement.CreateHtmlTable(new TableHeader[]
        {
            new(nameof(DataSchema.Name), "nam"), new(nameof(DataSchema.Age), "sen"), new(nameof(DataSchema.Actions), "Actions", false)
        }, data, 1, "100%");
        WriteLine(table);
    }

    private static void CreateRazorTable()
    {
        var getEditButton = () => HtmlElement.New("button").AddAttribute("type", "button").AddAttribute("@onclick", "EditPersonButtonClick(this.Item.Id)").SetInnerHtml("Edit");
        var getDeleteButton = () => HtmlElement.New("button").AddAttribute("type", "button").AddAttribute("@onclick", "DeletePersonButtonClick(this.Item.Id)").SetInnerHtml("Delete");
        var table = TableElement.CreateRazorTable(new TableHeader[]
                {
                    new(nameof(DataSchema.Name), "nam"), new(nameof(DataSchema.Age), "sen"), new(nameof(DataSchema.Actions), "Actions", false)
                }, "this.People", 1, "100%", ("Actions", new[] { getEditButton(), getDeleteButton() }));
        WriteLine(table);
    }

    private static void Main()
    {
        CreateRazorTable();
    }

    public record DataSchema(in long id, in string Name, in int Age, params IHtmlElementInfo[] Actions);
}