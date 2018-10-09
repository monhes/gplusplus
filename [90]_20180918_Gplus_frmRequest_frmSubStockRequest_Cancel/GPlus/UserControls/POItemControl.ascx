<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POItemControl.ascx.cs" Inherits="GPlus.UserControls.POItemControl" %>

<table>
    <tr>
        <td><asp:TextBox ID="txtItemNoPO" runat="server" MaxLength="10" Width="100" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnPOSearch" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
    </tr>
</table>
