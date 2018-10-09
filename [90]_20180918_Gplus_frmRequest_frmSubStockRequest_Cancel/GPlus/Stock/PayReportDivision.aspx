<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="PayReportDivision.aspx.cs" Inherits="GPlus.PRPO.PayReportDivision" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                รายงานสรุปบัญชีค่าใช้จ่ายวัสดุ-อุปกรณ์แยกตามเจ้าของและฝ่ายที่เบิก
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="90%">
                      <tr>
                        <td style="width: 160px;" align="right">
                            คลังสินค้า
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="StockType_Name" DataValueField="StockType_ID" Enabled="true">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                         <td style="width: 160px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" 
                                    DataTextField="MaterialType_Name" DataValueField="MaterialType_ID"
                                    >
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;" align="right">
                            เดือน : 
                        </td>
                        <td colspan="3"  width = "400px" align="left">
                            <asp:DropDownList ID="ddlMonthStart" runat="server" Width="150" DataTextField="ddlMonth_Name"
                                DataValueField="ddlMonth_ID"  >
                                <%--<asp:ListItem Value="00">เลือกเดือน</asp:ListItem>--%>
                                <asp:ListItem Value="01">มกราคม</asp:ListItem>
                                <asp:ListItem Value="02">กุมภาพันธ์</asp:ListItem>
                                <asp:ListItem Value="03">มีนาคม</asp:ListItem>
                                <asp:ListItem Value="04">เมษายน</asp:ListItem>
                                <asp:ListItem Value="05">พฤษภาคม</asp:ListItem>
                                <asp:ListItem Value="06">มิถุนายน</asp:ListItem>
                                <asp:ListItem Value="07">กรกฎาคม</asp:ListItem>
                                <asp:ListItem Value="08">สิงหาคม</asp:ListItem>
                                <asp:ListItem Value="09">กันยายน</asp:ListItem>
                                <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                                <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                                <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlYearStart" runat="server"  >
                            </asp:DropDownList>
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
                    <LocalReport ReportPath="Stock/PayReportDivision.rdlc">
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

