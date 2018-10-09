<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupReceiveDetail.aspx.cs" Inherits="GPlus.Request.popupReceiveDetail" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<base target="_self"/>--%>
    <title>รายละเอียดสินค้า</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
     <link href="../themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css"/>
    <script src="../js/ui/1.10.3/jquery-ui.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 105px;
        }
        .style3
        {
            width: 55px;
        }
        .style4
        {
            width: 66px;
        }
        .style5
        {
            width: 67px;
        }
        .style6
        {
            height: 14px;
        }
    </style>
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
            var rcvUnit = 0; // document.getElementById('txtTotalUnit').value; // จำนวนนับรวม
            var vat = 0; // document.getElementById('hdVat').value;
            var totalPriceSum = 0; //  document.getElementById('hdTotalPriceSum').value; // จำนวนนับรวม
            var totalPrice = 0; // rcvUnit * unitPrice;
            var tradeDisType = 0; // document.getElementById('hdTradeDiscount_Type').value;
            var vatUnitType = 0; // document.getElementById('hdVatUnit_Type').value;


            var maxRecv = 0;



            maxRecv = document.getElementById('hdTotalUnit').value;

            unitPrice = document.getElementById('txtUnitPrice').value;
            tradeDisCountPer = document.getElementById('hdTradeDiscountPercent').value;
            tradeDisCountPrice = document.getElementById('hdTradeDiscountPrice').value;
            rcvUnit = document.getElementById('txtTotalUnit').value; // จำนวนนับรวม

            document.getElementById('dlLot_lucLot_0_txtReceiveNumber_0').value = rcvUnit;

            vat = document.getElementById('hdVat').value;
            totalPriceSum = document.getElementById('hdTotalPriceSum').value; // จำนวนนับรวม
            totalPrice = rcvUnit * unitPrice;
            tradeDisType = document.getElementById('hdTradeDiscount_Type').value;
            vatUnitType = document.getElementById('hdVatUnit_Type').value;



            //txtIncludeVatPrice

            if (tradeDisType == "0") { // ส่วนลด รวม

                if (document.getElementById('hdTradeDiscountPercent').value > 0) {  // ส่วนลดเป็น % มากกว่า 0
                    disPrice = tradeDisCountPer * totalPrice / 100;

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
                vatPrice = (vat / 100) * totalBeforeVat;
                includeVatPrice = totalBeforeVat + vatPrice;

            } else { // include vat

                includeVatPrice = totalPrice - disPrice;
                totalBeforeVat = includeVatPrice / (1 + vat / 100);
                vatPrice = includeVatPrice - totalBeforeVat;



            }







            document.getElementById('txtIncludeVatPrice').value = (Math.round(includeVatPrice * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            document.getElementById('txtTotalBeforeVat').value = (Math.round(totalBeforeVat * 1e2) / 1e2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
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
    <center>
        <table cellpadding="0" cellspacing="0" width="95%">
     
            <tr>
                <td class="tableHeader" align="left">
                    รายการรับ</td>
            </tr>
            <tr>
                <td class="tableBody" align="left">
                    <table cellpadding="0" cellspacing="0" width="auto">
                       <tr style="height:25px;">
        <td></td>

        </tr>
                       <tr >
                            <td style="width: 105px" align="right">
                                ครั้งที่จ่าย</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 50px" align="right">
                                <asp:TextBox ID="txtNoPay" runat="server" Width="67px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td class="style3">
                                วันที่จ่าย</td>
                            <td align="right" class="style1">
                           
                                <asp:TextBox ID="txtDatePay" runat="server" ReadOnly="True"></asp:TextBox>
                           
                            </td>
                            <td>
                                    &nbsp;</td>
                            <td style="width: 105px;" align="right">
                           
                                ผู้จ่าย</td>
                            <td style="width: 100px" align="right">
                           
                                <asp:TextBox ID="txtWhoPay" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 50px" align="right">
                                &nbsp;</td>
                            <td class="style3">
                                <table cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                          
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" class="style1">
                                &nbsp;</td>
                            <td>
                             
                                &nbsp;</td>
                            <td style="width: 105px" align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                รายการรับ</td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 50px;" align="right">
                              <span style=" display:none;">  ส่วนลดเงินสด </span>
                            </td>
                            <td class="style3">
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
                            <td align="right" class="style1">
                              
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 105px" align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>



                                <tr>
                            <td colspan="10" align="right" style=" height:15px;" >
                           <div style="width: 100%; max-height: 200; overflow:auto" >
                                                <asp:GridView runat="server" ID="gvReqRec" OnRowDataBound="GvReqRecRowDataBound" OnRowCommand="GvReqRecRowCommand" AutoGenerateColumns="false" 
                                                    Width="100%"
                                                   SkinId="GvNormal">
                                                    <Columns>

                                                                 <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="ViewReceive"
                                                                CausesValidation="false"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:BoundField HeaderText="รับครั้งที่" DataField="rownumber" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="ผู้รับ" DataField="Account_Fname" ItemStyle-HorizontalAlign="Center"/>
                                                        <asp:BoundField HeaderText="วันที่รับ" DataField="Receive_Date" ItemStyle-HorizontalAlign="Center"/>
                                                       <asp:BoundField HeaderText="สถานะ" DataField="Status" ItemStyle-HorizontalAlign="Center"/>
                                               

                                                   
                                                   
                                                    </Columns>
                                                </asp:GridView>
                                                </div>
                            </td>
                            
                        </tr>


                            <tr>
                            <td colspan="7" align="right" >
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
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
                        รายละเอียกดการรับ</td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                        <td class="style6"></td>
                        </tr>
                            <tr>
                                <td style="width: 100px;" align=right >
                                    รับครั้งที่</td>
                                    <td style="width: 100px" >
                                        <asp:TextBox ID="txtNoRcv" runat="server" Width="67px" ReadOnly="True"></asp:TextBox>
                                </td>
                                       <td align=right class="style4"  >
                                           ผู้รับ</td>

                                     <td style="width: 120px" >
                                         <asp:TextBox ID="txtRcver" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                                </td>
                                     <td  align=right class="style5" >
                                         วันที่รับ</td>
                                <td align="left">
                                    <asp:TextBox ID="txtRcvDate" runat="server" Width="150px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                                 <tr>
                                <td style="width: 100px" >
                                    &nbsp;</td>
                                    <td style="width: 100px" >
                                    &nbsp;</td>
                                       <td class="style4" >
                                    &nbsp;</td>

                                     <td style="width: 100px" >
                                    &nbsp;</td>
                                     <td class="style5" >
                                    &nbsp;</td>
                                <td align="right">
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel runat="server" ID="giveAwayItemNamePanel" Visible="true">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="tableBody">
                                        <table cellpadding="0" cellspacing="0" width="93%">
                                            <tr>
                                                <td style="width: 100%" align="center">

                                              <asp:GridView ID="gvReqRecItem" runat="server" AutoGenerateColumns="false"
                                                 OnRowDataBound = "gvReqRecItemDataBound"
                                                       Width="100%"  SkinId="GvLong">
                                                        <Columns>
                                                       
                                                            <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Inv_ItemCode" HeaderText="รหัสสินค้า" 
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Inv_ItemName" HeaderText="ชื่อสินค้า" 
                                                                ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Description" HeaderText="หน่วยที่เบิก" 
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="Avg_Cost" HeaderText="ราคาต่อหน่วย" 
                                                                ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                    
                                                            <asp:BoundField DataField="Pay_Quantity" HeaderText="จำนวนที่จ่ายสะสม" 
                                                                ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="Receive_Quantity" HeaderText="จำนวนทีรับสะสม" 
                                                                ItemStyle-HorizontalAlign="Right" />
                                                     
                                                         


                                                             <asp:BoundField DataField="Receive_Qty" HeaderText="รับครั้งนี้" 
                                                                ItemStyle-HorizontalAlign="Right" />


                                                           
                                        <asp:TemplateField HeaderText="ยกเลิกรับ">

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCancelList" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                                         <%--<asp:BoundField DataField="Pack_id" HeaderText="Pack_ID" 
                                                                ItemStyle-HorizontalAlign="Right" />--%>


                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField  ID="packId" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    &nbsp;
                                                    
                                                    <br />
                                                    <asp:Button ID="btCancelReceive" align="center" runat="server" 
                                                        Text="ยกเลิกการรับ" onclick="btCancelReceive_Click" OnClientClick="return cancel();" />
                                                </td>
                                                                               
                                            </tr>

                                            </table>
                                                  
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
        <asp:Panel ID="multiLotPanel" runat="server" Visible="false">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableBody">
                        &nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
    </center>
    </form>
    <div id="popup" 
        style="border-color: rgb(236, 70, 126); 
        border-style: solid; 
        border-width: medium; 
        display:none; 
        text-align:center">
            <br /><br />กำลังดำเนินการ.....
    </div>
    <script type="text/javascript">
        function cancel()
        {
            if (confirm('คุณต้องการยกเลิกรับของหรือไม่') == true)
            {
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
        // CalculateNetPrice();
    </script>
</body>
</html>
