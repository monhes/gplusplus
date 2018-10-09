<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ApproverMgt.aspx.cs" Inherits="GPlus.MasterData.ApproverMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาผู้อนุมัติของหน่วยงาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            ฝ่าย
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDivCodeSearch" runat="server" Width="80" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 40px;" align="right">
                            ชื่อฝ่าย
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDivNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            ทีม
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDepCodeSearch" runat="server" Width="80" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 40px;" align="right">
                            ชื่อทีม
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDepNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
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
                        <td colspan="6" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvDepartment" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                    Width="100%" OnRowCommand="gvDepartment_RowCommand" OnRowDataBound="gvDepartment_RowDataBound"
                    OnSorting="gvDepartment_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField="" SortExpression="" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ทีม" DataField="" SortExpression="" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="เบิกของจากคลัง" DataField="Stock_Name" SortExpression="Stock_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
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
                                รหัสฝ่าย/ทีมงาน
                            </td>
                            <td>
                                <asp:TextBox ID="txtDivCode" runat="server" Width="60" BackColor="WhiteSmoke" ReadOnly="true"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtDepCode" runat="server" Width="60" BackColor="WhiteSmoke" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชื่อฝ่าย/ทีมงาน
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeptName" runat="server" Width="300" BackColor="WhiteSmoke" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right" valign="top">
                                อนุมัติข้อมูล
                            </td>
                            <td align="left" valign="top">
                                <asp:DropDownList ID="ddlApprovePart" runat="server" Width="155" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlApprovePart_SelectedIndexChanged">
                                    <asp:ListItem Text="ใบขอซื้อ/ข้อจ้าง(PR)" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ใบสั่งซื้อ/สังจ้าง(PO)" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="เบิกวัสดุ - อุปกรณ์" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" valign="top" align="left" style=" display:none">
                                <fieldset>
                                    <legend>ใบเบิก - วัสดุอุปกรณ์</legend>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:RadioButtonList ID="rblByPass" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="ต้องผ่านอนุมัติ" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="ไม่ต้องผ่านอนุมัติ" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>วันที่มีผล</td>
                                            <td><uc2:CalendarControl ID="ccByPassFrom" runat="server" /></td>
                                            <td>-</td>
                                            <td><uc2:CalendarControl ID="ccByPassTo" runat="server" /></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="4">
                                <asp:Button ID="btnAddApprover" runat="server" Text="เพิ่มผู้อนุมัติ" CausesValidation="false" SkinID="ButtonMiddle"/>
                                <asp:GridView ID="gvApprover" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                                    Width="100%" OnRowCommand="gvApprover_RowCommand" OnRowDataBound="gvApprover_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="ลำดับ" />
                                        <asp:BoundField HeaderText="ผู้อนุมัติ" DataField="Approve_Name" />
                                        <asp:BoundField HeaderText="วันที่เริ่มต้น" DataField="Effective_Date" />
                                        <asp:BoundField HeaderText="วันที่สิ้นสุด" DataField="Expire_Date" />
                                        <asp:BoundField HeaderText="สถานะ" DataField="Status" />
                                        <asp:TemplateField HeaderText="ลบ">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDelete" runat="server" Text="ลบ" CommandName="Del"></asp:LinkButton>
                                                <asp:HiddenField ID="hdID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" colspan="4">
                                <asp:Button ID="btnAddTempApprover" runat="server" Text="เพิ่มผู้อนุมัติแทน" CausesValidation="false" SkinID="ButtonMiddle" />
                                <asp:GridView ID="gvTempApprove" runat="server" AutoGenerateColumns="false" AllowSorting="false"
                                    Width="100%" OnRowCommand="gvTempApprove_RowCommand" OnRowDataBound="gvTempApprove_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="ลำดับ" />
                                        <asp:BoundField HeaderText="ผู้อนุมัติ" DataField="Approve_Name" />
                                        <asp:BoundField HeaderText="วันที่เริ่มต้น" DataField="Effective_Date" />
                                        <asp:BoundField HeaderText="วันที่สิ้นสุด" DataField="Expire_Date" />
                                        <asp:BoundField HeaderText="เหตุผล" DataField="Reason" />
                                        <asp:BoundField HeaderText="สถานะ" DataField="Status" />
                                        <asp:TemplateField HeaderText="ลบ">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDelete" runat="server" Text="ลบ" CommandName="Del"></asp:LinkButton>
                                                <asp:HiddenField ID="hdID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" Visible="false"/>
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ปิด" CausesValidation="False" OnClick="btnCancel_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlApprover" runat="server" Width="800" BackColor="White" Style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 130px;" align="right">
                    ผู้อนุมัติ<span style="Color:Red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddlApprover" runat="server" Width="250">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุผู้อนุมัติ" ValidationGroup="a"
                        ControlToValidate="ddlApprover" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                    </asp:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td style="width: 130px;" align="right">
                    วันที่มีผล
                </td>
                <td>
                    <uc2:CalendarControl ID="ccApproveStart" runat="server" />
                </td>
                <td style="width: 130px;" align="right">
                    วันที่สิ้นสุด
                </td>
                <td>
                    <uc2:CalendarControl ID="ccApproveEnd" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="width: 130px;" align="right">
                    สถานะการใช้งาน
                </td>
                <td>
                    <asp:RadioButtonList ID="rdbApproveStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Inactive"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="left">
                    <asp:Button ID="btnApproveSave" runat="server" Text="บันทึก" OnClick="btnApproveSave_Click" ValidationGroup="a" />&nbsp;
                    <asp:Button ID="btnApproveCancel" runat="server" Text="ยกเลิก" CausesValidation="false"
                        OnClick="btnApproveCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mpeApprover" runat="server" TargetControlID="btnAddApprover"
        PopupControlID="pnlApprover" BackgroundCssClass="modalBackground" DropShadow="true" />

     <asp:Panel ID="pnlTempApprover" runat="server" Width="800" BackColor="White" Style="display: none">
        <table width="100%">
            <tr>
                <td style="width: 130px; color: Red;" align="right">
                    ผู้อนุมัติแทน
                </td>
                <td>
                    <asp:DropDownList ID="ddlTempApprover" runat="server" Width="250">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="t" ErrorMessage="กรุณาระบุผู้อนุมัติแทน"
                        ControlToValidate="ddlTempApprover" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                    </asp:ValidatorCalloutExtender>
                </td>
            </tr>
            <tr>
                <td style="width: 130px;" align="right">
                    วันที่มีผล
                </td>
                <td>
                    <uc2:CalendarControl ID="ccTempStart" runat="server" />
                </td>
                <td style="width: 130px;" align="right">
                    วันที่สิ้นสุด
                </td>
                <td>
                    <uc2:CalendarControl ID="ccTempEnd" runat="server" />
                </td>
            </tr>
              <tr>
                <td style="width: 130px;" align="right">
                    เหตุผล
                </td>
                <td>
                    <asp:TextBox ID="txtReason" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                </td>
                </tr>
            <tr>
                <td style="width: 130px;" align="right">
                    สถานะการใช้งาน
                </td>
                <td>
                    <asp:RadioButtonList ID="rdbTempApproveStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Inactive"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="left">
                    <asp:Button ID="btnTempApproveSave" runat="server" Text="บันทึก" OnClick="btnTempApproveSave_Click"  ValidationGroup="t" />&nbsp;
                    <asp:Button ID="btnTempApproveCancel" runat="server" Text="ยกเลิก" CausesValidation="false"
                        OnClick="btnTempApproveCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnAddTempApprover"
        PopupControlID="pnlTempApprover" BackgroundCssClass="modalBackground" DropShadow="true" />
</asp:Content>
