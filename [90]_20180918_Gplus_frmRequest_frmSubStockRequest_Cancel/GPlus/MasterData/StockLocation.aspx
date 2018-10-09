<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="StockLocation.aspx.cs" Inherits="GPlus.MasterData.StockLocation" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 217px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาสถานที่จัดเก็บคลังสินค้า
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            คลังสินค้า
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="StockType_Name"
                                DataValueField="StockType_ID">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสสถานที่จัดเก็บ
                        </td>
                        <td align="left" class="style1">
                            <asp:TextBox ID="txtLocationCodeSearch" runat="server" Width="190" MaxLength="10"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ชื่อสถานที่จัดเก็บ
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLocationNameSearch" runat="server" Width="190" MaxLength="10"></asp:TextBox>
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
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvStockLocation" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvStockLocation_RowCommand" OnRowDataBound="gvStockLocation_RowDataBound"
                    OnSorting="gvStockLocation_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" DataField="Location_Code" SortExpression="Location_Code"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ชื่อสถานที่จัดเก็บสินค้า" DataField="Location_Name" SortExpression="Location_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Location_Status" SortExpression="Location_Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่สร้าง" SortExpression="Create_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้ที่สร้าง" DataField="FullName_Create_By" SortExpression="FullName_Create_By"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="LocationID" DataField="Location_Id" ItemStyle-HorizontalAlign="Left"
                            Visible="false" />
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
                    <asp:HiddenField ID="hdStockID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                รหัสสถานที่จัดเก็บ<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocationCode" runat="server" Width="150" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัสสถานที่จัดเก็บ"
                                    ControlToValidate="txtLocationCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชื่อสถานที่จัดเก็บสินค้า<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLocationName" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุชื่อสถานที่จัดเก็บสินค้า"
                                    ControlToValidate="txtLocationName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                คลังสินค้า<span style="color: Red">*</span>
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="ddlStockAdd" runat="server" Width="153" DataTextField="StockType_Name"
                                    DataValueField="StockType_ID" AppendDataBoundItems="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุคลังสินค้า"
                                    ControlToValidate="ddlStockAdd" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                            </td>
                            <td>
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
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbDefaultLocation" Text="รับเข้าสถานที่จัดเก็บสินค้านี้อัตโนมัติ" />
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
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
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
