<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_ProductPackBig.aspx.cs" Inherits="GPlus.UserControls.pop_ProductPackBig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>รายการสินค้า</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <center>
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader" align="left">
                    ระบุรายการสินค้า
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table cellpadding="0" cellspacing="0" width="800">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 130px;" align="right">
                                            รหัสสินค้า
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtProductCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px;" align="right">
                                            ชื่อสินค้า
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtProductName" runat="server" Width="190"></asp:TextBox>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="false" Width="97%"
                                    AllowSorting="false" OnRowCommand="gvItem_RowCommand" OnRowDataBound="gvItem_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSelect" runat="server" Text="เลือก" CommandName="Sel"></asp:LinkButton>
                                                <asp:HiddenField ID="hdID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="หน่วยนับ" DataField="Description" SortExpression="Description"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <uc1:PagingControl ID="PagingControl1" runat="server" />
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
