#region File Notice
// Created at: 2013/12/24 3:41 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Library35.Data.Transformation
{
	public static class ExcelTransform
	{
		public static DataSet Export(string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			var builder = new StringBuilder();
			builder.AppendLine("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?>");
			builder.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
			builder.AppendLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
			builder.AppendLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
			builder.AppendLine("<Styles><Style ss:ID='sText'>" + "<NumberFormat ss:Format='@'/></Style>");
			builder.AppendLine("<Style ss:ID='sDate'><NumberFormat" + " ss:Format='[$-409]m/d/yy\\ h:mm\\ AM/PM;@'/>");
			builder.AppendLine("</Style></Styles>");

			var file = new FileInfo(fileName);
			var result = new DataSet(file.Name);
			var reader = file.OpenRead();
			var buffer = new byte[reader.Length];
			reader.Read(buffer, 0, buffer.Length);
			var rawXml = string.Concat("<Workbook>", Encoding.UTF8.GetString(buffer).Substring(builder.ToString().Length)).Replace("ss:", "");
			var xDoc = XDocument.Parse(rawXml);
			var xWorksheets = xDoc.Elements("Workbook").Elements("Worksheet");
			foreach (var worksheet in xWorksheets)
			{
				var dt = result.Tables.Add(worksheet.FirstAttribute.Value);
				var xRows = worksheet.Elements("Table").Elements("Row");
				var schema = xRows.ElementAt(0);
				foreach (var xCol in schema.Elements("Cell").Elements("Data"))
					dt.Columns.Add(xCol.Value);
				foreach (var xRow in xRows.Skip(1))
				{
					var xData = xRow.Elements("Cell").Elements("Data");
					var row = dt.NewRow();
					for (var columnIndex = 0; columnIndex < dt.Columns.Count; columnIndex++)
						row[columnIndex] = xData.ElementAt(columnIndex).Value;
					dt.Rows.Add(row);
				}
			}
			return result;
		}

		public static string Import(DataTable table, string fileName = "")
		{
			if (table == null)
				throw new ArgumentNullException("table");
			var file = new FileInfo(string.IsNullOrEmpty(fileName) ? string.Concat(table.TableName, ".xml") : fileName);

			var builder = new StringBuilder();
			builder.AppendLine("<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?>");
			builder.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
			builder.AppendLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
			builder.AppendLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
			builder.AppendLine("<Styles><Style ss:ID='sText'>" + "<NumberFormat ss:Format='@'/></Style>");
			builder.AppendLine("<Style ss:ID='sDate'><NumberFormat" + " ss:Format='[$-409]m/d/yy\\ h:mm\\ AM/PM;@'/>");
			builder.AppendLine("</Style></Styles>");

			using (var writer = new XmlTextWriter(file.FullName, Encoding.UTF8))
			{
				writer.WriteRaw(builder.ToString());
				var sheetName = !string.IsNullOrEmpty(table.TableName) ? table.TableName : new Guid().ToString("");
				writer.WriteRaw("<Worksheet ss:Name='" + sheetName + "'>");
				writer.WriteRaw("<Table>");
				var columnTypes = new string[table.Columns.Count];

				string colType;
				for (var i = 0; i < table.Columns.Count; i++)
				{
					colType = table.Columns[i].DataType.ToString().ToLower();

					if (colType.Contains("datetime"))
					{
						columnTypes[i] = "DateTime";
						writer.WriteRaw("<Column ss:StyleID='sDate'/>");
					}
					else if (colType.Contains("string"))
					{
						columnTypes[i] = "String";
						writer.WriteRaw("<Column ss:StyleID='sText'/>");
					}
					else
					{
						writer.WriteRaw("<Column />");

						if (colType.Contains("boolean"))
							columnTypes[i] = "Boolean";
						else
							columnTypes[i] = "Number";
					}
				}
				writer.WriteRaw("<Row>");
				foreach (DataColumn col in table.Columns)
				{
					writer.WriteRaw("<Cell ss:StyleID='sText'><Data ss:Type='String'>");
					writer.WriteRaw(col.ColumnName);
					writer.WriteRaw("</Data></Cell>");
				}
				writer.WriteRaw("</Row>");
				foreach (DataRow row in table.Rows)
				{
					writer.WriteRaw("<Row>");
					for (var i = 0; i < table.Columns.Count; i++)
					{
						writer.WriteRaw("<Cell><Data ss:Type='" + columnTypes[i] + "'>");

						switch (columnTypes[i])
						{
							case "DateTime":
								writer.WriteRaw(((DateTime)row[i]).ToString("s"));
								break;
							case "Boolean":
								writer.WriteRaw(((bool)row[i]) ? "1" : "0");
								break;
							case "String":
								writer.WriteString(row[i].ToString());
								break;
							default:
								writer.WriteString(row[i].ToString());
								break;
						}

						writer.WriteRaw("</Data></Cell>");
					}
					writer.WriteRaw("</Row>");
				}
				writer.WriteRaw("</Table></Worksheet>");
				writer.WriteRaw("</Workbook>");
			}
			return file.FullName;
		}
	}
}