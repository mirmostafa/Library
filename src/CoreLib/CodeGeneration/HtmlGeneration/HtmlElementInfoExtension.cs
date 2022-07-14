using Library.Interfaces;

namespace Library.CodeGeneration.HtmlGeneration;

public static class HtmlElementInfoExtension
{
    public static string ToHtml(this IHtmlElementInfo element, string indent = " ")
    {
        var sb = new StringBuilder(indent).Append($"<{element.Name}");
        if (element is IAutoCoder auto)
        {
            return auto.GenerateCode();
        }
        foreach (var (key, value) in element.Attributes)
        {
            _ = sb.Append(value is not null ? $" {key}=\"{value}\"" : $" \"{key}\"");
        }
        _ = sb.Append('>');
        if (element is IHasChild<IHtmlElementInfo> parent)
        {
            foreach (var child in parent.Children)
            {
                _ = sb.AppendLine().Append(child.ToHtml(indent + indent));
            }
        }

        _ = sb.AppendLine().Append(indent).Append($"</{element.Name}>");
        return sb.ToString();
    }
}