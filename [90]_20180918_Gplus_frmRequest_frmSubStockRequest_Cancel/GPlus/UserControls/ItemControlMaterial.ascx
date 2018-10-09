<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemControlMaterial.ascx.cs"
    Inherits="GPlus.UserControls.ItemControlMaterial" %>
<asp:HiddenField ID="hdID" runat="server" />
<asp:HiddenField ID="hdMaterialID" runat="server" />
<asp:HiddenField ID="hdMaterialName" runat="server" />
<asp:HiddenField ID="hdSubMaterialID" runat="server" />
<asp:HiddenField ID="hdSubMaterialName" runat="server" />
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="txtItemCode" runat="server" MaxLength="20" Width="85" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
        <td>
            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="200" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
        </td>
        <td>
            <asp:ImageButton ID="btnSelect1" runat="server" ImageUrl="~/images/Commands/view.png"/>
        </td>
    </tr>
</table>