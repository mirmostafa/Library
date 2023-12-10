using Library.CodeGeneration.v2.Front.HtmlGeneration;
using Library.Interfaces;

namespace Library.CodeGeneration.v2.Front;

public static class HtmlElementInfoExtension
{
    public static string ToHtml(this IHtmlElementInfo element, string indent = " ")
    {
        if (element is ISelfCoder auto)
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
        else if (element is IHasChildren<IHtmlElementInfo> parent && parent.Children.Any())
        {
            _ = codeStatement.Append('>');
            foreach (var child in parent.Children)
            {
                _ = codeStatement.AppendLine().Append(child.ToHtml(string.Concat(indent, indent)));
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
                    _ = codeStatement.Append('>');
                    _ = codeStatement.AppendLine().Append(indent).Append($"</{element.Name}>");
                    break;
            }
        }
        return codeStatement.ToString();
    }
}