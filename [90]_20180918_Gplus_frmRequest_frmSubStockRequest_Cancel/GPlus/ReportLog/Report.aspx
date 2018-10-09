<%@ Title=""  Language="C#"  AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="GPlus.ReportLog.Report"
    MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css">
    <%--    <script type="text/javascript" src="../js/ui/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/ui/jquery.min.js"></script>--%>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <%--    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
    <script type="text/javascript" src="../js/jquery-1.12.4.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>

    <script>
        var vx = 'report1', j = 0;
        //$(function () {
        //    $("#date_start_1").datepicker();
        //    lang: 'th';
        //});
        //$(function () {
        //    $("#date_end_1").datepicker();
        //    lang: 'th';
        //});
        $(function () {
            $("#date_start_2").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_2").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_start_3").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_3").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_start_4").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_4").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_start_5").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_5").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_start_6").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_6").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_start_7").datepicker();
            lang: 'th';
        });
        $(function () {
            $("#date_end_7").datepicker();
            lang: 'th';
        });
        //$(function () {
        //    $("#date_start_8").datepicker();
        //    lang: 'th';
        //});
        //$(function () {
        //    $("#date_end_8").datepicker();
        //    lang: 'th';
        //});

    </script>

    <script>

        $(document).ready(function () {
            //$('#example').DataTable();
            var url_string = window.location.href;
            var url = new URL(url_string);

            var strTab = url.searchParams.get("tab");
            var strTabname = url.searchParams.get("tabname");

            if (strTab == null) {
                strTab = 0;
            }

            if (strTabname == null) {
                strTabname = 'report1';
            }

            tabcontent = document.getElementsByClassName("tabcontent");
            document.getElementById(strTabname).style.display = "block";

            tablinks = document.getElementsByClassName("tablinks");
            tablinks[strTab].className += " active";
        });

        function openCity(evt, cityName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
                if (tabcontent[i].getAttribute('id') == cityName) {
                    j = i;
                    document.getElementsByName("city").value = i;
                    vx = cityName;
                    document.getElementsByName("ind").value = cityName;
                }
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(cityName).style.display = "block";
            tablinks[j].className += " active";
            //evt.currentTarget.className += " active";
        }
    </script>

</head>

<style>
    thead {
        text-align: "center";
        background-color: aliceblue;
    }

    #form1 {
        height: 193px;
    }

    body {
        font-family: Arial;
    }

    /* Style the tab */
    .tab {
        overflow: hidden;
        border: 1px solid #ccc;
        background-color: #f1f1f1;
    }

        /* Style the buttons inside the tab */
        .tab button {
            background-color: inherit;
            float: left;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 14px 16px;
            transition: 0.3s;
            font-size: 17px;
        }

            /* Change background color of buttons on hover */
            .tab button:hover {
                background-color: pink;
            }

            /* Create an active/current tablink class */
            .tab button.active {
                background-color: deeppink;
            }

    /* Style the tab content */
    .tabcontent {
        display: none;
        padding: 6px 12px;
        border: 1px solid #ccc;
        border-top: none;
    }

    table, th, td {
        /*border: 1px solid black;*/
        border-collapse: collapse;
    }

    th, td {
        padding: 5px;
        text-align: left;
    }
</style>

