<%@ Control Language="C#" ClassName="DisplayModeMenuCS" %>
<script runat="server">

        // Use a field to reference the current WebPartManager control.
    private WebPartManager _manager;

    [__ReSharperSynthetic]
    private void __ReSharper_Render(object expression) { }

    [__ReSharperSynthetic]
    private void __ReSharper_Data_Bind__Conversion<T>(T _, T expression) { }

    [__ReSharperSynthetic]
    private void __ReSharper_Data_Bind__Conversion<T>(T _, object expression) { }

    [__ReSharperSynthetic]
    private void __ReSharper_Render(object expression) { }

    [__ReSharperSynthetic]
    private void __ReSharper_Data_Bind__Conversion<T>(T _, T expression) { }

    [__ReSharperSynthetic]
    private void __ReSharper_Data_Bind__Conversion<T>(T _, object expression) { }

    private void Page_Init(object sender, EventArgs e) { this.Page.InitComplete += this.InitComplete; }

    private void InitComplete(object sender, EventArgs e)
    {
        this._manager = WebPartManager.GetCurrentWebPartManager(this.Page);

        var browseModeName = WebPartManager.BrowseDisplayMode.Name;

        // Fill the drop-down step with the names of supported display modes.
        foreach (WebPartDisplayMode mode in
            this._manager.SupportedDisplayModes)
        {
            var modeName = mode.Name;
            // Make sure a mode is enabled before adding it.
            if (mode.IsEnabled(this._manager))
            {
                var item = new ListItem(modeName, modeName);
                this.DisplayModeDropdown.Items.Add(item);
            }
        }

        // If Shared scope is allowed for this user, display the 
        // scope-switching UI and select the appropriate radio 
        // button for the current user scope.
        if (this._manager.Personalization.CanEnterSharedScope)
        {
            this.Panel2.Visible = true;
            if (this._manager.Personalization.Scope == PersonalizationScope.User)
                this.RadioButton1.Checked = true;
            else
                this.RadioButton2.Checked = true;
        }
    }

        // Change the page to the selected display mode.
    private void DisplayModeDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedMode = this.DisplayModeDropdown.SelectedValue;

        var mode = this._manager.SupportedDisplayModes[selectedMode];
        if (mode != null)
            this._manager.DisplayMode = mode;
    }

        // Set the selected item equal to the current display mode.
    private void Page_PreRender(object sender, EventArgs e)
    {
        var items = this.DisplayModeDropdown.Items;
        var selectedIndex = items.IndexOf(items.FindByText(this._manager.DisplayMode.Name));
        this.DisplayModeDropdown.SelectedIndex = selectedIndex;
    }

        // Reset all of a user's personalization data for the page.
    protected void LinkButton1_Click(object sender, EventArgs e) { this._manager.Personalization.ResetPersonalizationState(); }
        // If not in User personalization scope, toggle into it.
    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        if (this._manager.Personalization.Scope == PersonalizationScope.Shared)
            this._manager.Personalization.ToggleScope();
    }

        // If not in Shared scope, and if user has permission, toggle 
        // the scope.
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        if (this._manager.Personalization.CanEnterSharedScope && this._manager.Personalization.Scope == PersonalizationScope.User)
            this._manager.Personalization.ToggleScope();
    }

</script>
<div>
    <asp:Panel ID="Panel1" runat="server" BorderWidth="1" Width="230" BackColor="lightgray"
               Font-Names="Verdana, Arial, Sans Serif">
        <asp:Label ID="Label1" runat="server" Text="&nbsp;Display Mode" Font-Bold="true"
                   Font-Size="8" Width="120"/>
        <asp:DropDownList ID="DisplayModeDropdown" runat="server" AutoPostBack="true" Width="120"
                          OnSelectedIndexChanged="DisplayModeDropdown_SelectedIndexChanged"/>
        <asp:LinkButton ID="LinkButton1" runat="server" Text="Reset User State" ToolTip="Reset the current user's personalization data for 
      the page." Font-Size="8" OnClick="LinkButton1_Click"/>
        <asp:Panel ID="Panel2" runat="server" GroupingText="Personalization Scope" Font-Bold="true"
                   Font-Size="8" Visible="false">
            <asp:RadioButton ID="RadioButton1" runat="server" Text="User" AutoPostBack="true"
                             GroupName="Scope" OnCheckedChanged="RadioButton1_CheckedChanged"/>
            <asp:RadioButton ID="RadioButton2" runat="server" Text="Shared" AutoPostBack="true"
                             GroupName="Scope" OnCheckedChanged="RadioButton2_CheckedChanged"/>
        </asp:Panel>
    </asp:Panel>
</div>