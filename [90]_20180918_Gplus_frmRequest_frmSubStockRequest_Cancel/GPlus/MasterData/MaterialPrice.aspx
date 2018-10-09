<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="MaterialPrice.aspx.cs" Inherits="GPlus.MasterData.MaterialPrice" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc3" %>
<%@ Register src="../UserControls/ItemPackSelectorControl.ascx" tagname="ItemPackSelectorControl" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">

    function ConfrimationForm() {
        if (confirm("คุณต้องการบันทึกข้อมูลหรือไม่") == true) {
            return true;
        }
        return false;
    }


</script>
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาราคาวัสดุอุปกรณ์
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัส
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaterialCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            ชื่อ
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaterialNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            หน่วย
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPackageName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            สถานะ
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="155">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <%--<asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />--%>
                <asp:GridView ID="gvMaterialPrice" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvMaterialPrice_RowCommand" OnRowDataBound="gvMaterialPrice_RowDataBound"
                    OnSorting="gvMaterialPrice_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ชื่อ" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="หน่วยบรรจุ" DataField="Description" SortExpression="Description"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ราคาทุน" DataField="Avg_Cost" SortExpression="Avg_Cost"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="ItemPack_Status" SortExpression="ItemPack_Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                            ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
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
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:HiddenField ID="hdPackID" runat="server" />
                    <asp:HiddenField ID="hdOldAvg_Cost" runat="server" />
                    <table width="100%">
                        <tr>
                            <td colspan="4" align="left">
                                <uc4:ItemPackSelectorControl ID="ItemPackSelectorControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ราคาทุนเฉลี่ย
                            </td>
                            <td>
                                <asp:TextBox ID="txtBasePrice" runat="server" Width="90" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>บาท
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุราคาทุนเฉลี่ย"
                                    ControlToValidate="txtBasePrice" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ลงวันที่
                            </td>
                            <td>
                                <uc3:CalendarControl ID="CalendarControl1" runat="server" IsRequire="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ราคาขาย
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalePrice" runat="server" Width="90" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>บาท
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุราคาขาย"
                                    ControlToValidate="txtSalePrice" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 130px;" align="right">
                                Barcode
                            </td>
                            <td>
                                <asp:TextBox ID="txtBarcode" runat="server" MaxLength="50" Width="190"></asp:TextBox>
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
                            <td colspan="4">
                                <span style=" color:Red; font-size:13px; padding-left:50px;">** <u>หมายเหตุ</u> : ราคาทุนเฉลี่ยที่เปลี่ยนแปลงมีผลกับราคาจ่าย</span>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClientClick="return ConfrimationForm();" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
