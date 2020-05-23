#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Library40.DesignPatterns.Creational.Singletons;
using Library40.EventsArgs;
using Library40.Helpers;

namespace Library40.Data.Ado
{
	public class ExcelExport : Singleton<ExcelExport>
	{
		//Row limits older excel verion per sheet, the row limit for excel 2003 is 65536
		private int _RowLimit = 65000;
		private bool _TruncateLongSheetName = true;

		private ExcelExport()
		{
		}

		/// <summary>
		///     Gets or sets a value indicating whether truncate long sheet name if it is longer than 30 chars.
		/// </summary>
		/// <value>
		///     <c>true</c> if truncate long sheet name; otherwise, <c>false</c> .
		/// </value>
		public bool TruncateLongSheetName
		{
			get { return this._TruncateLongSheetName; }
			set { this._TruncateLongSheetName = value; }
		}

		/// <summary>
		///     Gets or sets the row limit per sheet.
		/// </summary>
		/// <value> The row limit. </value>
		public int RowLimit
		{
			get { return this._RowLimit; }
			set { this._RowLimit = value; }
		}

		/// <summary>
		///     Occurs when formatting DateTime data.
		/// </summary>
		public event EventHandler<ItemActedEventArgs<string>> FormattingDateTime;

		/// <summary>
		///     Raises the <see cref="E:FormattingDateTime" /> event.
		/// </summary>
		/// <param name="e">
		///     The <see cref="string" /> instance containing the event data.
		/// </param>
		/// <returns> </returns>
		protected virtual ItemActedEventArgs<string> OnFormattingDateTime(ItemActedEventArgs<string> e)
		{
			var handler = this.FormattingDateTime;
			if (handler != null)
				handler(this, e);
			return e;
		}

