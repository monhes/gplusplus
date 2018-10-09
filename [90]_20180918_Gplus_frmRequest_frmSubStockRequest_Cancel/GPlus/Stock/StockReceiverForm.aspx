<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="StockReceiverForm.aspx.cs"
    Inherits="GPlus.Stock.StockReciverForm" MasterPageFile="../MasterPage/Main.Master"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function radioMe(e) {
            if (!e) e = window.event;
            var sender = e.target || e.srcElement;

            if (sender.nodeName != 'INPUT') return;
            var checker = sender;
            var chkBox = document.getElementById('<%= chkPaymentType.ClientID %>');
            var chks = chkBox.getElementsByTagName('INPUT');
            for (i = 0; i < chks.length; i++) {
                if (chks[i] != checker)
                    chks[i].checked = false;
            }
        }

        function InvokePop(p1, p2) {
            val1 = document.getElementById(p1).value;
            val2 = document.getElementById(p2).value;
            open_popup("StockReceiverLotPopup.aspx?ItemId=" + val2 + "&PoId=" + val1, "800", "500", 'Show Popup Window', 'yes', 'yes', 'yes');
            return false;
            // to handle in IE 7.0          
            //            if (window.showModalDialog)
            //            {      
            //                retVal = window.showModalDialog("StockReceiverLotPopup.aspx?ItemId=" + val2 + "&PoId=" + val1, 'Show Popup Window', "unadorned:yes;dialogHeight:500px;dialogWidth:800px;resizable:no;scrollbars:yes");
            //                //document.getElementById(fname).value = retVal;
            //            }      
        }

        function InvokePop2(p1, p2) {
            open_popup("StockReceiverLotPopup.aspx?ItemId=" + p2 + "&PoId=" + p1, "800", "500", 'Show Popup Window', 'yes', 'yes', 'yes');
            window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem').click();
            return false;
            // to handle in IE 7.0          
            //            if (window.showModalDialog) {
            //                retVal = window.showModalDialog("StockReceiverLotPopup.aspx?ItemId=" + p2 + "&PoId=" + p1, 'Show Popup Window', "unadorned:yes;dialogHeight:500px;dialogWidth:800px;resizable:no;scrollbars:yes");
            //                //document.getElementById(fname).value = retVal;
            //                __doPostBack('__Page', '');
            //            }     
        }

        function __doPostBack(eventTarget, eventArgument) {
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        }
        function OnHasVatChanged() {

            document.getElementById('<%= this.txtVat.ClientID %>').disabled = true;

        }

        function OnTextChange(textbox, nextextbox) {
            var tb = document.getElementById(textbox);
            var ntb = document.getElementById(nextextbox);
            if (tb.value == '') {
                ntb.disabled = false;
            }
            else {
                ntb.disabled = true;
            }
        }
    </script>
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาข้อมูลการรับของเข้าคลัง
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right" />
                        <td>
                            <asp:RadioButtonList ID="rdStockType" runat="server" RepeatDirection="Horizontal"
                                onKeyPress="return (event.keyCode!=13);" 
                                onKeyUp="return (event.keyCode!=13);" AutoPostBack="True" 
                                onselectedindexchanged="rdStockType_SelectedIndexChanged">
                                <asp:ListItem Text="รอรับเข้าคลัง" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="รับเข้าคลังแล้ว" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 130px;" align="right">
                            คลัง
                        </td>
                        <td>
                            <asp:DropDownList ID="cbStock" runat="server" Width="155" DataValueField="Stock_ID"
                                DataTextField="Stock_Name" onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            เลขที่รับเข้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtPRCodeSearch" runat="server" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่รับเข้า
                        </td>
                        <td>
                            <uc3:CalendarControl ID="ccFrom" runat="server" />
                        </td>
                        <td style="width: 130px;" align="right">
                            ถึงวันที่
                        </td>
                        <td>
                            <uc3:CalendarControl ID="ccTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            เลขที่ PO
                        </td>
                        <td>
                            <asp:TextBox ID="txtPOCodeSearch" runat="server" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่ออก PO
                        </td>
                        <td>
                            <uc3:CalendarControl ID="poFrom" runat="server" />
                        </td>
                        <td style="width: 130px;" align="right">
                            ถึงวันที่
                        </td>
                        <td>
                            <uc3:CalendarControl ID="poTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="BtnSearchClick" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="BtnCancelClick" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvStk" runat="server" AutoGenerateColumns="false" Width="100%"
                    OnRowCommand="gvStk_RowCommand" AllowSorting="true" 
                    PageIndexChanging= "gvStk_PageChanging" OnRowDataBound="gvStk_RowDataBound"
                    OnSorting="gvStk_Sorting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAdd" runat="server" CommandName="Ad" CausesValidation="false">
                                    <img src="../images/Commands/ebbtcbindex1_0.gif" alt="รับเพิ่ม" 
                                    onmouseover="this.src = '../images/Commands/ebbtcbindex1_2.gif';" onmouseout="this.src = '../images/Commands/ebbtcbindex1_0.gif';" border="0" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="พิมพ์">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="เลขที่รับเข้า" DataField="Receive_Stk_No" SortExpression="Receive_Stk_No" />
                        <asp:BoundField HeaderText="วันที่รับเข้า" DataField="Receive_Date" SortExpression="Receive_Date"   DataFormatString="{0:dd/MM/yyyy}"/>
                        
                        <asp:BoundField HeaderText="จำนวนเงินรับเข้า" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"
                                                    DataField="Sum_Net_Amount" SortExpression="Sum_Net_Amount" />
                        <asp:BoundField HeaderText="เลขที่PO" DataField="PO_Code" SortExpression="PO_Code" />
                        <asp:BoundField HeaderText="วันที่ส่งซื้อ" DataField="PO_Date" SortExpression="PO_Date"  DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="ยอดเงินในPO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}"
                            DataField="PO_Net_Amount" SortExpression="Total_Price" />
                        <asp:BoundField HeaderText="ทีมงาน"  DataField="Description" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField="" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Receive_Stk_Status" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" class="tableBody">
                <uc1:PagingControl ID="PagingControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlDetail" runat="server" Visible="false"   >
        <table cellpadding="0" cellspacing="0" width="805" >
            <tr>
                <td class="tableHeader">
                    รายการสินค้าที่รับ
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField runat="server" ID="hdReceiveID" />
                    <asp:HiddenField runat="server" ID="hdPOID" />
                    <table width="auto" style="overflow: scroll">
                        <tr>
                            <td colspan="4">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 130px;" align="right">
                                            เลขที่รับเข้า
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:TextBox ID="txtRecieveStkNo" runat="server" Width="100" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;" align="right">
                                            วันที่รับเข้า
                                        </td>
                                        <td style="width: 140px;" align="right">
                                            <asp:TextBox ID="txtRecieveDateTime" runat="server" Width="130" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 100px;" align="right">
                                            เลขที่สั่งซื้อ
                                        </td>
                                        <td style="width: 100px;" align="right">
                                            <asp:TextBox ID="txtPOCode" runat="server" Width="130" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 75px;" align="right">
                                            วันที่สั่งซื้อ
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:TextBox ID="txtPODate" runat="server" Width="100" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                หน่่วยงานที่ออกPO
                            </td>
                            <td>
                                <asp:TextBox ID="txtDepName" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชื่อโปรเจค
                            </td>
                            <td>
                                <asp:TextBox ID="txtProjectName" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                วัตถุประสงค์
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtReason" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                ผู้จำหน่าย/ผู้ผลิด
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierName" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                            </td>
                            <td align="right">
                                เลขที่ Invoice
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNo" runat="server" Width="100" onKeyPress="return (event.keyCode!=13);"
                                    onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                การชำระเงิน
                            </td>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="chkPaymentType" runat="server" RepeatDirection="Horizontal"
                                                onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);">
                                                <asp:ListItem Text="Credit" Value="0" />
                                                <asp:ListItem Text="Cash" Value="1" />
                                            </asp:CheckBoxList>
                                        </td>
                                        <td align="right">
                                            Credit Term
                                        </td>
                                        <td align="right">
                                            <asp:TextBox runat="server" ID="txtCreditTerm" Width="40" Style="text-align: right;"
                                                onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            วัน
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 130px;" align="right">
                                เอกสารใบส่งของ
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryDoc" runat="server" Width="100" onKeyPress="return (event.keyCode!=13);"
                                    onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่วางบิล
                            </td>
                            <td colspan="3">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <uc3:CalendarControl ID="dtInvoiceDate" runat="server" Width="100"></uc3:CalendarControl>
                                        </td>
                                        <td align="left">
                                            จำนวนเงินตามใบส่งของ
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvoiceAmount" runat="server" Width="80" Style="text-align: right;"
                                                onkeyup="commaSeparateNumber(this.value);"
                                                onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            บาท
                                        </td>
                                        <td align="right">
                                            วันที่ส่งของ
                                        </td>
                                        <td>
                                            <uc3:CalendarControl ID="dtDeliveryDate" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset>
                                    <asp:Panel runat="server" ID="panel2">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkDealDiscount" Text="ส่วนลดการค้า" onKeyPress="return (event.keyCode!=13);"
                                                        onKeyUp="return (event.keyCode!=13);" />
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rdTradeDiscountType" runat="server" RepeatDirection="Vertical"
                                                        onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);">
                                                        <asp:ListItem Text="ส่วนลดรวม" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="สวนลดแต่ละรายการ" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTradeDiscountPercent" runat="server" Width="30" Style="text-align: right;"
                                                        onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                        onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    %
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTradeDiscountAmount" runat="server" Width="40" Style="text-align: right;"
                                                        onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                        onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    บาท
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkCashDiscount" Text="ส่วนลดเงินสด" onKeyPress="return (event.keyCode!=13);"
                                                        onKeyUp="return (event.keyCode!=13);" Visible="False" />
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rdCashDiscountType" runat="server" RepeatDirection="Vertical"
                                                        onKeyPress="return (event.keyCode!=13);" 
                                                        onKeyUp="return (event.keyCode!=13);" Visible="False">
                                                        <asp:ListItem Text="ส่วนลดร่วม" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="สวนลดแต่ละรายการ" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCashDiscountPercent" runat="server" Width="30" Style="text-align: right;"
                                                        onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                        onpaste="return CancelKeyPaste(this)" Visible="False"></asp:TextBox>
                                                </td>
                                           <%--     <td align="left">
                                                    %
                                                </td>--%>
                                                <td>
                                                    <asp:TextBox ID="txtCashDiscountAmount" runat="server" Width="40" Style="text-align: right;"
                                                        onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                        onpaste="return CancelKeyPaste(this)" Visible="False"></asp:TextBox>
                                                </td>
                                           <%--     <td align="left">
                                                    บาท
                                                </td>--%>
                                          <%--      <td>
                                                    <asp:CheckBox runat="server" ID="chkHasVat" Text="คิด Vat" onKeyPress="return (event.keyCode!=13);"
                                                        onKeyUp="return (event.keyCode!=13);" onchange="OnHasVatChanged();" />
                                                </td>--%>
                                       

                                           



                                                      <td>   
                                                 
                                                    <fieldset style="width:auto">
                                    <legend>ภาษีมูลค่าเพิ่ม</legend>

                                    <table>
                                    <tr>
                                    
                                    <td>
                                        <asp:RadioButtonList ID="rblVatType" runat="server" Width="151px" style="float:right;">
                                         <asp:ListItem Text="Vat รวม" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Vat แต่ละรายการ" Value="1"></asp:ListItem>
                                     
                                    </asp:RadioButtonList>
                                    </td>


                                       <td>
                                      <asp:TextBox ID="txtVat" runat="server" Width="32" Style="text-align: right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                        onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>


                                                      
                                    </td>

                                    <td>
                                      &nbsp;       %  &nbsp;  
                                    </td>

                                       <td>
                                                          <fieldset>
                                    <legend>ราคาต่อหน่วย</legend>
                                    <asp:RadioButtonList ID="rblVat" runat="server" Width="118px" style="float:right;">
                                        <asp:ListItem Text="Include Vat" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Exclude Vat" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                                    </td>


                                    </tr>
                                    </table>
                                



                                   

                                


                                </fieldset>
                                                      
                                                    </td>

                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:LinkButton ID="btnRefreshItem" runat="server" Text="Refresh" CausesValidation="false"
                                     OnClick="btnRefreshItem_Click" Style="display: none;"></asp:LinkButton>
                                <asp:LinkButton ID="btnRefreshClose" runat="server" Text="Refresh" CausesValidation="false"
                                     OnClick="btnRefreshClose_Click" Style="display: none;"></asp:LinkButton>
                               
                             <div style=" overflow: scroll">
                             
                        
                               <asp:HiddenField ID="hdPO_ID" runat="server"/>
                          
                                <asp:GridView ID="gvStkItem" runat="server" AutoGenerateColumns="False" 
                                   style=" width:100%;"  onrowdatabound="gvStkItem_RowDataBound" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="รายละเอียด">
                                            <ItemTemplate>
                                               <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                                    CausesValidation="false"></asp:LinkButton>




                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" SortExpression="rownumber"
                                            ItemStyle-HorizontalAlign="center" >

                                      


                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                      


                                        <asp:BoundField HeaderText="รายการสินค้า" DataField="Inv_ItemCode"  SortExpression="Inv_ItemCode" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า/ของแถม"  DataField="Inv_ItemName" SortExpression="Inv_ItemName" />
                                        <asp:BoundField DataField="Package_Name" HeaderText="หน่วยนับ" 
                                            SortExpression="Package_Name" />
                                        <asp:BoundField DataField="Unit_Price" DataFormatString="{0:N2}" 
                                            HeaderText="ราคาต่อหน่วย" ItemStyle-HorizontalAlign="Right" 
                                            SortExpression="Unit_Price">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Unit_Quantity" DataFormatString="{0:N0}" 
                                            HeaderText="จำนวนที่สั่ง" ItemStyle-HorizontalAlign="Right" 
                                            SortExpression="Unit_Quantity">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="POItem_Receive_Quantity" HeaderText="รับแล้ว"  DataFormatString="{0:N0}" 
                                            ItemStyle-HorizontalAlign="Right" SortExpression="Receive_Quantity">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Receive_Quantity" HeaderText="จำนวนรับ"  DataFormatString="{0:N0}" 
                                            ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Temp_Receive_Quantity" HeaderText="จำนวนรับ"  DataFormatString="{0:N0}" 
                                            ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemDiscount_Price" DataFormatString="{0:N2}" 
                                            HeaderText="ส่วนลด" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GiveAway_Unit"
                                            HeaderText="ของแถม" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Total_Before_Vat" DataFormatString="{0:N2}" 
                                            HeaderText="จำนวนเงิน" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Vat_Amount" DataFormatString="{0:N2}" 
                                            HeaderText="Vat" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Net_Amount" DataFormatString="{0:N2}" 
                                            HeaderText="ราคารวม Vat" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                         

    
                                        <asp:TemplateField HeaderText="แตก Pack">
                            
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPack" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                         

    
                                   <asp:TemplateField HeaderText="ยกเลิกรับ">
                              
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCancelList" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

    

    
                                        <asp:BoundField DataField="POItem_TradeDiscount_Percent" 
                                            HeaderText="TradeDiscount_Percent" Visible="False" />
                                        <asp:BoundField DataField="POItem_TradeDiscount_Price" 
                                            HeaderText="TradeDiscount_Price" Visible="False" />


                                              <asp:BoundField DataField="Pack_Id_Base" 
                                            HeaderText="PackIdBase" Visible="False" />

    

    
                                        <asp:BoundField DataField="POItem_Vat" HeaderText="POItem_Vat" 
                                            Visible="False" />

                                         <asp:TemplateField HeaderText="ปิดการรับ">
                            
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkClose" runat="server" />
                                                <asp:HiddenField ID="hdRemarkClose" runat="server"/>
                                                <asp:HiddenField ID="hdPOItem_ID" runat="server"/>
                                                <asp:HiddenField ID="hdCheckFromDB" runat="server"/>
                                            <asp:ImageButton runat="server" ID="CommentClose" ImageUrl="~/images/application-view-list.png"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>


    
                                    </Columns>
                                </asp:GridView>
                              </div>
                          
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table>
                                    <tr>
                                        <td style="width:auto;" align="right">
                                      <%--    ราคารวมมูลค่ารับ--%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTotal" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                     <%--      บาท--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table>
                                    <tr>
                                        <td style="width:auto" align="right">
                                         <%--  ส่วนลด--%>
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txtTotalDiscount" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                          <%--  บาท--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:auto;" align="right">
                                            ราคาสุทธิ
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txtNetAmount" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true"  DataFormatString="{0:N2}"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            บาท
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                               <asp:TextBox ID="txtTotalBeforeVat" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>
                        </tr>

                           <tr>
                               <asp:TextBox ID="txtVatAmount" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>

                                                <asp:TextBox ID="txtVatUnitType" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>
                        </tr>


                        <tr>
                        
                        
                                                <asp:TextBox ID="txtNetSum" Style="text-align: right;" Enabled="false" runat="server"
                                                Width="150" Font-Bold="true" Visible="False"></asp:TextBox>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="BtnSaveClick" 
                            Enabled="False" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="ยกเลิกการรับของ" CausesValidation="False"
                            OnClick="BtnCancelClick2" SkinID="ButtonMiddleLong" OnClientClick="return confirm('คุณต้องการยกเลิกรับของหรือไม่');" />
                        &nbsp; &nbsp;
                        <asp:Button ID="Button1" runat="server" Text="ล้างข้อมูล" CausesValidation="False"
                            OnClick="BtnCancelClick1" />
                        <asp:Button ID="btnCloseReceive" runat="server" Text="ปิดการรับสินค้า" SkinID="ButtonMiddleLong" OnClick="btnCloseReceiveClick" Visible = "false" />

                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script type="text/javascript">

        function commaSeparateNumber(input) {
            input = input.replace(/\,/g, '');

            var output = input
            if (parseFloat(input)) {
                input = new String(input); // so you can perform string operations
                var parts = input.split("."); // remove the decimal part
                parts[0] = parts[0].split("").reverse().join("").replace(/(\d{3})(?!$)/g, "$1,").split("").reverse().join("");
                output = parts.join(".");
            }

            document.getElementById('<%=txtInvoiceAmount.ClientID %>').value = output;
        }
    </script>
</asp:Content>
