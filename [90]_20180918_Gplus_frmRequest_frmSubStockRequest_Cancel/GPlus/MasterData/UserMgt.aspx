<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="UserMgt.aspx.cs" Inherits="GPlus.images.UserMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาผู้ใช้งาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            ชื่อ
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFirstNameSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            นามสกุล
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLastNameSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            User Name
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtUserNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            กลุ่มผู้ใช้งาน
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlUserGroupSearch" runat="server" Width="155" DataTextField="UserGroup_Name"
                                DataValueField="UserGroup_Id">
                            </asp:DropDownList>
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
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvUserName" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvUserName_RowCommand" OnRowDataBound="gvUserName_RowDataBound"
                    OnSorting="gvUserName_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="User Name" DataField="Account_UserName" SortExpression="Account_UserName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ชื่อ" DataField="Account_Fname" SortExpression="Account_Fname"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="นามสกุล" DataField="Account_Lname" SortExpression="Account_Lname"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มผู้ใช้งาน" DataField="UserGroup_Name" SortExpression="UserGroup_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Account_Status" SortExpression="Account_Status"
                            ItemStyle-HorizontalAlign="Left" />
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
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อ<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุชื่อ"
                                    ControlToValidate="txtFirstName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                นามสกุล<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุนามสกุล"
                                    ControlToValidate="txtLastName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                User Name<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="50" Width="190"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุ User Name"
                                    ControlToValidate="txtUserName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            
                        </tr>
                        <tr>
                            <%--<td style="width: 130px;" align="right">
                                กลุ่มผู้ใช้งาน<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUserGroup" runat="server" Width="155" DataTextField="UserGroup_Name"
                                    DataValueField="UserGroup_Id">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุกลุ่มผู้ใช้งาน"
                                    ControlToValidate="ddlUserGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>
                            </td>--%>
                            <td style="width: 130px;" align="right">
                                Password<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" Width="150" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="กรุณาระบุ Password"
                                    ControlToValidate="txtPassword" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="rfvPassword_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="rfvPassword">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                Confirm Password
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirm" runat="server" MaxLength="50" Width="150" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุ Confirm Password"
                                    ControlToValidate="txtConfirm" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="รหัสผ่านไม่ตรงกัน"
                                    ControlToCompare="txtPassword" ControlToValidate="txtConfirm" 
                                    ForeColor="Red">*</asp:CompareValidator>
                                <asp:ValidatorCalloutExtender ID="CompareValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="CompareValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ฝ่าย/ทีม<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server" DataTextField="Description"
                                    DataValueField="Department_ID" Width="470">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="กรุณาระบุแผนก"
                                    ControlToValidate="ddlDepartment" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator5_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;display:none;" align="right">
                                คลังสินค้า
                            </td>
                            <td style="display:none;">
                                <asp:DropDownList ID="ddlStock" runat="server" Width="155" DataTextField="Stock_Name"
                                    DataValueField="Stock_ID">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                เบอร์ต่อ
                            </td>
                            <td>
                                <asp:TextBox ID="txtExt_No" runat="server" Width="190" MaxLength="50"></asp:TextBox>
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
                        </tr>
                        <tr>
                            <td align="right" valign="top">กลุ่มผู้ใช้งาน</td>
                            <td colspan="3" align="left">
                                <asp:CheckBoxList id="cblUserGroup" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" 
                                    DataTextField="UserGroup_Name" DataValueField="UserGroup_Id">
                                </asp:CheckBoxList>
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
