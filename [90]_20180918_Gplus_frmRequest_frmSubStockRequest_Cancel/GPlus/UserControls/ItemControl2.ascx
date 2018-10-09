<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ItemControl2.ascx.cs" Inherits="GPlus.UserControls.ItemControl2" %>
<asp:HiddenField ID="hdID" runat="server" />
<asp:HiddenField ID="hdUnitID" runat="server" />
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:TextBox ID="txtItemCode" runat="server" Width="85" BackColor="WhiteSmoke" 
                onkeypress="return false;" AutoPostBack="True"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
        <td>
            <asp:TextBox ID="txtItemName" runat="server"  Width="200" 
                BackColor="WhiteSmoke" onkeypress="return false;" AutoPostBack="True"></asp:TextBox>
        </td>
        <td>&nbsp;</td>
         <td>
            <asp:TextBox ID="txtPackName" runat="server"  BackColor="WhiteSmoke" 
                 onkeypress="return false;" AutoPostBack="True"></asp:TextBox>
        </td>
        <td>
            <asp:ImageButton ID="btnSelect1" runat="server" ImageUrl="~/images/Commands/view.png"/>
        </td>
    </tr>
</table>