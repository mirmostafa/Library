#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.ComponentModel;

namespace Library35.Windows.Controls
{
	/// <summary>
	///     SetProperty the behavior of common dialog boxes such as MessageBox, FontDialog and so on.
	/// </summary>
	[ToolboxItem(true)]
	[Category("Company")]
	public partial class DialogBoxController : Component, IDisposable
	{
		private bool disposed;

		/// <summary>
		///     Constructs a new instance.
		/// </summary>
		public DialogBoxController()
		{
			this.InitializeComponent();
		}

		/// <summary>
		///     Constructs a new instance depending on the given container
		/// </summary>
		/// <param name="container">Container</param>
		public DialogBoxController(IContainer container)
		{
			if (container != null)
				container.Add(this);

			this.InitializeComponent();
		}

		/// <summary>
		///     Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && (this.components != null))
					this.components.Dispose();

				if (!this.disposed)
					if (disposing)
					{
						this.Enable = false;
						this.Font.Dispose();
					}
			}
			finally
			{
				base.Dispose(disposing);
				this.disposed = true;
			}
		}
	}
}