<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Routine.aspx.cs" Inherits="GPlus.PRPO.Routine" %>


<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl2.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../Script/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="../Script/jquery-ui.js"></script>
    
    <script type="text/javascript" src="../Script/gridviewScroll.min.js"></script>


    <style>
        .tableHeader2
        {
            height: 29px;
            background-image: url(../Images/Stock/box_data_head2.png);
            background-repeat: no-repeat;
            padding-left: 10px;
            font-weight: bold;
            color: #EC467E;
        }
        .tableBody2
        {
            background-image: url(../Images/Stock/box_data_body2.png);
            background-repeat: repeat-y;
            padding-left: 10px;
            padding-right: 10px;
            padding-top: 5px;
        }
        .tableFooter2
        {
            background-image: url(../Images/Stock/box_data_foot2.png);
            background-repeat: no-repeat;
            height: 12px;
        }
        
        .GridviewScrollHeader TH, .GridviewScrollHeader TD
        {
            font-weight: bold;
            white-space: nowrap;
            text-align: center;
            vertical-align: middle;
        }
        .GridviewScrollItem TD
        {
            white-space: nowrap;
        }
        .GridviewScrollPager
        {
        }
        .GridviewScrollPager TD
        {
        }
        .GridviewScrollPager A
        {
        }
        .GridviewScrollPager SPAN
        {
        }
    </style>


    


   <%-- <script src="../../autocomplete.asmx/js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function isNumericKey(e) {
            var charInp = window.event.keyCode;
             
            if (charInp > 31 && (charInp < 48 || charInp > 57)  ) {
                return false;
            }
             return true;
         }


         function Confirm() {
//             var confirm_value = document.createElement("INPUT");
//             confirm_value.type = "hidden";
//             confirm_value.name = "confirm_value";
             if (confirm("มีข้อมูลใบเบิกที่มีการเปลี่ยนแปลง ต้องปรับปรุงข้อมูลหรือไม่")) {
                 //confirm_value.value = "Yes";

                 window.location.assign(document.URL + "&type=Y");
                 
                 //alert(document.URL);

             } else {
                 //confirm_value.value = "No";
                 // ดูข้อมูลเดิม
                 window.location.assign(document.URL + "&type=N");
             }
             //document.forms[0].appendChild(confirm_value);
         }


         function Confirm2(inputdata) {

             if (inputdata == 'Y') {
                
                window.location.assign(document.URL + "&type=Y");
             
             } else {

                 window.location.assign(document.URL + "&type=N");
             }

         }


//         var dialogConfirmed = false;

//         function ConfirmDialog() {
//             if (!dialogConfirmed) {
//                 

//                 $('#dialog').dialog
//                ({
//                    height: 110,
//                    modal: true,
//                    resizable: false,
//                    draggable: false,
//                    close: function (event, ui) { $('body').find('#dialog').remove(); },
//                    buttons:
//                    {
//                        'ยืนยัน': function () {
//                            $(this).dialog('close');
//                            dialogConfirmed = true;
//                            //if (obj) obj.click();
//                        },
//                        'ดูข้อมูลเดิม': function () {
//                            $(this).dialog('close');
//                        }
//                    }
//                });
//             }

//             return dialogConfirmed;
//         }


    </script>

</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <%--<ajaxtoolkit:toolkitscriptmanager runat="server" ID="ScriptManager1">
        <Services>
            <asp:ServiceReference Path="~/autocomplete.asmx" />
        </Services>
    </ajaxtoolkit:toolkitscriptmanager>--%>
<asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
    <center>


    <div  id="dialog" title="aaa">
    
    </div>
        
                <table width="1024" cellpadding="0" cellspacing="0">
                    <%--<tr>
                        <td style="width: 201px;" align="left" valign="middle">
                            <img src="../images/logo_Muangthai.png" />
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td style="width: 799px;" align="right" valign="middle">
                            
                            <img src="../images/logo_gplus.png" />
                        </td>
                    </tr>
                    <tr>
                <td></td>
                <td colspan="2" align="right" style="font-size:11pt;">
                    ยินดีต้อนรับ คุณ <asp:Label ID="lblUser" runat="server"></asp:Label>
                    &nbsp;<asp:HyperLink ID="hplLogout" runat="server" Text="ออกจากระบบ" NavigateUrl="~/Default.aspx"></asp:HyperLink>
                </td>
            </tr>--%>
                    <tr align="left" valign="top">
                        <td colspan="3" align="center">
                            <asp:Panel ID="Routine_Panel" runat="server">
                                <table cellpadding="0" cellspacing="0" width="1024">
                                    <tr>
                                        <td class="tableHeader2" align="center">
                                            Routine การจ่ายวัน<asp:Label ID="lbl_day" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tableBody2">
                                            <div id="div_search" runat="server">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="24%" align="right">
                                                            วันที่จ่าย
                                                        </td>
                                                        <td width="15%" align="center">
                                                            <uc2:CalendarControl ID="Routine_Date" runat="server" />
                                                        </td>
                                                        <td width="15%" align="center">
                                                            วัน<asp:Label ID="lbl_Routine_day" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                        <td width="46%" align="left">
                                                            <asp:Button ID="btn_routine_search" runat="server" Text="ค้นหา" OnClick="btn_routine_search_Click" />&nbsp;&nbsp;
                                                            <asp:Button ID="btn_exit_1" runat="server" Text="ออกจากหน้าจอ" SkinID="ButtonMiddle"
                                                                OnClick="btn_exit_Click" />
                                                            <asp:HiddenField ID="hid_Summary_ReqId" runat="server" />
                                                            <asp:HiddenField ID="hid_Summary_Date" runat="server" />
                                                            <asp:HiddenField ID="hid_Day_of_Week" runat="server" />
                                                            <asp:HiddenField ID="hid_Stock_id" runat="server" />
                                                            <asp:HiddenField ID="hid_status" runat="server" />
                                                            <asp:HiddenField ID="hid_chk_postback" runat="server" />
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div id="div_data" runat="server" visible="false">
                                                        <table width="1000px">
                                                            <tr>
                                                                <td width="105px">
                                                                
                                                                </td>
                                                                <td align="right" width="173px">
                                                                    จำนวนใบเบิกทั้งหมด
                                                                </td>
                                                                <td align="center" width="60px">
                                                                    <asp:TextBox ID="txt_Number_Of_Req" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="10px">
                                                                    ใบ
                                                                </td>
                                                                <td align="right" width="125px">
                                                                    เบิกตามรอบ
                                                                </td>
                                                                <td align="center" width="60px">
                                                                    <asp:TextBox ID="txt_Number_Of_Routine" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="10px">
                                                                    ใบ
                                                                </td>
                                                                <td align="right" width="85px">
                                                                    เบิกด่วน
                                                                </td>
                                                                <td align="center" width="60px">
                                                                    <asp:TextBox ID="txt_Number_Of_Stat" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="10px">
                                                                    ใบ
                                                                </td>
                                                                <td align="right" width="110px" style="color: Red;">
                                                                    เบิกผิดวัน
                                                                </td>
                                                                <td align="center" width="60px">
                                                                    <asp:TextBox ID="txt_Number_Of_NotRoutine" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="27px">
                                                                    ใบ
                                                                </td>
                                                                <td width="105px">
                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="105px">
                                                                
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:Button ID="btn_back" runat="server" SkinID="ButtonMiddleLong" Visible="false" />
                                                                </td>
                                                                <td colspan="6">
                                                                    <fieldset style="width: 400px">
                                                                        <legend style="color: Blue;">ใบเบิกค้างจ่าย</legend>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="right" width="87px">
                                                                                    เบิกตามรอบ
                                                                                </td>
                                                                                <td align="center" width="60px">
                                                                                    <asp:TextBox ID="txt_Number_Pending_InDue" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="10px">
                                                                                    ใบ
                                                                                </td>
                                                                                <td align="right" width="150px">
                                                                                    เกินระยะเวลากำหนด
                                                                                </td>
                                                                                <td align="center" width="60px">
                                                                                    <asp:TextBox ID="txt_Number_Pending_OutDue" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left" width="33px">
                                                                                    ใบ
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td colspan="3" align="center">
                                                                    <asp:Label ID="lbl_status" runat="server" Text="รอการ Allocate" ForeColor="Red" Font-Bold="true"
                                                                        Font-Size="Medium"></asp:Label>
                                                                </td>
                                                                <td width="105px">
                                                                
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <div id="div_gvResult_routine" runat="server" >
                                                            <asp:GridView SkinID="Grid2" ID="gvResult_routine" runat="server" CellSpacing="0"
                                                                CellPadding="0" AutoGenerateColumns="false" EnableModelValidation="true" Width="100%"
                                                                GridLines="None" OnRowCreated="gvResult_routine_RowCreated" EnableViewState="false"
                                                                OnRowDataBound="gvResult_routine_RowDataBound">

                                                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                                                <RowStyle CssClass="GridviewScrollItem" />

                                                            </asp:GridView>
                                                        </div>
                                                        <%--<asp:GridView SkinID="Grid2" ID="gvResult_routine" runat="server" CellSpacing="0"
                                                    CellPadding="0" AutoGenerateColumns="false" EnableModelValidation="True" Width="100%"
                                                    GridLines="None" OnRowCreated="gvResult_routine_RowCreated">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="ลำดับที่" DataField="Inv_ItemCode" />
                                                        <asp:BoundField HeaderText="รายการ" DataField="Inv_ItemCode" />
                                                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" />
                                                        <asp:BoundField HeaderText="หน่วยนับ" DataField="Inv_ItemName" />
                                                        <asp:TemplateField HeaderText="เบิก">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_req_1" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="จ่าย">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_pay_1" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="เบิก">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_req_2" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="จ่าย">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_pay_2" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="เบิก">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_req_3" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="จ่าย">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_pay_3" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="เบิก">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_req_4" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="จ่าย">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_pay_4" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="เบิก">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_req_5" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="จ่าย">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_pay_5" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="รวมเบิก" DataField="Asset_Status" />
                                                        <asp:BoundField HeaderText="ยอดคงคลัง" DataField="Asset_Status" />
                                                        <asp:TemplateField HeaderText="Allocate">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_allocate" runat="server" Width="30px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>--%>
                                                        &nbsp;<br /><br />
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center" width="28%">
                                                                    <asp:Button ID="btn_Submit_Summary_Print" runat="server" Text="ยืนยันสรุปและพิมพ์"
                                                                        SkinID="ButtonMiddleLong" OnClick="btn_Submit_Summary_Print_Click" />
                                                                </td>
                                                                <td align="center" width="28%">
                                                                    <asp:Button ID="btn_Submit_Allocate_Print" runat="server" Text="ยืนยัน Allocate/พิมพ์"
                                                                        SkinID="ButtonMiddleLong" OnClick="btn_Submit_Allocate_Print_Click" />
                                                                </td>
                                                                <td align="center" width="19%">
                                                                    <asp:Button ID="btn_RePrint" runat="server" Text="พิมพ์ซ้ำ" SkinID="ButtonMiddle"
                                                                        OnClick="btn_RePrint_Click" />
                                                                </td>
                                                                <td align="center" width="19%">
                                                                    <asp:Button ID="btn_Export_Excel" runat="server" Text="Export Excel" SkinID="ButtonMiddle"
                                                                        OnClick="btn_Export_Excel_Click" />
                                                                </td>
                                                                <td align="center" width="25%">
                                                                    <asp:Button ID="btn_exit_2" runat="server" Text="ออกจากหน้าจอ" SkinID="ButtonMiddle"
                                                                        OnClick="btn_exit_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />

                                                     

                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btn_Export_Excel" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tableFooter2">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            

        <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
        <asp:ModalPopupExtender ID="LinkButton1_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
            Enabled="True" TargetControlID="LinkButton1" PopupControlID="Panel1" PopupDragHandleControlID="Panel2">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Panel ID="Panel2" runat="server">
                <div id="div_popup" runat="server" style="width:400px; background-color:#DDDDDD; margin-top:20px; margin-bottom:20px;">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <br />
                                มีข้อมูลใบเบิกที่มีการเปลี่ยนแปลง ต้องการปรับปรุงข้อมูลหรือไม่
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">

                                <%--<asp:Button ID="btn_popup_submit" runat="server" Text="ยืนยัน" OnClientClick="Confirm2('Y');" />&nbsp;
                                <asp:Button ID="btn_popup_cancel" runat="server" SkinID="ButtonMiddle" Text="ดูข้อมูลเดิม" OnClientClick="Confirm2('N');" />--%>
                                <input id="bt_yes" type="button" onclick="Confirm2('Y');" value="ยืนยัน" />&nbsp;
                                <input id="bt_no" type="button" onclick="Confirm2('N');" value="ดูข้อมูลเดิม" />
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>

        <asp:LinkButton ID="LinkButton2" runat="server"></asp:LinkButton>
        <asp:ModalPopupExtender ID="LinkButton2_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
            Enabled="True" TargetControlID="LinkButton2" PopupControlID="Panel3" PopupDragHandleControlID="Panel4" CancelControlID="btn_popup_cancel2">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel3" runat="server">
            <asp:Panel ID="Panel4" runat="server">
                <div id="div1" runat="server" style="width:400px; background-color:#DDDDDD; margin-top:20px; margin-bottom:20px;">
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <br />
                                มีข้อมูลใบเบิกที่มีการเปลี่ยนแปลง ต้องการยืนยันข้อมูลเดิมหรือไม่
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_popup_submit2" runat="server" Text="ยืนยัน" SkinID="null" 
                                    onclick="btn_popup_submit2_Click" />&nbsp;
                                <asp:Button ID="btn_popup_cancel2" runat="server" Text="ยกเลิก" SkinID="null"
                                     />
                                    <%--<input id="btn_popup_cancel2" type="button" value="ยกเลิก" />--%>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>


    </center>
    <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />
    <asp:HiddenField ID="hfScrollPositionTop" runat="server" Value="0" />


    <asp:HiddenField ID="hfgvResult_routineSV" runat="server" /> 
    <asp:HiddenField ID="hfgvResult_routineSH" runat="server" />

     <script type="text/javascript">
        window.onload = function () {
            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            //document.getElementById("<%=div_gvResult_routine.ClientID%>").scrollTop = h.value;
        }
        function SetDivPosition() {
            var intX = document.getElementById("<%=div_gvResult_routine.ClientID%>").scrollLeft;

            //var intY = document.getElementById("<%=div_gvResult_routine.ClientID%>").scrollTop;

            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            h.value = intX;

//            var i = document.getElementById("<%=hfScrollPositionTop.ClientID%>");
//            i.value = intY;
//           
        }

        function afterpostback() {

            var h = document.getElementById("<%=hfScrollPosition.ClientID%>");
            document.getElementById("<%=div_gvResult_routine.ClientID%>").scrollLeft = h.value;

//            var i = document.getElementById("<%=hfScrollPositionTop.ClientID%>");
//            document.getElementById("<%=div_gvResult_routine.ClientID%>").scrollTop = i.value;
         
        }


        $(document).ready(function () {
            gridviewScroll();
        });

        function gridviewScroll() {
            $('#<%=gvResult_routine.ClientID%>').gridviewScroll({
                width: 994,
                height: 334,
                freezesize: 4,
                headerrowcount: 2,

                arrowsize: 30,
                varrowtopimg: "../images/arrowvt.png",
                varrowbottomimg: "../images/arrowvb.png",
                harrowleftimg: "../images/arrowhl.png",
                harrowrightimg: "../images/arrowhr.png",

                startVertical: $("#<%=hfgvResult_routineSV.ClientID%>").val(),
                startHorizontal: $("#<%=hfgvResult_routineSH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfgvResult_routineSV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfgvResult_routineSH.ClientID%>").val(delta);
                }





            });
        } 

    </script> 

    </form>
</body>
</html>
