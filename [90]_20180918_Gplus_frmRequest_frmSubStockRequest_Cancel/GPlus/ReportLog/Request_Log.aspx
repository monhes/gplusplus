<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="Request_Log.aspx.cs" Inherits="GPlus.ReportLog.Request_Log" %>
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
    <script language="javascript" type="text/javascript"  src="jquery-3.3.1.min.js">
        $(function () {
            $("#date_start_2").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_2").datepicker();
            lang: 'th';
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--newD--%>
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                การคืนของจากหน่วยงาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td>
                           <asp:DropDownList ID="D2" runat="server" Width="125px">
                                <asp:ListItem Value="0" Selected >วันที่ EP ส่งข้อมูล</asp:ListItem>
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
                                    <asp:ListItem Text="ผิดพลาด"  Value="0"  ></asp:ListItem>
                                    <asp:ListItem Text="สำเร็จ" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ทั้งหมด"   Value="2" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                            </td>
                        

                    </tr><%--6td--%>
                    <tr>
                            <td></td>
                            <td  align="right">
                            เลขที่ใบเบิก :
                            </td>
                            <td colspan="1">
                            <asp:TextBox ID="Text_Req_No" runat="server" MaxLength="20" Width="115px"></asp:TextBox>
                            </td>
                            
                            <td  align="right">
                            รหัสวัสดุอุปกรณ์ :
                            </td>
                            <td colspan="2">
                            <asp:TextBox ID="Text_Matl_Code" runat="server" MaxLength="20" Width="115px"></asp:TextBox>
                            </td>
                        
                    </tr>
                    <tr>
                       
                        <td colspan="6" align="center" style="margin-top:50px">
                            <asp:Button ID="Button3" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btn_summit_Click" />&nbsp;&nbsp;
                            <asp:Button ID="Button4" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btn_clear_Click" />
                        </td>
                          
                    </tr>
                </table>    
                            
                            <center>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" style="height:auto; width:auto;" >
                            </rsweb:ReportViewer>
                            </center>

            </td>
         </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>

    <%--newD--%>
 



</asp:Content>
