<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_PaymentNo.aspx.cs" Inherits="GPlus.PRPO.pop_PaymentNo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>เลขที่การจ่ายเงิน</title>
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
                    เลขที่การจ่ายเงิน
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:TextBox ID="tbDetail" runat="server" Width="90%" Height="90" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="bSave" runat="server" Text="บันทึก" OnClick="bSave_Click" />
                                &nbsp;<asp:Button ID="bCancel" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;"/>
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
