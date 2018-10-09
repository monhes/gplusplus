<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Conversion_content.aspx.cs" Inherits="GPlus.ReportLog.Conversion_content" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                หน่วยบรรจุ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td>
                           <asp:DropDownList ID="D6" runat="server" Width="125px">
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
                                    <asp:ListItem Text="ทั้งหมด" Selected="True" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                            </td>
                        

                    </tr><%--6td--%>
                    <tr>
                            <td  align="right"colspan="2">
                            รหัสวัสดุอุปกรณ์ :
                            </td>
                            <td colspan="1">
                            <asp:TextBox ID="txt_MaterialCode" runat="server" MaxLength="20" Width="115px"></asp:TextBox>
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

</asp:Content>
