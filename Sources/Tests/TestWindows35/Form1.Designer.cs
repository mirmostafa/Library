using Library35.Windows.Controls;

namespace TestWindows35
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.connectionStringBox1 = new ConnectionStringBox();
			this.SuspendLayout();
			// 
			// connectionStringBox1
			// 
			this.connectionStringBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.connectionStringBox1.ConnectionString = null;
			this.connectionStringBox1.DialogPrompt = "This is a test";
			this.connectionStringBox1.DialogText = "Test";
			this.connectionStringBox1.Location = new System.Drawing.Point(12, 12);
			this.connectionStringBox1.MaximumSize = new System.Drawing.Size(10000000, 21);
			this.connectionStringBox1.MinimumSize = new System.Drawing.Size(0, 21);
			this.connectionStringBox1.Name = "connectionStringBox1";
			this.connectionStringBox1.Size = new System.Drawing.Size(602, 21);
			this.connectionStringBox1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(626, 266);
			this.Controls.Add(this.connectionStringBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private ConnectionStringBox connectionStringBox1;

	}
}

