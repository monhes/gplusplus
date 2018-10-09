<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_SupplierAccount.aspx.cs"
    Inherits="GPlus.MasterData.pop_SupplierAccount" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supplier Account</title>
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
                    Supplier Account
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:GridView ID="gvAccount" runat="server" AutoGenerateColumns="false" Width="100%"
                        AllowSorting="true" OnRowCommand="gvAccount_RowCommand" OnRowDataBound="gvAccount_RowDataBound"
                        OnSorting="gvAccount_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="แก้ไข">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDetail" runat="server" Text="แก้ไข" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ลบ">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDel" runat="server" Text="ลบ" CommandName="Del" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ชื่อ-นามสกุล" DataField="Account_FullName" SortExpression="Account_FullName"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="User Name" DataField="Account_Username" SortExpression="Account_Username"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Password" DataField="Account_Password" SortExpression="Account_Password"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="Email" DataField="Email" SortExpression="Email"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status" ItemStyle-Width="100"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                                ItemStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                    <div align="left">
                        <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                            OnClick="btnAdd_Click" />
                    </div>
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
                    <td class="tableHeader" align="left">
                        รายละเอียด
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <asp:HiddenField ID="hdID" runat="server" />
                        <table width="100%">
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ชื่อ-นามสกุล<span style="color: Red">*</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFullName" runat="server" Width="190"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุชื่อ-นามสกุล"
                                        ControlToValidate="txtFullName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator3">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td style="width: 130px;" align="right">
                                    Expire Date
                                </td>
                                <td align="left">
                                    <uc1:CalendarControl ID="CalendarControl1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    User Name<span style="color: Red">*</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtUserName" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุ User Name"
                                        ControlToValidate="txtUserName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td style="width: 130px;" align="right">
                                    Password<span style="color: Red">*</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPassword" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุ Password"
                                        ControlToValidate="txtPassword" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    Email <span style="color: Red">*</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุ Email"
                                        ControlToValidate="txtEmail" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
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
                                <td align="left">
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
                                <td align="left">
                                    <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                                </td>
                                <td style="width: 130px;" align="right">
                                    ผู้ที่สร้าง
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    วันที่แก้ไขล่าสุด
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                                </td>
                                <td style="width: 130px;" align="right">
                                    ผู้ที่แก้ไขล่าสุด
                                </td>
                                <td align="left">
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
    </center>
    </form>
</body>
</html>
