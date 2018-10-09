<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" 
CodeBehind="RoutineStock.aspx.cs" Inherits="GPlus.PRPO.RoutineStock" %>


<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl2.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function isNumericKey(e) {
            var charInp = window.event.keyCode;

            if (charInp > 31 && (charInp < 48 || charInp > 57)) {
                return false;
            }
            return true;
        }
        //        function popup() {
        //            window.open('pop_ProductHelp.aspx', 'Popup', 'toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
        //            //  fillter(1, 2, 3, 4, 5);
        //        }
        function fillter(barcode, itemCode, itemName, unitPack, LotNO) {
            $("#<%= txt_barcode.ClientID  %>").val(barcode);
            $("#<%= txt_No.ClientID  %>").val(itemCode);
            $("#<%= txt_item_name.ClientID  %>").val(itemName);
            $("#<%= txt_pack.ClientID  %>").val(unitPack);
            $("#<%= txt_lot_no.ClientID  %>").val(LotNO);
        }

//        function scrollWindow() {
//            window.scrollTo(2200, 1200)

////            var div = $('#div_Detail_withdrawal');
////            div.scrollTop = div.scrollHeight;
//        }


    </script>
    <style>
        .modalBG
        {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        
        
        .popupHover
        {
            background: #DDD;
            color: #555;
            border-right: 1px solid #B2B2B2;
            backgrouund-position: left top;
        }
        .auto-style1 {
            width: 25%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    

    <asp:Panel ID="Control_Panel" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    ค้นหาใบเบิกจ่าย
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td align="right" class="auto-style1">
                                &nbsp;
                            </td>
                            <td width="20%">
                                &nbsp;
                            </td>
                            <td width="15%" align="right">
                                ชื่อคลัง
                            </td>
                            <td width="40%">
                                <asp:DropDownList ID="ddl_stock" runat="server" DataValueField="Stock_ID" DataTextField="Stock_Name">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:RadioButton ID="rdb_request_status_1" runat="server" Text="รอการจ่าย" Checked="true"
                                    GroupName="request_status" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="rdb_request_status_2" runat="server" Text="จ่ายแล้ว" GroupName="request_status" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="rdb_request_status_3" runat="server" Text="ค้างจ่าย" GroupName="request_status" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                เลขที่ใบเบิก
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="txt_Request_No" runat="server"></asp:TextBox>
                            </td>
                            <td width="15%" align="right">
                                &nbsp;
                            </td>
                            <td width="40%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                วันที่เบิก
                            </td>
                            <td width="20%">
                                <uc2:CalendarControl ID="Request_Date_From" runat="server" />
                            </td>
                            <td width="15%" align="right">
                                ถึงวันที่
                            </td>
                            <td width="40%">
                                <uc2:CalendarControl ID="Request_Date_To" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                วันที่จ่าย
                            </td>
                            <td width="20%">
                                <uc2:CalendarControl ID="ccFrom" runat="server" />
                            </td>
                            <td width="15%" align="right">
                                ถึงวันที่
                            </td>
                            <td width="40%">
                                <uc2:CalendarControl ID="ccTo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                &nbsp;
                            </td>
                            <td width="20%">
                                <asp:CheckBox ID="cb_OrgStruc" runat="server" Text="ใบเบิกหน่วยงาน" Checked="true" /><br />
                                <asp:CheckBox ID="cb_Stock" runat="server" Text="ใบเบิกคลัง" Checked="true" />
                            </td>
                            <td style="align="right" colspan="2">
                                <fieldset style="width:150px">
                                    <legend>แสดง</legend>
                                    <asp:RadioButton ID="rdb_summary_req" runat="server" Text="ยอดสรุป" 
                                        GroupName="request" oncheckedchanged="rdb_request_CheckedChanged" AutoPostBack="true"/>
                                    <asp:RadioButton ID="rdb_inv_req" runat="server" Text="ใบเบิก" 
                                        GroupName="request" Checked="true" oncheckedchanged="rdb_request_CheckedChanged" AutoPostBack="true"/>&nbsp;&nbsp;&nbsp;
                                    
                                   <%-- <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="ยอดสรุป" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="ใบเบิก"></asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                 
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" />
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnAdd" runat="server" Text="เพิ่ม Routine การจ่าย" SkinID="ButtonMiddleLong"
                        OnClick="btnAdd_Click" />
                </td>
            </tr>

            </table>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="805">
                    <tr>
                        <td class="tableBody">
                            <asp:Panel ID="Control_Panel_Allocate" runat="server" Visible="false">
                                <br />
                                <div style="background-color: #EC467E; height: 20px; text-align: center;">
                                    <table width="100%" border="0" style="text-align: center; font-family: Tahoma, Geneva, sans-serif;">
                                        <tr style="text-align: center; color: #FFFFFF">
                                            <td colspan="4">
                                                <b>รายการ Allocate แล้ว ที่รอการจ่าย</b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:GridView ID="gvResult_Allocate" SkinID="Grid2" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                                    Width="100%" OnRowCommand="gvResult_Allocate_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="รายใบเบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnReq" runat="server" Text="รายใบเบิก" CommandName="Req" CausesValidation="false"
                                                    CommandArgument='<%# string.Format("{0}", Eval("Summary_ReqId")) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ตารางสรุป<br>การจ่าย" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnSum" runat="server" Text="สรุปการจ่าย" CommandName="Sum" CausesValidation="false"
                                                    CommandArgument='<%# string.Format("{0}", Eval("Summary_ReqId")) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ลำดับที่" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex   + 1) +(No2) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Routine<br>การจ่ายวันที่" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Summary_Date" runat="server" Text='<%# Eval("Summary_Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จำนวนใบเบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Number_Of_Req" runat="server" Text='<%# Eval("Number_Of_Req") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จำนวนใบเบิก<br>(ยืนยันการจ่าย)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Number_Req_Dispense" runat="server" Text='<%# (Eval("Number_Req_Dispense")==System.DBNull.Value)?"0":Eval("Number_Req_Dispense") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Status" runat="server" Text='<%# (Eval("Status").ToString()=="0")?"ยกเลิก":Eval("Status").ToString()=="1"?"พิมพ์สรุป":
                                        Eval("Status").ToString()=="2"?"Allocated":Eval("Status").ToString()=="3"?"จ่ายแล้ว":"" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วัน-เวลาที่บันทึก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Create_Date" runat="server" Text='<%# Eval("Create_Date","{0:dd/MM/yyyy hh:mm}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ผู้บันทึก" HeaderStyle-HorizontalAlign="Center" DataField="Request_By" />
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagingControl ID="PagingControl2" runat="server" />
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="Control_Panel_withdrawal" runat="server" Visible="false">
                                <br />
                                <div style="background-color: #EC467E; height: 20px; text-align: center;">
                                    <table width="100%" border="0" style="text-align: center; font-family: Tahoma, Geneva, sans-serif;">
                                        <tr style="text-align: center; color: #FFFFFF">
                                            <td colspan="4">
                                                <b>รายการรอจ่ายตามใบเบิก/จ่าย</b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:GridView SkinID="Grid2" ID="gvResult_withdrawal" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="true" Width="100%" OnRowCommand="gvResult_withdrawal_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Detail"
                                                    CommandArgument='<%# Eval("Request_Id") %>' CausesValidation="false" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ลำดับที่" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex   + 1) +(No1) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เลขที่<br>ใบเบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Request_No" runat="server" Text='<%# Eval("Request_No") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วันที่เบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Request_Date" runat="server" Text='<%# Eval("Request_Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ทีมงาน<br>ที่เบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Dep" runat="server" Text='<%# (Eval("Dep")==System.DBNull.Value)?"-":Eval("Dep") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ฝ่ายเบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Div" runat="server" Text='<%# (Eval("Div")==System.DBNull.Value)?"-":Eval("Div") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="คลังที่เบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Stock_Name" runat="server" Text='<%# (Eval("Stock_Name")==System.DBNull.Value)?"-":Eval("Stock_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ชื่อพนักงานที่เบิก" HeaderStyle-HorizontalAlign="Center" DataField="Request_By" />
                                        <asp:TemplateField HeaderText="ยอดเงินรวม" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pay_Amount" runat="server" Text='<%# Eval("Pay_Amount","{0:###,###,##0.0000}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ประเภท<br>การ<br>ขอเบิก" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Request_Type" runat="server" Text='<%# (Eval("Request_Type").ToString()=="0")?"รอบ":Eval("Request_Type").ToString()=="1"?"เบิกด่วน":Eval("Request_Type").ToString()=="2"?"เบิกผิดวัน":"" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Request_Status" runat="server" Text='<%# (Eval("Request_Status").ToString()=="0")?"ยกเลิกการเบิก":Eval("Request_Status").ToString()=="1"?"รอนุมัติเบิก":
                                        Eval("Request_Status").ToString()=="2"?"รอจ่าย":Eval("Request_Status").ToString()=="3"?"พิมพ์สรุปจ่าย":Eval("Request_Status").ToString()=="4"?"Allocated":
                                        Eval("Request_Status").ToString()=="5"?"ค้างจ่าย":Eval("Request_Status").ToString()=="6"?"จ่ายเรียบร้อยแล้ว":"" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="พิมพ์<br>ใบเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png"
                                                    CommandName="Print" CommandArgument='<%# Eval("Request_Id") %>' CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagingControl ID="PagingControl1" runat="server" />
                                <br />
                                <div id="div_Detail_withdrawal" runat="server" visible="false" style="text-align: center;">
                                    <fieldset style="width: 760px;">
                                        <legend style="text-align: left;">ใบเบิก</legend>
                                        <table width="100%">
                                            <tr>
                                                <td align="right" width="25%">
                                                    เลขที่ใบเบิก
                                                </td>
                                                <td align="left" width="20%">
                                                    <asp:TextBox ID="txt_req_No" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td align="right" width="15%">
                                                    วัน-เวลา ที่เบิก
                                                </td>
                                                <td align="left" width="40%">
                                                    <asp:TextBox ID="txt_req_date" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="25%">
                                                    ผู้เบิก
                                                </td>
                                                <td align="left" width="20%">
                                                    <asp:RadioButton ID="rdb_req_from1" runat="server" Text="หน่วยงาน" GroupName="req_from"
                                                        Enabled="false" />&nbsp;&nbsp;
                                                    <asp:RadioButton ID="rdb_req_from2" runat="server" Text="คลัง" GroupName="req_from"
                                                        Enabled="false" />
                                                </td>
                                                <td colspan="2">
                                                    <fieldset align="left" style="width: 250px;">
                                                        <legend align="left">ประเภทการเบิก</legend>
                                                        <div style="text-align: center;">
                                                            <asp:RadioButton ID="rdb_req_type_1" runat="server" Text="รอบ" GroupName="req_type"
                                                                Enabled="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rdb_req_type_2" runat="server" Text="ด่วน" GroupName="req_type"
                                                                Enabled="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="rdb_req_type_3" runat="server" Text="ผิดวัน" GroupName="req_type"
                                                                Enabled="false" />
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="25%">
                                                    ฝ่าย
                                                </td>
                                                <td align="left" width="20%">
                                                    <asp:TextBox ID="txt_Div" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td align="right" width="15%">
                                                    ทีมงาน
                                                </td>
                                                <td align="left" width="40%">
                                                    <asp:TextBox ID="txt_Dep" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="25%">
                                                    ชื่อคลัง
                                                </td>
                                                <td align="left" width="20%">
                                                    <asp:TextBox ID="txt_stock" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td align="right" width="15%">
                                                    ผู้เบิก
                                                </td>
                                                <td align="left" width="40%">
                                                    <asp:TextBox ID="txt_req_by" runat="server" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <fieldset align="left" style="width: 450px;">
                                                        <legend align="left">การจ่าย</legend>
                                                        <div style="text-align: center;">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left" width="15%">
                                                                        สถานที่ส่งของ/ผู้รับของ
                                                                    </td>
                                                                    <td align="left" width="40%">
                                                                        <asp:TextBox ID="txt_Pay_address" runat="server" width="96.5%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table width="100%">
                                                                
                                                                <tr>
                                                                    <td align="right" width="20%">
                                                                        วันที่จ่าย
                                                                    </td>
                                                                    <td align="left" width="20%">
                                                                        <asp:TextBox ID="txt_Pay_dttm" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td align="right" width="15%">
                                                                        ผู้จ่าย
                                                                    </td>
                                                                    <td align="left" width="45%">
                                                                        <asp:TextBox ID="txt_Pay_by" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr runat="server" id ="tr_1" visible="false">
                                                <td align="center" colspan="4">
                                                    <br />
                                                    <fieldset style="width: 720px;">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right" width="12%">
                                                                    รหัส Barcode
                                                                </td>
                                                                <td align="left" width="15%">
                                                                    <asp:TextBox ID="txt_barcode" runat="server" Width="110px"></asp:TextBox>
                                                                </td>
                                                                <td align="right" width="10%">
                                                                    รหัส
                                                                </td>
                                                                <td align="left" width="13%">
                                                                    <asp:TextBox ID="txt_No" runat="server" Width="80px" 
                                                                        ontextchanged="txt_No_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td align="right" width="10%">
                                                                    ชื่อสินค้า
                                                                </td>
                                                                <td align="left" width="31%">
                                                                    <asp:TextBox ID="txt_item_name" runat="server" Width="220px"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="9%">
                                                                    <%--<asp:ImageButton ID="btnSelect1" runat="server" 
                                                                ImageUrl="~/images/Commands/view.png" onclick="btnSelect1_Click" OnClientClick="popup()" />--%>
                                                                    <asp:ImageButton ID="btnItem_Help" runat="server" ImageUrl="~/images/Commands/view.png"
                                                                        OnClick="btnItem_Help_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="12%">
                                                                    หน่วย
                                                                </td>
                                                                <td align="left" width="15%">
                                                                    <asp:TextBox ID="txt_pack" runat="server" Width="70px"></asp:TextBox>
                                                                </td>
                                                                <td align="right" width="10%">
                                                                    Lot No.
                                                                </td>
                                                                <td align="left" width="13%">
                                                                    <asp:TextBox ID="txt_lot_no" runat="server" Width="50px"></asp:TextBox>
                                                                </td>
                                                                <td align="right" width="10%">
                                                                    จำนวน
                                                                </td>
                                                                <td align="left" width="31%">
                                                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btn_assure">
                                                                        <asp:TextBox ID="txt_qty" runat="server" Width="70px" onkeypress="return isNumericKey(event);"></asp:TextBox>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td align="left" width="9%">
                                                                    <asp:Button ID="btn_assure" runat="server" Text="ยืนยัน" OnClick="btn_assure_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <br />
                                                    <asp:GridView SkinID="Grid2" ID="gvDetail_withdrawal" runat="server" CellSpacing="0"
                                                        CellPadding="0" AutoGenerateColumns="false" EnableModelValidation="True" Width="754px"
                                                        GridLines="None" OnRowCreated="gvDetail_withdrawal_RowCreated" OnRowDataBound="gvDetail_withdrawal_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ลำดับที่">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex + 1) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="รหัสสินค้า" ItemStyle-Wrap="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_item_no" runat="server" Text='<%# Eval("Inv_ItemCode") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hid_Inv_ItemID" runat="server" Value='<%# Eval("Inv_ItemID") %>' />
                                                                    <asp:HiddenField ID="hid_Pack_ID" runat="server" Value='<%# Eval("Pack_ID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="77px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ชื่อสินค้า">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_item_name" runat="server" Text='<%# Eval("Item_Search_Desc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="120px"  HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="หน่วย">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_pack_name" runat="server" Text='<%# Eval("Pack_Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ราคา<br>ต่อหน่วย">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_unit_price" runat="server" Text='<%# Eval("Avg_Cost","{0:###,###,###.0000}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="เบิก">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Order_Qty" runat="server" Text='<%# Eval("Order_Quantity") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="จ่ายสะสม">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Pay_Qty" runat="server" Text='<%# Eval("Pay_Qty") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="คงค้าง">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lbl_Remain_Qty" runat="server" Text='<%# (Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?"0":(Convert.ToInt32(Eval("Order_Quantity")) - (Convert.ToInt32(Eval("Pay_Qty")) + Convert.ToInt32((Eval("Allocate")==System.DBNull.Value)?"0":Eval("Allocate")))).ToString() %>'></asp:Label>--%>
                                                                    <asp:Label ID="lbl_Remain_Qty" runat="server" Text='<%# (Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?"0":(Convert.ToInt32(Eval("Order_Quantity")) - Convert.ToInt32(Eval("Pay_Qty"))).ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="จำนวนจ่าย<br>ครั้งนี้">
                                                                <ItemTemplate>
                                                                    <%--<asp:TextBox ID="txt_qty" runat="server" Style="text-align: right" AutoPostBack="true"
                                                                        onkeypress="return isNumericKey(event);" OnTextChanged="txt_qty_TextChanged"
                                                                        Width="30px" Text='<%# (Eval("Req_ItemStatus").ToString()=="2")?"0":(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?"0":Eval("Allocate") %>'></asp:TextBox>--%>
                                                                    <asp:TextBox ID="txt_qty" runat="server" Style="text-align: right" AutoPostBack="true"
                                                                        onkeypress="return isNumericKey(event);" OnTextChanged="txt_qty_TextChanged" Width="30px" 
                                                                        Enabled='<%# (Eval("Req_ItemStatus").ToString()=="2")?false:(Eval("Req_ItemStatus").ToString()=="1")?true:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?false:fn_chk_status(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate"), "txt_qty", "Enabled") %>'/>

                                                                    <asp:HiddenField ID="hid_Lot" runat="server" Value='<%# (Eval("Allocate")==System.DBNull.Value)?"":"-" %>' />
                                                                    <asp:HiddenField ID="hid_Amount" runat="server" />
                                                                    <asp:HiddenField ID="hid_Allocate" runat="server" Value='<%# Eval("Allocate").ToString() %>' />
                                                                    <%--<asp:HiddenField ID="HiddenField1" runat="server" Value='<%# (Eval("Allocate")==System.DBNull.Value)?"":Eval("Allocate") %>' />--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" HorizontalAlign="Right"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ในคลัง">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_store_Qty" runat="server" Text='<%# Eval("OnHand_Qty","{0:0}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" HorizontalAlign="Right"  />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ค้างจ่าย">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cb_Remain" runat="server" AutoPostBack="true" OnCheckedChanged="Checkbox_CheckedChanged"
                                                                        Checked='<%# (Eval("Req_ItemStatus").ToString()=="2")?false:(Eval("Req_ItemStatus").ToString()=="1")?true:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?false:fn_chk_status(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate"), "cb_Remain", "Chk") %>'
                                                                        Enabled='<%# (Eval("Req_ItemStatus").ToString()=="2")?false:(Eval("Req_ItemStatus").ToString()=="1")?true:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?false:fn_chk_status(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate"), "cb_Remain", "Enabled") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ปิดการจ่าย">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cb_Close" runat="server" AutoPostBack="true" OnCheckedChanged="Checkbox_CheckedChanged"
                                                                        Checked='<%# (Eval("Req_ItemStatus").ToString()=="2")?true:(Eval("Req_ItemStatus").ToString()=="1")?false:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?true:fn_chk_status(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate"), "cb_Close" , "Chk") %>'
                                                                        Enabled='<%# (Eval("Req_ItemStatus").ToString()=="2")?false:(Eval("Req_ItemStatus").ToString()=="1")?true:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?false:fn_chk_status(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate"), "cb_Close" , "Enabled") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="รวมเงิน">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_summary" runat="server" Text='<%# (Eval("Req_ItemStatus").ToString()=="2")?"0.0000":(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?"0.0000":fn_summary_cost(Eval("Allocate"), Eval("Avg_Cost")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="75px" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="สถานะ"> 
                                                                <ItemTemplate>           <%--&& Eval("Summary_ReqItem_Id")==System.DBNull.Value--%>
                                                                    <asp:Label ID="lbl_status" runat="server" Width="40" Text='<%# (Eval("Req_ItemStatus").ToString()=="2")?"จ่ายไม่ครบ<br>(ปิดการจ่าย)":(Eval("Req_ItemStatus").ToString()=="1")?"ค้างจ่าย":(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?"จ่ายครบ":fn_chk_status_txt(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate")) %>'></asp:Label>
                                                                    <asp:HiddenField ID="hid_status" runat="server" Value='<%# (Eval("Req_ItemStatus").ToString()=="2")?2:(Eval("Req_ItemStatus").ToString()=="1")?1:(Eval("Order_Quantity").ToString()==Eval("Pay_Qty").ToString())?3:fn_chk_status_int(Eval("Order_Quantity"), Eval("Pay_Qty"), Eval("Allocate")) %>' />
                                                                     <asp:HiddenField ID="hdRemarkOrg" runat="server"/>
                                                                     <asp:HiddenField ID="hdRemarkStock" runat="server"/>
                                                                    <asp:HiddenField ID="Remark" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="" HeaderText="หมายเหตุคลัง" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="" HeaderText="หมายเหตุหน่วยงาน" ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <br />
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right" width="20%">
                                                                <%--ผู้แก้ไขล่าสุด--%>
                                                            </td>
                                                            <td align="left" width="15%">
                                                                <%--<asp:TextBox ID="txt_last_chg_by" runat="server" Width="110px"></asp:TextBox>--%>
                                                            </td>
                                                            <td align="right" width="15%">
                                                                <%--วันที่แก้ไขล่าสุด--%>
                                                            </td>
                                                            <td align="left" width="10%">
                                                                <%--<asp:TextBox ID="txt_last_chg_date" runat="server" Width="110px"></asp:TextBox>--%>
                                                            </td>
                                                            <td align="right" width="40%">
                                                                ราคารวมทุนที่จ่าย &nbsp;
                                                                <asp:Label ID="lbl_total_all" runat="server" Text="0"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <table width="100%">
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:Button ID="btn_Submit" runat="server" Text="บันทึก" SkinID="ButtonMiddle" OnClick="btn_Submit_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btn_Clear_Data" runat="server" Text="ล้างหน้าจอ" SkinID="ButtonMiddle"
                                                    OnClick="btn_Clear_Data_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btn_Cancel_Summary" runat="server" Text="ยกเลิกจ่าย" SkinID="ButtonMiddle"
                                                    OnClick="btn_Cancel_Summary_Click" />
                                                <asp:HiddenField ID="hid_Summary_ReqId" runat="server" />
                                                <asp:HiddenField ID="hid_Request_Id" runat="server" />
                                                <asp:HiddenField ID="hid_Request_Status" runat="server" />
                                                <asp:HiddenField ID="hid_rowIndex" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableFooter">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>


    </asp:Panel>
    
<%--    <asp:UpdateProgress runat="server" ID="UpdateProgress2">
        <ProgressTemplate>
            <div class="modalBG">
                <div style="position: fixed; top: 40%; left: 40%; background-color: White;" align="center"
                    class="no-print">
                    <div style="border-color: rgb(236, 70, 126); border-style: solid; border-width: medium; width: 200px;
                        height: 40px; padding-top: 15px;">
                        <span style="color: Black;">กำลังดำเนินการ.....</span>
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>

    

</asp:Content>
