<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="SupplierMgt.aspx.cs"
 Inherits="GPlus.MasterData.SupplierMgt" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหา Supplier
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 120px;" align="right">
                            รหัส Supplier
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSupplierCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 120px;" align="right">
                            ชื่อ Supplier
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSupplierNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px;" align="right">
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
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvSupplier" runat="server" AutoGenerateColumns="false" Width="100%" AllowSorting="true" 
                    OnRowCommand="gvSupplier_RowCommand" OnRowDataBound="gvSupplier_RowDataBound" OnSorting="gvSupplier_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส Supplier" DataField="Supplier_Code" SortExpression="Supplier_Code"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ชื่อ Supplier" DataField="Supplier_Name" SortExpression="Supplier_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Supplier_Status" SortExpression="Supplier_Status"
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
                    รายละเอียด Supplier
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td style="width: 120px;" align="right">
                                รหัส Supplier<span style="Color:Red">*</span>
                            </td>
                            <td >
                                <asp:TextBox ID="txtSupplierCode" runat="server" Width="150" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัส Supplier"
                                    ControlToValidate="txtSupplierCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                                <asp:Button ID="btnPopAccount" runat="server" SkinID="ButtonMiddleLong" Text="Account Downloadใบสั่ง" 
                                    OnClientClick="return false;" />
                            </td>
                            <td style="width: 120px;" align="right">
                                ชื่อ Supplier<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierName" runat="server" Width="230" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุชื่อ Supplier"
                                    ControlToValidate="txtSupplierName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" 
                                    TargetControlID="RequiredFieldValidator2"></asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                ที่อยู่
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAddress" runat="server" Width="350" Height="80" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                            <table style="border:1px; border-style:solid; border-color:Gray; width:150px;">
                               <tr>
                                 <%--<td colspan = "4">
                                    <asp:RadioButtonList ID="rdbSupplierType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="โรงพิมพ์นอก" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="โรงพิมพ์บริษัท"></asp:ListItem>
                                    </asp:RadioButtonList>
                                  </td>--%>
                                  <td colspan = "4"  style="padding-left:15px;">
                                     <asp:CheckBox ID="chkSupplier_Type" Text="  บริษัทเมืองไทย  " runat="server" />
                                  </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                จังหวัด
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProvince" runat="server" Width="155" 
                                    DataValueField="ProvinceID" DataTextField="ProvinceName" AutoPostBack="true" 
                                    onselectedindexchanged="ddlProvince_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px;" align="right">
                                อำเภอ
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAmphur" runat="server" Width="155" 
                                    DataValueField="AmphurID" DataTextField="AmphurName" AutoPostBack="true" 
                                    onselectedindexchanged="ddlAmphur_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                ตำบล
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTumbon" runat="server" Width="155" DataValueField="TumbonID" DataTextField="TumbonName" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px;" align="right">
                                รหัสไปรษณีย์
                            </td>
                            <td>
                                <asp:TextBox ID="txtPostCode" runat="server" MaxLength="50" Width="190"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                เบอร์โทรศัพท์
                            </td>
                            <td>
                                <asp:TextBox ID="txtTel" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="width: 120px;" align="right">
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="190"></asp:TextBox>
                                <br />
                                <asp:CheckBox ID="chkEmailPO" runat="server" Text="Mail PO" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                Fax
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="width: 120px;" align="right">
                                <%--สถานที่วางบิล--%>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBillSupplier" runat="server" Width="155" Visible="false" 
                                    DataTextField="Supplier_Name" DataValueField="Supplier_ID"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                การชำระเงิน
                            </td>
                            <td colspan="3">
                                <asp:RadioButton ID="chkCheque" runat="server" Text="Cheque"  GroupName="Pay"/>&nbsp;&nbsp;
                                <asp:RadioButton ID="chkCash" runat="server" Text="Cash" GroupName="Pay"/>&nbsp;&nbsp;
                                Credit Term&nbsp;
                                <asp:TextBox ID="txtCreditTerm" runat="server" MaxLength="3" Width="70" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                     onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right"></asp:TextBox>&nbsp;&nbsp;
                                <asp:CheckBox ID="chkIncludeVat" runat="server" Text="คิด vat" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 120px;" align="right">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                วันที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 120px;" align="right">
                                ผู้ที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;" align="right">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 120px;" align="right">
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
