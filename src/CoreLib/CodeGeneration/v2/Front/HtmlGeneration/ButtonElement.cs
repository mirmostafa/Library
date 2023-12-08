namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public sealed class ButtonElement : HtmlElement<ButtonElement>
{
    public ButtonElement(string type = "button") : base("button")
        => this.AttributeList.Add("type", type);

    public static ButtonElement New(string caption, params (string Key, string? value)[] attributes)
    {
        var result = new ButtonElement();
        attributes.ForEach(x => result.AddAttribute(x.Key, x.value));
        if (caption is not null)
        {
            _ = result.SetInnerHtml(caption);
        }

        return result;
    }
}