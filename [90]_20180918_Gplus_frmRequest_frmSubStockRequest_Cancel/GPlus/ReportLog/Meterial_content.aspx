<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Meterial_content.aspx.cs" Inherits="GPlus.ReportLog.Meterial_content" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ข้อมูลวัสดุอุปกรณ์
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td>
                           <asp:DropDownList ID="D5" runat="server" Width="125px">
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                           วันที่ :
                        </td>
                        <td>
                          
                           <uc2:CalendarControl ID="CalendarControl1" runat="server" />
                        </td>
                        <td align="right">
                           ถึงวันที่ :
                        </td>
                        <td align="left">
                           
                            <uc2:CalendarControl ID="CalendarControl2" runat="server" />
                        </td>
                        
                            <td colspan="1" align="center">
                            <fieldset style="width:200px">
                                <legend>สถานะการส่ง</legend>
                                 <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"  AutoPostBack="false">
                                    <asp:ListItem Text="ผิดพลาด"  Value="0"></asp:ListItem>
                                    <asp:ListItem Text="สำเร็จ" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ทั้งหมด" Value="2" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                            </td>
                        

                    </tr><%--6td--%>
                    <tr>
                            
                            <td  align="right" colspan="2">
                            รหัสวัสดุอุปกรณ์ :
                            </td>
                            <td colspan="1">
                            <asp:TextBox ID="txt_po5" runat="server" MaxLength="20" Width="115px"></asp:TextBox>
                            </td>
                            <td></td>
                            <td  align="right">
                            
                            </td>
                            <td colspan="2">
                            
                            </td>
                            <td></td>
                    </tr>
                    <tr>
                       
                        <td colspan="6" align="center" style="margin-top:50px">
                            <asp:Button ID="Button1" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btn_summit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btn_clear_Click" />
                        </td>
                          
                    </tr>
                </table>    
                            
                            <center>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="height:auto; width:auto;" Visible="False">
                            </rsweb:ReportViewer>
                            </center>

            </td>
         </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>

    <%--<div id="report5" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D51" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_5" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image3" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="date_end_5" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image4" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
                        </td>
                        <td style="border:hidden">สถานะการส่ง</td>
                        <td style="border:hidden">
                            <asp:RadioButtonList ID="rdl5" runat="server" RepeatDirection="Horizontal" style="border:hidden">
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
                            <asp:TextBox ID="txt_po5" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td style="border:hidden">
                            เลขที่อ้างอิง EP
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="txt_ep5" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="btn_summit5" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="btn_clear5" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
                            </center>
                        </td>
                    </tr>
                </table>
                    </center>
            </div>
            <br />

        </div>--%>
  <%--  <table style="border-style: solid; border-width: 1px; padding: 20px;  border-radius: 25px;width:1000px;"> 
                <tr>
                    <td>    
                        <br>
                        <center>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="400px" Width="900px"></rsweb:ReportViewer>
                    </center>
                            </td>
                </tr>
            </table>--%>
</asp:Content>
