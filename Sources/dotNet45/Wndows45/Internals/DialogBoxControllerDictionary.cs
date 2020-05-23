using System;
using System.Collections.Generic;

namespace Mohammad.Win.Internals
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
        internal Dictionary<string, string> GetDictionary()
        {
            var result = new Dictionary<string, string>();
            foreach (var property in this.GetType().GetProperties())
                result.Add(((SourceAttrinute) property.GetCustomAttributes(typeof(SourceAttrinute), false)[0]).Text, property.GetValue(this, null).ToString());
            return result;
        }

        #region OpenDialog

        /// <summary>
        ///     The text of Open Common Dialog
        /// </summary>
        [SourceAttrinute(Text = "Open")]
        public string OpenDialog { get; set; } = "»«“ ‰„Êœ‰";

        #endregion

        #region PrintDialog

        /// <summary>
        ///     The text of Print Common Dialog
        /// </summary>
        [SourceAttrinute(Text = "Print")]
        public string PrintDialog { get; set; } = "ç«Å";

        #endregion

        #region SaveAsDialog

        /// <summary>
        ///     The text of Save As Common Dialog
        /// </summary>
        [SourceAttrinute(Text = "Save As")]
        public string SaveAsDialog { get; set; } = "–ŒÌ—Â";

        #endregion

        #region BrowseForFolderDialog

        /// <summary>
        ///     The text of Browse For Folder Common Dialog
        /// </summary>
        [SourceAttrinute(Text = "Browse For Folder")]
        public string BrowseForFolderDialog { get; set; } = "Ã” ÃÊ »—«Ì ÅÊ‘Â";

        #endregion

        #region FontDialog

        /// <summary>
        ///     The text of Font Common Dialog
        /// </summary>
        [SourceAttrinute(Text = "Font")]
        public string FontDialog { get; set; } = "ﬁ·„";

        #endregion

        #region Ok

        /// <summary>
        ///     The text of OK button
        /// </summary>
        [SourceAttrinute(Text = "OK")]
        public string Ok { get; set; } = " «ÌÌœ";

        #endregion

        #region Handled

        /// <summary>
        ///     The text of Handled button
        /// </summary>
        [SourceAttrinute(Text = "Handled")]
        public string Cancel { get; set; } = "«‰’—«›";

        #endregion

        #region Yes

        /// <summary>
        ///     The text of Yes button
        /// </summary>
        [SourceAttrinute(Text = "&Yes")]
        public string Yes { get; set; } = "&»·Ì";

        #endregion

        #region No

        /// <summary>
        ///     The text of No button
        /// </summary>
        [SourceAttrinute(Text = "&No")]
        public string No { get; set; } = "&ŒÌ—";

        #endregion

        #region Abort

        /// <summary>
        ///     The text of Abort button
        /// </summary>
        [SourceAttrinute(Text = "&Abort")]
        public string Abort { get; set; } = "&·€Ê";

        #endregion

        #region Retry

        /// <summary>
        ///     The text of Retry button
        /// </summary>
        [SourceAttrinute(Text = "&Retry")]
        public string Retry { get; set; } = "&”⁄Ì „Ãœœ";

        #endregion

        #region Ignore

        /// <summary>
        ///     The text of Ignore button
        /// </summary>
        [SourceAttrinute(Text = "&Ignore")]
        public string Ignore { get; set; } = "&‰«œÌœÂ˛ê—› ‰";

        #endregion

        #region Continue

        /// <summary>
        ///     The text of Continue button
        /// </summary>
        [SourceAttrinute(Text = "&Continue")]
        public string Continue { get; set; } = "&«œ«„Â";

        #endregion

        #region Open

        /// <summary>
        ///     The text of Open button
        /// </summary>
        [SourceAttrinute(Text = "&Open")]
        public string Open { get; set; } = "&»«“ ò—œ‰";

        #endregion

        #region Save

        /// <summary>
        ///     The text of Save button
        /// </summary>
        [SourceAttrinute(Text = "&Save")]
        public string Save { get; set; } = "&–ŒÌ—Â ò—œ‰";

        #endregion

        #region MakeNewFolder

        /// <summary>
        ///     The text of Make New Folder button
        /// </summary>
        [SourceAttrinute(Text = "&Make New Folder")]
        public string MakeNewFolder { get; set; } = "&«ÌÃ«œ ÅÊ‘Â ÃœÌœ";

        #endregion

        #region Strikeout

        /// <summary>
        ///     The text of Strikeout check box
        /// </summary>
        [SourceAttrinute(Text = "Stri&keout")]
        public string Strikeout { get; set; } = "Œÿ —Ê";

        #endregion

        #region Underline

        /// <summary>
        ///     The text of Underline check box
        /// </summary>
        [SourceAttrinute(Text = "&Underline")]
        public string Underline { get; set; } = "&Œÿ “Ì—";

        #endregion

        #region Effects

        /// <summary>
        ///     The text of Effects group box
        /// </summary>
        [SourceAttrinute(Text = "Effects")]
        public string Effects { get; set; } = " «ÀÌ—";

        #endregion

        #region Sample

        /// <summary>
        ///     The text of Sample group box
        /// </summary>
        [SourceAttrinute(Text = "Sample")]
        public string Sample { get; set; } = "‰„Ê‰Â";

        #endregion

        #region LookIn

        /// <summary>
        ///     The text of Look in lable
        /// </summary>
        [SourceAttrinute(Text = "Look &in:")]
        public string LookIn { get; set; } = ":Ã” ÃÊ &œ—";

        #endregion

        #region SaveIn

        /// <summary>
        ///     The text of Save in lable
        /// </summary>
        [SourceAttrinute(Text = "Save &in:")]
        public string SaveIn { get; set; } = ":–ŒÌ—Â &œ—";

        #endregion

        #region FileName

        /// <summary>
        ///     The text of File name label
        /// </summary>
        [SourceAttrinute(Text = "File &name:")]
        public string FileName { get; set; } = ":‰«„ &›«Ì·";

        #endregion

        #region FilesOfType

        /// <summary>
        ///     The text of Files of type label
        /// </summary>
        [SourceAttrinute(Text = "Files of &type:")]
        public string FilesOfType { get; set; } = ":›«Ì·Â«Ì &‰Ê⁄";

        #endregion

        #region Font

        /// <summary>
        ///     The text of Font label
        /// </summary>
        [SourceAttrinute(Text = "&Font:")]
        public string Font { get; set; } = "&ﬁ·„:";

        #endregion

        #region FontStyle

        /// <summary>
        ///     The text of Font style name label
        /// </summary>
        [SourceAttrinute(Text = "Font st&yle:")]
        public string FontStyle { get; set; } = "&‘ÌÊÂ ﬁ·„:";

        #endregion

        #region Size

        /// <summary>
        ///     The text of Size label
        /// </summary>
        [SourceAttrinute(Text = "&Size:")]
        public string Size { get; set; } = "&«‰œ«“Â:";

        #endregion

        #region Script

        /// <summary>
        ///     The text of Script label
        /// </summary>
        [SourceAttrinute(Text = "Sc&ript:")]
        public string Script { get; set; } = "«”ò—Ì&Å ";

        #endregion

        #region SaveAsType

        /// <summary>
        ///     The text of Save as type label
        /// </summary>
        [SourceAttrinute(Text = "Save as &type:")]
        public string SaveAsType { get; set; } = "–ŒÌ—Â «“ &‰Ê⁄:";

        #endregion

        #region OpenAsReadOnly

        /// <summary>
        ///     The text of Open ad read-only check box
        /// </summary>
        [SourceAttrinute(Text = "Open as &read-only")]
        public string OpenAsReadOnly { get; set; } = "›ﬁÿ ŒÊ«‰œ‰Ì »«“ ò‰";

        #endregion

        #region Help

        /// <summary>
        ///     The text of Help button
        /// </summary>
        [SourceAttrinute(Text = "&Help")]
        public string Help { get; set; } = "ò„ò";

        #endregion

        #region Printer

        /// <summary>
        ///     The text of Printer group box
        /// </summary>
        [SourceAttrinute(Text = "Printer")]
        public string Printer { get; set; } = "ç«Åê—";

        #endregion

        #region PrintRange

        /// <summary>
        ///     The text of Print range group box
        /// </summary>
        [SourceAttrinute(Text = "Print range")]
        public string PrintRange { get; set; } = "œ«„‰Â ç«Å";

        #endregion

        #region Copies

        /// <summary>
        ///     The text of Copies group box
        /// </summary>
        [SourceAttrinute(Text = "Copies")]
        public string Copies { get; set; } = "òÅÌ";

        #endregion

        #region Name

        /// <summary>
        ///     The text of Name label
        /// </summary>
        [SourceAttrinute(Text = "&Name:")]
        public string Name { get; set; } = "‰«„:";

        #endregion

        #region Status

        /// <summary>
        ///     The text of Status label
        /// </summary>
        [SourceAttrinute(Text = "Status:")]
        public string Status { get; set; } = "Ê÷⁄Ì :";

        #endregion
    }
}