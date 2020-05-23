#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Library40.Helpers
{
	public static class SerializationHelper
	{
		/// <summary>
		///     Serializes the specified TClass and writes the XML document to a file using the specified xmlFilePath.
		/// </summary>
		/// <param name="xmlFilePath">The string used to catch to the XML document.</param>
		/// <param name="value">The TClass to serialize.</param>
		private static void Serialize<TClass>(string xmlFilePath, TClass value)
		{
			var xmlSerializer = new XmlSerializer(value.GetType());
			using (Stream stream = new FileStream(xmlFilePath, FileMode.Create))
			{
				var settings = new XmlWriterSettings
				               {
					               Indent = true
				               };
				using (var xmlWriter = XmlWriter.Create(stream, settings))
					xmlSerializer.Serialize(xmlWriter, value);
			}
		}

		/// <summary>
		///     Deserializes the XML document contained by the specified xmlFilePath.
		/// </summary>
		/// <returns>The TClass being deserialized.</returns>
		/// <param name="xmlFilePath">The path of XML document to deserialize.</param>
		private static TClass Deserialize<TClass>(string xmlFilePath)
		{
			TClass result;
			var xmlSerializer = new XmlSerializer(typeof (TClass));
			using (XmlReader xmlreader = new XmlTextReader(new FileStream(xmlFilePath, FileMode.Open)))
				result = (TClass)xmlSerializer.Deserialize(xmlreader);
			return result;
		}

		public static TClass Load<TClass>(string xmlFilePath, TClass defaultValue) where TClass : class
		{
			var xmlFile = new FileInfo(xmlFilePath);
			if (!xmlFile.Exists)
			{
				if (xmlFile.Directory != null && !xmlFile.Directory.Exists)
					xmlFile.Directory.Create();
				Save(xmlFilePath, defaultValue);
			}
			var result = Load<TClass>(xmlFilePath);
			return result ?? defaultValue;
		}

		public static TClass Load<TClass>(string xmlFilePath)
		{
			return Deserialize<TClass>(xmlFilePath);
		}

		public static void Save<TClass>(string xmlFilePath, TClass value)
		{
			Serialize(xmlFilePath, value);
		}

		public static byte[] SerializeToBinary(object value)
		{
			var formatter = new BinaryFormatter();

			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, value);
				return stream.ToArray();
			}
		}

		public static string SerializeToXml(object value)
		{
			return SerializeToXml(value, value.GetType());
		}

		public static string SerializeToXml(object value, params Type[] extraTypes)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			var serializer = new XmlSerializer(value.GetType(), extraTypes);

			using (var writer = new StringWriter())
			{
				serializer.Serialize(writer, value);
				return writer.ToString();
			}
		}
	}
}