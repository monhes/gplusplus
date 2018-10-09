<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDivDepControl.ascx.cs" Inherits="GPlus.UserControls.ItemDivDepControl" %>

<table border="0">
    <tr>
        
        <td style="width: 130px" align="right"><asp:Label ID="lblDiv" runat="server"></asp:Label></td>
        <td><asp:TextBox ID="txtItemDivName" runat="server" MaxLength="10" Width="150" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:Label ID="lblDep" runat="server"></asp:Label></td>
        <td><asp:TextBox ID="TxtItemDepName" runat="server" MaxLength="10" Width="150" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnDep" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
    </tr>
</table>