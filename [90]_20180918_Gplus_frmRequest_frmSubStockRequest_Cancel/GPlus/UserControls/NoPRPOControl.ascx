<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoPRPOControl.ascx.cs" Inherits="GPlus.UserControls.NoPRPOControl" %>

<table>
    <tr>
        <td style="width: 130px;" align="right">เลขที่ขอซื้อ/จ้าง</td>
        <td><asp:TextBox ID="txtNoPRPO" runat="server" MaxLength="10" Width="100" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox></td>
        <td><asp:ImageButton ID="btnNoPRPO" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
    </tr>
</table>