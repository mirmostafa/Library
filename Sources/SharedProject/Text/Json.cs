using System.Linq;
using System.Text;
using Mohammad.Helpers;

namespace Mohammad.Text
{
    public static class Json
    {
        public static string Reformat(string jsonText)
        {
            const string indentString = "    ";
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < jsonText.Length; i++)
            {
                var ch = jsonText[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        _ = sb.Append(ch);
                        if (!quoted)
                        {
                            _ = sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(indentString));
                        }

                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            _ = sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(indentString));
                        }

                        _ = sb.Append(ch);
                        break;
                    case '"':
                        _ = sb.Append(ch);
                        var escaped = false;
                        var index = i;
                        while (index > 0 && jsonText[--index] == '\\')
                        {
                            escaped = !escaped;
                        }

                        if (!escaped)
                        {
                            quoted = !quoted;
                        }

                        break;
                    case ',':
                        _ = sb.Append(ch);
                        if (!quoted)
                        {
                            _ = sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(indentString));
                        }

                        break;
                    case ':':
                        _ = sb.Append(ch);
                        if (!quoted)
                        {
                            _ = sb.Append(" ");
                        }

                        break;
                    default:
                        _ = sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}