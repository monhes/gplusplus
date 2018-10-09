<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_ProductReOrderPointSelect.aspx.cs"
    Inherits="GPlus.PRPO.pop_ProductReOrderPointSelect" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReOrder</title>
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
                                            รหัสคลังสินค้า
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStockCode" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;" align="right">
                                            คลังสินค้า
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStockName" runat="server" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px;" align="right">
                                            รหัสสินค้า
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" runat="server" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;" align="right">
                                            ชื่อสินค้า
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                  <%--  <tr>
                                        <td style="width: 130px;" align="right">
                                            Supplier
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" runat="server" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;" align="right">
                                            ชื่อ Supplier
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierName" runat="server" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" 
                                                onclick="btnSearch_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                                                onclick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="false" 
                                    Width="97%" onrowdatabound="gvProduct_RowDataBound">
                                    <Columns>
                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkDH" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkD" runat="server" />
                                                <asp:HiddenField ID="hdID" runat="server" />
                                                <asp:HiddenField ID="hdUnitID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="หน่วย" DataField="Description" SortExpression="Description"
                                                ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="รับครั้งสุดท้าย" />
                                      <%--  <asp:BoundField HeaderText="จำนวน" />
                                        <asp:BoundField HeaderText="หน่วย" />--%>
                                        <asp:BoundField HeaderText="ราคาต่อหน่วย" DataField="Avg_Cost" SortExpression="Avg_Cost" />
                                        <asp:BoundField HeaderText="จำนวนในคลัง" DataField="OnHand_Qty" SortExpression="OnHand_Qty" />
                                        <asp:BoundField HeaderText="reorderpoint" />
                                        <asp:BoundField HeaderText="หน่วยที่ขอซื้อ" DataField="Description" SortExpression="Description"
                                                ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="หน่วยที่สั่งซื้อ" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPack" runat="server" DataTextField="Description" DataValueField="Pack_ID">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จำนวนที่ขอซื้อ">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                                    Style="text-align: right" MaxLength="6"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
</body>
</html>
