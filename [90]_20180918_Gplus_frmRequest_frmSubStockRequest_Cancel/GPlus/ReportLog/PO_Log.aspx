<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="PO_Log.aspx.cs" Inherits="GPlus.ReportLog.PO_Log" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 95px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ใบส่งซื้อ(PO)</td>
        </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%" >
                    <tr style=" height:35px">
                        
                        <td>
                            <asp:DropDownList ID="D3" runat="server" Width="125px">
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                       <%-- &nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp;--%>
                        <td align="right" >
                           วันที่ :
                        </td>
                        <td style="width:50px">
                           <uc2:CalendarControl ID="dtStart" runat="server" />
                        </td>
                        <td align="right" class="style1">
                           ถึงวันที่ :
                        </td>
                        <td align="left">
                            <uc2:CalendarControl ID="dtStop" runat="server" />
                        </td>
                        
                        
                            <td colspan="1" align="center">
                            <fieldset style="width:200px">
                                <legend>สถานะการส่ง</legend>
                                 <asp:RadioButtonList ID="rdbType" runat="server" RepeatDirection="Horizontal"  AutoPostBack="false">
                                    <asp:ListItem Text="ผิดพลาด"  Value="0"></asp:ListItem>
                                    <asp:ListItem Text="สำเร็จ" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ทั้งหมด" Value="2" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                            </td>
                       
                    </tr>
                    <tr>
                            <td></td>
                            <td  align="right">
                            เลขที่ PO :
                            </td>
                            <td colspan="6">
                            <asp:TextBox ID="TextBox3" runat="server" MaxLength="20" Width="115px"></asp:TextBox>
                            </td>
           
                    </tr>
                     <tr style="margin-top : 35px; align-self:center;">
                        <td colspan="7" align="center">
                            <asp:Button ID="Button3" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btn_summit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="Button4" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btn_clear_Click" />
                        </td>
                    </tr>
                </table>
                            
                           
                                <center>
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="height:auto; width:auto;" Visible="False">
                                </rsweb:ReportViewer>
                                </center>
                         

                </td><%--class table--%>
            </tr>
                
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
        </table>

    <%--<div id="report3" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D3" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_3" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image9" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="date_end_3" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image10" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
                        </td>
                        <td style="border:hidden">สถานะการส่ง</td>
                        <td style="border:hidden">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" style="border:hidden">
                                <asp:ListItem Value="0">ผิดพลาด</asp:ListItem>
                                <asp:ListItem Value="1">สำเร็จ</asp:ListItem>
                                <asp:ListItem Value="2">ทั้งหมด</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="border:hidden">
                            เลขที่ PO
                        </td>
                        <td  style="border:hidden">
                            <asp:TextBox ID="TextBox33" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td style="border:hidden">
                            เลขที่อ้างอิง EP
                        </td>
                       
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="Button31" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="Button41" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
                            </center>
                        </td>
                    </tr>
                </table>
                    </center>
            </div>
        </div>--%>
        <%--<div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;width:1000px;height=1000px">--%>
            <%--<table style="border-style: solid; border-width: 1px; padding: 20px;  border-radius: 25px;width:1000px;">
                <tr>
                    <td>
                        <br />
                        <center>
                            <rsweb:ReportViewer ID="ReportViewer2" runat="server" Height="400px" Width="900px">
                            </rsweb:ReportViewer>
                        </center>
                    </td>
                </tr>
        </table>--%>
        <%--</div>--%>
</asp:Content>
