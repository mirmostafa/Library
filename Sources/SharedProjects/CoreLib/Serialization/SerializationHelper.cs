using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Mohammad.Security.Cryptography;

namespace Mohammad.Serialization
{
    public static class SerializationHelper
    {
        /// <summary>
        ///     Serializes the specified TClass and writes the XML document to a file using the specified xmlFilePath.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="xmlFilePath">The string used to catch to the XML document.</param>
        /// <param name="value">The TClass to serialize.</param>
        /// <param name="encrypt">
        ///     if set to <c>true</c> [encrypt after save].
        /// </param>
        /// <param name="password">The password.</param>
        private static void Serialize<TClass>(string xmlFilePath, TClass value, bool encrypt, string password)
        {
            var ser = new XmlSerializer(value.GetType());
            if (!encrypt)
            {
                using (Stream stream = new FileStream(xmlFilePath, FileMode.Create))
                using (var xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings {Indent = true}))
                    ser.Serialize(xmlWriter, value);
            }
            else
            {
                var builder = new StringBuilder();
                using (var strWriter = new StringWriter(builder))
                    ser.Serialize(strWriter, value);
                using (var writer = File.CreateText(xmlFilePath))
                    writer.Write(RijndaelEncryption.Encrypt(builder.ToString(), password));
            }
        }

        /// <summary>
        ///     Deserializes the XML document contained by the specified xmlFilePath.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="xmlFilePath">The path of XML document to deserialize.</param>
        /// <param name="isEncrypted">
        ///     if set to <c>true</c> [is encrypted].
        /// </param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     The TClass being deserialized.
        /// </returns>
        private static TClass Deserialize<TClass>(string xmlFilePath, bool isEncrypted, string password)
        {
            var xmlSerializer = new XmlSerializer(typeof(TClass));
            if (!isEncrypted)
                using (var stream = new FileStream(xmlFilePath, FileMode.Open))
                using (XmlReader xmlReader = new XmlTextReader(stream))
                    return (TClass) xmlSerializer.Deserialize(xmlReader);
            using (var reader = new StringReader(RijndaelEncryption.Decrypt(File.ReadAllText(xmlFilePath), password)))
            using (XmlReader xmlreader = new XmlTextReader(reader))
                return (TClass) xmlSerializer.Deserialize(xmlreader);
        }

        /// <summary>
        ///     Loads the specified XML file path.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="isEncrypted">
        ///     if set to <c>true</c> [is encrypted].
        /// </param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static TClass Load<TClass>(string xmlFilePath, TClass defaultValue, bool isEncrypted = false, string password = null) where TClass : class
        {
            var xmlFile = new FileInfo(xmlFilePath);
            if (!xmlFile.Exists)
            {
                if (xmlFile.Directory != null && !xmlFile.Directory.Exists)
                    xmlFile.Directory.Create();
                Save(xmlFilePath, defaultValue, isEncrypted, password);
            }
            var result = Load<TClass>(xmlFilePath, isEncrypted, password);
            return result ?? defaultValue;
        }

        public static TClass Load<TClass>(string xmlFilePath, bool isEncrypted = false, string password = null)
        {
            return Deserialize<TClass>(xmlFilePath, isEncrypted, password);
        }

        public static void Save<TClass>(string xmlFilePath, TClass value, bool encryptAfterSave = false, string password = null)
        {
            Serialize(xmlFilePath, value, encryptAfterSave, password);
        }

        public static byte[] SerializeToBinary(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public static string SerializeToXml(object value, params Type[] extraTypes)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var serializer = new XmlSerializer(value.GetType(), extraTypes);

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }
    }
}