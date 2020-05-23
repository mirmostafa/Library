using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mohammad.Web.Asp.UI.WebControls.Tiles
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:AjaxTile runat=server></{0}:AjaxTile>")]
    public partial class AjaxTile : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title { get { return this.TitleLabel.Text; } set { this.TitleLabel.Text = value; } }

        protected override void RenderContents(HtmlTextWriter output) { output.Write(this.Title); }
        protected void Page_Load(object sender, EventArgs e) { }
    }
}