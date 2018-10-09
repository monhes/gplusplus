<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="PRApproveMgt.aspx.cs" Inherits="GPlus.PRPO.PRApproveMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PRControl.ascx" TagName="PRControl" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                อนุมัติขอซื้อ (PR)
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdbIsWait" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                <asp:ListItem Text="รอพิจารณา" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="พิจารณาแล้ว" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 130px;" align="right">
                            ผลการพิจารณา
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                    <asp:ListItem Text="ไม่อนุมัติ" Value="0"></asp:ListItem>
                                <asp:ListItem Text="อนุมัติ" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="ส่งกลับไปแก้ไข" Value="2"></asp:ListItem>
                            
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่ขอซื้อ
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccFrom" runat="server" />
                        </td>
                        <td style="width: 130px;" align="right">
                            ถึงวันที่
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfPrID" runat="server" />
                <asp:HiddenField ID="hfPrStatus" runat="server" />
                <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="gvPR_RowCommand"
                    OnRowDataBound="gvPR_RowDataBound" OnSorting="gvPR_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="วันที่ขอซื้อ" DataField="Request_Date" SortExpression="Request_Date" />
                        <asp:BoundField HeaderText="เลขที่PR" DataField="PR_Code" SortExpression="PR_Code" />
                        <asp:BoundField HeaderText="ชื่อ Supplier" DataField="Supplier_Name" SortExpression="Supplier_Name" />
                        <asp:BoundField HeaderText="ยอดเงินของ PR" DataField="Net_Amonut" SortExpression="Net_Amonut" />
                        <asp:BoundField HeaderText="ผู้สร้าง PR" DataField="Create_By" SortExpression="Create_By" />
                        <asp:BoundField HeaderText="ฝ่าย/ทีม" DataField="Dep_Name" SortExpression="Dep_Name" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status" />
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
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    <asp:Label ID="lblHeader" runat="server" Text="ผู้บริหารอนุมัติ"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:Panel ID="pnlApprover" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผลการพิจารณา
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblApproverStatus" runat="server" RepeatDirection="Horizontal">
                                       <asp:ListItem Text="ไม่อนุมัติ" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="อนุมัติ" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="ส่งกลับไปแก้ไข" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาเลือกผลการพิจารณา"
                                        ControlToValidate="rblApproverStatus" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    เหตุผล
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" Width="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผู้อนุมัติ
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApproverName" runat="server" Width="180" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 130px;" align="right">
                                    วัน/เวลา
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateTime" runat="server" Width="180" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnApproverSave" runat="server" Text="บันทึก" />&nbsp;
                                    <asp:Button ID="btnCancelSave" runat="server" Text="ล้างหน้าจอ" CausesValidation="false"
                                        OnClick="btnCancelSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:ModalPopupExtender ID="mpeApprover" runat="server" TargetControlID="btnApproverSave"
                        PopupControlID="pnlApproverConfirm" BackgroundCssClass="modalBackground" CancelControlID="btnApproverCancel"
                        DropShadow="true" />
                    <asp:Panel ID="pnlApproverConfirm" runat="server" BackColor="White" Style="display: none">
                        <table cellpadding="0" cellspacing="0" width="400">
                            <tr>
                                <td class="tableHeaderM" align="left">
                                    ยืนยันรหัสผ่าน
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBodyM" align="center" valign="middle">
                                    <table width="100%" cellspacing="3" style="margin-top:12px;margin-bottom:12px;">
                                        <tr>
                                            <td style="width: 130px;" align="right">
                                                ยืนยันรหัสผ่าน
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtApproverPassword" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px;" align="right">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnApproverConfirmSave" runat="server" Text="บันทึก" OnClick="btnApproverConfirmSave_Click" />&nbsp;
                                                <asp:Button ID="btnApproverCancel" runat="server" Text="ยกเลิก" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableFooterM">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlTempApprove" runat="server">
                        <table width="100%">
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผลการพิจารณา
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblTempApproverStatus" runat="server" RepeatDirection="Horizontal">
                                       <asp:ListItem Text="ไม่อนุมัติ" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="อนุมัติ" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="ส่งกลับไปแก้ไข" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาเลือกผลการพิจารณา"
                                        ControlToValidate="rblApproverStatus" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผู้อนุมัติ<span style="color: Red;">*</span>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlApprover" runat="server" DataTextField="Approve_Name" DataValueField="Approve_ID"
                                        Width="300">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาเลือกผู้อนุมัติ"
                                        ControlToValidate="ddlApprover" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผู้อนุมัติแทน
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTempApprover" runat="server" Width="180" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 130px;" align="right">
                                    วัน/เวลา
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTempApproverDate" runat="server" Width="180" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    เหตุผล
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTempReason" runat="server" Width="300" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnTempApproverSave" runat="server" Text="บันทึก" />&nbsp;
                                    <asp:Button ID="btnTempApproverCancel" runat="server" Text="ยกเลิก" CausesValidation="false"
                                        OnClick="btnTempApproverCancel_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:ModalPopupExtender ID="mpeTempApprove" runat="server" TargetControlID="btnTempApproverSave"
                            PopupControlID="pnlTempApproverConfirm" BackgroundCssClass="modalBackground"
                            CancelControlID="btnTempApproveCancel" DropShadow="true" />
                        <asp:Panel ID="pnlTempApproverConfirm" runat="server" BackColor="White" Style="display: none">
                            <table cellpadding="0" cellspacing="0" width="400">
                                <tr>
                                    <td class="tableHeaderM" align="left">
                                        ยืนยันรหัสผ่าน
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBodyM" align="center" valign="middle">
                                        <table width="100%" cellspacing="3" style="margin-top:12px;margin-bottom:12px;">
                                            <tr>
                                                <td style="width: 120px;" align="right">
                                                    ยืนยันรหัสผ่าน
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTempApprovePassword" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnTempApproverConfirmSave" runat="server" Text="บันทึก" OnClick="btnTempApproverConfirmSave_Click" />&nbsp;
                                                    <asp:Button ID="btnTempApproveCancel" runat="server" Text="ยกเลิก" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
            <tr>
                <td class="tableFooterM">
                </td>
            </tr>
        </table>
        </asp:Panel> </asp:Panel>
        <fieldset>
            <legend>ใบขอซื้อ</legend>
            <uc3:PRControl ID="PRControl1" runat="server" />
        </fieldset>
        </td> </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
        </table>
    </asp:Panel>
</asp:Content>
