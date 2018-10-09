<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_ProductReOrderPointSelect2.aspx.cs"
    Inherits="GPlus.PRPO.pop_ProductReOrderPointSelect2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ReOrder</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px; font-family:Tahoma; font-size: 12px">
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
                    <div>
                        <table>
                            <tr>
                                <td>รหัสสินค้า</td>
                                <td><asp:TextBox ID="tbItemCode" runat="server"></asp:TextBox></td>
                                <td></td>
                                <td>ชื่อสินค้า</td>
                                <td><asp:TextBox ID="tbItemName" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>วันที่ถึงจุดสั่งซื้อเพิ่ม</td>
                                <td> <uc2:CalendarControl ID="ccStartDate" runat="server" /></td>
                                <td>-</td>
                                <td><uc2:CalendarControl ID="ccEndDate" runat="server" /></td>
                                <td></td>
                                <td>ประเภทวัสดุอุปกรณ์</td>
                                <td><asp:DropDownList ID="ddlCategory" runat="server" DataTextField="Cat_Name" DataValueField="Cate_ID"></asp:DropDownList></td>
                            </tr>
                        </table>
                    </div>
                    <table cellpadding="0" cellspacing="0" width="800">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td colspan="6" align="center">
                                            <asp:Button ID="bSearch" runat="server" Text="ค้นหา" 
                                                onclick="bSearch_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="bCancel1" runat="server" Text="ยกเลิก" 
                                                onclick="bCancel1_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvProduct" runat="server" AutoGenerateColumns="false" 
                                    Width="97%" onrowdatabound="gvProduct_RowDataBound" SkinID="GvLong" Font-Bold="false">
                                    <Columns>
                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkDH" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelect" runat="server" />
                                                <asp:HiddenField ID="hfItemID" runat="server" />
                                                <asp:HiddenField ID="hfPackID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="รหัส" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" DataField="Inv_ItemCode" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" ItemStyle-HorizontalAlign="Left" DataField="Inv_ItemName" />
                                        <asp:BoundField HeaderText="รับครั้งสุดท้าย" ItemStyle-HorizontalAlign="Center" DataField="Receive_Stock_Date" />
                                        <asp:BoundField HeaderText="ราคา/หน่วย" ItemStyle-HorizontalAlign="Right" DataField="Avg_Cost" DataFormatString="{0:N2}"/>
                                        <asp:BoundField HeaderText="จำนวนในคลัง" ItemStyle-HorizontalAlign="Right" DataField="OnHand_Qty" DataFormatString="{0:N0}" />
                                        <asp:BoundField HeaderText="reorderpoint" ItemStyle-HorizontalAlign="Right" DataField="Reorder_Point" DataFormatString="{0:N0}" />
                                        <asp:BoundField HeaderText="หน่วยที่สั่งซื้อ" ItemStyle-HorizontalAlign="Center" DataField="Pack_Description" />
                                        <%--<asp:TemplateField HeaderText="หน่วยที่สั่งซื้อ" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPack" runat="server" DataTextField="Description" DataValueField="Pack_ID">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="จำนวนที่ขอซื้อ">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tbQuantity" runat="server" Width="50" 
                                                    onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" 
                                                    onpaste="return CancelKeyPaste(this)"
                                                    Style="text-align: right" MaxLength="6">
                                                </asp:TextBox>
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
                                <asp:Button ID="bOK" runat="server" Text="ตกลง" onclick="bOK_Click" />
                                <asp:Button ID="bCancel2" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;" />
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
