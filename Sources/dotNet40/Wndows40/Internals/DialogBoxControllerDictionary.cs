#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;

namespace Library40.Win.Internals
{
	/// <summary>
	///     Specifices the text of control to be searched for.
	/// </summary>
	[Serializable]
	internal sealed class SourceAttrinute : Attribute
	{
		#region Text
		/// <summary>
		///     Gets or sets the text of control.
		/// </summary>
		public string Text { get; set; }
		#endregion
	}

	/// <summary>
	///     The dictionary to be used when translating the dialogs.
	/// </summary>
	public class DialogBoxControllerDict
	{
		#region OpenDialog
		private string _OpenDialog = "»«“ ‰„Êœ‰";

		/// <summary>
		///     The text of Open Common Dialog
		/// </summary>
		[SourceAttrinute(Text = "Open")]
		public string OpenDialog
		{
			get { return this._OpenDialog; }
			set { this._OpenDialog = value; }
		}
		#endregion

		#region PrintDialog
		private string _PrintDialog = "ç«Å";

		/// <summary>
		///     The text of Print Common Dialog
		/// </summary>
		[SourceAttrinute(Text = "Print")]
		public string PrintDialog
		{
			get { return this._PrintDialog; }
			set { this._PrintDialog = value; }
		}
		#endregion

		#region SaveAsDialog
		private string _SaveAsDialog = "–ŒÌ—Â";

		/// <summary>
		///     The text of Save As Common Dialog
		/// </summary>
		[SourceAttrinute(Text = "Save As")]
		public string SaveAsDialog
		{
			get { return this._SaveAsDialog; }
			set { this._SaveAsDialog = value; }
		}
		#endregion

		#region BrowseForFolderDialog
		private string _BrowseForFolderDialog = "Ã” ÃÊ »—«Ì ÅÊ‘Â";

		/// <summary>
		///     The text of Browse For Folder Common Dialog
		/// </summary>
		[SourceAttrinute(Text = "Browse For Folder")]
		public string BrowseForFolderDialog
		{
			get { return this._BrowseForFolderDialog; }
			set { this._BrowseForFolderDialog = value; }
		}
		#endregion

		#region FontDialog
		private string _FontDialog = "ﬁ·„";

		/// <summary>
		///     The text of Font Common Dialog
		/// </summary>
		[SourceAttrinute(Text = "Font")]
		public string FontDialog
		{
			get { return this._FontDialog; }
			set { this._FontDialog = value; }
		}
		#endregion

		#region Ok
		private string _Ok = " «ÌÌœ";

		/// <summary>
		///     The text of OK button
		/// </summary>
		[SourceAttrinute(Text = "OK")]
		public string Ok
		{
			get { return this._Ok; }
			set { this._Ok = value; }
		}
		#endregion

		#region Handled
		private string _Cancel = "«‰’—«›";

		/// <summary>
		///     The text of Handled button
		/// </summary>
		[SourceAttrinute(Text = "Handled")]
		public string Cancel
		{
			get { return this._Cancel; }
			set { this._Cancel = value; }
		}
		#endregion

		#region Yes
		private string _Yes = "&»·Ì";

		/// <summary>
		///     The text of Yes button
		/// </summary>
		[SourceAttrinute(Text = "&Yes")]
		public string Yes
		{
			get { return this._Yes; }
			set { this._Yes = value; }
		}
		#endregion

		#region No
		private string _No = "&ŒÌ—";

		/// <summary>
		///     The text of No button
		/// </summary>
		[SourceAttrinute(Text = "&No")]
		public string No
		{
			get { return this._No; }
			set { this._No = value; }
		}
		#endregion

		#region Abort
		private string _Abort = "&·€Ê";

		/// <summary>
		///     The text of Abort button
		/// </summary>
		[SourceAttrinute(Text = "&Abort")]
		public string Abort
		{
			get { return this._Abort; }
			set { this._Abort = value; }
		}
		#endregion

		#region Retry
		private string _Retry = "&”⁄Ì „Ãœœ";

		/// <summary>
		///     The text of Retry button
		/// </summary>
		[SourceAttrinute(Text = "&Retry")]
		public string Retry
		{
			get { return this._Retry; }
			set { this._Retry = value; }
		}
		#endregion

