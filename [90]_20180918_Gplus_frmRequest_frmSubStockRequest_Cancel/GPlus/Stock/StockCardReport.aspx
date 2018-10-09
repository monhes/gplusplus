<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="StockCardReport.aspx.cs" Inherits="GPlus.Stock.StockCardReport" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../UserControls/ItemControlMaterial.ascx" TagName="ItemControlMaterial" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงาน Stock Card
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            คลังสินค้า
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="StockType_Name"
                                DataValueField="StockType_ID">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <%--<td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCate" runat="server" Width="195">
                            </asp:DropDownList>
                        </td>--%>
                        <td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" DataTextField="MaterialType_Name"
                                DataValueField="MaterialType_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterialType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                            ประเภทอุปกรณ์ย่อย
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubMaterialType" runat="server" Width="195" DataTextField="SubMaterialType_Name"
                                DataValueField="SubMaterialType_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlSubMaterialType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รายการสินค้า
                        </td>
                        <td colspan="3">
                            <uc1:ItemControlMaterial ID="ItemControlMaterial" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left" style=" padding-left:75px">
                            <fieldset style="width: 480px;">
                                <legend>ช่วงเวลา </legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbDate" runat="server" Text="  วันที่ : " GroupName="Group1"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <uc2:CalendarControl ID="dtStart" runat="server" />
                                        </td>
                                        <td style="width: 50px;" align="right">
                                            ถึงวันที่ :
                                        </td>
                                        <td align="left">
                                            <uc2:CalendarControl ID="dtStop" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbMonth" runat="server" Text="  เดือน " GroupName="Group1"></asp:RadioButton>
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
                                            <%--&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp;
                                            
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
                                            <asp:DropDownList ID="ddlYearEnd" runat="server">
                                            </asp:DropDownList>--%>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="Stock\StockCardReport.rdlc">
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
