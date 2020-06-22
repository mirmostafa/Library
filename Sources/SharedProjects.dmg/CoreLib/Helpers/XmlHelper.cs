using System.Collections.Generic;
using System.Xml.Linq;
using Mohammad.Dynamic.Xml;
using XNode = Mohammad.Dynamic.Xml.XNode;

namespace Mohammad.Helpers
{
    public static class XmlHelper
    {
        public static dynamic AsDynamic(this XElement element) => new XNode(element);
        public static dynamic AsDynamic(this IEnumerable<XElement> elements) => new XNodeList(elements);
        public static dynamic LoadFile(string xmlFilePath) => XDocument.Load(xmlFilePath).Elements().ElementAt(0).As<XElement>().AsDynamic();
    }
}