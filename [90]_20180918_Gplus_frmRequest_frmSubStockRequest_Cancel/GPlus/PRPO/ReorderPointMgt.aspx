<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ReorderPointMgt.aspx.cs"
    Inherits="GPlus.PRPO.ReorderPointMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 202px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหา Reorder Point และจุดสั่งซื้อ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสคลัง
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="ddlSearchStock" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <fieldset>
                                <legend>กำหนดจุด Reorder Point</legend>
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="กำหนดแล้ว" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="ยังไม่กำหนด" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ทั้งหมด" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รายการสินค้า
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtSearchProductName" runat="server" MaxLength="150"></asp:TextBox>
                        </td>
                        <td style="width: 60px;" align="right">
                            หน่วย
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchUnitName" runat="server" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสสินค้า
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtSearchProductCode" runat="server" MaxLength="150"></asp:TextBox>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvReorderPoint" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="100%"
                    OnRowCommand="gvReorderPoint_RowCommand" OnRowDataBound="gvReorderPoint_RowDataBound"
                    OnSorting="gvReorderPoint_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                                <asp:HiddenField ID="hdIStockID" runat="server" />
                                <asp:HiddenField ID="hdIItemID" runat="server" />
                                <asp:HiddenField ID="hdIPackID" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="รายการสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName" />
                        <asp:BoundField HeaderText="หน่วย" DataField="Description" SortExpression="Description"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Reorder Point" DataField="Reorder_Point" SortExpression="Reorder_Point"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="จำนวนสูงสุดของ Stock" DataField="Maximum_Qty" SortExpression="Maximum_Qty"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="หน่วยที่สั่งซื้อ" DataField="Pack_Name_Purchase" SortExpression="Pack_Name_Purchase"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="หน่วยที่เบิกใช้" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="ImgBaseUnitFlag" runat="server" ImageUrl="~/images/Commands/dialog-accept.png" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" class="tableBody">
                <uc1:PagingControl ID="PagingControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <asp:HiddenField ID="hdStockID" runat="server" />
        <asp:HiddenField ID="hdItemID" runat="server" />
        <asp:HiddenField ID="hdPackID" runat="server" />
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    ข้อมูล Reorder Point และจุดสั่งซื้อ
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                รหัสสินค้า
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductCode" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                รายการสินค้า
                            </td>
                            <td>
                                <asp:TextBox ID="txtProductName" runat="server" Enabled="false"  Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                หน่วย
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPack" runat="server">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="ChkBaseUnit" runat="server" Text="  หน่วยที่ใช้เบิก" Enabled ="false" />
                            </td>
                            <td style="width: 130px;" align="right">
                                ReOrder Point
                            </td>
                            <td>
                                <asp:TextBox ID="txtReorderPoint" runat="server" MaxLength="10" Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                จำนวนสูงสุดของ Stock
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtMaxStock" runat="server" MaxLength="10" Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right" valign="top">
                                ReOrder Point<br />
                                (คำนวณจากสูตร)
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtCalReorderPoint" runat="server" MaxLength="10" Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                           <td style="width: 130px;" align="right">
                                รายละเอียดการจัดซื้อ
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDetail" runat="server" Width="250" Height="60" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                หน่วยที่สั่งซื้อหรือหน่วยคลังย่อยเบิก
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUnitPurchase" runat="server">
                                </asp:DropDownList>
                                <asp:CheckBox ID="chkPack" runat="server" Text="แตก Pack รับสินค้า" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="false"
                                    OnClick="btnCancel_Click" />&nbsp;
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
    </asp:Panel>
    <asp:LinkButton ID="btnTempSave" runat="server" Text="" style="display:none;"></asp:LinkButton>
    <asp:ModalPopupExtender ID="mpeConfirmSave" runat="server" TargetControlID="btnTempSave"
        PopupControlID="pnlTempSaveConfirm" BackgroundCssClass="modalBackground"
        CancelControlID="btnConfirmCancel" DropShadow="true" />
    <asp:Panel ID="pnlTempSaveConfirm" runat="server" Width="600" BackColor="White" Style="display: none">
        <table width="100%">
            <tr>
                <td align="center" style="font-weight:bold;">
                    &nbsp;<br />
                     คำเตือน : มีการกำหนด Reorder Point ไว้ที่ <asp:Label ID="lblOldPack" runat="server"></asp:Label> ต้องการเปลี่ยนแปลงหรือไม่?
                     <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnConfirmSave" runat="server" Text="บันทึก" 
                        onclick="btnConfirmSave_Click" />&nbsp;
                    <asp:Button ID="btnConfirmCancel" runat="server" Text="ยกเลิก" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
