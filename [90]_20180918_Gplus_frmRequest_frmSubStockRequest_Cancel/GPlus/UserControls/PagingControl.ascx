<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingControl.ascx.cs" Inherits="GPlus.UserControls.PagingControl" %>
<asp:Panel ID="pnlPaging" runat="server" style="margin:2px 0 0 0; padding:5px;">
<center>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblPage" runat="server" Text="หน้าที่"></asp:Label>&nbsp;
        </td>
        <td>
            <asp:DropDownList ID="ddlCurrentPage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrentPage_SelectedIndexChanged">
                <asp:ListItem Text="1"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
           &nbsp; <asp:Label ID="lblOf" runat="server" Text="/"></asp:Label>
        </td>
        <td>
            &nbsp;<asp:Label ID="lblPageCount" runat="server" Text="1"></asp:Label>
        </td>
        <td>
            &nbsp;&nbsp;จำนวน <asp:Label ID="lblRecord" runat="server" Text="10"></asp:Label>&nbsp;
            <asp:Label ID="lblR" runat="server" Text="รายการ"></asp:Label>
        </td>
        <td>
            &nbsp;&nbsp;<asp:Label ID="lblPageSize" runat="server" Text="จำนวนรายการต่อหน้า"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Text="10"></asp:ListItem>
                <asp:ListItem Text="15"></asp:ListItem>
                <asp:ListItem Text="20"></asp:ListItem>
                <asp:ListItem Text="30"></asp:ListItem>
                <asp:ListItem Text="50"></asp:ListItem>
                <asp:ListItem Text="100"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
</center></asp:Panel>