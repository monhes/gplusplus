<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemOrgStructControl.ascx.cs" Inherits="GPlus.UserControls.ItemOrgStructControl" %>

<style type="text/css">
    .style1
    {
        width: 208px;
    }
</style>

<table border="0">
    <tr>
        
        <td style="width: 130px" align="right">ชื่อฝ่าย&nbsp;&nbsp;</td>
        <td class="style1"><asp:TextBox ID="txtItemDivName" runat="server" MaxLength="100" 
                Width="241px" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td>&nbsp;&nbsp;ชื่อทีม&nbsp;&nbsp;</td>
        <td><asp:TextBox ID="TxtItemDepName" runat="server" MaxLength="100" Width="214px" 
                BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnDep" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
        
    </tr>
    <tr>
    <td><asp:HiddenField runat="server" ID="hdOrgId" /></td>
    <td><asp:HiddenField runat="server" ID="HdDiv" /></td>
    <td><asp:HiddenField runat="server" ID="HdDep" /></td>
    </tr>
</table>