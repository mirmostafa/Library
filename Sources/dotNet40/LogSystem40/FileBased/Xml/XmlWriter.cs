#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:06 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.IO;
using System.Linq;
using System.Xml;
using Library40.Helpers;
using Library40.LogSystem.Entities;
using Library40.LogSystem.Internals;

namespace Library40.LogSystem.FileBased.Xml
{
	public class XmlWriter<TLogEntity> : Writer<FileInfo, TLogEntity>
		where TLogEntity : LogEntity
	{
		public XmlWriter(string xmlLogFileSpec)
			: base(new FileInfo(xmlLogFileSpec))
		{
		}

		protected override void InnerWrite(TLogEntity logEntity)
		{
			lock (this)
			{
				var row = ObjectHelper.ReflectProperties(logEntity);
				if (logEntity.Level == LogLevel.Fatal || logEntity.Level == LogLevel.Error)
					row["StackTrace"] = logEntity.StackTrace;

				var doc = new XmlDocument();
				if (!this.Log.Directory.Exists)
					this.Log.Directory.Create();
				this.Log.Refresh();
				if (this.Log.Exists)
					using (var reader = new XmlTextReader(this.Log.FullName))
					{
						reader.WhitespaceHandling = WhitespaceHandling.None;
						doc.Load(reader);
					}
				else
					doc.LoadXml(string.Format("<XmlLogger Generator='Log System Center'><File>\n\r{0}\n\r</File></XmlLogger>", this.Log.FullName));
				using (var writer = new XmlTextWriter(this.Log.FullName, null)
				                    {
					                    Formatting = Formatting.Indented
				                    })
				{
					XmlNode node = doc.DocumentElement;
					node = node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Log", "Descriptor", logEntity.Descriptor));

					foreach (var field in
						row.Where(fld => (fld.Value != null && !String.IsNullOrEmpty(fld.Key)) && !fld.Key.IsInRange("Descriptor")))
					{
						var elem = doc.CreateElement(field.Key);
						if (field.Value != null)
							elem.InnerText = field.Value.ToString();
						node.AppendChild(elem);
					}

					doc.Save(writer);
				}
			}
		}

		public override void LoadLastLog()
		{
			throw new NotSupportedException();
		}
	}
}