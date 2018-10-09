<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmRequest.aspx.cs" Inherits="GPlus.Request.frmRequest"  MasterPageFile="../MasterPage/Main.Master" 
    MaintainScrollPositionOnPostback="true"%>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../UserControls/CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>
<%@ Register Src="~/UserControls/ItemOrgStructControl.ascx" TagName="orgControl" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 760px;
        }
        .style3
        {
            width: 67px;
        }
        .style4
        {
            width: 124px;
        }
        .style6
        {
            width: 182px;
        }
        .style7
        {
            width: 37px;
        }
        .style8
        {
            width: 32px;
        }
        .style10
        {
            width: 152px;
        }
        .style11
        {
            width: 125px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <script language="javascript" type="text/javascript">
        function OpenEmployeePopup(orgId) {
            if (window.showModalDialog) {
                retVal = window.showModalDialog("../reports/dgEmplyeeRequest.aspx?OrgId=" + orgId, 'Show Popup Window', "unadorned:yes;dialogHeight:400px;dialogWidth:550px;resizable:no;scrollbars:yes");
                __doPostBack('__Page', '');
            }
        }
        function OpenItemCategoryPopup() {
            if (window.showModalDialog) {
                open_popup('dgItemCatagory.aspx', 840, 400, 'popAcc', 'yes', 'yes', 'yes');
                //retVal = window.showModalDialog("dgItemCatagory.aspx", 'Show Popup Window', "unadorned:yes;dialogHeight:450px;dialogWidth:810px;resizable:no;scrollbars:yes");
               // __doPostBack('__Page', '');
            }
        }
        function __doPostBack(eventTarget, eventArgument) {
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        }
        //        function ConfrimationSave() {
        //            if (confirm("คุณต้องการบันทึกหรือแก้ไขใบเบิกนี้ใช้หรือไม่?") == true) {
        //                return true;
        //            }
        //            return false;
        //        }
        function ConfrimationCancelForm() {
            if (confirm("คุณต้องการยกเลิกการเพิ่มหรือแก้ไขใบเบิกนี้ใช้หรือไม่?") == true) {
                return true;
            }
            return false;
        }
        function ConfrimationCancelRequest() {
            if (confirm("คุณต้องการยกเลิกใบเบิกนี้ใช้หรือไม่?") == true) {
                return true;
            }
            return false;
        }
        //        function ConfrimationApprove() {
        //            if (confirm("คุณต้องการบันทึกใช่หรือไม่?") == true) {
        //                return true;
        //            }
        //            return false;
        //        }

        $(document).ready(function () {
            $('#<%=rblConsiderTypes.ClientID %>').change(function () {
                var rdbConsiderApproveObj = $('#<%=rblConsiderTypes.ClientID %> input[type=radio]:checked');
                if (rdbConsiderApproveObj.val() == 2) {
                    //document.getElementById('<%= tbConsiderReason.ClientID %>').value = "";
                    $('#<%=tbConsiderReason.ClientID %>').val("");
                }
            });
        });

        function ConfrimationApprove() {
            var rdbConsiderApproveObj = $('#<%=rblConsiderTypes.ClientID %> input[type=radio]:checked');
            //alert("rdb " + rdbConsiderApproveObj.size());

            if (rdbConsiderApproveObj.size() <= 0) {
                alert("กรูณาเลือกผลการพิจารณา");
                return false;
            }
            else if (rdbConsiderApproveObj.val() == 0 || rdbConsiderApproveObj.val() == 1) {
                if (document.getElementById('<%= tbConsiderReason.ClientID %>').value.length == 0) {
                    alert("กรูณาระบุเหตุผล");
                    document.getElementById('<%= tbConsiderReason.ClientID %>').focus();
                    return false;
                }
            }

            if (confirm("คุณต้องการบันทึกใช่หรือไม่?") == true) {
                return true;
            }
            return false;
        }

        function ConfrimationCancleRec() {
            if (confirm("คุณต้องการยกเลิกการรับครั้งนี้ใช่หรือไม่?") == true) {

                $("#popup").dialog
                ({
                    open: function () {
                        $(".ui-dialog-titlebar").hide();
                    },
                    resizable: false,
                    height: 140,
                    modal: true
                });

                return true;
            }
            return false;
        }
        function validationItem() {
            var itemName = document.getElementById('<%=this.tbItemName.ClientID %>').value;
            if (itemName == "") {
                alert('กรุณาทำการเลือกวัสดุ-อุปกรณ์ ก่อนจะเพิ่ม');
                return false;
            }
            else
                return true;
        }
        function isNumericKey(e) {
            var charInp = window.event.keyCode;

            if (charInp > 31 && (charInp < 48 || charInp > 57)) {
                return false;
            }
            return true;
        }
    </script>
   
   <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">หน่วยงาน/คลังย่อยเบิกวัสดุ-อุปกรณ์</td>
        </tr>
        <tr>
            <td class="tableBody">
                <table cellpadding="5" cellspacing="0" width="100%">
                    
                    <tr>
                        <td colspan="7">
                        <uc4:orgcontrol ID="orgCtrl" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:150px" align="right"></td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rdbIsWait" runat="server"
                                RepeatColumns="3" RepeatDirection="Horizontal" Visible="false" 
                                Width="277px">
                                <asp:ListItem Text="รอพิจารณา" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="พิจารณาแล้ว" Value="0"></asp:ListItem>
                                <asp:ListItem Text="จ่ายแล้ว" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right"><asp:Label ID="lbConsider" runat="server">ผลการพิจารณา</asp:Label></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlStatus" Width="150px" Enabled="false" >
                                <asp:ListItem Text="ทั้งหมด" Value="" />
                                <asp:ListItem Text="อนุมัติเบิก" Value="2" />
                                <asp:ListItem Text="ไม่อนุมัติ" Value="0" />
                                <asp:ListItem Text="ส่งกลับไปแก้ไข" Value="1" />
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td  align="right">เลขที่ใบเบิก</td>
                            <td >
                            <asp:TextBox runat="server" ID="txtRequestNo" Width="100px" />
                        &nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                      <tr>
                      <td colspan="7"> 
                       <table width="100%" >
                           <tr>
                           <td  align="right" class="style10"><asp:Label ID="lb" runat="server">วันที่เบิก&nbsp;</asp:Label></td>
                           <td colspan="1" class="style11">
                                <uc3:CalendarControl runat="server" ID="ccForm" /></td> <td  align="center">-</td>
                                 <td ><uc3:CalendarControl runat="server" ID="ccTo" /></td> 
                            </tr>
                     </table>
                     </td>
                     </tr>
                     <tr>
                        <td  align="right" >ชื่อผู้เบิก</td>
                        <td  >
                            <asp:TextBox runat="server" ID="txtName" Width="200px" />
                        </td>
                    </tr>

                   
                    <tr>
                        <td colspan="7" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="BtnSearchClick" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="BtnCancelClick" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:150px">
                            <asp:Button ID="btnAddNewRequest" runat="server" SkinID="ButtonMiddleLong" Text="เบิกวัสดุ-อุปกรณ์" OnClick="BtnAddNewRequestClick" Width="120" />
                        </td>
                    </tr>
                  </table>
            </td>
        </tr>
   </table>
   <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
    <table cellpadding="0" cellspacing="0" width="805">
        <%--<tr>
            <td class="tableHeader">หน่วยงาน/คลังย่อยเบิกวัสดุ-อุปกรณ์</td>
        </tr>--%>
        <tr>
            <td class="tableBody">
                <table cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="6">
                            <asp:HiddenField runat="server" ID="hdRequestId" />
                            <asp:GridView runat="server" ID="gvRequestList" OnRowDataBound="GvRequestListRowDataBound" OnRowCommand="GvRequestListRowCommand" Width="100%"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnSelect" Text="รายละเอียด" CommandName="Select" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="ลำดับ" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField HeaderText="เลขที่ใบเบิก" DataField="Request_No" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="วันที่เบิก" DataField="Request_Date" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="ฝ่าย" DataField="" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="ทีม" DataField="" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="คลังที่จ่าย" DataField="Stock_Name" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="ชื่อพนักงานที่เบิก" DataField="Request_By_FullName" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="ยอดเงินรวม" DataField="Total_Order_Amount" DataFormatString="{0:#,###.0000}" 
                                    ItemStyle-HorizontalAlign="Right" Visible="false" />
                                    <%--<asp:BoundField HeaderText="สถานะ" DataField="Status_Desc" ItemStyle-HorizontalAlign="Center" />--%>
                                    <asp:BoundField HeaderText="สถานะ" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="พิมพ์ใบเบิก" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:Button runat="server" ID="btnPrint" CommandName="Print" SkinID="PrintButton" />--%>
                                            <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="พิมพ์ใบเบิก" DataField="" ItemStyle-HorizontalAlign="Center" />--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <uc1:PagingControl runat="server" ID="pagingControlReqList" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <asp:Panel runat="server" ID="pnApproval" Visible="false">
                    <fieldset>
                        <legend>อนุมัติเบิกของ</legend>
                         <table cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" class="style7">
                                    พิจารณา
                                </td>
                                <td style="width:260px">
                                    <fieldset style="width:255px">
                                        <asp:RadioButtonList ID="rblConsiderTypes" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="อนุมัติเบิก" Value="2" />
                                            <asp:ListItem Text="ไม่อนุมัติ" Value="0" />
                                            <asp:ListItem Text="ส่งกลับไปแก้ไข" Value="1" />
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                                <td align="right" class="style8">
                                    เหตุผล
                                </td>
                                <td>
                                    <asp:TextBox ID="tbConsiderReason" runat="server" Width="395px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style7">
                                    ผู้อนุมัติ
                                </td>
                                <td style="width:260px">
                                    <asp:TextBox ID="tbConsiderBy" runat="server" Width="200" Enabled="false" />
                                </td>
                                <td align="right" class="style8">
                                    วัน/เวลา
                                </td>
                                <td>
                                    <asp:TextBox ID="tbConsiderDate" runat="server" Width="161px" Enabled="false" />
                                </td>
                            </tr>
                              <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnApprove" runat="server" Text="บันทึก" SkinID="ButtonMiddle" OnClick="BtnApproveClick" OnClientClick="return ConfrimationApprove();" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelApprove" runat="server" Text="ล้างหน้าจอ" SkinID="ButtonMiddle" OnClick="BtnCancelApproveClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <asp:Panel runat="server" ID="pnDetail" Visible="false">
                   <fieldset>
                        <legend>ข้อมูลใบเบิก</legend>
                        <table cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" class="style3">เลขที่ใบเบิก</td>
                                <td class="style4">
                                    <asp:TextBox runat="server" ID="tbRequestNo" Width="140px" Enabled="false" />
                                </td>
                                <td align="center" class="style6">
                                    <fieldset style="width:190px">
                                        <legend>ประเภทการเบิก</legend>
                                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rdlRequestType">
                                            <asp:ListItem Text="รอบ" Value="0" />
                                            <asp:ListItem Text="ด่วน" Value="1" />
                                            <asp:ListItem Text="ผิดวัน" Value="2" />
                                        </asp:RadioButtonList>
                                    </fieldset>
                                </td>
                                <td style="width:60px" align="right">วันที่เบิก</td>
                                <td style="width:110px" align="left">
                                    <asp:TextBox runat="server" ID="tbRequestDate" Width="100px" Enabled="false" />
                                </td>
                            </tr>
                            <div id="divConsiderReason" runat="server"> 
                            <tr>
                                    <td>เหตุผลการพิจารณา </td>
                                    <td colspan="4">
                                        <asp:TextBox runat="server" ID="tbShowConsiderReason" Width="576px" 
                                            Enabled="false" />
                                    </td>
                            </tr>
                            </div>
                            <div id="divNotApprove" runat="server"> 
                            <tr>
                                <td></td>
                                <td colspan = "4">
                                    <asp:CheckBox ID="cb_NotApproveFlag" runat="server" Text="  เบิกโดยไม่ต้องอนุมติ  " Checked="true" />
                                </td>
                            </tr>
                            </div>
                            <tr>
                                <td align="right" class="style3">ผู้เบิก</td>
                                <td class="style4">
                                    <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblDeptType">
                                        <asp:ListItem Text="หน่วยงาน" Value="0" />
                                        <asp:ListItem Text="คลัง" Value="1" />
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="3">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:60px" align="right">ชื่อผู้เบิก</td>
                                            <td style="width:160px" align="right">
                                                <asp:TextBox runat="server" ID="tbRequestName" Width="150px" Enabled="false" />
                                            </td>
                                            <td colspan="2">
                                                <asp:Button SkinID="SearchButton" ID="btnFindRequestName" runat="server" OnClick="BtnFindRequestNameClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">ฝ่าย</td>
                                <td class="style4">
                                    <asp:TextBox runat="server" ID="txtDivName" Width="140px" Enabled="false" />
                                </td>
                                <td class="style6">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:60px" align="right">ทีมงาน</td>
                                            <td style="width:160px" align="right">
                                                <asp:TextBox runat="server" ID="txtDepName" Width="150px" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2" rowspan="2">
                                    <div id = "divImport" visible="false" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                Load Excel : 
                                            </td>
                                            <td>
                                            <asp:FileUpload ID="FileImport" runat="server" Width="201px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btnImport" runat="server" Text="Import" 
                                                onclick="btnImport_Click" Width="69px"/>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">ชื่อคลังที่จ่าย</td>
                                <td class="style4">
                                    <asp:TextBox runat="server" ID="tbStockName" Width="140px" Enabled="false" />
                                </td>
                                <td class="style6">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:60px" align="right">ผู้จ่าย</td>
                                            <td style="width:160px" align="right">
                                                <asp:TextBox runat="server" ID="tbDistributeName" Width="150px" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <table cellpadding="10" cellspacing="0">
                                        <tr>
                                            <td class="style2">
                                                <fieldset>
                                                    <asp:Panel ID="pnlEnt" runat="server" DefaultButton="btnAddItem">
                                                        <table cellpadding="4" cellspacing="0">
                                                        <tr>
                                                            <td style="width:80px" align="right">รหัสสินค้า</td>
                                                            <td style="width:150px" align="right">
                                                                <%--<asp:TextBox runat="server" ID="tbItemCode" Width="140px" Enabled="false" />--%>
<%--                                                                <asp:TextBox runat="server" ID="tbItemCode" Width="140px" Enabled="false" onkeypress="return tbItemCodeSearch(event);" /> --%>
                                                                  <asp:TextBox runat="server" ID="tbItemCode" Width="140px" Enabled="true" OnTextChanged="ItemCodeSearchChanged" AutoPostBack="true" /> 
                                                                <asp:LinkButton ID="btnFileItem" runat="server" Text="..." 
                                                                    onclick="btnFileItem_Click" style="display:none"></asp:LinkButton>
                                                            </td>
                                                            <td style="width:80px" align="right">ชื่อสินค้า</td>
                                                            <td style="width:200px" align="right">
                                                                <asp:TextBox runat="server" ID="tbItemName" Width="190px" Enabled="false" />
                                                            </td>
                                                            <td style="width:60px" align="right">หน่วยนับ</td>
                                                            <td style="width:80px" align="right">
                                                                <asp:TextBox runat="server" ID="tbUnitName" Width="70px" Enabled="false" />
                                                            </td>
                                                            <td style="width:50px">
                                                               <%-- <asp:Button runat="server" SkinID="SearchButton" ID="btnFindItem" OnClick="BtnFindItemClick" />--%>
                                                               <asp:ImageButton ID="btnFindItem" runat="server" OnClick="BtnFindItemClick" ImageUrl="~/images/Commands/view.png" />
                                                               <asp:LinkButton ClientIDMode="Static" OnClick="btnRefresh_Click" ID="btnRefresh" runat="server"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:80px">&nbsp;</td>
                                                            <td style="width:150px">&nbsp;</td>
                                                            <td colspan="2" align="right">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td style="width:80px; display:none;" align="right">ราคาต่อหน่วย</td>
                                                                        <td style="width:90px; display:none;" align="right">
                                                                            <asp:TextBox runat="server" ID="tbPricePerUnit" Width="70px" Enabled="false" style="text-align:right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" />
                                                                        </td>
                                                                        <td style="width:40px; display:none;" align="center">บาท</td>
                                                                        <td style="width:40px" align="right">จำนวน&nbsp;&nbsp;</td>
                                                                        <td style="width:40px" align="right">
                                                                            <%--<asp:TextBox runat="server" ID="tbQty" Width="80px" style="text-align:right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" 
                                                                                 onpaste="return CancelKeyPaste(this);" />--%>
                                                                                 <asp:TextBox runat="server" ID="tbQty" Width="80px" style="text-align:right;" onKeyPress="return tbQtyEnter(event);" 
                                                                                 onpaste="return CancelKeyPaste(this);" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width:60px; display:none;" align="right" >ราคารวม</td>
                                                            <td style="width:80px; display:none;" align="right">
                                                                <asp:TextBox runat="server" ID="tbTotalPrice" Width="70px" Enabled="false" style="text-align:right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnAddItem" Text="เพิ่ม" OnClick="BtnAddItemClick" OnClientClick="return validationItem();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                <asp:Panel runat="server" ID="panelDelItem" Visible="false">
                                                    <asp:Button runat="server" ID="btnDelItem" Text="ลบ" OnClick="BtnDelItemClick"  />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                            <div style="width: 100%; max-height: 200; overflow:auto" >
                                                <asp:GridView runat="server" ID="gvStockPay" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvStockPayRowDataBound" OnRowCommand="gvStockPayRowCommand"  SkinId="GvNormal">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ครั้งที่การจ่าย" ItemStyle-HorizontalAlign ="Center" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnSelect" Text='<%# "เลือกการรับเข้าครั้งที่ "+Eval("row_num") %>' CommandName="Select" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="วันที่จ่าย" DataField="Pay_Date" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField HeaderText="ผู้จ่าย" DataField="Pay_Name" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField HeaderText="ผู้รับ" DataField="Receive_Name" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="วันที่รับ" DataField="Receive_Date" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="สถานะ" DataField="Rec_Status" ItemStyle-HorizontalAlign="Center"/>
                                                    </Columns>
                                                </asp:GridView>
                                                </div>
                                            </td> 
                                        <tr>
                                            <td class="style2">
                                               <%-- <div style="width: 100%; max-height: 375px; overflow:auto">--%>
                                                    <asp:GridView ID="gvRequestItem" runat="server" AutoGenerateColumns="false"
                                                        OnRowDataBound="GvRequestItemRowDataBound" 
                                                        OnRowCreated="GvRequestItem_RowCreated" Width="100%"  SkinId="GvLong">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="ลำดับ" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Inv_ItemCode" HeaderText="รหัสสินค้า" 
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Inv_ItemName" HeaderText="ชื่อสินค้า" 
                                                                ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Description" HeaderText="หน่วยที่เบิก" 
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Avg_Cost" HeaderText="ราคาต่อหน่วย" 
                                                                ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                            <%--<asp:BoundField DataField="Order_Quantity" HeaderText="จำนวนเบิก" 
                                                                ItemStyle-HorizontalAlign="Right" />--%>
                                                            <asp:TemplateField HeaderText="จำนวนเบิก">
                                                                <ItemTemplate>
                                                                   <%-- <asp:TextBox ID="txt_OrderQty" runat="server" Text='<%# Eval("Order_Quantity") %>' Enabled="false" AutoPostBack="true" 
                                                                     Width="50px"></asp:TextBox>--%>
                                                                     <asp:TextBox ID="txt_OrderQty" runat="server" Text='<%# Eval("Order_Quantity") %>' Enabled="false" onkeypress="return isNumericKey(event);" 
                                                                     Width="50px"></asp:TextBox>
                                                                     <asp:HiddenField ID="hdRemarkOrg" runat="server"/>
                                                                     <asp:HiddenField ID="hdRemarkStock" runat="server"/>
                                                                     <asp:HiddenField ID="hdInvItemID" runat="server"/>
                                                                     <asp:HiddenField ID="hdPackID" runat="server"/>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Pay_Qty" HeaderText="จำนวนที่จ่ายสะสม" 
                                                                ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="Receive_Qty" HeaderText="จำนวนทีรับสะสม" 
                                                                ItemStyle-HorizontalAlign="Right" />
                                                            <%--<asp:BoundField DataField="data_pay_qty" HeaderText="จำนวนจ่าย" 
                                                                ItemStyle-HorizontalAlign="Right" />--%>
                                                            <asp:TemplateField HeaderText="จำนวนจ่าย">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_PayQty" runat="server" Text='<%# Eval("data_pay_qty") %>' Enabled="false" AutoPostBack="true" 
                                                                     Width="50px"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField HeaderText="จำนวนที่รับ" DataField="" ItemStyle-HorizontalAlign="Right"/>--%>
                                                            <asp:TemplateField HeaderText="จำนวนที่รับ">
                                                                <ItemTemplate>
                                                                    <%--<asp:TextBox ID="txt_RecQty" runat="server" AutoPostBack="true" 
                                                                        onkeypress="return isNumericKey(event);" OnTextChanged="txt_RecQty_TextChanged" 
                                                                        Width="50px" Text='<%# Eval("data_rec_qty") %>'></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txt_RecQty" runat="server" 
                                                                        onkeypress="return isNumericKey(event);" 
                                                                        Width="50px" Text='<%# Eval("data_rec_qty") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Order_Amount" HeaderText="รวมเงินเบิก" 
                                                                ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                            <asp:BoundField DataField="" HeaderText="สถานะ" 
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <%--<asp:BoundField HeaderText="หมายเหตุ" DataField="" ItemStyle-HorizontalAlign="Center"/>--%>
                                                            <asp:TemplateField HeaderText="หมายเหตุรับสินค้า">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_Comment" runat="server"  Width="80px" Text='<%# Eval("ReqRecItem_Remark") %>'></asp:TextBox>
                                                                    <%--<asp:TextBox ID="txt_Comment" runat="server" AutoPostBack="true" Width="80px" Text='<%# Eval("Remark") %>'></asp:TextBox>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="" HeaderText="หมายเหตุหน่วยงาน" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="" HeaderText="หมายเหตุคลัง" ItemStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                    </asp:GridView>
                                                <%--</div>--%>
                                            </td>
                                            </tr>
                                            <tr style="display:none;">
                                                <asp:Panel ID="pnTotalOrder" runat="server" Visible="false">
                                                    <td align="right" colspan="5">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td align="left" style="width:90px">
                                                                    รวมมูลค่าเบิก</td>
                                                                <td>
                                                                    <asp:Label ID="txtTotalOrder" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width:40px">
                                                                    บาท</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <td class="style2">
                                                    <table cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td align="right" style="width:80px">
                                                                ผู้สร้าง</td>
                                                            <td align="right" style="width:150px">
                                                                <asp:TextBox ID="tbCreatedBy" runat="server" Enabled="false" Width="140px" />
                                                            </td>
                                                            <td align="right" style="width:100px">
                                                                วันที่สร้างข้อมูล</td>
                                                            <td align="right" style="width:150px">
                                                                <asp:TextBox ID="tbCreatedDate" runat="server" Enabled="false" Width="140px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="width:80px">
                                                                ผู้แก้ไข</td>
                                                            <td align="right" style="width:150px">
                                                                <asp:TextBox ID="tbUpdatedBy" runat="server" Enabled="false" Width="140px" />
                                                            </td>
                                                            <td align="right" style="width:100px">
                                                                วันที่แก้ไขข้อมูล</td>
                                                            <td align="right" style="width:150px">
                                                                <asp:TextBox ID="tbUpdatedDate" runat="server" Enabled="false" Width="140px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="style2">
                                                    <asp:Button ID="btnSave" runat="server" OnClick="BtnSaveClick" 
                                                        OnClientClick="return ChkDataOrder();" Text="บันทึก" />
                                                    &nbsp;
                                                    <%--<asp:Button ID="btnDelete" runat="server" Text="ลบ" OnClick="BtnDeleteClick" OnClientClick="return ConfrimationCancelRequest();" />&nbsp;--%>
                                                    <asp:Button ID="btnReset" runat="server" OnClick="BtnResetClick" 
                                                        SkinID="ButtonMiddle" Text="ล้างหน้าจอ" />
                                                    &nbsp;
                                                    <asp:Button ID="btnCancelForm" runat="server" OnClick="BtnCancelFormClick" 
                                                        OnClientClick="return ConfrimationCancelForm();" SkinID="ButtonMiddle" Text="ยกเลิกใบเบิก" />
                                                    &nbsp;
                                                    <asp:Button ID="btnPrint" runat="server" OnClick="BtnPrintClick" 
                                                        SkinID="ButtonMiddle" Text="พิมพ์ใบเบิก" />
                                                    &nbsp;
                                                    <%-- <asp:Panel ID="receiveBtn" runat="server" Visible="false">--%>
                                                    <asp:Button ID="BtnRec" runat="server" OnClick="BtnRec_Click" 
                                                        SkinID="ButtonMiddle" Text="ทำการรับ" OnClientClick="return ChkDataRec();" />
                                                    &nbsp;
                                                    <asp:Button ID="BtnCancleRec" runat="server" OnClick="BtnCancleRec_Click" 
                                                    OnClientClick="return ConfrimationCancleRec();" SkinID="ButtonMiddle" Text="ยกเลิกรับ" />
                                                    &nbsp; <%--  </asp:Panel>--%>
                                               </td>
                                            </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                   </fieldset>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="tableFooter"></td>
        </tr>
    </table>
    <div id="popup" 
        style="border-color: rgb(236, 70, 126); 
        border-style: solid; 
        border-width: medium; 
        display:none; 
        text-align:center">
            <br /><br />กำลังดำเนินการ.....
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>

<script type="text/javascript">
    function tbItemCodeSearch(e) {

        if (e.keyCode == 13) {
            if (document.getElementById('<%= tbItemCode.ClientID %>').value.length > 0) {
                document.getElementById('<%= btnFileItem.ClientID %>').click();
                e.cancelBubble = true;
            }
            else {
                alert('กรุณาระบุรหัสสินค้า');
                document.getElementById('<%= tbItemCode.ClientID %>').focus();
            }
        }
        //        return false;
    }

    function tbQtyEnter(e) {
        if (e.keyCode == 13) {
            document.getElementById('<%= btnAddItem.ClientID %>').click();
        }
        return NumberBoxKeyPress(event, 0, 46, false);
    }

    $(document).ready(function () {
        $('#<%=rdbIsWait.ClientID%>').change(function () {
            var rbvalue = $("input[name='<%=rdbIsWait.UniqueID%>']:radio:checked").val();

            var ddlStatus = $("#<%=ddlStatus.ClientID %>");
            // พิจารณาแล้ว
            if (rbvalue == "0") {
                ddlStatus.prop('disabled', false);
                document.getElementById('<%=lb.ClientID %>').innerText = "วันที่เบิก  "
            }
            else if (rbvalue == "1") {
                $("#<%=ddlStatus.ClientID %>").val("");
                ddlStatus.prop('disabled', true);
                document.getElementById('<%=lb.ClientID %>').innerText = "วันที่เบิก  "
            }
            else if (rbvalue == "2") {
                $("#<%=ddlStatus.ClientID %>").val("");
                ddlStatus.prop('disabled', true);
                document.getElementById('<%=lb.ClientID %>').innerText = "วันที่จ่าย  "
            }
        });
    });


    function txt_RecQtyChange(ctrlReqRec, ctrlPay, ctrlComment) {

        var RecQty = document.getElementById(ctrlReqRec).value;
        var PayQty = document.getElementById(ctrlPay).value;


        if (RecQty == "") {
            alert("กรุณาระบุจำนวนที่รับ หากไม่มีจำนวนที่รับ กรุณาใส่ 0");
            document.getElementById(ctrlReqRec).focus();
        }
        else if (RecQty != PayQty) {
            alert("กรุณาระบุหมายเหตุ เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย");
            document.getElementById(ctrlComment).focus();
        }

    }

    function txt_RecQtyChange(ctrlReqRec, ctrlPay, ctrlComment) {

        var RecQty = document.getElementById(ctrlReqRec).value;
        var PayQty = document.getElementById(ctrlPay).value;


        if (RecQty == "") {
            alert("กรุณาระบุจำนวนที่รับ หากไม่มีจำนวนที่รับ กรุณาใส่ 0");
            document.getElementById(ctrlReqRec).focus();
        }
        else if (RecQty != PayQty) {
            alert("กรุณาระบุหมายเหตุ เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย");
            document.getElementById(ctrlComment).focus();
        }

    }

    function ChkDataRec() {

        var gv1 = document.getElementById('<%= gvRequestItem.ClientID %>')
        //alert(gv1.rows.length);
        for (var i = 3; i < gv1.rows.length; ++i) {


            var PayQtyObj = gv1.rows[i].cells[7].getElementsByTagName('input')[0];
            var RecQtyObj = gv1.rows[i].cells[8].getElementsByTagName('input')[0];
            var CommentObj = gv1.rows[i].cells[10].getElementsByTagName('input')[0];

            // alert("Rec " + RecQtyObj.value);
            // alert("Pay " + PayQtyObj.value);
            // alert("Comment " + CommentObj.value);

            if (RecQtyObj.value == "" && PayQtyObj.value != "0") {
                alert("กรุณาระบุจำนวนที่รับ หากไม่มีจำนวนที่รับ กรุณาใส่ 0");
                RecQtyObj.focus();
                return false;
            }
            else if ((RecQtyObj.value != PayQtyObj.value) && CommentObj.value == "" && PayQtyObj.value != "0") {
                // alert("Pay" + PayQtyObj.value);
                alert("กรุณาระบุหมายเหตุ เนื่องจากทำการรับมากกว่าหรือน้อยกว่าจำนวนที่จ่าย");
                CommentObj.focus();
                return false;
            }

        }

        $("#popup").dialog
        ({
            open: function () {
                $(".ui-dialog-titlebar").hide();
            },
            resizable: false,
            height: 140,
            modal: true
        });

        return true;

    }

    function ChkDataOrder() {

        var gv1 = document.getElementById('<%= gvRequestItem.ClientID %>')

        //        alert(gv1);

        if (gv1 != null) {
            //   alert(gv1.rows.length);
            for (var i = 3; i < gv1.rows.length; ++i) {

                //alert(gv1.rows[i].cells[5]);
                var OrderQtyObj = gv1.rows[i].cells[5].getElementsByTagName('input')[0];

                //                alert("Order " + OrderQtyObj.value);


                if (parseInt(OrderQtyObj.value == "" ? "0" : OrderQtyObj.value) <= 0) {
                    alert("กรุณาใส่จำนวนที่เบิกค่ามากกว่า 0");
                    OrderQtyObj.focus();
                    return false;
                }

            }

            if (confirm("คุณต้องการบันทึกหรือแก้ไขใบเบิกนี้ใช้หรือไม่?") == true) {

                $("#popup").dialog
               ({
                   open: function () {
                       $(".ui-dialog-titlebar").hide();
                   },
                   resizable: false,
                   height: 140,
                   modal: true
               });

                return true;
            }
            return false;
        }
        else {

            alert("กรุณาเลือกวัสดุ-อุปกรณ์ที่ต้องการเบิกอย่างน้อยหนึ่งรายการ");
            return false;
        }

    }
   
</script>

</asp:Content>