<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="ReorderPointReport.aspx.cs"
     Inherits="GPlus.PRPO.ReorderPointReport" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานการสั่งซื้อเพิ่ม
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
                            <asp:DropDownList ID="ddlStock"  runat="server" Width="195"></asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCate" runat="server" AutoPostBack="true" Width="195" 
                                onselectedindexchanged="ddlCate_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                            ประเภทอุปกรณ์ย่อย
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubCate" runat="server" Width="195"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCode" runat="server" MaxLength="20" Width="190"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            รายการสินค้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="PRPO\ReorderPointReport.rdlc">
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
