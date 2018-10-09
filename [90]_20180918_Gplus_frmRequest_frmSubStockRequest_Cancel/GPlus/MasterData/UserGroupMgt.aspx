<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="UserGroupMgt.aspx.cs" Inherits="GPlus.MasterData.UserGroupMgt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหากลุ่มผู้ใช้งาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสกลุ่มผู้ใช้งาน
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserGroupCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ชื่อกลุ่มผู้ใช้งาน
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserGroupSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            สถานะ
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="155">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click"
                                CausesValidation="false" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" OnClick="btnCancelSearch_Click"
                                CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                    <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="false"
                        OnClick="btnAdd_Click" />
                    <asp:GridView ID="gvUserGroup" runat="server" AutoGenerateColumns="false" Width="100%"
                        AllowSorting="true" OnRowCommand="gvUserGroup_RowCommand" OnRowDataBound="gvUserGroup_RowDataBound"
                        OnSorting="gvUserGroup_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="รายละเอียด">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                        ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="รหัสกลุ่มผู้ใช้งาน" DataField="UserGroup_Code" SortExpression="UserGroup_Code"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="กลุ่มผู้ใช้งาน" DataField="UserGroup_Name" SortExpression="UserGroup_Name"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="สถานะ" DataField="UserGroup_Status" SortExpression="UserGroup_Status"
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
                    <asp:Label ID="lblUserGroupName" runat="server"></asp:Label>
                    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                        <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="ข้อมูลกลุ่มผู้ใช้งาน">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdID" runat="server" />
                                <table width="100%">
                                    <tr>
                                        <td style="width: 130px;" align="right">
                                            รหัสกลุ่มผู้ใช้งาน<span style="Color:Red">*</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUserGroupCode" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุรหัสกลุ่มผู้ใช้งาน"
                                                ControlToValidate="txtUserGroupCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                TargetControlID="RequiredFieldValidator2">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                        <td style="width: 130px;" align="right">
                                            ชื่อกลุ่มผู้ใช้งาน<span style="Color:Red">*</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUserGroup" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุกลุ่มผู้ใช้งาน"
                                                ControlToValidate="txtUserGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td style="width: 130px;" align="right">
                                            รายละเอียด
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" Width="190" MaxLength="200"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;" align="right">
                                            สถานะการใช้งาน
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Inactive"></asp:ListItem>
                                            </asp:RadioButtonList>
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
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="ข้อมูลสิทธิ์การใช้งาน">
                            <ContentTemplate>
                                <asp:GridView ID="gvPermission" runat="server" AutoGenerateColumns="False" 
                                    OnRowDataBound="gvPermission_RowDataBound" Width="100%">
                                    <Columns>
                                        <asp:BoundField HeaderText="Menu Group" DataField="MenuGroup_Name">
                                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Menu" DataField="Menu_Name">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ดูข้อมูล">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdMenuID" runat="server" />
                                                <asp:CheckBox ID="chkCanView" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เพิ่มข้อมูล">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCanAdd" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="แก้ไขข้อมูล">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCanUpdate" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="อนุมัติ" Visible="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCanApprove" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click"
                            CausesValidation="false" />&nbsp;
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
