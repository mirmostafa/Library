using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mohammad.Helpers;

namespace Mohammad.Dynamic.Xml
{
    public sealed class XNode : DynamicObject
    {
        private readonly XElement _Element;

        public string Xml
        {
            get
            {
                var sw = new StringWriter();
                this._Element.Save(sw, SaveOptions.None);
                return sw.ToString();
            }
        }

        public XNodeList Nodes => new XNodeList(this._Element.Elements());
        public string Value => this._Element.Value;

        public XNode Parent
        {
            get
            {
                var parent = this._Element.Parent;
                return null != parent ? new XNode(parent) : null;
            }
        }

        public string Name => this._Element.Name.LocalName;

        public XNode(string name)
            : this(new XElement(name)) { }

        public XNode(XElement element) { this._Element = element; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;

            if (string.CompareOrdinal(name, "Name") == 0)
            {
                result = this.Name;
                return true;
            }
            if (string.CompareOrdinal(name, "Parent") == 0)
            {
                result = this.Parent;
                return false;
            }
            if (string.CompareOrdinal(name, "Value") == 0)
            {
                result = this.Value;
                return true;
            }
            if (string.CompareOrdinal(name, "Nodes") == 0)
            {
                result = this.Nodes;
                return true;
            }
            if (string.CompareOrdinal(name, "Xml") == 0)
            {
                result = this.Xml;
                return true;
            }
            var attribute = this._Element.Attribute(name);
            if (attribute != null)
            {
                result = attribute.Value;
                return true;
            }

            var childNode = this._Element.Element(name);
            if (childNode != null)
            {
                if (childNode.HasElements == false)
                {
                    result = childNode.Value;
                    return true;
                }
                result = new XNode(childNode);
                return true;
            }
            var nodes = (from element in this._Element.Descendants()
                         where string.CompareOrdinal(element.Name.LocalName, name) == 0
                         select new XNode(element)).ToList();
            if (!nodes.Any())
                return base.TryGetMember(binder, out result);
            if (nodes.HasItemsAtLeast(2))
                result = nodes;
            else
                result = nodes.ElementAt(0);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var name = binder.Name;

            if (string.CompareOrdinal(name, "SelectAll") == 0)
            {
                IEnumerable<XElement> selectedElements;

                if (args.Length == 0)
                {
                    selectedElements = this._Element.Descendants();
                }
                else if (args.Length == 1)
                {
                    selectedElements = this._Element.Descendants(args[0].ToString());
                }
                else
                {
                    result = false;
                    return false;
                }
                result = new XNodeList(selectedElements);
                return true;
            }
            if (string.CompareOrdinal(name, "SelectChildren") != 0)
                return base.TryInvokeMember(binder, args, out result);
            {
                IEnumerable<XElement> selectedElements;

                if (args.Length == 0)
                {
                    selectedElements = this._Element.Elements();
                }
                else if (args.Length == 1)
                {
                    selectedElements = this._Element.Elements(args[0].ToString());
                }
                else
                {
                    result = false;
                    return false;
                }
                result = new XNodeList(selectedElements);
                return true;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;

            if (string.CompareOrdinal(name, "Value") != 0)
                return base.TrySetMember(binder, value);
            this._Element.Value = value?.ToString() ?? string.Empty;
            return true;
        }
    }
}