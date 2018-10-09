<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="GPlus.ReportLog.WebForm1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                คำสั่งใบเบิก</td>
        </tr>
    </table>


    <center>
        <div class="tab">
            <a href="Report.aspx?tab=0&amp;tabname=report1">
            <button class="tablinks" onclick="openCity(event, 'report1')">
                ใบเบิก
            </button>
            </a><a href="Report.aspx?tab=1&amp;tabname=report2">
            <button class="tablinks" onclick="openCity(event, 'report2')">
                การคืนของจากหน่วยงาน
            </button>
            </a><a href="Report.aspx?tab=2&amp;tabname=report3">
            <button class="tablinks" onclick="openCity(event, 'report3')">
                ใบสั่งซื้อ (PO)
            </button>
            </a><a href="Report.aspx?tab=3&amp;tabname=report4">
            <button class="tablinks" onclick="openCity(event, 'report4')">
                รับของเข้าคลัง
            </button>
            </a><a href="Report.aspx?tab=4&amp;tabname=report5">
            <button class="tablinks" onclick="openCity(event, 'report5')">
                ข้อมูลวัสดุอุปกรณ์
            </button>
            </a><a href="Report.aspx?tab=5&amp;tabname=report6">
            <button class="tablinks" onclick="openCity(event, 'report6')">
                หน่วยบรรจุ
            </button>
            </a><a href="Report.aspx?tab=6&amp;tabname=report7">
            <button class="tablinks" onclick="openCity(event, 'report7')">
                ผู้ขาย (Supplier)
            </button>
            </a><a href="Report.aspx?tab=7&amp;tabname=report8">
            <button class="tablinks" onclick="openCity(event, 'report8')">
                หน่วยงาน
            </button>
            </a>
        </div>
        <%--<asp:ToolkitScriptManager ID="ScriptManager1" runat="server" />--%>
        <div id="report1" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
            </div>
        </div>
        <%--<div id="report2" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D2" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_2" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image11" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_2" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image12" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
                            </td>
                            <td style="border:hidden">สถานะการส่ง</td>
                            <td style="border:hidden">
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal" style="border:hidden">
                                    <asp:ListItem Value="0">ผิดพลาด</asp:ListItem>
                                    <asp:ListItem Value="1">สำเร็จ</asp:ListItem>
                                    <asp:ListItem Value="2">ทั้งหมด</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="border:hidden">เลขที่ใบเบิก </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="Text_Req_No" runat="server" Text=""></asp:TextBox>
                            </td>
                            <td style="border:hidden">รหัสวัสดุอุปกรณ์ </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="Text_Matl_Code" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="Button5" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="Button6" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>//udohere
        </div>--%>
        <div id="report3" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D3" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_3" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image9" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_3" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image10" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
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
                            <td style="border:hidden">เลขที่ PO </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="TextBox3" runat="server" Text=""></asp:TextBox>
                            </td>
                            <td style="border:hidden">เลขที่อ้างอิง EP </td>
                        <%--<td style="border:hidden">
                            <asp:TextBox ID="TextBox4" runat="server" Text=""></asp:TextBox>
                        </td>--%>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="Button3" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="Button4" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </div>
        <div id="report4" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D4" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_4" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image7" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_4" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image8" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
                            </td>
                            <td style="border:hidden">สถานะการส่ง</td>
                            <td style="border:hidden">
                                <asp:RadioButtonList ID="rdl4" runat="server" RepeatDirection="Horizontal" style="border:hidden">
                                    <asp:ListItem Value="0">ผิดพลาด</asp:ListItem>
                                    <asp:ListItem Value="1">สำเร็จ</asp:ListItem>
                                    <asp:ListItem Value="2">ทั้งหมด</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="border:hidden">เลขที่ PO </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="txt_po4" runat="server" Text=""></asp:TextBox>
                            </td>
                            <td style="border:hidden">เลขที่อ้างอิง EP </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="txt_ep4" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="Button1" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="Button2" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </div>
        <div id="report5" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D5" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_5" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image3" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_5" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image4" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
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
                            <td style="border:hidden">เลขที่ PO </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="txt_po5" runat="server" Text=""></asp:TextBox>
                            </td>
                            <td style="border:hidden">เลขที่อ้างอิง EP </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="txt_ep5" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="btn_summit5" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="btn_clear5" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
        </div>
        <div id="report6" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D6" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_6" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image5" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_6" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image6" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
                            </td>
                            <td style="border:hidden">สถานะการส่ง</td>
                            <td style="border:hidden">
                                <asp:RadioButtonList ID="rdl6" runat="server" RepeatDirection="Horizontal" style="border:hidden">
                                    <asp:ListItem Value="0">ผิดพลาด</asp:ListItem>
                                    <asp:ListItem Value="1">สำเร็จ</asp:ListItem>
                                    <asp:ListItem Value="2">ทั้งหมด</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="border:hidden">รหัสวัสดุอุปกรณ์ </td>
                            <td colspan="3" style="border:hidden">
                                <asp:TextBox ID="txt_MaterialCode" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="btn_summit6" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="btn_clear6" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
        </div>
        <div id="report7" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                    <table>
                        <tr>
                            <td style="border:hidden">
                                <asp:DropDownList ID="D7" runat="server">
                                    <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                    <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="border:hidden">
                                <asp:TextBox ID="date_start_7" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                                &nbsp;-&nbsp;
                                <asp:TextBox ID="date_end_7" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                                <asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Style="margin-right: 100px" Width="24px" />
                            </td>
                            <td style="border:hidden">สถานะการส่ง</td>
                            <td style="border:hidden">
                                <asp:RadioButtonList ID="rdl7" runat="server" RepeatDirection="Horizontal" style="border:hidden">
                                    <asp:ListItem Value="0">ผิดพลาด</asp:ListItem>
                                    <asp:ListItem Value="1">สำเร็จ</asp:ListItem>
                                    <asp:ListItem Value="2">ทั้งหมด</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="border:hidden">รหัส Supplier </td>
                            <td colspan="3" style="border:hidden">
                                <asp:TextBox ID="txt_Supplier7" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border:hidden">
                                <center>
                                    <asp:Button ID="btn_summit7" runat="server" OnClick="btn_summit_Click" Text="ค้นหา" />
                                    <asp:Button ID="btn_clear7" runat="server" OnClick="btn_clear_Click" Text="ยกเลิก" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <br />
        </div>
        <div id="report8" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
            </div>
            <br />
        </div>

        <%--<div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;width:1000px;height=1000px">--%>
            <table style="border-style: solid; border-width: 1px; padding: 20px;  border-radius: 25px;width:1000px;">
                <tr>
                    <td>
                        <br />
                        <center>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="400px" Width="900px">
                            </rsweb:ReportViewer>
                        </center>
                    </td>
                </tr>
        </table>
        <%--</div>--%>
    </center>

</asp:Content>
