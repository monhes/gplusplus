<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ReportRemainingPO.aspx.cs" Inherits="GPlus.PRPO.ReportRemainingPO" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 220px;
        }
        .style2
        {
            width: 96px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานแสดงรายการสินค้าที่ค้างรับ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                           วันที่ของการสั่งซื้อ&nbsp;&nbsp;
                        </td>
                        <td class="style1">
                           <uc2:CalendarControl ID="dtCreatePOStart" runat="server" />
                        </td>
                        <td align="right" class="style2">
                            ถึงวันที่&nbsp;&nbsp;
                        </td>
                        <td>
                            <uc2:CalendarControl ID="dtCreatePOStop" runat="server" />
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                             รหัส Supplier&nbsp;&nbsp;
                        </td>
                        <td class="style1">
                             <asp:TextBox ID="txtSupplierCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td align="right" class="style2">
                             ชื่อ Supplier&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="300"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสวัสดุอุปกรณ์&nbsp;&nbsp;
                        </td>
                        <td align="left" class="style1">
                            <asp:TextBox ID="txtItemId" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td align="right" class="style2">
                            รายการสินค้า&nbsp;&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtItemName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
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
                    <LocalReport ReportPath="PRPO\ReportRemainingPO.rdlc">
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

