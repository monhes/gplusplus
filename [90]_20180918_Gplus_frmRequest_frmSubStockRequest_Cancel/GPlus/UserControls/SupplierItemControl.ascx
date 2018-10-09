<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierItemControl.ascx.cs" Inherits="GPlus.UserControls.SupplierItemControl" %>

<table>
    <tr>
        <td><asp:TextBox ID="txtItemSupplierName" runat="server" MaxLength="10" Width="100" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnSupplierSearch" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
    </tr>
</table>
