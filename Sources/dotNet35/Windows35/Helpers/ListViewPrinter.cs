#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Library35.Windows.Helpers
{
	public class ListViewPrinter
	{
		private readonly PrintPreviewDialog m_previewDlg = new PrintPreviewDialog();
		private readonly PrintDialog m_printDlg = new PrintDialog();
		private readonly PrintDocument m_printDoc = new PrintDocument();
		private readonly PageSetupDialog m_setupDlg = new PageSetupDialog();
		private float[] m_arColsWidth;
		private bool m_bFitToPage;
		private bool m_bPrintSel;

		private float m_fDpi = 96.0f;
		private float m_fListWidth;
		private int m_nPageNumber = 1;
		private int m_nStartCol;
		private int m_nStartRow;

		private string m_strTitle = "";

		#region Properties
		/// <summary>
		///     Gets or sets whether to fit the list width on a single page
		/// </summary>
		/// <value>
		///     <c>True</c> if you want to scale the list width so it will fit on a single page.
		/// </value>
		/// <remarks>
		///     If you choose false (the default value), and the list width exceeds the page width, the list
		///     will be broken in multiple page.
		/// </remarks>
		public bool FitToPage
		{
			get { return this.m_bFitToPage; }
			set { this.m_bFitToPage = value; }
		}

		/// <summary>
		///     Gets or sets the title to dispaly as page header in the printed list
		/// </summary>
		/// <value>
		///     A <see cref="string" /> the represents the title printed as page header.
		/// </value>
		public string Title
		{
			get { return this.m_strTitle; }
			set { this.m_strTitle = value; }
		}
		#endregion

		/// <summary>
		///     Required designer variable.
		/// </summary>
		public ListViewPrinter(ListView listView)
		{
			this.ListView = listView;

			this.m_printDoc.BeginPrint += this.OnBeginPrint;
			this.m_printDoc.PrintPage += this.OnPrintPage;

			this.m_setupDlg.Document = this.m_printDoc;
			this.m_previewDlg.Document = this.m_printDoc;
			this.m_printDlg.Document = this.m_printDoc;

			this.m_printDlg.AllowSomePages = false;
		}

		public ListView ListView { get; private set; }

		/// <summary>
		///     WaitFor the standard page setup dialog box that lets the user specify
		///     margins, page orientation, page sources, and paper sizes.
		/// </summary>
		public void PageSetup()
		{
			this.m_setupDlg.ShowDialog();
		}

		/// <summary>
		///     WaitFor the standard print preview dialog box.
		/// </summary>
		public void PrintPreview()
		{
			this.m_printDoc.DocumentName = "List View";

			this.m_nPageNumber = 1;
			this.m_bPrintSel = false;

			this.m_previewDlg.ShowDialog(this.ListView);
		}

		/// <summary>
		///     Start the print process.
		/// </summary>
		public void Print()
		{
			this.m_printDlg.AllowSelection = this.ListView.SelectedItems.Count > 0;

			// WaitFor the standard print dialog box, that lets the user select a printer
			// and change the settings for that printer.
			if (this.m_printDlg.ShowDialog(this.ListView) != DialogResult.OK)
				return;
			this.m_printDoc.DocumentName = this.m_strTitle;

			this.m_bPrintSel = this.m_printDlg.PrinterSettings.PrintRange == PrintRange.Selection;

			this.m_nPageNumber = 1;

			// Start print
			this.m_printDoc.Print();
		}

		private int GetItemsCount()
		{
			return this.m_bPrintSel ? this.ListView.SelectedItems.Count : this.ListView.Items.Count;
		}

		private ListViewItem GetItem(int index)
		{
			return this.m_bPrintSel ? this.ListView.SelectedItems[index] : this.ListView.Items[index];
		}

		private void PreparePrint()
		{
			// Gets the list width and the columns width in units of hundredths of an inch.
			this.m_fListWidth = 0.0f;
			this.m_arColsWidth = new float[this.ListView.Columns.Count];

			var g = this.ListView.CreateGraphics();
			this.m_fDpi = g.DpiX;
			g.Dispose();

			for (var i = 0; i < this.ListView.Columns.Count; i++)
			{
				var ch = this.ListView.Columns[i];
				var fWidth = ch.Width / this.m_fDpi * 100 + 1; // Column width + separator
				this.m_fListWidth += fWidth;
				this.m_arColsWidth[i] = fWidth;
			}
			this.m_fListWidth += 1; // separator
		}

		#region Events Handlers
		private void OnBeginPrint(object sender, PrintEventArgs e)
		{
			this.PreparePrint();
		}

		private void OnPrintPage(object sender, PrintPageEventArgs e)
		{
			var nNumItems = this.GetItemsCount(); // Number of items to print

			if (nNumItems == 0 || this.m_nStartRow >= nNumItems)
			{
				e.HasMorePages = false;
				return;
			}

			var nNextStartCol = 0; // First column exeeding the page width
			var x = 0.0f; // Current horizontal coordinate
			var y = 0.0f; // Current vertical coordinate
			const float cx = 4.0f;
			// of the padding between items text and
			// their cell boundaries.
			var fScale = 1.0f; // Scale factor when fit to page is enabled
			var fRowHeight = 0.0f; // The height of the current row
			var fColWidth = 0.0f; // The width of the current column

			RectangleF rectFull; // The full available space
			RectangleF rectBody; // Area for the list items

			var bUnprintable = false;

			var g = e.Graphics;

			if (g.VisibleClipBounds.X < 0) // Print preview
			{
				rectFull = e.MarginBounds;

				// Convert to hundredths of an inch
				rectFull = new RectangleF(rectFull.X / this.m_fDpi * 100.0f,
					rectFull.Y / this.m_fDpi * 100.0f,
					rectFull.Width / this.m_fDpi * 100.0f,
					rectFull.Height / this.m_fDpi * 100.0f);
			}
			else // Print
				// Printable area (approximately) of the page, taking into account the user margins
				rectFull = new RectangleF(e.MarginBounds.Left - (e.PageBounds.Width - g.VisibleClipBounds.Width) / 2,
					e.MarginBounds.Top - (e.PageBounds.Height - g.VisibleClipBounds.Height) / 2,
					e.MarginBounds.Width,
					e.MarginBounds.Height);

			rectBody = RectangleF.Inflate(rectFull, 0, -2 * this.ListView.Font.GetHeight(g));

			// Display title at top
			var sfmt = new StringFormat
			           {
				           Alignment = StringAlignment.Center
			           };
			g.DrawString(this.m_strTitle, this.ListView.Font, Brushes.Black, rectFull, sfmt);

			// Display page number at bottom
			sfmt.LineAlignment = StringAlignment.Far;
			g.DrawString("Page " + this.m_nPageNumber, this.ListView.Font, Brushes.Black, rectFull, sfmt);

			if (this.m_nStartCol == 0 && this.m_bFitToPage && this.m_fListWidth > rectBody.Width)
				// Calculate scale factor
				fScale = rectBody.Width / this.m_fListWidth;

			// Scale the printable area
			rectFull = new RectangleF(rectFull.X / fScale, rectFull.Y / fScale, rectFull.Width / fScale, rectFull.Height / fScale);

			rectBody = new RectangleF(rectBody.X / fScale, rectBody.Y / fScale, rectBody.Width / fScale, rectBody.Height / fScale);

			// Setting scale factor and unit of measure
			g.ScaleTransform(fScale, fScale);
			g.PageUnit = GraphicsUnit.Inch;
			g.PageScale = 0.01f;

			// Start print
			nNextStartCol = 0;
			y = rectBody.Top;

			// Columns headers ----------------------------------------
			Brush brushHeader = new SolidBrush(Color.LightGray);
			var fontHeader = new Font(this.ListView.Font, FontStyle.Bold);
			fRowHeight = fontHeader.GetHeight(g) * 3.0f;
			x = rectBody.Left;

			for (var i = this.m_nStartCol; i < this.ListView.Columns.Count; i++)
			{
				var ch = this.ListView.Columns[i];
				fColWidth = this.m_arColsWidth[i];

				if ((x + fColWidth) <= rectBody.Right)
				{
					// Rectangle
					g.FillRectangle(brushHeader, x, y, fColWidth, fRowHeight);
					g.DrawRectangle(Pens.Black, x, y, fColWidth, fRowHeight);

					// Text
					var sf = new StringFormat();
					switch (ch.TextAlign)
					{
						case HorizontalAlignment.Left:
							sf.Alignment = StringAlignment.Near;
							break;
						case HorizontalAlignment.Center:
							sf.Alignment = StringAlignment.Center;
							break;
						default:
							sf.Alignment = StringAlignment.Far;
							break;
					}

					sf.LineAlignment = StringAlignment.Center;
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.Trimming = StringTrimming.EllipsisCharacter;

					var rectText = new RectangleF(x + cx, y, fColWidth - 1 - 2 * cx, fRowHeight);
					g.DrawString(ch.Text, fontHeader, Brushes.Black, rectText, sf);
					x += fColWidth;
				}
				else
				{
					if (i == this.m_nStartCol)
						bUnprintable = true;

					nNextStartCol = i;
					break;
				}
			}
			y += fRowHeight;

			// Rows ---------------------------------------------------
			var nRow = this.m_nStartRow;
			var bEndOfPage = false;
			while (!bEndOfPage && nRow < nNumItems)
			{
				var item = this.GetItem(nRow);

				fRowHeight = item.Bounds.Height / this.m_fDpi * 100.0f + 5.0f;

				if (y + fRowHeight > rectBody.Bottom)
					bEndOfPage = true;
				else
				{
					x = rectBody.Left;

					for (var i = this.m_nStartCol; i < this.ListView.Columns.Count; i++)
					{
						var ch = this.ListView.Columns[i];
						fColWidth = this.m_arColsWidth[i];

						if ((x + fColWidth) <= rectBody.Right)
						{
							// Rectangle
							g.DrawRectangle(Pens.Black, x, y, fColWidth, fRowHeight);

							// Text
							var sf = new StringFormat();
							switch (ch.TextAlign)
							{
								case HorizontalAlignment.Left:
									sf.Alignment = StringAlignment.Near;
									break;
								case HorizontalAlignment.Center:
									sf.Alignment = StringAlignment.Center;
									break;
								default:
									sf.Alignment = StringAlignment.Far;
									break;
							}

							sf.LineAlignment = StringAlignment.Center;
							sf.FormatFlags = StringFormatFlags.NoWrap;
							sf.Trimming = StringTrimming.EllipsisCharacter;

							// Text
							var strText = i == 0 ? item.Text : item.SubItems[i].Text;
							var font = i == 0 ? item.Font : item.SubItems[i].Font;

							var rectText = new RectangleF(x + cx, y, fColWidth - 1 - 2 * cx, fRowHeight);
							g.DrawString(strText, font, Brushes.Black, rectText, sf);
							x += fColWidth;
						}
						else
						{
							nNextStartCol = i;
							break;
						}
					}

					y += fRowHeight;
					nRow++;
				}
			}

			if (nNextStartCol == 0)
				this.m_nStartRow = nRow;

			this.m_nStartCol = nNextStartCol;

			this.m_nPageNumber++;

			e.HasMorePages = (!bUnprintable && (this.m_nStartRow > 0 && this.m_nStartRow < nNumItems) || this.m_nStartCol > 0);

			if (!e.HasMorePages)
			{
				this.m_nPageNumber = 1;
				this.m_nStartRow = 0;
				this.m_nStartCol = 0;
			}

			brushHeader.Dispose();
		}
		#endregion
	}
}