<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" 
CodeBehind="ReportOrderPO.aspx.cs" Inherits="GPlus.PRPO.ReportOrderPO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/ReportOrderPO.ascx" TagName="ReportOrderPO" TagPrefix="uc2" %>
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
                รายงานแสดงยอดการสั่งซื้อสินค้า
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                           วันที่ของการสั่งซื้อ&nbsp;&nbsp;
                        </td>
                        <td>
                           <uc2:CalendarControl ID="dtCreatePOStart" runat="server" />
                        </td>
                        <td style="width: 80px;" align="right">
                            ถึงวันที่&nbsp;&nbsp;
                        </td>
                        <td>
                            <uc2:CalendarControl ID="dtCreatePOStop" runat="server" />
                        </td>
                       
                    </tr>
                    <tr>
                     <td style="width: 130px" align="right">
                            ประเภท&nbsp;&nbsp;
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlMaterialTypeSearch"  runat="server" Width="195">
                            </asp:DropDownList>
                        </td>
                         <td colspan = "4">
                            <uc2:ReportOrderPO ID="SupplierSearch" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td style="width: 130px;" align="right">
                            รหัสวัสดุอุปกรณ์&nbsp;&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtItemId" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width: 80px;" align="right">
                            รายการสินค้า&nbsp;&nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtItemName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click"  />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancel_Click"  />
                        </td>
                    </tr>
                </table>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="PRPO\ReportOrderPO.rdlc">
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
