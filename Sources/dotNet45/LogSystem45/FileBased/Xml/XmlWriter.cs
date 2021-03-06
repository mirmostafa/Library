using System;
using System.IO;
using System.Linq;
using System.Xml;
using Mohammad.Helpers;
using Mohammad.Logging.Entities;
using Mohammad.Logging.Internals;

namespace Mohammad.Logging.FileBased.Xml
{
    public class XmlWriter<TLogEntity> : Writer<FileInfo, TLogEntity>
        where TLogEntity : LogEntity
    {
        public XmlWriter(string xmlLogFileSpec)
            : base(new FileInfo(xmlLogFileSpec))
        {
        }

        public override void LoadLastLog()
        {
            throw new NotSupportedException();
        }

        protected override void InnerWrite(TLogEntity logEntity)
        {
            lock (this)
            {
                var row = ObjectHelper.GetProperties(logEntity);
                if (logEntity.Level == LogLevel.Fatal || logEntity.Level == LogLevel.Error)
                {
                    row.ByName("StackTrace", logEntity.StackTrace);
                }

                var doc = new XmlDocument();
                if (!this.Log.Directory.Exists)
                {
                    this.Log.Directory.Create();
                }

                this.Log.Refresh();
                if (this.Log.Exists)
                {
                    using (var reader = new XmlTextReader(this.Log.FullName))
                    {
                        reader.WhitespaceHandling = WhitespaceHandling.None;
                        doc.Load(reader);
                    }
                }
                else
                {
                    doc.LoadXml(string.Format("<XmlLogger Generator='Log System Center'><File>\n\r{0}\n\r</File></XmlLogger>", this.Log.FullName));
                }

                using (var writer = new XmlTextWriter(this.Log.FullName, null) {Formatting = Formatting.Indented})
                {
                    XmlNode node = doc.DocumentElement;
                    node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Log", "Descriptor", logEntity.Descriptor));

                    foreach (var field in
                        row.Where(fld => fld.Value != null && !string.IsNullOrEmpty(fld.Name) && !fld.Name.IsInRange("Descriptor")))
                    {
                        var elem = doc.CreateElement(field.Name);
                        if (field.Value != null)
                        {
                            elem.InnerText = field.Value.ToString();
                        }

                        node.AppendChild(elem);
                    }

                    doc.Save(writer);
                }
            }
        }
    }
}