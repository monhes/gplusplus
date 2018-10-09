<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemPackSelectorControl.ascx.cs"
    Inherits="GPlus.UserControls.ItemPackSelectorControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../Script/default.css" rel="stylesheet" type="text/css" />
<script src="../Script/custom.js" type="text/javascript"></script>
<asp:HiddenField ID="hdItemID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdPackID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdStatus" runat="server" Value="0" ClientIDMode="Static" />
<table cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 130px;" align="right">รหัส</td>
        <td colspan="3" style="padding-left:2px;" align="left">
            <asp:TextBox ID="txtProductCode" runat="server" Width="80"></asp:TextBox>
        </td>
    </tr>
    <tr style="padding-top:1px;">
        <td  align="right">ชื่อวัสดุ</td>
        <td style="padding-left:2px;" align="left">
            <asp:TextBox ID="txtProductName" runat="server" Width="300"></asp:TextBox>
        </td>
        <td style="width: 60px;" align="right">หน่วย</td>
        <td style="padding-left:2px;" align="left">
            <asp:TextBox ID="txtPackage" runat="server" Width="130"></asp:TextBox>
        </td>
    </tr>
</table>