<body>

    <center>

        <div class="tab">

        <a href="Report.aspx?tab=0&tabname=report1">
            <button class="tablinks" onclick="openCity(event, 'report1')">ใบเบิก</button>
        </a>
        <a href="Report.aspx?tab=1&tabname=report2">
            <button class="tablinks" onclick="openCity(event, 'report2')">การคืนของจากหน่วยงาน</button>
        </a>
        <a href="Report.aspx?tab=2&tabname=report3">
            <button class="tablinks" onclick="openCity(event, 'report3')">ใบสั่งซื้อ (PO)</button>
        </a>
        <a href="Report.aspx?tab=3&tabname=report4">
            <button class="tablinks" onclick="openCity(event, 'report4')">รับของเข้าคลัง</button>
        </a>
        <a href="Report.aspx?tab=4&tabname=report5">
            <button class="tablinks" onclick="openCity(event, 'report5')">ข้อมูลวัสดุอุปกรณ์</button>
        </a>
        <a href="Report.aspx?tab=5&tabname=report6">
            <button class="tablinks" onclick="openCity(event, 'report6')">หน่วยบรรจุ</button>
        </a>
        <a href="Report.aspx?tab=6&tabname=report7">
            <button class="tablinks" onclick="openCity(event, 'report7')">ผู้ขาย (Supplier)</button>
        </a>
        <a href="Report.aspx?tab=7&tabname=report8">
            <button class="tablinks" onclick="openCity(event, 'report8')">หน่วยงาน</button>
        </a>

    </div>

    <form id="form1" runat="server">
         <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />

        <div id="report1" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
            </div>
        </div>

        <div id="report2" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D2" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_2" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image11" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="date_end_2" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image12" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
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
                        <td style="border:hidden">
                            เลขที่ใบเบิก
                        </td>
                        <td  style="border:hidden">
                            <asp:TextBox ID="Text_Req_No" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td style="border:hidden">
                            รหัสวัสดุอุปกรณ์
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="Text_Matl_Code" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="Button5" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="Button6" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
                            </center>
                        </td>
                    </tr>
                </table>
                    </center>
            </div>
        </div>

        <div id="report3" class="tabcontent" style="width:1000px">
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
                            <asp:TextBox ID="TextBox3" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td style="border:hidden">
                            เลขที่อ้างอิง EP
                        </td>
                        <%--<td style="border:hidden">
                            <asp:TextBox ID="TextBox4" runat="server" Text=""></asp:TextBox>
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="Button3" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="Button4" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
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
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D4" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_4" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image7" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="date_end_4" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image8" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
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
                        <td style="border:hidden">
                            เลขที่ PO
                        </td>
                        <td  style="border:hidden">
                            <asp:TextBox ID="txt_po4" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td style="border:hidden">
                            เลขที่อ้างอิง EP
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="txt_ep4" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="Button1" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="Button2" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
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
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D5" runat="server" >
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

        </div>

        <div id="report6" class="tabcontent" style="width:1000px">
            <div style="border-style: solid; border-width: 1px; padding: 20px; width: 100%; height: 25%; border-radius: 25px;">
                <center>
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D6" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_6" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image5" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                <asp:TextBox ID="date_end_6" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image6" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
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
                        <td style="border:hidden">
                            รหัสวัสดุอุปกรณ์
                        </td>
                        <td colspan="3" style="border:hidden">
                            <asp:TextBox ID="txt_MaterialCode" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="btn_summit6" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="btn_clear6" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
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
                <table >
                    <tr>
                        <td style="border:hidden">
                            <asp:DropDownList ID="D7" runat="server" >
                                <asp:ListItem Value="0">วันที่ EP ส่งข้อมูล</asp:ListItem>
                                <asp:ListItem Value="1">วันที่รับเข้าระบบ</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="border:hidden">
                            <asp:TextBox ID="date_start_7" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" />
                            &nbsp;-&nbsp;
                <asp:TextBox ID="date_end_7" runat="server" Text="วัน/เดือน/ปี"></asp:TextBox>
                            <asp:Image ID="Image2" runat="server" ImageAlign="AbsMiddle" ImageUrl="https://i1.wp.com/ibsschool.net/eng/wp-content/uploads/2017/05/calendar-icon-e1442284038630.png" Width="24px" Style="margin-right: 100px" />
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
                        <td style="border:hidden">
                            รหัส Supplier
                        </td>
                        <td colspan="3" style="border:hidden">
                            <asp:TextBox ID="txt_Supplier7" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="border:hidden">
                            <center>
                                <asp:Button ID="btn_summit7" runat="server" Text="ค้นหา" OnClick="btn_summit_Click"></asp:Button>
                                <asp:Button ID="btn_clear7" runat="server" Text="ยกเลิก" OnClick="btn_clear_Click"></asp:Button>
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
                        <br>
                        <center>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="400px" Width="900px"></rsweb:ReportViewer>
                    </center>
                            </td>
                </tr>
            </table>
        <%--</div>--%>
    </form>
    </center>

</body>
</html>
