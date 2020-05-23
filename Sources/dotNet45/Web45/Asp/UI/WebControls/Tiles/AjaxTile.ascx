<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AjaxTile.ascx.cs" Inherits="Mohammad.Web.Asp.UI.WebControls.Tiles.AjaxTile" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <table style="border: 1px; height: 75px; width: 155px;" border="1">
            <tr>
                <td>

                    <table>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Label runat="server" ID="TitleLabel"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>