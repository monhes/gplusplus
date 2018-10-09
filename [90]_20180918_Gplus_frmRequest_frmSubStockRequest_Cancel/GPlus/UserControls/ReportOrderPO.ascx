<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportOrderPO.ascx.cs" Inherits="GPlus.UserControls.ReportOrderPO" %>
<style type="text/css">
    .style1
    {
        width: 208px;
    }
</style>

<table border="0">
    <tr>
       <td style="width: 76px;" align="right">
       Supplier&nbsp;&nbsp;</td>
        <td><asp:TextBox ID="TxtSupplier" runat="server" MaxLength="100" Width="214px" 
                BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnDep" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
        
    </tr>
    <tr>
    <td><asp:HiddenField runat="server" ID="hdSupplierId" /></td>
    <td><asp:HiddenField runat="server" ID="HdDiv" /></td>
    
    </tr>
</table>