<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="PRTrackingReport.aspx.cs" Inherits="GPlus.PRPO.PRTrackingReport" %>

<%@ Register Src="~/UserControls/CalendarControl.ascx" TagName="calendarControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ItemDivDepControl.ascx" TagName="divdepControl" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/NoPRPOControl.ascx" TagName="noPRPOControl" TagPrefix="uc3" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <table cellpadding="0" cellspacing="0" width="805">
    <tr>
        <td class="tableHeader">
            รายงานติดตามสถานะใบขอซื้อ-ขอจ้าง
        </td>
    </tr>
    <tr>
        <td class="tableBody">
            <table border="0">
                <tr>
                    <td colspan="4">
                        <uc3:noPRPOControl ID="noPRPOCtrl" runat="server" />
                    </td>
                    <td><asp:TextBox ID="txtDivDepName" width="250px" Enabled="false" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 130px;" align="right">
                        วันที่ขอซื้อ/ขอจ้าง
                    </td>
                    <td>
                        <uc1:CalendarControl ID="ccFrom" runat="server" />
                    </td>
                    <td>
                        ถึงวันที่
                    </td>
                    <td>
                        <uc1:CalendarControl ID="ccTo" runat="server" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 130px;" align="right">
                        วิธีการขอซื้อ/ขอจ้าง
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPRType" runat="server">
                            <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                            <asp:ListItem Text="ขอซื้อ" Value="1"></asp:ListItem>
                            <asp:ListItem Text="ขอจ้าง" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"><uc2:divdepControl ID="divdepCtrl" runat="server" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
              <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="PRPO\PRTrackingReport.rdlc">
                    </LocalReport>
            </rsweb:ReportViewer>
        </td>
    </tr>
    </table>
</asp:Content>