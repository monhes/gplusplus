<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemControlSupplier.ascx.cs" Inherits="GPlus.UserControls.ItemControlSupplier" %>
<style type="text/css">
    .style1
    {
        width: 208px;
    }
</style>

<table border="0">
    <tr>
       <td style="width:110px;" align="right">
       Supplier&nbsp;&nbsp;</td>
        <td><asp:TextBox ID="TxtSupplier" runat="server" MaxLength="100" Width="240px" 
                BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnDep" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
        
    </tr>
    <tr>
     <td><asp:HiddenField ID="hdID" runat="server" /></td>
    
    </tr>
</table>