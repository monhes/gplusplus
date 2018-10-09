<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_PO.aspx.cs" Inherits="GPlus.PRPO.pop_PO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>บันทึก PO</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <center>
        <table cellpadding="0" cellspacing="0" width="500">
            <tr>
                <td class="tableHeader" align="left">
                    บันทึก PO
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                เลขที่ PO
                            </td>
                            <td style="width: 350px;" align="left">
                                <asp:Label ID="lblPO" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnPrint" runat="server" Text="พิมพ์ใบ PO" />
                                <asp:Button ID="btnPrintForm" runat="server" Text="พิมพ์ใบอนุมัติสั่งพิมพ์" SkinID="ButtonMiddleLong"/>
                                <asp:Button ID="btnPrintPR" runat="server" Text="พิมพ์ใบแนบรายการ PR" SkinID="ButtonMiddleLong"/>
                                <asp:Button ID="btnPrintAttach" runat="server" Text="ใบอนุมัติค่าใช้จ่ายสต๊อก" SkinID="ButtonMiddleLong"/>
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
