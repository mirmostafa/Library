using Library.Interfaces;

namespace Library.CodeGeneration.HtmlGeneration;

public static class HtmlElementInfoExtension
{
    public static string ToHtml(this IHtmlElementInfo element, string indent = " ")
    {
        if (element is IAutoCoder auto)
        {
            return auto.GenerateCodeStatement();
        }

        var codeStatement = new StringBuilder(indent).Append($"<{element.Name}");
        foreach (var (key, value) in element.Attributes)
        {
            _ = codeStatement.Append(value is not null ? $" {key}=\"{value}\"" : $" \"{key}\"");
        }

        if (element.InnerHtml is not null)
        {
            _ = codeStatement.Append('>');
            _ = codeStatement.AppendLine().Append(indent).Append(element.InnerHtml);
            _ = codeStatement.AppendLine().Append(indent).Append($"</{element.Name}>");
        }
        else if (element is IHasChild<IHtmlElementInfo> parent && parent.Children.Any())
        {
            _ = codeStatement.Append('>');
            foreach (var child in parent.Children)
            {
                _ = codeStatement.AppendLine().Append(child.ToHtml(indent + indent));
            }
            _ = codeStatement.AppendLine().Append(indent).Append($"</{element.Name}>");
        }
        else
        {
            switch (element.ClosingTagType)
            {
                case ClosingTagType.None:
                    _ = codeStatement.Append('>');
                    break;

                case ClosingTagType.Slash:
                    _ = codeStatement.Append("/>");
                    break;

                case ClosingTagType.Full:
                    _ = codeStatement.Append(">");
                    _ = codeStatement.AppendLine().Append(indent).Append($"</{element.Name}>");
                    break;
            }
        }
        return codeStatement.ToString();
    }
}