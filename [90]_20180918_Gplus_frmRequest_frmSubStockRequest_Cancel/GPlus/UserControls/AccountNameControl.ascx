<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountNameControl.ascx.cs" Inherits="GPlus.UserControls.AccountNameControl" %>
<table>
    <tr>
        <td>
            <asp:TextBox runat="server" ID="txtAccName" Width="180px" 
                    onkeyup="return false;" 
                    onkeydown="return false;"
                    onkeypress="return false;"></asp:TextBox>
        </td>
        <td>
            <asp:ImageButton ID="btnAccSearch" runat="server" ImageUrl="~/images/Commands/view.png"/>
            <asp:HiddenField ID="HDAccId" runat="server"/>
            <%--<asp:TextBox ID="tb" runat="server"></asp:TextBox>--%>
        </td>
    </tr>
</table>