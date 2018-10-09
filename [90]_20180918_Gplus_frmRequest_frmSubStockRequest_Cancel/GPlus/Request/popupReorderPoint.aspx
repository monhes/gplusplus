<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupReorderPoint.aspx.cs" Inherits="GPlus.Request.popupReorderPoint" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<base target="_self"/>--%>
    <title>รายละเอียดสินค้า</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
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
        .style7
        {
            width: 205px;
        }
    </style>
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); background-repeat:repeat-y; padding-top: 7px;" onload="javascript:OnLoadBody();">
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
                    ระบุรายการสินค้า</td>
            </tr>
            <tr>
                <td class="tableBody" align="left">
                    <table cellpadding="0" cellspacing="0" width="auto">
                       <tr style="height:25px;">
        <td></td>

        </tr>
                       <tr >
                            <td style="width: 105px" align="right">
                                ประเภทวัสดุอุปกรณ์</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 50px" align="right">
                                &nbsp;</td>
                            <td class="style3">
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195"  AutoPostBack="true" 
                                    DataTextField="Cat_Name" DataValueField="Cate_ID" onselectedindexchanged="ddlMaterialType_SelectedIndexChanged"
                                    >
                            </asp:DropDownList>
                            </td>
                            <td align="right" class="style1">
                           
                                ประเภทอุปกรณ์ย่อย</td>
                            <td>
                                    &nbsp;</td>
                            <td style="width: 105px;" align="right">
                           
                            <asp:DropDownList ID="ddlSubMaterialType" runat="server" Width="195" 
                                    DataTextField="SubCate_Name" DataValueField="SubCate_ID" onselectedindexchanged="ddlSubMaterialType_SelectedIndexChanged"
                                    >
                            </asp:DropDownList>
                            </td>
                            <td style="width: 100px" align="right">
                           
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                รหัสสินค้า</td>
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
                                          
                                            <asp:TextBox ID="txtItemCode" runat="server"></asp:TextBox>
                                          
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" class="style1">
                                ชื่อสินค้า</td>
                            <td>
                             
                                &nbsp;</td>
                            <td style="width: 105px" align="right">
                                <asp:TextBox ID="txtItemName" Width="154px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
                                &nbsp;</td>
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
                           <div align="center" style="width: 100%; max-height: 200; overflow:auto;" >
                               <asp:Button ID="btSearch" runat="server" Text="ค้นหา" 
                                   onclick="btSearch_Click" />   
                               &nbsp; &nbsp; &nbsp;     
                               <asp:Button ID="btCancel" runat="server" Text="ยกเลิก" 
                                   onclick="btCancel_Click" />             
                                                </div>
                            </td>
                            
                        </tr>


                            <tr>
                            <td colspan="7" align="right" >
                           
                                </td>
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
                        ผลการค้นหา</td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                        <td class="style6"></td>
                        </tr>
                            <tr>
                                <td style="width: 200px;" align=right >
                                    &nbsp;</td>
                                    
                                       <td align=right class="style7"  >
                                           <asp:Label ID="lbNotfound" runat="server" Font-Bold="True" Font-Size="Large" 
                                               ForeColor="#0066FF" Text="ไม่พบรายการ"></asp:Label>
                                </td>

                                     <td style="width: 120px" >
                                         &nbsp;</td>
                                     <td  align=right class="style5" >
                                         &nbsp;</td>
                                <td align="left">
                                    &nbsp;</td>
                            </tr>
                                 <tr>
                                <td style="width: 100px" >
                                    &nbsp;</td>
                                    <td class="style7" >
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
                   <td class="tableBody">
                        <center>
                   
                        <asp:GridView ID="gvRequestItem" runat="server" AutoGenerateColumns="false"     OnRowDataBound="GvRequestItemRowDataBound"  OnRowCommand="GvRequestListRowCommand" 
                           SkinId="GvLong" Width="98%">
                            <Columns>
                         
       <%--                         <asp:BoundField HeaderText="ลำดับ" DataField="rownumber"  ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="Inv_ItemCode" HeaderText="รหัสสินค้า" 
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Inv_ItemName" HeaderText="ชื่อสินค้า" 
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="หน่วยที่เบิก" 
                                    ItemStyle-HorizontalAlign="Center" />
                                   <asp:BoundField DataField="OnHand_Qty" HeaderText="จำนวนในคลัง" 
                                    ItemStyle-HorizontalAlign="Right"     />                           
                      
                          
                                 <asp:BoundField DataField="Reorder_Point" HeaderText="Reorder Point" 
                                    ItemStyle-HorizontalAlign="Right"  />
                             
                                <asp:TemplateField HeaderText="จำนวนเบิก">
                                    <ItemTemplate>
                         
                                        <asp:TextBox ID="txt_OrderQty" runat="server" 
                                         onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                            Width="50px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="packId" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lbSelect" runat="server" Text = "เลือก" CommandName="Select" />
                                   
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                   
                    </center>
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter">
                    <uc1:PagingControl runat="server" ID="pagingControlReqList" />

                    </td>
                </tr>
            </table>
        </asp:Panel>
 <asp:Panel ID="panel1" runat="server">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableHeader" align="left">
                        รายการที่เลือก</td>
                </tr>
                <tr>
                    <td class="tableBody">
                    
                    </td>
                </tr>
                <tr>
                     <td class="tableBody">
                        <center>
                        <asp:GridView ID="gvRequestItemResult" runat="server" AutoGenerateColumns="false"  
                         OnRowDataBound="GvRequestItemResultRowDataBound" OnRowDeleting="GvRequestItemResultRowDeleting"  OnRowCommand="GvRequestListResultRowCommand" 
                           SkinId="GvLong" Width="98%">
                            <Columns>
                         
                               <%-- <asp:BoundField HeaderText="ลำดับ" DataField="rownumber"  ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="Inv_ItemCode" HeaderText="รหัสสินค้า" 
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Inv_ItemName" HeaderText="ชื่อสินค้า" 
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="หน่วยที่เบิก" 
                                    ItemStyle-HorizontalAlign="Center" />
                                   <asp:BoundField DataField="OnHand_Qty" HeaderText="จำนวนในคลัง" 
                                    ItemStyle-HorizontalAlign="Right"     />                           
                      
                          
                                 <asp:BoundField DataField="Reorder_Point" HeaderText="Reorder Point" 
                                    ItemStyle-HorizontalAlign="Right"  />

                              <%--   <asp:BoundField DataField="Receive_Qty" HeaderText="จำนวนเบิก" 
                                    ItemStyle-HorizontalAlign="Right"  />--%>
                             
                                <asp:TemplateField HeaderText="จำนวนเบิก">
                                    <ItemTemplate>
                         
                                        <asp:TextBox ID="txt_OrderQty" runat="server" 
                                         onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)"
                                            Width="50px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="packId" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lbDelete" runat="server" Text = "ลบ" CommandName="Delete" />
                                   
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                      </center>
                      <br />
                    
                    
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter">
                    <center>
                           <asp:Button ID="btOK" runat="server" Text="ตกลง" onclick="btOK_Click" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btClose" runat="server" Text="ยกเลิก" onclick="btClose_Click" />
                    </center>
                   
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