		/// <summary>
		///     Gets the workbook template.
		/// </summary>
		/// <returns> </returns>
		protected static string GetWorkbookTemplate()
		{
			Contract.Ensures(1 <= Environment.NewLine.Length);

			var sb = new StringBuilder(818);
			sb.AppendFormat(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>{0}", Environment.NewLine);
			sb.AppendFormat(@"<?mso-application progid=""Excel.Sheet""?>{0}", Environment.NewLine);
			sb.AppendFormat(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
			sb.AppendFormat(@" xmlns:o=""urn:schemas-microsoft-com:office:office""{0}", Environment.NewLine);
			sb.AppendFormat(@" xmlns:x=""urn:schemas-microsoft-com:office:excel""{0}", Environment.NewLine);
			sb.AppendFormat(@" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);
			sb.AppendFormat(@" xmlns:html=""http://www.w3.org/TR/REC-html40"">{0}", Environment.NewLine);
			sb.AppendFormat(@" <Styles>{0}", Environment.NewLine);
			sb.AppendFormat(@"  <Style ss:ID=""Default"" ss:Name=""Normal"">{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Alignment ss:Vertical=""Bottom""/>{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Borders/>{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Font ss:FontName=""Tahoma"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Interior/>{0}", Environment.NewLine);
			sb.AppendFormat(@"   <NumberFormat/>{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Protection/>{0}", Environment.NewLine);
			sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
			sb.AppendFormat(@"  <Style ss:ID=""s62"">{0}", Environment.NewLine);
			sb.AppendFormat(@"   <Font ss:FontName=""Tahoma"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""{0}", Environment.NewLine);
			sb.AppendFormat(@"    ss:Bold=""1""/>{0}", Environment.NewLine);
			sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
			sb.AppendFormat(@"  <Style ss:ID=""s63"">{0}", Environment.NewLine);
			sb.AppendFormat(@"   <NumberFormat ss:Format=""Short Date""/>{0}", Environment.NewLine);
			sb.AppendFormat(@"  </Style>{0}", Environment.NewLine);
			sb.AppendFormat(@" </Styles>{0}", Environment.NewLine);
			sb.AppendLine(@"{0}");
			sb.AppendLine(@"</Workbook>");
			return sb.ToString();
		}

		/// <summary>
		///     Gets the worksheets.
		/// </summary>
		/// <param name="source"> The source. </param>
		/// <returns> </returns>
		protected virtual string GetWorksheets(DataSet source)
		{
			#region replaceXmlChar
			Func<string, string> replaceXmlChar = delegate(string input)
			                                      {
				                                      input = input.Replace("&", "&amp");
				                                      input = input.Replace("<", "&lt;");
				                                      input = input.Replace(">", "&gt;");
				                                      input = input.Replace("\"", "&quot;");
				                                      input = input.Replace("'", "&apos;");
				                                      return input;
			                                      };
			#endregion

			#region getCell
			Func<Type, object, string> getCell = delegate(Type type, object cellData)
			                                     {
				                                     var data = (cellData is DBNull) ? "" : cellData;
				                                     if (type.Name.Contains("Int") || type.Name.Contains("Double") || type.Name.Contains("Decimal") || type.Name.Contains("Byte"))
					                                     return string.Format("<Cell><Data ss:Type=\"Number\">{0}</Data></Cell>{1}", data, Environment.NewLine);
				                                     if (type.Name.Contains("Date") && data.ToString() != string.Empty)
					                                     return string.Format("<Cell ss:StyleID=\"s63\"><Data ss:Type=\"String\">{0}</Data></Cell>{1}",
						                                     this.FormatDateTime(data),
						                                     Environment.NewLine);
				                                     return string.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell>{1}", replaceXmlChar(data.ToString()), Environment.NewLine);
			                                     };
			#endregion

			#region validateSheetName
			Func<DataTable, string> validateSheetName = dt => this.TruncateLongSheetName && dt.TableName.Length > 30 ? dt.TableName.Substring(0, 30) : dt.TableName;
			#endregion

			var sw = new StringWriter();
			if (source == null || source.Tables.Count == 0)
			{
				sw.Write("<Worksheet ss:Name=\"Sheet1\">\r\n<Table>\r\n<Row>\r\n<Cell><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
				return sw.ToString();
			}
			foreach (DataTable dt in source.Tables)
				if (dt.Rows.Count == 0)
					sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(validateSheetName(dt)) +
					         "\">\r\n<Table>\r\n<Row>\r\n<Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
				else
				{
					//write each row data                
					var sheetCount = 0;
					for (var i = 0; i < dt.Rows.Count; i++)
					{
						if ((i % this.RowLimit) == 0)
						{
							//add close tags for previous sheet of the same data table
							if ((i / this.RowLimit) > sheetCount)
							{
								sw.Write("\r\n</Table>\r\n</Worksheet>");
								sheetCount = (i / this.RowLimit);
							}
							sw.Write("\r\n<Worksheet ss:Name=\"" + replaceXmlChar(validateSheetName(dt)) + (((i / this.RowLimit) == 0) ? "" : Convert.ToString(i / this.RowLimit)) + "\">\r\n<Table>");
							//write column name row
							sw.Write("\r\n<Row>\r\n");
							foreach (DataColumn dc in dt.Columns)
								sw.Write("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>\r\n", replaceXmlChar(dc.ColumnName));
							sw.Write("</Row>");
						}
						sw.Write("\r\n<Row>\r\n");
						foreach (DataColumn dc in dt.Columns)
							sw.Write(getCell(dc.DataType, dt.Rows[i][dc.ColumnName]));
						sw.Write("</Row>");
					}
					sw.Write("\r\n</Table>\r\n</Worksheet>");
				}

			return sw.ToString();
		}

		/// <summary>
		///     Formats the DateTime data.
		/// </summary>
		/// <param name="data"> The data. </param>
		/// <returns> </returns>
		protected virtual string FormatDateTime(object data)
		{
			var result = string.Empty;
			if (data != null)
				result = data.ToString();
			var e = this.OnFormattingDateTime(new ItemActedEventArgs<string>(result));
			result = e.Item;
			return result;
		}

		/// <summary>
		///     Writes the bytes to file.
		/// </summary>
		/// <param name="file"> The file. </param>
		/// <param name="bytes"> The bytes. </param>
		protected virtual void WriteToFile(FileInfo file, byte[] bytes)
		{
			if (file.Exists)
				file.Delete();
			file.Create().Close();
			using (var fileStream = file.OpenWrite())
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
		}

		/// <summary>
		///     Gets the excel XML.
		/// </summary>
		/// <param name="table"> The table. </param>
		/// <returns> </returns>
		public string GetExcelXml(DataTable table)
		{
			return GetExcelXml(new[]
			                   {
				                   table
			                   });
		}

		/// <summary>
		///     Gets the excel XML.
		/// </summary>
		/// <param name="tables"> The tables. </param>
		/// <returns> </returns>
		public string GetExcelXml(IEnumerable<DataTable> tables)
		{
			return GetExcelXml(tables.ToArray());
		}

		/// <summary>
		///     Gets the excel XML.
		/// </summary>
		/// <param name="tables"> The tables. </param>
		/// <returns> </returns>
		public string GetExcelXml(params DataTable[] tables)
		{
			var ds = new DataSet();
			foreach (var table in tables)
				ds.Tables.Add(table.Copy());
			return GetExcelXml(ds);
		}

		/// <summary>
		///     Gets the excel XMLs.
		/// </summary>
		/// <param name="tables"> The tables. </param>
		/// <returns> </returns>
		public IEnumerable<string> GetExcelXml(DataTableCollection tables)
		{
			return from DataTable table in tables select this.GetExcelXml(table);
		}

		/// <summary>
		///     Gets the excel XML.
		/// </summary>
		/// <param name="dataSet"> The data set. </param>
		/// <returns> </returns>
		public string GetExcelXml(DataSet dataSet)
		{
			return this.GetXmlCore(dataSet);
		}

		/// <summary>
		///     Gets the XML [core method].
		/// </summary>
		/// <param name="dataSet"> The data set. </param>
		/// <returns> </returns>
		protected virtual string GetXmlCore(DataSet dataSet)
		{
			var excelTemplate = GetWorkbookTemplate();
			var worksheets = this.GetWorksheets(dataSet);
			var excelXml = string.Format(excelTemplate, worksheets);
			return excelXml;
		}

		/// <summary>
		///     Exports the specified datatables to excel.
		/// </summary>
		/// <param name="datatables"> The datatables. </param>
		/// <param name="filename"> The filename. </param>
		/// <returns> </returns>
		public IEnumerable<FileInfo> Export(DataTableCollection datatables, string filename = null)
		{
			return from DataTable table in datatables select this.Export(table);
		}

		/// <summary>
		///     Exports the specified DataSet to excel.
		/// </summary>
		/// <param name="dataSet"> The data set. </param>
		/// <param name="filename"> The filename. </param>
		/// <returns> </returns>
		public FileInfo Export(DataSet dataSet, string filename = null)
		{
			var excelXml = GetExcelXml(dataSet);
			if (filename.IsNullOrEmpty())
				filename = dataSet.DataSetName;
			if (filename == null)
				throw new ArgumentNullException("filename");
			if (!filename.ToUpper().EndsWith(".XLS"))
				filename = string.Concat(filename, ".xls");
			var bytes = Encoding.UTF8.GetBytes(excelXml);
			var result = new FileInfo(filename);
			this.WriteToFile(result, bytes);
			return result;
		}

		/// <summary>
		///     Exports the specified table to excel.
		/// </summary>
		/// <param name="table"> The table. </param>
		/// <param name="filename"> The filename. </param>
		/// <returns> </returns>
		public FileInfo Export(DataTable table, string filename = null)
		{
			var ds = new DataSet();
			ds.Tables.Add(table.Copy());
			return this.Export(ds, filename ?? table.TableName);
		}

		/// <summary>
		///     Exports the specified tables to excel.
		/// </summary>
		/// <param name="tables"> The tables. </param>
		/// <param name="filename"> The filename. </param>
		/// <returns> </returns>
		public FileInfo Export(IEnumerable<DataTable> tables, string filename = null)
		{
			return this.ExportCore(tables, filename);
		}

		/// <summary>
		///     Core export method.
		/// </summary>
		/// <param name="tables"> The tables. </param>
		/// <param name="filename"> The filename. </param>
		/// <returns> </returns>
		protected virtual FileInfo ExportCore(IEnumerable<DataTable> tables, string filename = null)
		{
			var ds = new DataSet();
			foreach (var table in tables)
				ds.Tables.Add(table.Copy());
			return this.Export(ds, filename);
		}
	}
}