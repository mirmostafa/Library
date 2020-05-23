#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Library40.Win.Controls.BarChart
{
	public class CPrinter
	{
		private readonly PrintDocument document = new PrintDocument();

		public CPrinter()
		{
			this.document.PrintPage += this.document_PrintPage;
			//add_PrintPage(new PrintPageEventHandler(this, (IntPtr)this.OnPrintPage));
		}

		public Bitmap BmpBuffer { get; set; }

		public PrintDocument Document
		{
			get { return this.document; }
			set { }
		}

		public bool FitToPaper { get; set; }

		public int PageCount { get; set; }

		public int PagesPrinted { get; set; }

		private void document_PrintPage(object sender, PrintPageEventArgs e)
		{
			if (this.FitToPaper)
				e.Graphics.DrawImage(this.BmpBuffer, e.PageBounds, new Rectangle(0, 0, this.BmpBuffer.Width, this.BmpBuffer.Height), GraphicsUnit.Pixel);
			else
				e.Graphics.DrawImageUnscaled(this.BmpBuffer, e.PageBounds.Left, e.PageBounds.Top);
			e.HasMorePages = false;
		}

		private void OnPrintPage(object sender, PrintPageEventArgs e)
		{
			//if (this.FitToPaper)
			//{
			//    e.get_Graphics().DrawImage(this.BmpBuffer, e.get_PageBounds(), new Rectangle(0, 0, this.BmpBuffer.Width, this.BmpBuffer.Height), GraphicsUnit.Pixel);
			//}
			//else
			//{
			//    e.get_Graphics().DrawImageUnscaled(this.BmpBuffer, e.get_PageBounds().Left, e.get_PageBounds().Top);
			//}
			//e.set_HasMorePages(false);
		}

		public bool Print()
		{
			if (this.BmpBuffer == null)
				return false;
			this.PageCount = 0;
			this.document.Print();
			return true;
		}

		public bool ShowOptions()
		{
			var flag = false;
			var dialog = new PrintDialog
			             {
				             Document = this.document,
				             UseEXDialog = true
			             };
			switch (dialog.ShowDialog())
			{
				case DialogResult.OK:
				case DialogResult.Yes:
					flag = true;
					break;
			}
			dialog.Dispose();
			return flag;
		}
	}
}