		#region Ignore
		private string _Ignore = "&‰«œÌœÂ˛ê—› ‰";

		/// <summary>
		///     The text of Ignore button
		/// </summary>
		[SourceAttrinute(Text = "&Ignore")]
		public string Ignore
		{
			get { return this._Ignore; }
			set { this._Ignore = value; }
		}
		#endregion

		#region Continue
		private string _Continue = "&«œ«„Â";

		/// <summary>
		///     The text of Continue button
		/// </summary>
		[SourceAttrinute(Text = "&Continue")]
		public string Continue
		{
			get { return this._Continue; }
			set { this._Continue = value; }
		}
		#endregion

		#region Open
		private string _Open = "&»«“ ò—œ‰";

		/// <summary>
		///     The text of Open button
		/// </summary>
		[SourceAttrinute(Text = "&Open")]
		public string Open
		{
			get { return this._Open; }
			set { this._Open = value; }
		}
		#endregion

		#region Save
		private string _Save = "&–ŒÌ—Â ò—œ‰";

		/// <summary>
		///     The text of Save button
		/// </summary>
		[SourceAttrinute(Text = "&Save")]
		public string Save
		{
			get { return this._Save; }
			set { this._Save = value; }
		}
		#endregion

		#region MakeNewFolder
		private string _MakeNewFolder = "&«ÌÃ«œ ÅÊ‘Â ÃœÌœ";

		/// <summary>
		///     The text of Make New Folder button
		/// </summary>
		[SourceAttrinute(Text = "&Make New Folder")]
		public string MakeNewFolder
		{
			get { return this._MakeNewFolder; }
			set { this._MakeNewFolder = value; }
		}
		#endregion

		#region Strikeout
		private string _Strikeout = "Œÿ —Ê";

		/// <summary>
		///     The text of Strikeout check box
		/// </summary>
		[SourceAttrinute(Text = "Stri&keout")]
		public string Strikeout
		{
			get { return this._Strikeout; }
			set { this._Strikeout = value; }
		}
		#endregion

		#region Underline
		private string _Underline = "&Œÿ “Ì—";

		/// <summary>
		///     The text of Underline check box
		/// </summary>
		[SourceAttrinute(Text = "&Underline")]
		public string Underline
		{
			get { return this._Underline; }
			set { this._Underline = value; }
		}
		#endregion

		#region Effects
		private string _Effects = " «ÀÌ—";

		/// <summary>
		///     The text of Effects group box
		/// </summary>
		[SourceAttrinute(Text = "Effects")]
		public string Effects
		{
			get { return this._Effects; }
			set { this._Effects = value; }
		}
		#endregion

		#region Sample
		private string _Sample = "‰„Ê‰Â";

		/// <summary>
		///     The text of Sample group box
		/// </summary>
		[SourceAttrinute(Text = "Sample")]
		public string Sample
		{
			get { return this._Sample; }
			set { this._Sample = value; }
		}
		#endregion

		#region LookIn
		private string _LookIn = ":Ã” ÃÊ &œ—";

		/// <summary>
		///     The text of Look in lable
		/// </summary>
		[SourceAttrinute(Text = "Look &in:")]
		public string LookIn
		{
			get { return this._LookIn; }
			set { this._LookIn = value; }
		}
		#endregion

		#region SaveIn
		private string _SaveIn = ":–ŒÌ—Â &œ—";

		/// <summary>
		///     The text of Save in lable
		/// </summary>
		[SourceAttrinute(Text = "Save &in:")]
		public string SaveIn
		{
			get { return this._SaveIn; }
			set { this._SaveIn = value; }
		}
		#endregion

		#region FileName
		private string _FileName = ":‰«„ &›«Ì·";

		/// <summary>
		///     The text of File name label
		/// </summary>
		[SourceAttrinute(Text = "File &name:")]
		public string FileName
		{
			get { return this._FileName; }
			set { this._FileName = value; }
		}
		#endregion

		#region FilesOfType
		private string _FilesOfType = ":›«Ì·Â«Ì &‰Ê⁄";

		/// <summary>
		///     The text of Files of type label
		/// </summary>
		[SourceAttrinute(Text = "Files of &type:")]
		public string FilesOfType
		{
			get { return this._FilesOfType; }
			set { this._FilesOfType = value; }
		}
		#endregion

