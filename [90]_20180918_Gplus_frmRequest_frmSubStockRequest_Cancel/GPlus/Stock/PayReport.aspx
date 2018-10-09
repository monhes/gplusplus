<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="PayReport.aspx.cs" Inherits="GPlus.PayReport" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div align="center" style=" width :auto;">
    <table cellpadding="0" cellspacing="0" width="805">

         <tr>
            <td class="tableHeader">
                ค้นหาข้อมูลการรับของเข้าคลัง
            </td>
        </tr>
                  

                         <td class="tableBody">
                           <table align="center"  width="90%">
                                <tr>
                        <td align="right" style="width:40%">
                            คลังสินค้า
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStock" runat="server" Width="195" DataTextField="Stock_Name" DataValueField="Stock_Id" Enabled="true">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                         <td align="right"  style="width:40%">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td  align="left">
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" 
                                    DataTextField="Cat_Name" DataValueField="Cate_ID"
                                    >
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                    <tr>
                        <td align="right"  style="width:40%">
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

                            <%--    <asp:ListItem Value="2560">2560</asp:ListItem>
                                <asp:ListItem Value="2559">2559</asp:ListItem>
                                <asp:ListItem Value="2558">2558</asp:ListItem>
                                <asp:ListItem Value="2557">2557</asp:ListItem>
                                <asp:ListItem Selected =True Value="2556">2556</asp:ListItem>
                                <asp:ListItem Value="2555">2555</asp:ListItem>
                                <asp:ListItem Value="2554">2554</asp:ListItem>
                                <asp:ListItem Value="2553">2553</asp:ListItem>
                                <asp:ListItem Value="2552">2552</asp:ListItem>
                                <asp:ListItem Value="2551">2551</asp:ListItem>
                                <asp:ListItem Value="2550">2550</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" class="style1">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancel_Click" />
                        </td>
                    </tr>

                           </table>
                         </td>

               
                </table>
    
    </div>
    <div style=" background-color:White;">
        <rsweb:ReportViewer ID="ReportViewer" runat="server" Width="100%"
                    Height="350px" Font-Names="Verdana" Font-Size="8pt" 
                    InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                    WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="Stock/Report2.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
    </div>
 
</asp:Content>
