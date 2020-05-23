#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Library35.Helpers;
using Library35.Helpers.Win;

namespace Library35.Windows.Settings
{
	[Serializable]
	public class FormSettings<TForm> : SettingsItemBase
		where TForm : Form
	{
		private bool _ApplyListViews = true;
		private Collection<ListViewSettings> _ListViewsSettings;

		public Collection<ListViewSettings> ListViewsSettings
		{
			get { return this._ListViewsSettings ?? (this._ListViewsSettings = new Collection<ListViewSettings>()); }
			set { this._ListViewsSettings = value; }
		}

		public bool ApplyListViews
		{
			get { return this._ApplyListViews; }
			set { this._ApplyListViews = value; }
		}

		[XmlIgnore]
		public TForm Form { get; private set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool Initiated { get; set; }

		public int Left { get; set; }

		public int Top { get; set; }

		public int Height { get; set; }

		public int Width { get; set; }

		public FormWindowState WindowState { get; set; }

		public void LoadListViewsSettings()
		{
			var listViews = this.Form.GetControls<ListView>();
			foreach (var setting in this.ListViewsSettings)
			{
				var listView = listViews.Where(lv => lv.Name.Equals(setting.ListViewName)).FirstOrDefault();
				if (listView == null)
					continue;
				setting.Load(listView);
			}
		}

		public void SaveListViewsSettings()
		{
			this.ListViewsSettings.Clear();
			foreach (var listView in this.Form.GetControls<ListView>())
			{
				var listViewSettings = new ListViewSettings();
				listViewSettings.Save(listView);
				this.ListViewsSettings.Add(listViewSettings);
			}
		}

		public event EventHandler<ApplySettingsEventArgs<TForm>> ApplyingToForm;
		public event EventHandler<ApplySettingsEventArgs<TForm>> ApplyedToForm;
		public event EventHandler<ApplySettingsEventArgs<TForm>> ApplyingToSettings;
		public event EventHandler<ApplySettingsEventArgs<TForm>> ApplyedToSettings;
		public event EventHandler FormSet;

		public void SetForm(TForm form)
		{
			this.Form = form;
			this.Form.Shown += this.Form_Shown;
			this.Form.FormClosed += this.Form_FormClosed;
			this.FormSet.Raise(this);
		}

		public virtual void ApplyToForm(TForm form)
		{
			if (!this.Initiated)
				return;
			this.ApplyingToForm.Raise(this, new ApplySettingsEventArgs<TForm>(form));
			form.WindowState = this.WindowState;
			if (this.WindowState == FormWindowState.Maximized)
				return;

			form.Left = this.Left;
			form.Top = this.Top;
			form.Width = this.Width;
			form.Height = this.Height;

			if (this.ApplyListViews)
				this.LoadListViewsSettings();

			this.ApplyedToForm.Raise(this, new ApplySettingsEventArgs<TForm>(form));
		}

		public virtual void ApplyToSetting(TForm form)
		{
			this.Initiated = true;
			this.ApplyingToSettings.Raise(this, new ApplySettingsEventArgs<TForm>(form));
			this.WindowState = form.WindowState;
			if (form.WindowState == FormWindowState.Maximized)
				return;
			this.Left = form.Left;
			this.Top = form.Top;
			this.Width = form.Width;
			this.Height = form.Height;
			ToolStripManager.SaveSettings(form);

			if (this.ApplyListViews)
				this.SaveListViewsSettings();

			this.ApplyedToSettings.Raise(this, new ApplySettingsEventArgs<TForm>(form));
		}

		private void Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.ApplyToSetting((TForm)sender);
		}

		private void Form_Shown(object sender, EventArgs e)
		{
			this.ApplyToForm((TForm)sender);
		}
	}
}