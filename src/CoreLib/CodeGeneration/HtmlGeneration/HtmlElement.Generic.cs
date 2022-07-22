using System.ComponentModel;

namespace Library.CodeGeneration.HtmlGeneration;

[EditorBrowsable(EditorBrowsableState.Advanced)]
public abstract class HtmlElement<TSelf> : IParentHtmlElement
{
    protected Dictionary<string, string?> AttributeList = new();
    protected List<IHtmlElementInfo> ChildList = new();

    protected HtmlElement(string name)
        => this.Name = name;

    public IEnumerable<(string Key, string? Value)> Attributes => this.AttributeList.ToEnumerable();
    public IEnumerable<IHtmlElementInfo> Children => this.ChildList; public virtual ClosingTagType ClosingTagType { get; set; } = ClosingTagType.Full;
    public string Name { get; }

    public TSelf AddAttribute(string key, string? value = null)
    {
        this.AttributeList.Add(key, value);
        return this.This();
    }

    public TSelf AddChild(IHtmlElementInfo child)
    {
        this.ChildList.Add(child);
        return this.This();
    }

    public TSelf RemoveAttribute(string key)
    {
        _ = this.AttributeList.Remove(key);
        return this.This();
    }

    public TSelf RemoveChild(IHtmlElementInfo child)
    {
        _ = this.ChildList.Remove(child);
        return this.This();
    }

    protected virtual TSelf This()
        => this.To<TSelf>();
}