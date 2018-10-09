<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    MaintainScrollPositionOnPostback="true" CodeBehind="MaterialMgt.aspx.cs" Inherits="GPlus.MasterData.MaterialMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/ImageListControl.ascx" TagName="ImageListControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาวัสดุอุปกรณ์
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
                        <td style="width: 130px;" align="right">
                            ชื่อ
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaterialNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            ประเภท
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlMaterialTypeSearch" runat="server" Width="195" DataTextField="MaterialType_Name"
                                DataValueField="MaterialType_ID">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                            รหัสเดิม(AS400)
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOldCodeSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            สถานะ
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="195">
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
                <asp:GridView ID="gvMaterial" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvMaterial_RowCommand" OnRowDataBound="gvMaterial_RowDataBound"
                    OnSorting="gvMaterial_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ชื่อ" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ขนาดและคุณลักษณะ" DataField="Inv_Attrbute" SortExpression="Inv_Attrbute"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ประเภท" DataField="Cat_Name" SortExpression="Cat_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="รหัสเดิม(AS400)" DataField="Inv_AS400" SortExpression="Inv_AS400"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มผู้ใช้งาน" DataField="Type_Name" SortExpression="Type_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มอุปกรณ์" DataField="SubCate_Name" SortExpression="SubCate_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Asset_Status" SortExpression="Asset_Status"
                            ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center"
                            Visible="false" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                            ItemStyle-HorizontalAlign="Left" Visible="false" />
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
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                รหัส
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtMaterialCode" runat="server" Width="190" MaxLength="100" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                หมายเลข
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtKeyCode" runat="server" Width="100"  MaxLength="130" Enabled="true" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อ<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaterialName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุชื่อ"
                                    ControlToValidate="txtMaterialName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ขนาดและคุณลักษณะ<%--<span style="color: Red">*</span>--%></td>
                            <td>
                                <asp:TextBox ID="txtMaterialProperty" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุขนาดและคุณลักษณะ"
                                    ControlToValidate="txtMaterialProperty" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ประเภทอุปกรณ์<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" DataTextField="MaterialType_Name"
                                    DataValueField="MaterialType_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterialType_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุประเภทอุปกรณ์"
                                    ControlToValidate="ddlMaterialType" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชนิด
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Width="195">
                                    <asp:ListItem Text="เลือกชนิด" Value=""></asp:ListItem>
                                    <asp:ListItem Text="ผง" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="น้ำ" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="แท่ง" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="หลอด" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุชนิด"
                                    ControlToValidate="ddlType" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                กลุ่มผู้ใช้งาน<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUserGroup" runat="server" Width="195" DataTextField="UserGroup_Name"
                                    DataValueField="UserGroup_Id">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="กรุณาระบุกลุ่มผู้ใช้งาน"
                                    ControlToValidate="ddlUserGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator5">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                รหัสเดิม(AS400)
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldMaterialCode" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ประเภทอุปกรณ์ย่อย<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubMaterialType" runat="server" Width="195" DataTextField="SubMaterialType_Name"
                                    DataValueField="SubMaterialType_ID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="กรุณาระบุประเภทอุปกรณ์ย่อย"
                                    ControlToValidate="ddlSubMaterialType" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator7">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                รายละเอียดการจัดซื้อ
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDetail" runat="server" Width="250" Height="60" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="updPackage" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddPackage" runat="server" Text="เพิ่ม Package" CausesValidation="false"
                                                        SkinID="ButtonMiddle" OnClick="btnAddPackage_Click" />
                                                </td>
                                                <td>
                                                    ป้อนข้อมูลเรียงจากหน่วยย่อยไปหน่วยใหญ่
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvPackage" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowCommand="gvPackage_RowCommand" OnRowDataBound="gvPackage_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ลบ">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDel" runat="server" Text="ลบ" CommandName="Del" CausesValidation="false"></asp:LinkButton>
                                                        <asp:HiddenField ID="hdDetailID" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ลำดับ">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btnUp" runat="server" CausesValidation="false" ImageUrl="~/images/arrow-up-3.png"
                                                                        CommandName="Up" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSequence" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="btnDown" runat="server" CausesValidation="false" ImageUrl="~/images/arrow-down-3.png"
                                                                        CommandName="Down" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="หน่วยนับ">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlPackageUnit" runat="server" Width="135" DataTextField="Package_Name"
                                                            DataValueField="Pack_ID">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlPackageUnit" runat="server" ErrorMessage="กรุณาระบุหน่วย"
                                                            ControlToValidate="ddlPackageUnit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="exrfvddlPackageUnit" runat="server" Enabled="True"
                                                            TargetControlID="rfvddlPackageUnit">
                                                        </asp:ValidatorCalloutExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="จำนวน">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" Width="80" MaxLength="5" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                                            Style="text-align: right"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtQuantity" runat="server" ErrorMessage="กรุณาระบุจำนวน"
                                                            ControlToValidate="txtQuantity" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="exrfvtxtQuantity" runat="server" Enabled="True"
                                                            TargetControlID="rfvtxtQuantity">
                                                        </asp:ValidatorCalloutExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="หน่วยย่อย">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlPackageBase" runat="server" Width="135" DataTextField="Package_Name"
                                                            DataValueField="Pack_ID">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlPackageBase" runat="server" ErrorMessage="กรุณาระบุหน่วยนับ"
                                                            ControlToValidate="ddlPackageBase" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="exddlPackageBase" runat="server" Enabled="True"
                                                            TargetControlID="rfvddlPackageBase">
                                                        </asp:ValidatorCalloutExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="รายละเอียด">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPackageDetail" runat="server" Width="130" MaxLength="50"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtPackageDetail" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                                                            ControlToValidate="txtPackageDetail" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="exrfvtxtPackageDetail" runat="server" Enabled="True"
                                                            TargetControlID="rfvtxtPackageDetail">
                                                        </asp:ValidatorCalloutExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="หน่วยที่เบิกใช้" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsBase" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="สถานะ" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="stats" runat="server" Width="50" MaxLength="50" Enabled="False"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ภาพสินค้า
                            </td>
                            <td colspan="3">
                                <uc2:ImageListControl ID="ImageListControl1" runat="server" />
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
