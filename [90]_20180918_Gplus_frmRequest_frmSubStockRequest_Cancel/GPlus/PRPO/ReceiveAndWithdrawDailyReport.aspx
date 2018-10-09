<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="ReceiveAndWithdrawDailyReport.aspx.cs" Inherits="GPlus.PRPO.ReceiveAndWithdrawDailyReport" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 59px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานสรุปรายการรับ – เบิก ประจำวัน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="90%">
                      <tr>
                        <td colspan="2" align="center">
                            <fieldset style="width:165px">
                                <legend>ประเภท</legend>
                                 <asp:RadioButtonList ID="rdbType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="รายการรับ" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="รายการเบิก"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td align="right" class="style1">
                            คลังสินค้า
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="StockType_Name" DataValueField="StockType_ID" Enabled="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                           วันที่ :
                        </td>
                        <td>
                           <uc2:CalendarControl ID="dtStart" runat="server" />
                        </td>
                        <td align="right" class="style1">
                           ถึงวันที่ :
                        </td>
                        <td align="left">
                            <uc2:CalendarControl ID="dtStop" runat="server" />
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
                        <td align="right" class="style1">
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
                        <td align="right" class="style1">
                            รายการสินค้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="190"></asp:TextBox>
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
                    <LocalReport ReportPath="PRPO\ReceiveAndWithdrawDailyReport.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
                <rsweb:ReportViewer ID="ReportViewer2" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="PRPO\ReceiveAndWithdrawDailyReport_Pay.rdlc">
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

