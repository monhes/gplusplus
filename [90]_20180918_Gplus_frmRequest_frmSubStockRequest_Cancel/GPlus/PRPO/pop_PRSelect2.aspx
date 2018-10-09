<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="pop_PRSelect2.aspx.cs" Inherits="GPlus.PRPO.pop_PRSelect2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>เลือก PR เพื่อสร้าง PO</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px; font-family: Tahoma; font-size: 12px">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <center>
                <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader" align="left">
                    เลือก PR เพื่อสร้าง PO</td>
            </tr>
            <tr>
                <td class="tableBody">
                    <div>
                        <table>
                            <tr>
                                <td>วันที่สร้าง PR</td>
                                <td><uc2:CalendarControl ID="ccFrom" runat="server" /></td>
                                <td>ถึงวันที่</td>
                                <td><uc2:CalendarControl ID="ccTo" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>Supplier</td>
                                <td><asp:DropDownList ID="ddlSupplier" runat="server" Width="200"></asp:DropDownList></td>
                                <td><%--<asp:CheckBox ID="chkApprove" runat="server" Checked="true" Text="PR รอออกใบสั่งซื้อ" />--%></td>
                                <td>&nbsp;&nbsp;เลขที่ PR</td>
                                <td><asp:TextBox runat="server" Width="100" ID="tbPRCode"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td><asp:Button ID="btnSearch" runat="server" Text="ค้นหา" 
                                                onclick="btnSearch_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                                                onclick="btnCancel_Click" /></td>
                            </tr>
                        </table>
                    </div>
                    <table cellpadding="0" cellspacing="0" width="800">
                        <tr>
                            <td>
                                <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="false" Width="97%" 
                                    AllowSorting="false" onrowdatabound="gvPR_RowDataBound" SkinID="GvLong">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkDH" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkD" runat="server" />
                                                <asp:HiddenField ID="hdID" runat="server" />
                                                <asp:HiddenField ID="hdItemID" runat="server" />
                                                <asp:HiddenField ID="hdInvItemID" runat="server" />
                                                <asp:HiddenField ID="hdPackID" runat="server" />
                                                <asp:HiddenField ID="hdUnitPrice" runat="server" />
                                                <asp:HiddenField ID="hfPrintFormID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="เลขที่ PR" DataField="PR_Code" SortExpression="PR_Code"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90" />
                                        <asp:BoundField HeaderText="รหัสสินค้า" ItemStyle-Width="80" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="จำนวน" ItemStyle-Width="50" DataField="Unit_Quantity" SortExpression="Unit_Quantity" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="หน่วย" ItemStyle-Width="70" DataField="Description" SortExpression="Description"
                                                ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="วันที่สร้าง PR" DataField="Create_Date" SortExpression="Create_Date"/>
                                        <asp:BoundField HeaderText="ผู้สร้าง PR" DataField="Create_By" SortExpression="Create_By"/>
                                        <asp:BoundField HeaderText="ทีม/ฝ่าย" ItemStyle-HorizontalAlign="Left" DataField="Dep_Name" SortExpression="Dep_Name"/>
                                        <%--<asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status" />--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <uc1:PagingControl ID="PagingControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnOK" runat="server" Text="ตกลง" onclick="btnOK_Click" />
                                <asp:Button ID="btnCancel1" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;" />
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
    <p>
&nbsp;</p>
</body>
</html>
