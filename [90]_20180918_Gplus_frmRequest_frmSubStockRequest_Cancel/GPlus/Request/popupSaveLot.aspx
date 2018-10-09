<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupSaveLot.aspx.cs" Inherits="GPlus.Request.popupSaveLot" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>



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


        function EnterLotAuto() {
          var   rcvUnit = document.getElementById('txtTotalUnit').value; // จำนวนนับรวม

            document.getElementById('dlLot_lucLot_0_txtReceiveNumber_0').value = rcvUnit;



            document.getElementById('dlLot_lucLot_0_gvStk_0_txtEachUnitNumber_0').value = rcvUnit;
        }
       

        function checkNumberPress(e) {
            // allow number key only
            if (!(e.keyCode > 47 && e.keyCode < 58) && e.keyCode != 8 && !(e.keyCode > 95 && e.keyCode < 106) && e.keyCode != 37 && e.keyCode != 39) {
                e.preventDefault();
            }
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
                                หน่วย</td>

                              
                            <td>
                                <asp:TextBox ID="txtUnit" runat="server" Width="100" ReadOnly="True"></asp:TextBox>
                                           
                                     </td>
                            <td style="width: 105px;" align="right">
                                จำนวนรวม
                            </td>
                            <td style="width: 100px" align="right">
                                <asp:TextBox ID="txtTotalUnit" runat="server" CausesValidation="false" Style="text-align: right;"
                                    Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" 
                                    onkeyDown= "checkNumberPress(event);" onKeyUp="EnterLotAuto();"
                                    onpaste="return CancelKeyPaste(this)" autocomplete="off" 
                                    BackColor="#CCFFFF"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtLotNo2" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                                    ControlToValidate="txtTotalUnit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="exrfvtxtLotNo2" runat="server" Enabled="True" TargetControlID="rfvtxtLotNo2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 105px" align="right">
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
                              
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 105px; margin-right :10px;" >
                        <asp:Button ID="btnAddLot" runat="server" Text="Add Lot" onclick="btnAddLot_Click" />
                            </td>
                            <td >  
                        <asp:Button ID="btnDeleteLot" runat="server" Text="Delete Lot" onclick="btnDeleteLot_Click" />
                            </td>
                        </tr>



                                <tr>
                            <td colspan="7" align="right" style=" height:15px;" >
                           
                                &nbsp;</td>
                            
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
                                    <asp:DataList ID="dlLot" runat="server" >
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
        <asp:Panel ID="panelButton" runat="server">
            <table cellpadding="0" cellspacing="0" width="95%">
                <tr>
                    <td class="tableBody" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" onclick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClientClick="window.close();"
                            CausesValidation="false" onclick="btnCancel_Click"  />
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

