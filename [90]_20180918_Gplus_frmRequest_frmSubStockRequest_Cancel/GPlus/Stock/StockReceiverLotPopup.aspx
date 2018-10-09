<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="StockReceiverLotPopup.aspx.cs"
    Inherits="GPlus.Stock.StockReceiverLotPopup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="~/Stock/StockReceiverStockItemUserControl.ascx" TagName="StockUserControl"
    TagPrefix="sctl1" %>
<%@ Register Src="~/Stock/StockReceiverLotUserControl.ascx" TagName="LotUserControl" TagPrefix="sctl2"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<base target="_self"/>--%>
    <title>รายละเอียดสินค้า</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;" onload="javascript:OnLoadBody();">
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript">
        function OnLoadBody() {

          //  CalculateNetPrice();
        }
        function PassValues() {
            //window.opener.document.forms(0).submit();
            self.close();
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
           // CalculateNetPrice();
        }


        function CalculatePrice() {
            var totalPrice = 0;
            var disPrice = 0;
            var totalBeforeVat = 0;
            var vatPrice = 0;

            var includeVatPrice = 0; // document.getElementById('txtIncludeVatPrice').value;


            var unitPrice = 0;  //document.getElementById('txtUnitPrice').value;
            var tradeDisCountPer = 0;  //document.getElementById('hdTradeDiscountPercent').value;
            var tradeDisCountPrice = 0; //document.getElementById('hdTradeDiscountPrice').value;
            var rcvUnit = 0;// document.getElementById('txtTotalUnit').value; // จำนวนนับรวม
            var vat = 0;// document.getElementById('hdVat').value;
            var totalPriceSum = 0; //  document.getElementById('hdTotalPriceSum').value; // จำนวนนับรวม
            var totalPrice = 0; // rcvUnit * unitPrice;
            var tradeDisType = 0; // document.getElementById('hdTradeDiscount_Type').value;
            var vatUnitType = 0; // document.getElementById('hdVatUnit_Type').value;


            var maxRecv = 0;

           
             
             maxRecv  = document.getElementById('hdTotalUnit').value;

             unitPrice = document.getElementById('txtUnitPrice').value;
             tradeDisCountPer = document.getElementById('hdTradeDiscountPercent').value;
             tradeDisCountPrice = document.getElementById('hdTradeDiscountPrice').value;
             rcvUnit = document.getElementById('txtTotalUnit').value; // จำนวนนับรวม

             document.getElementById('dlLot_lucLot_0_txtReceiveNumber_0').value = rcvUnit;


             document.getElementById('dlLot_lucLot_0_gvStk_0_txtEachUnitNumber_0').value = rcvUnit;
             vat = document.getElementById('hdVat').value;
             totalPriceSum = document.getElementById('hdTotalPriceSum').value; // จำนวนนับรวม
             totalPrice = rcvUnit.replace(/\,/g,'') * unitPrice.replace(/\,/g,'');
             tradeDisType = document.getElementById('hdTradeDiscount_Type').value;
             vatUnitType = document.getElementById('hdVatUnit_Type').value;



             //txtIncludeVatPrice
           
            if (tradeDisType == "0") { // ส่วนลด รวม

                if (document.getElementById('hdTradeDiscountPercent').value > 0) {  // ส่วนลดเป็น % มากกว่า 0
                   disPrice = tradeDisCountPer * totalPrice/100;

                } else { // ส่วนลดเป็นราคา
                   disPrice = tradeDisCountPrice * totalPrice / totalPriceSum;
               }

            } else {   //ส่วนลดแยก
             
                if (document.getElementById('hdTradeDiscountPercent').value > 0) {  // ส่วนลดเป็น % มากกว่า 0
                    disPrice = tradeDisCountPer * totalPrice / 100;
                } else { // ส่วนลดเป็นราคา
                    disPrice = tradeDisCountPrice;
                }

            }

            if (vatUnitType == "0") { // exclude vat
          
                totalBeforeVat = totalPrice - disPrice;
                vatPrice = (vat/100) * totalBeforeVat;
                includeVatPrice = totalBeforeVat + vatPrice;

            } else { // include vat

                includeVatPrice = totalPrice - disPrice;
                totalBeforeVat = includeVatPrice / (1 + vat / 100);
                vatPrice = includeVatPrice - totalBeforeVat;



            }







            document.getElementById('txtIncludeVatPrice').value = (Math.round(includeVatPrice * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            document.getElementById('txtTotalBeforeVat').value =  (Math.round(totalBeforeVat * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            document.getElementById('txtTotalPrice').value = (Math.round(totalPrice * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            document.getElementById('txtVatPrice').value = (Math.round(vatPrice * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            document.getElementById('txtDiscountPrice').value = (Math.round(disPrice * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
         
        
        }

        function checkNumberPress(e) {
            // allow number key only
            if (!(e.keyCode > 47 && e.keyCode < 58) && e.keyCode != 8 && !(e.keyCode > 95 && e.keyCode < 106) && e.keyCode != 37 && e.keyCode != 39) {
                e.preventDefault();
            }
        }


        function CalculateNetPrice() {


        }

    </script>
    <asp:HiddenField runat="server" ID="_hfTotalPrice" />
    <asp:HiddenField runat="server" ID="_hfTotalDiscount" />
    <asp:HiddenField runat="server" ID="_hfNetAmount" />
    <asp:HiddenField runat="server" ID="hdPackID" />
    <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
    <asp:HiddenField ID="hdTotalUnit" runat="server" />
       <asp:HiddenField ID="hdVat" runat="server" />
        <asp:HiddenField ID="hdTradeDiscountPrice" runat="server" />
            <asp:HiddenField ID="hdTradeDiscountPercent" runat="server" />
                 <asp:HiddenField ID="hdTradeDiscount_Type" runat="server" />

            <asp:HiddenField ID="hdTotalPriceSum" runat="server" />

              <asp:HiddenField ID="hdVatUnit_Type" runat="server" />
    <center>
        <table cellpadding="0" cellspacing="0" width="95%">
            <tr>
                <td class="tableHeader" align="left">
                    สินค้า
                </td>
            </tr>
            <tr>
                <td class="tableBody" align="left">
                    <table cellpadding="0" cellspacing="0" width="auto">
                        <tr>
                            <td style="width: 105px" align="right">
                                รหัสสินค้า
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemCode" Style="text-align: right;" runat="server" 
                                    Width="100" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 50px" align="right">
                                รายการ
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName" runat="server" Width="120" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 105px" align="right">
                                ส่วนลดการค้า
                            </td>
                            <td>
                                            <asp:TextBox ID="txtTradeDiscountPercent" CausesValidation="false" Style="text-align: left;"
                                                runat="server" Width="30" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" 
                                                onpaste="return CancelKeyPaste(this)" ReadOnly="True"></asp:TextBox>
                                    &nbsp; %
                                     </td>
                            <td style="width: 105px;" align="right">
                                จำนวนนับรวม
                            </td>
                            <td style="width: 100px" align="right">
                                <asp:TextBox ID="txtTotalUnit" runat="server" CausesValidation="false" Style="text-align: right;"
                                    Width="80" onKeyPress="return (event.keyCode!=13);"
                                 onkeyDown= "checkNumberPress(event);" onKeyUp="CalculatePrice();"
                                    onpaste="return CancelKeyPaste(this)" autocomplete="off" Text="200" BackColor="#CCFFFF"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtLotNo2" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                                    ControlToValidate="txtTotalUnit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="exrfvtxtLotNo2" runat="server" Enabled="True" TargetControlID="rfvtxtLotNo2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                ราคาต่อหน่วย
                            </td>
                            <td>
                                <asp:TextBox ID="txtUnitPrice" CausesValidation="false" runat="server" 
                                    Style="text-align: right;" Width="100" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 50px" align="right">
                                หน่วยนับ
                            </td>
                            <td>
                                <table cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                <asp:TextBox ID="txtUnit" runat="server" Width="100" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                          
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 105px" align="right">
                                ราคารวม
                            </td>
                            <td>
                             
                                <asp:TextBox ID="txtTotalPrice" Style="text-align: right;" CausesValidation="false"
                                    runat="server" Width="80" align="right" ReadOnly="True"></asp:TextBox>
                                        </td>
                            <td style="width: 105px" align="right">
                                ส่วนลด
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscountPrice" Style="text-align: right;"
                                    runat="server" Width="80" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 50px;" align="right">
                              <span style=" display:none;">  ส่วนลดเงินสด </span>
                            </td>
                            <td>
                                <table cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <span style=" display:none;">   %  </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 105px" align="right">
                              
                                ราคารวมก่อน Vat</td>
                            <td>
                                <asp:TextBox ID="txtTotalBeforeVat" Style="text-align: right; " CausesValidation="false"
                                   align="right"  runat="server" Width="80" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 105px" align="right">
                                Vat
                            </td>
                            <td>
                                <asp:TextBox ID="txtVatPrice" Style="text-align: right;" CausesValidation="false"
                                    runat="server" Width="80" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>



                                <tr>
                            <td colspan="7" align="right" style=" height:15px;" >
                           
                            </td>
                            
                        </tr>


                            <tr>
                            <td colspan="7" align="right" >
                                ราคารวม Vat
                            </td>
                            <td>
                                <asp:TextBox ID="txtIncludeVatPrice" Style="text-align: right;" runat="server"
                                    Width="80" BackColor="#CCFFFF" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>


                  
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="panel0" runat="server">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableHeader" align="left">
                        ของแถมรายการกับที่ซื้อ
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdGiveawayType" RepeatDirection="Horizontal" AutoPostBack="true"
                                        runat="server" OnSelectedIndexChanged="RdGiveawayTypeSelectedIndexChanged">
                                        <asp:ListItem Text="ไม่มีของแถม" Value="1" Selected="True" />
                                        <asp:ListItem Text="มีของแถมรายการและหน่วยเดียวกับที่ซื้อ" Value="2" />
                                      <%--  <asp:ListItem Text="มีของแถมต่างรายการหรือต่างหน่วย"  Value="3" />--%>
                                    </asp:RadioButtonList>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btAddLot" runat="server" CausesValidation="false" 
                                        OnClick="BtnAddLotClick" style="" Text="Add Lot" />

                                    <asp:Button ID="btnDelete" runat="server" CausesValidation="false" 
                                        Enabled="False" OnClick="BtnDeleteLotClick" style="" Text="Delete Lot" />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel runat="server" ID="giveAwayItemNamePanel" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="tableBody">
                                        <table cellpadding="0" cellspacing="0" width="93%">
                                            <tr>
                                                <td style="width: 100px" align="center">
                                                    รายการ
                                                </td>
                                                <td style="width: 150px" align="left">
                                                    <asp:TextBox ID="txtFreeItemName" runat="server" Enabled="false" Width="200px"></asp:TextBox>
                                                </td>
                                                <td style="width: 100px" align="right">
                                                    จำนวน
                                                </td>
                                                <td style="width: 70px" align="right">
                                                    <asp:TextBox ID="txtFreeItemTotalUnit" CausesValidation="false" AutoPostBack="false"
                                                        Style="text-align: right;" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                        onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                                </td>
                                                <td style="width: 105px">
                                                    <asp:DropDownList runat="server" ID="ddPackList" Width="100" DataTextField="Package_Name"
                                                        DataValueField="Pack_ID" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="multiGiveAwayPanel" runat="server" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="tableBody">
                                        <fieldset>
                                            <legend>ของแถมต่างรายการหรือต่างหน่วย </legend>
                                            <table cellpadding="0" cellspacing="0" width="93%">
                                                <tr>
                                                    <td>
                                                        <sctl1:StockUserControl ID="suControl1" runat="server" />
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:Button runat="server" ID="btnAddItem" Width="150" Text="เพิ่มรายการ" OnClick="BtnAddItemClick"
                                                            CausesValidation="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="lotPanel" runat="server" Visible="true">
            <br />
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableHeader" align="left">
                        Lot สินค้า
                    </td>
                   
                </tr>
               <%-- <tr>
                    <td class="tableBody">
                        <asp:Panel ID="PanelStockLotItem" runat="server" Visible="true">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <sctl2:LotUserControl ID="lucLotMain" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>--%>
                <tr>
                    <td class="tableBody">
                        <table cellpadding="0" cellspacing="0" width="93%">
                            <tr>
                                <td>
                                    <asp:DataList ID="dlLot" runat="server" onitemdatabound="dlLot_ItemDataBound">
                                        <ItemTemplate>
                                            <sctl2:LotUserControl ID="lucLot" runat="server" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="multiLotPanel" runat="server" Visible="false">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableBody">
                        <center>
                            <asp:GridView ID="gdStrkItem" runat="server" AutoGenerateColumns="false" Width="100%"
                                OnRowCommand="GdStrkItemRowCommand" OnRowDataBound="GdStrkItemRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="ลบ">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDel" runat="server" Text="ลบ" CommandName="Del" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="ลำดับ" DataField="" SortExpression="" />
                                    <asp:BoundField HeaderText="รหัสสินค้า" DataField="" SortExpression="" />
                                    <asp:BoundField HeaderText="รายการ" DataField="" SortExpression="" />
                                    <asp:TemplateField HeaderText="หน่วยนับ">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddUnit" runat="server" CausesValidation="false" DataTextField="Package_Name"
                                                DataValueField="Pack_ID">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="จำนวน" HeaderStyle-Width="50">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtItemCount" Style="text-align: right;" Width="30" runat="server"
                                                CausesValidation="false" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                            <asp:CompareValidator ID="CompareFieldValidator1" runat="server" ForeColor="Red"
                                                ControlToValidate="txtItemCount" ValueToCompare="1" Type="Integer" Operator="GreaterThanEqual"
                                                ErrorMessage="กรุณาใส่จำนวนที่มากกว่า 0">
                                                    *
                                            </asp:CompareValidator>
                                            <asp:ValidatorCalloutExtender ID="exrtxtItemCount" runat="server" Enabled="True"
                                                TargetControlID="CompareFieldValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot No." HeaderStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLotNo" Style="text-align: right;" Width="40" runat="server" CausesValidation="false"
                                                onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                                onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvtxtLotNo" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                                                ControlToValidate="txtLotNo" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="exrfvtxtLotNo" runat="server" Enabled="True" TargetControlID="rfvtxtLotNo">
                                            </asp:ValidatorCalloutExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="เพิ่ม Lot">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnAddSameLot" runat="server" Text="เพิ่ม" CommandName="AS" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Barcode Supplier">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSupBar" Width="60" runat="server" CausesValidation="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="จำนวน Barcode">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSupBarCount" Style="text-align: right;" Width="40" runat="server"
                                                CausesValidation="false" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="สถานที่">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddLocation" runat="server" DataTextField="Location_Name" DataValueField="Location_ID"
                                                CommandName="AS" CausesValidation="false">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="เพิ่มสถานที่จัดเก็บ">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnAddLocation" runat="server" Text="เพิ่ม" CommandName="AL"
                                                CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </center>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="panelButton" runat="server">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableBody" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="BtnSaveClick" />
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClientClick="window.close();"
                            CausesValidation="false" onclick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter">
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
    </form>
    <script type="text/javascript">
        // CalculateNetPrice();
    </script>
</body>
</html>
