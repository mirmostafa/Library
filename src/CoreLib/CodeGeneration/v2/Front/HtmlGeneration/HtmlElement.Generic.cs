using System.ComponentModel;

namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public abstract class HtmlElement<TSelf> : IParentHtmlElement
{
    protected Dictionary<string, string?> AttributeList = new();

    protected HtmlElement(string name)
        => this.Name = name;

    public IEnumerable<(string Key, string? Value)> Attributes => this.AttributeList.ToEnumerable();

    public IList<IHtmlElementInfo> Children => new List<IHtmlElementInfo>();

    public virtual ClosingTagType ClosingTagType { get; set; } = ClosingTagType.Full;

    public string? InnerHtml { get; set; }

    public string Name { get; }

    public TSelf AddAttribute(string key, string? value = null)
    {
        this.AttributeList.Add(key, value);
        return this.This();
    }

    public TSelf AddChild(IHtmlElementInfo child)
    {
        this.Children.Add(child);
        return this.This();
    }

    public TSelf RemoveAttribute(string key)
    {
        _ = this.AttributeList.Remove(key);
        return this.This();
    }

    public TSelf RemoveChild(IHtmlElementInfo child)
    {
        _ = this.Children.Remove(child);
        return this.This();
    }

    public TSelf SetInnerHtml(string? value)
    {
        this.InnerHtml = value;
        return this.This();
    }

    protected virtual TSelf This()
        => this.Cast().To<TSelf>();
}