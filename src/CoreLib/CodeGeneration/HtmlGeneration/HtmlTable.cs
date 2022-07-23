using System.Collections;

using Library.Validations;

namespace Library.CodeGeneration.HtmlGeneration;

public class TableElement : HtmlElement<TableElement>, IAutoCoder
{
    public TableElement() : base("table")
    {
    }

    public int? BorderSize { get; set; }
    public IEnumerable? Data { get; set; }
    public List<TableHeader> Headers { get; } = new();
    public string? Width { get; set; }

    public static string CreateHtmlTable(in IEnumerable<TableHeader> columns
        , in IEnumerable data
        , in int? border = null
        , in string? width = null
        , in Func<IHtmlElementInfo, string>? toHtml = null)
    {
        var table = HtmlElement.New("table");
        var thead = HtmlElement.New("thead");

        var tr = HtmlElement.New("tr");
        foreach (var (_, caption, _) in columns)
        {
            var th = HtmlElement.New("th").SetInnerHtml(caption);
            _ = tr.AddChild(th);
        }
        _ = thead.AddChild(tr);
        _ = table.AddChild(thead);
        var tbody = HtmlElement.New("tbody");
        foreach (var item in data)
        {
            tr = HtmlElement.New("tr");
            foreach (var (PropName, _, _) in columns)
            {
                var value = item.GetType().GetProperty(PropName)?.GetValue(item);
                var td = value switch
                {
                    IHtmlElementInfo element => HtmlElement.New("td").SetInnerHtml(element.ToHtml()),
                    IEnumerable<IHtmlElementInfo> elements => HtmlElement.New("td").SetInnerHtml(elements.Select(x => x.ToHtml()).ConcatStrings()),
                    _ => HtmlElement.New("td").SetInnerHtml(value?.ToString())
                };
                _ = tr.AddChild(td);
            }
            _ = tbody.AddChild(tr);
        }
        _ = table.AddChild(tbody);

        if (border is not null)
        {
            _ = table.AddAttribute("border", border.ToString());
        }
        if (width is not null)
        {
            _ = table.AddAttribute("width", width);
        }

        return toHtml is null ? table.ToHtml() : toHtml(table);
    }

    public static string CreateRazorTable(in IEnumerable<TableHeader> columns
        , in string itemsSourceBindingPath
        , in int? border = null
        , in string? width = null
        , in Func<string>? getIteratorHtml = null
        , in Func<IHtmlElementInfo, string>? toHtml = null)
    {
        var table = HtmlElement.New("table");
        var thead = HtmlElement.New("thead");

        var tr = HtmlElement.New("tr");
        foreach (var (_, caption, _) in columns)
        {
            var th = HtmlElement.New("th").SetInnerHtml(caption);
            _ = tr.AddChild(th);
        }
        _ = thead.AddChild(tr);
        _ = table.AddChild(thead);
        var tbody = HtmlElement.New("tbody");
        var iterator = getIteratorHtml is not null ? getIteratorHtml() : innerGetIteratorHtml(columns, itemsSourceBindingPath);
        tbody.InnerHtml = iterator;
        _ = table.AddChild(tbody);

        if (border is not null)
        {
            _ = table.AddAttribute("border", border.ToString());
        }
        if (width is not null)
        {
            _ = table.AddAttribute("width", width);
        }

        return toHtml is null ? table.ToHtml() : toHtml(table);

        static string innerGetIteratorHtml(IEnumerable<TableHeader> columns, string itemsSourceBindingPath)
        {
            var trs = new StringBuilder();
            foreach (var column in columns)
            {
                if (column.ShowInIteration)
                {
                    _ = trs.AppendLine($"             <td>@item.{column.BindingPath}</td>");
                }
            }
            var iterator =
                    $@"   @foreach (var item in {itemsSourceBindingPath})
      {{
         <tr>
{trs.ToString().TrimEnd()}
         </tr>
      }}";
            return iterator;
        }
    }

    public string GenerateCodeStatement()
        => CreateHtmlTable(this.Headers, this.Data.NotNull(), this.BorderSize, this.Width);
}

public record TableHeader(string BindingPath, string Caption, bool ShowInIteration = true);