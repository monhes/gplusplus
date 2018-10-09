<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_PR.aspx.cs" Inherits="GPlus.PRPO.pop_PR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>บันทึก PR</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <center>
        <table cellpadding="0" cellspacing="0" width="450">
            <tr>
                <td class="tableHeader" align="left">
                    บันทึก PR
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td style="width: 100px;" align="right">
                                เลขที่ PR
                            </td>
                            <td align="left" style="width: 350px;">
                                <asp:Label ID="lblPR" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnPrint" runat="server" Text="พิมพ์ใบ PR" />
                                <asp:Button ID="btnPrint3" runat="server" Text="พิมพ์ใบขอสั่งพิมพ์" SkinID="ButtonMiddleLong" Visible="false" />
                                <asp:Button ID="btnPrint2" runat="server" Text="พิมพ์ใบเอกสารแนบบัญชีค่าใช้จ่าย" SkinID="ButtonMiddleLongLong" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </center>
    </form>
</body>
</html>
