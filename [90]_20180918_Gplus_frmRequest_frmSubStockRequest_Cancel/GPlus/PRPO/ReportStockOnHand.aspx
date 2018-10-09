<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ReportStockOnHand.aspx.cs" Inherits="GPlus.PRPO.ReportStockOnHand" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานจำนวนสินค้าคงเหลือ
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
                        <td>
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" 
                                    DataTextField="MaterialType_Name" DataValueField="MaterialType_ID"
                                    AutoPostBack="true" 
                                    onselectedindexchanged="ddlMaterialType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
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
                        <td style="width: 130px;" align="right">
                            รายการสินค้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--<td style="width: 130px;" align="right">
                            จำนวนยอดคงคลัง
                        </td>
                        <td>
                            <table style='border: solid 1px #6E6E6E; width: 250px; height: 25px'>
                                <tr>
                                    <td>
                                        <input type="radio" name="groupTotal" id="radio_Morethan" value="M" checked />
                                        จำนวน > 0
                                        <input type="radio" name="groupTotal" id="radio_Equal" value="E" />
                                        จำนวน = 0
                                        <input type="radio" name="groupTotal" id="radio_All" value="A" />
                                        ทั้งหมด
                                    </td>
                                </tr>
                            </table>
                        </td>--%>
                        <td colspan="2">
                            <fieldset style="width:310px">
                                <legend>จำนวนยอดคงคลัง</legend>
                                 <asp:RadioButtonList ID="rdbOnHand" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="จำนวน > 0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="จำนวน = 0"></asp:ListItem>
                                    <asp:ListItem Text="ทั้งหมด"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <%--<td style="width: 130px;" align="right">
                            สถานะรายการสินค้า
                        </td>--%>
                      <%--  <td>
                            <table style='border: solid 1px #6E6E6E; width: 250px; height: 25px'>
                                <tr>
                                    <td>
                                        <input type="radio" name="groupStatus" id="radio_enable" value="SE" checked />
                                        ใช้งาน
                                        <input type="radio" name="groupStatus" id="radio_disable" value="SD" />
                                        ยกเลิก
                                        <input type="radio" name="groupStatus" id="radio_AllStatus" value="SA" />
                                        ทั้งหมด
                                    </td>
                                </tr>
                            </table>
                        </td>--%>
                        <td colspan="2">
                            <fieldset style="width:310px">
                                <legend>สถานะรายการสินค้า</legend>
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="InActive"></asp:ListItem>
                                    <asp:ListItem Text="All"></asp:ListItem>
                                </asp:RadioButtonList>
                                 
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
                    <LocalReport ReportPath="PRPO\ReportStockOnHand.rdlc">
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

