#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Xml.Linq;
using Library40.Dynamic.Xml;
using XNode = Library40.Dynamic.Xml.XNode;

namespace Library40.Helpers
{
	public static class XmlExtensions
	{
		public static dynamic AsDynamic(this XElement element)
		{
			return new XNode(element);
		}

		public static dynamic AsDynamic(this IEnumerable<XElement> elements)
		{
			return new XNodeList(elements);
		}

		public static dynamic LoadFile(string xmlFilePath)
		{
			var doc = XDocument.Load(xmlFilePath);
			return doc.Elements().ElementAt(0).As<XElement>().AsDynamic();
		}
	}
}