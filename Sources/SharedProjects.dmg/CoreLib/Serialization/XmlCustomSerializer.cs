using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Mohammad.Helpers;

namespace Mohammad.Serialization
{
    public class XmlCustomSerializer
    {
        public bool ShowEmptyMembers { get; set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string Serialize(object t, string rootname = "Root")
        {
            var xDoc = new XDocument(new XDeclaration("1.0", this.Encoding.HeaderName, "yes"));
            var root = new XElement(rootname);
            xDoc.Add(root);
            Action<XContainer, object> serializeSingleObjects = delegate(XContainer container, object obj)
            {
                foreach (var property in obj.GetType().GetProperties())
                {
                    var value = property.GetValue(obj, null);
                    if (value == null)
                    {
                        if (this.ShowEmptyMembers)
                            container.Add(new XElement(property.Name, null));
                    }
                    else
                    {
                        container.Add(new XElement(property.Name, value));
                    }
                }
            };
            if (!(t is IEnumerable))
            {
                serializeSingleObjects(root, t);
            }
            else
            {
                var source = (IEnumerable) t;
                source.ForEach(item =>
                {
                    var element = new XElement(item.GetType().Name);
                    serializeSingleObjects(element, item);
                    root.Add(element);
                });
            }
            var doc = new XmlDocument();
            doc.LoadXml(xDoc.ToString());
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0", this.Encoding.HeaderName, "yes"), doc.FirstChild);
            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder, new XmlWriterSettings {Indent = true, Encoding = this.Encoding}))
                doc.Save(writer);
            return builder.ToString();
        }

        [Obsolete("Not done yet.", true)]
        public T Deserialize<T>(string xml) where T : new()
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var result = new T();
            foreach (XmlNode node in doc.ChildNodes.Cast<XmlNode>().First().ChildNodes.Cast<XmlNode>().First())
                result.GetType().GetProperty(node.Name)?.SetValue(result, node.Value, new object[] {});
            return result;
        }
    }
}