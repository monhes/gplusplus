<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ItemControl.ascx.cs"
    Inherits="GPlus.UserControls.ItemControl" %>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 130px;" align="right">
         รายการวัสดุอุปกรณ์
          </td>
        <td style="width: 100px;" align="right">
            รหัสสินค้า&nbsp;&nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtItemCode" runat="server" Width="125" BackColor="WhiteSmoke" onkeypress="return false;" ></asp:TextBox>
        </td>
        <td style="width: 130px;" align="right">
            ชื่อสินค้า&nbsp;&nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtItemName" runat="server" Width="200" BackColor="WhiteSmoke" onkeypress="return false;" ></asp:TextBox>
        </td>
        <td>
            <asp:ImageButton ID="btnSelect1" runat="server" ImageUrl="~/images/Commands/view.png"/>
        </td>
        <td><asp:HiddenField ID="hdID" runat="server" /></td>
    </tr>
</table>