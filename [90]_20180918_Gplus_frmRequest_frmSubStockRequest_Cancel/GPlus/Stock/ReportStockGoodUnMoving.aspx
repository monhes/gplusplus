<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ReportStockGoodUnMoving.aspx.cs" Inherits="GPlus.Stock.ReportStockGoodUnMoving" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานจำนวนสินค้าคงเหลือที่เคลื่อนไหวน้อย
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            คลังสินค้า
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="StockType_Name" DataValueField="StockType_ID">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <fieldset style="width:350px">
                                <legend>ช่วงเวลา</legend>
                                <table>
                                    <tr>
                                        <td>
                                           <asp:RadioButton ID="chkDate" runat="server" Text="ช่วงวันที่"  GroupName="Pay" />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <uc2:CalendarControl ID="dtStart" runat="server" />
                                        </td>
                                        <td>
                                            -
                                        </td>
                                        <td>
                                            <uc2:CalendarControl ID="dtStop" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="chkMonth" runat="server" Text="ย้อนหลัง" GroupName="Pay" Checked="true"/>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMonthBack" runat="server" MaxLength="10" Width="90" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlFrequency" runat="server" Width="90">
                                                <asp:ListItem Value="d">วัน</asp:ListItem>
                                                <asp:ListItem Value="m">เดือน</asp:ListItem>
                                                <asp:ListItem Value="y">ปี</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                         <td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" 
                                    DataTextField="MaterialType_Name" DataValueField="MaterialType_ID"
                                    AutoPostBack="true" 
                                    onselectedindexchanged="ddlMaterialType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 60px;" align="right">
                            ประเภทอุปกรณ์ย่อย
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubMaterialType" runat="server" Width="195" DataTextField="SubMaterialType_Name" DataValueField="SubMaterialType_ID">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCode" runat="server" MaxLength="20" Width="190"></asp:TextBox>
                        </td>
                        <td style="width: 60px;" align="right">
                            รายการสินค้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style=" padding-left:50px;">
                            <fieldset style="width:300px">
                                <legend>ความเคลื่อนไหว</legend>
                                <table>
                                    <tr>
                                        <td colspan = "3">
                                           <asp:RadioButton ID="chk_NoMovement" runat="server" Text="ไม่มีความเคลื่อนไหว"  GroupName="Movement" Checked="true" />&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="chk_MovementCnt" runat="server" Text="เคลื่อนไหวไม่เกิน" GroupName="Movement" />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMovementCnt" runat="server" MaxLength="10" Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox> &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCount" Text="ครั้ง" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="Stock\ReportStockGoodUnMoving.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>

            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>
</asp:Content>