		#region Font
		private string _Font = "&ﬁ·„:";

		/// <summary>
		///     The text of Font label
		/// </summary>
		[SourceAttrinute(Text = "&Font:")]
		public string Font
		{
			get { return this._Font; }
			set { this._Font = value; }
		}
		#endregion

		#region FontStyle
		private string _FontStyle = "&‘ÌÊÂ ﬁ·„:";

		/// <summary>
		///     The text of Font style name label
		/// </summary>
		[SourceAttrinute(Text = "Font st&yle:")]
		public string FontStyle
		{
			get { return this._FontStyle; }
			set { this._FontStyle = value; }
		}
		#endregion

		#region Size
		private string _Size = "&«‰œ«“Â:";

		/// <summary>
		///     The text of Size label
		/// </summary>
		[SourceAttrinute(Text = "&Size:")]
		public string Size
		{
			get { return this._Size; }
			set { this._Size = value; }
		}
		#endregion

		#region Script
		private string _Script = "«”ò—Ì&Å ";

		/// <summary>
		///     The text of Script label
		/// </summary>
		[SourceAttrinute(Text = "Sc&ript:")]
		public string Script
		{
			get { return this._Script; }
			set { this._Script = value; }
		}
		#endregion

		#region SaveAsType
		private string _SaveAsType = "–ŒÌ—Â «“ &‰Ê⁄:";

		/// <summary>
		///     The text of Save as type label
		/// </summary>
		[SourceAttrinute(Text = "Save as &type:")]
		public string SaveAsType
		{
			get { return this._SaveAsType; }
			set { this._SaveAsType = value; }
		}
		#endregion

		#region OpenAsReadOnly
		private string _OpenAsReadOnly = "›ﬁÿ ŒÊ«‰œ‰Ì »«“ ò‰";

		/// <summary>
		///     The text of Open ad read-only check box
		/// </summary>
		[SourceAttrinute(Text = "Open as &read-only")]
		public string OpenAsReadOnly
		{
			get { return this._OpenAsReadOnly; }
			set { this._OpenAsReadOnly = value; }
		}
		#endregion

		#region Help
		private string _Help = "ò„ò";

		/// <summary>
		///     The text of Help button
		/// </summary>
		[SourceAttrinute(Text = "&Help")]
		public string Help
		{
			get { return this._Help; }
			set { this._Help = value; }
		}
		#endregion

		#region Printer
		private string _Printer = "ç«Åê—";

		/// <summary>
		///     The text of Printer group box
		/// </summary>
		[SourceAttrinute(Text = "Printer")]
		public string Printer
		{
			get { return this._Printer; }
			set { this._Printer = value; }
		}
		#endregion

		#region PrintRange
		private string _PrintRange = "œ«„‰Â ç«Å";

		/// <summary>
		///     The text of Print range group box
		/// </summary>
		[SourceAttrinute(Text = "Print range")]
		public string PrintRange
		{
			get { return this._PrintRange; }
			set { this._PrintRange = value; }
		}
		#endregion

		#region Copies
		private string _Copies = "òÅÌ";

		/// <summary>
		///     The text of Copies group box
		/// </summary>
		[SourceAttrinute(Text = "Copies")]
		public string Copies
		{
			get { return this._Copies; }
			set { this._Copies = value; }
		}
		#endregion

		#region Name
		private string _Name = "‰«„:";

		/// <summary>
		///     The text of Name label
		/// </summary>
		[SourceAttrinute(Text = "&Name:")]
		public string Name
		{
			get { return this._Name; }
			set { this._Name = value; }
		}
		#endregion

		#region Status
		private string _Status = "Ê÷⁄Ì :";

		/// <summary>
		///     The text of Status label
		/// </summary>
		[SourceAttrinute(Text = "Status:")]
		public string Status
		{
			get { return this._Status; }
			set { this._Status = value; }
		}
		#endregion

		internal Dictionary<string, string> GetDictionary()
		{
			var result = new Dictionary<string, string>();
			foreach (var property in this.GetType().GetProperties())
				result.Add(((SourceAttrinute)property.GetCustomAttributes(typeof (SourceAttrinute), false)[0]).Text, property.GetValue(this, null).ToString());
			return result;
		}
	}
}