namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public class ButtonElement : HtmlElement<ButtonElement>
{
    public ButtonElement(string type = "button") : base("button")
        => this.AttributeList.Add("type", type);

    public static ButtonElement New(string caption, params (string Key, string? value)[] attributes)
    {
        var result = new ButtonElement();
        _ = attributes.ForEachEager(x => result.AddAttribute(x.Key, x.value));
        if (caption is not null)
        {
            _ = result.SetInnerHtml(caption);
        }

        return result;
    }

    public static ButtonElement GetBlazorClickCodeStatement(string buttonName, string caption, string? handlerParameter)
    {
        var result = New(caption, ("name", buttonName), ("@onclick", $"{buttonName}_OnClick({handlerParameter})"));
        return result;
    }
}