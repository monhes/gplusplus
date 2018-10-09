<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POControl.ascx.cs" Inherits="GPlus.UserControls.POControl" %>

<%@ Register src="SupplierDropdownControl.ascx" tagname="SupplierDropdownControl" tagprefix="uc2" %>
<%@ Register src="CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Panel ID="pDetail" runat="server" style="font-family: Tahoma; font-size: 12px">
    <div>
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <asp:RadioButtonList ID="rblPOType" runat="server" RepeatDirection="Horizontal" onselectedindexchanged="rblPOType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="สั่งซื้อ" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="สั่งจ้าง" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                </td>
                <td>
                    <fieldset>
                        <asp:RadioButtonList ID="rblTypeAsset" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Stock" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Asset" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                </td>
                <td align="right">
                    เลขที่ใบสั่งซื้อ
                </td>
                <td>
                    <asp:TextBox ID="tbPOCode" BackColor="WhiteSmoke" Width="100" ReadOnly="true" runat="server"></asp:TextBox>
                </td>
                <td align="right">
                    วันเวลาที่สั่งซื้อ
                </td>
                <td>
                    <uc3:CalendarControl ID="ccPODate" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <fieldset>
            <table>
                <tr>
                    <td align="right">หน่วยงานที่ออก PO</td>
                    <td>
                        <asp:TextBox ID="tbDivPO" runat="server" ReadOnly="true" BackColor="WhiteSmoke" Width="300"></asp:TextBox>
                    </td>
                    <td>ชื่อโครงการ</td>
                    <td>
                        <asp:DropDownList ID="ddlProject" runat="server" Enabled="false"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">Objective</td>
                    <td>
                        <asp:TextBox ID="tbObjective" runat="server" Width="300" Enabled="false"></asp:TextBox>
                    </td>
                    <td align="right">
                        Ref PR.
                    </td>
                    <td>
                        <asp:TextBox runat="server" ReadOnly="true" BackColor="WhiteSmoke" Width="265" ID="tbRefPR"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div>
        <table>
            <tr>
                <td valign="top">
                    <fieldset style="padding: 5px">
                        <legend>เลือกรายการ</legend>
                        <table>
                        <tr>
                        <td colspan="2">
                            <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblItem">
                                <asp:ListItem Text="Reorder Point" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ระบุรายการสินค้า" Selected="True" Value="2"></asp:ListItem>
                                <asp:ListItem Text="เลือกจาก PR" Value="3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        </tr>
                        <tr>
                        <td align="right">
                            <asp:Label ID="lReorderPoint" runat="server" Text="วันที่ ReorderPoint" />
                        </td>
                        <td align="left">
                            <uc3:CalendarControl ID="reorderPoint_Date" runat="server"/>
                            <asp:HiddenField ID="hfreorderPoint_Date" runat="server" ClientIDMode="Static" />
                        </td>
                        </tr>
                        <tr>
                        <td colspan="2">
                            <center>
                                <asp:Button ID="bProductItem" Text="รายการสินค้า" SkinID="ButtonMiddleLong" runat="server" />
                            </center>
                        </td>
                        </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="width:30px"></td>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <fieldset style="padding: 5px">
                                <legend>ข้อมูลแบบพิมพ์</legend>
                                <asp:CheckBox ID="cbPrintForm" runat="server" Text="มีข้อมูลการขอแบบพิมพ์" />
                                <asp:Button ID="bPrintForm" runat="server" SkinID="ButtonMiddleLongLong" Text="แสดงข้อมูลการขอแบบพิมพ์" />
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table>
            <tr>
                <td valign="top">
                    <fieldset style="padding: 5px">
                        <legend>เอกสารแนบ</legend>
                        <iframe id="Iframe1" src="UploadFile.aspx" runat="server" height="215" width="320" frameborder="0"></iframe>
                    </fieldset>
                </td>
                <td style="width: 0px"></td>
                <td valign="top">
                    <div>
                        <br />
                        <fieldset style="padding: 0px">
                            <table border="0" cellspacing="2">
                                <tr>
                                    <td align="right" style="width: 250px">ผู้จัดจำหน่าย/ผู้ผลิต</td>
                                    <td colspan="4">
                                        <asp:UpdatePanel runat="server" ID="upSupplier" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc2:SupplierDropdownControl ID="ucDdlSupplier" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 200px">เลขที่ใบเสนอราคา</td>
                                    <td>
                                        <asp:TextBox ID="tbQuotationCode1" runat="server" Width="120"></asp:TextBox></td>
                                    <td colspan="2" style="width: 70px" align="center">ลงวันที่</td>
                                    <td>
                                        <uc3:CalendarControl ID="ccQuotationDate1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="tbQuotationCode2" runat="server" Width="120"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <uc3:CalendarControl ID="ccQuotationDate2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">การชำระเงิน</td>
                                    <td colspan="4">
                                        <asp:CheckBox ID="cbIsPayCheque" runat="server" Text="Cheque" Checked="true" />
                                        <asp:CheckBox ID="cbIsPayCash" runat="server" Text="Cash" />&nbsp;&nbsp;Credit Term&nbsp;&nbsp;
                                        <asp:TextBox ID="tbCreditTermDay" runat="server" Text="30" Width="50px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">วันที่ส่งของ</td>
                                    <td colspan="2">
                                        <uc3:CalendarControl ID="ccShippingDate" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">เลขที่การจ่ายเงิน</td>
                                    <td colspan="4"><asp:TextBox runat="server" ID="tbPaymentNo" Width="200" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;"></asp:TextBox>
                                    &nbsp;<asp:ImageButton runat="server" ID="ibPaymentNo" ImageUrl="~/images/application-view-list.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">สถานที่ส่งของ</td>
                                    <td colspan="4">
                                        <asp:TextBox ID="tbShippingAt" runat="server" Width="300"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">ผู้ติดต่อ MTL</td>
                                    <td colspan="4">
                                        <asp:TextBox ID="tbContractName" runat="server" Width="300"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel runat="server" ID="upRefresh" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td>
                            <div>
                                <fieldset>
                                    <legend>ส่วนลด</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbTradeDiscountType" runat="server" Text="ส่วนลดการค้า" />
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblTradeDiscountType" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="ส่วนลดรวม" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="ส่วนลดแต่ละรายการ" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbTradeDiscountPercent" onkeypress="return NumberBoxKeyPress(event, 2, 46, false)" onkeyup="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)" runat="server" Width="30" style="text-align: right">
                                                </asp:TextBox>&nbsp;%&nbsp;
                                                <asp:TextBox ID="tbTradeDiscountAmount" onkeypress="return NumberBoxKeyPress(event, 2, 46, false)" onkeyup="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)" runat="server" Width="70" style="text-align: right"></asp:TextBox>&nbsp;บาท
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hfTradeDiscountPercent" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                        <td></td>
                        <td>
                            <div>
                                <fieldset>
                                    <legend>ภาษีมูลค่าเพิ่ม</legend>
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <asp:RadioButtonList ID="rblVatType" runat="server" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="Vat รวม" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Vat แต่ละรายการ" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="tbVat" runat="server" Width="30" Text="7" style="text-align: right"></asp:TextBox>&nbsp;%
                                            </td>
                                            <td valign="top">
                                                <fieldset>
                                                    <legend>ราคาต่อหน่วย</legend>
                                                    <asp:RadioButtonList ID="rblVatUnitType" runat="server" RepeatDirection="Vertical">
                                                        <asp:ListItem Text="Include Vat" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Exclude Vat" Value="0" Selected="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Button ID="btnRefreshI" runat="server" style="display: none;" ClientIDMode="Static" OnClientClick="return true;" OnClick="Refresh_Click"></asp:Button>
                <asp:Panel ID="pPurchase" runat="server">
                    <asp:Button ID="bDeletePurchaseItem" runat="server" SkinID="ButtonMiddle" Text="ลบรายการ" onclick="bDeletePurchaseItem_Click" />
                    <!-- GridView สั่งซื้อ -->
                    <asp:GridView ID="gvPurchase" runat="server" AutoGenerateColumns="false" AllowSorting="false" Width="100%" 
                        onrowdatabound="gvPurchase_RowDataBound" SkinID="GvLong">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="15" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDelete" runat="server" />
                                    <asp:HiddenField ID="hfItemID" runat="server" />
                                    <asp:HiddenField ID="hfPackID" runat="server" />
                                    <asp:HiddenField ID="hfPopupType" runat="server" />
                                    <asp:HiddenField ID="hfPrID" runat="server" />
                                    <asp:HiddenField ID="hfPrItemID" runat="server" />
                                    <asp:HiddenField ID="hfPOItemID" runat="server" />
                                    <asp:HiddenField ID="hfSpecPurchase" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="ลำดับ" HeaderStyle-Font-Bold="false" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField HeaderText="รหัสสินค้า" DataField="InvItemCode" HeaderStyle-Font-Bold="false" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="ชื่อสินค้า" DataField="InvItemName" HeaderStyle-Font-Bold="false" />
                            <asp:BoundField HeaderText="หน่วยนับ" DataField="PackName" HeaderStyle-Font-Bold="false" />
                            <asp:TemplateField HeaderText="ราคา/หน่วย(รวม Vat)" ItemStyle-Width="60" HeaderStyle-Font-Bold="false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeaderP" runat="server" ClientIDMode="Static"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="tbUnitPrice" runat="server" Width="60" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false);" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false);" onpaste="return CancelKeyPaste(this);" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนที่สั่ง" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbUnitQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false);" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false);" onpaste="return CancelKeyPaste(this);" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ส่วนลด%" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTradeDiscountPercent" runat="server" Width="40" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false);" onKeyUp="return NumberBoxKeyUp(event, 2, 46, false);" onpaste="return CancelKeyPaste(this);" style="text-align: right" MaxLength="6"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ส่วนลดบาท" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTradeDiscountAmount" runat="server" Width="60" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false);" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false);" onpaste="return CancelKeyPaste(this);" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนเงิน(ก่อน Vat)" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbTotalBeforeVat" runat="server" Width="70" BackColor="WhiteSmoke"  onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vat %" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbVatPercent" runat="server" Width="30" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vat จำนวนเงิน" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbVatAmount" runat="server" Width="60" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนเงิน" ItemStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbNetAmount" runat="server" Width="70"  BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ปิดการรับ" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkClose" runat="server" Enabled="false"/>
                                    <asp:HiddenField ID="hdRemarkClose" runat="server"/>
                                <asp:ImageButton runat="server" ID="CommentClose" ImageUrl="~/images/application-view-list.png"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนรับ">
                                <ItemTemplate>
                                     <asp:TextBox ID="tbReceive_Quantity" runat="server" Width="60" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <!-- panel 2 -->

                <asp:Panel ID="pHire" runat="server" Visible="false">
                    <fieldset>
                        <legend>รายการ</legend>
                        <asp:Button ID="bDeleteHireItem" runat="server" Text="ลบ" CausesValidation="false" OnClientClick="return confirm('คุณต้องการลบรายการที่เลือกหรือไม่?');" OnClick="bDeleteHireItem_Click" />
                        <asp:GridView ID="gvHire" runat="server" AutoGenerateColumns="false" Width="100%" 
                            onrowdatabound="gvHire_RowDataBound" SkinID="GvLong">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbDelete" runat="server" />
                                        <asp:HiddenField ID="hfItemID" runat="server" />
                                        <asp:HiddenField ID="hfPackID" runat="server" />
                                        <asp:HiddenField ID="hfPrID" runat="server" />
                                        <asp:HiddenField ID="hfPrItemID" runat="server" />
                                        <asp:HiddenField ID="hdDetail" runat="server" />
                                        <asp:HiddenField ID="hfPoItemID" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="รายการ">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbProcureName" runat="server" MaxLength="100"></asp:TextBox>
                                        <asp:Literal ID="lblDetail" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="หน่วยนับ">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPackage" runat="server" DataTextField="Package_Name" DataValueField="Pack_ID">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="จำนวน" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbQuantity" runat="server" Width="40" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ราคา/หน่วย" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbUnitPrice" runat="server" Width="60" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right" MaxLength="10"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ส่วนลด%" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbTradeDiscountPercent" name="newrow" runat="server" Width="40" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right"
                                        MaxLength="6"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ส่วนลดบาท" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbTradeDiscountAmount" runat="server" Width="60" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right" MaxLength="12"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="จำนวนเงิน (ก่อนVat)" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbTotalBeforeVat" runat="server" Width="70" BackColor="WhiteSmoke"  onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat%" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbVatPercent" runat="server" Width="40" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat จำนวนเงิน" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbVatAmount" runat="server" Width="55" BackColor="WhiteSmoke"  onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="จำนวนเงิน" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbNetAmount" runat="server" Style="text-align: right" BackColor="WhiteSmoke"  onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" Width="70"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-BackColor="White" />
                                <%--11--%>
                                    <asp:TemplateField ItemStyle-BackColor="White">
                                        <ItemTemplate>
                                            คุณลักษณะ:
                                            <asp:TextBox ID="tbSpecify" runat="server" Height="70" Width="99%" TextMode="MultiLine"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <%-- account data goes here --%>
                        <asp:UpdatePanel ID="upAccount" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset runat="server" id="accountData">
                                    <legend>ข้อมูลบัญชี</legend>
                                    <asp:Button ID="bAddForm2" runat="server" Text="เพิ่มบัญชี" CausesValidation="false" onclick="bAddForm2_Click" />
                                    <asp:Button ID="bDeleteForm2" runat="server" Text="ลบ" CausesValidation="false" OnClientClick="return confirm('คุณต้องการลบรายการที่เลือกหรือไม่?');" OnClick="bDeleteForm2_Click" />
                                    <asp:GridView ID="gvForm2" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvForm2_RowDataBound" ShowFooter="true"
                                        SkinID="GvLong">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbDelete" runat="server" />
                                                    <asp:HiddenField ID="hfPoForm2ID" runat="server" />
                                                    <asp:HiddenField ID="hfPrForm2ID" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="กลุ่มค่าใช้จ่าย">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlExpense" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="บัญชี">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlAccExpense" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right; color: #555">
                                                        อัตราส่วนรวม
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="อัตราส่วน">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbPercentAllocate" runat="server" Width="40" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" Style="text-align: right" MaxLength="6"></asp:TextBox>
                                                </ItemTemplate>
                                                 <FooterTemplate>
                                                    <asp:TextBox ID="tbPercentAllocateSum" Width="40" runat="server" 
                                                        onKeyPress="return false;" 
                                                        onKeyUp="return false;" 
                                                        onpaste="return CancelKeyPaste(this)"
                                                        Enabled="false"
                                                    style="text-align: right"></asp:TextBox>&nbsp;<span style="color: #555">%</span>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="รวมเป็นเงิน">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbAmountAllocate" runat="server" Width="80" BackColor="WhiteSmoke" onkeypress="return false;" onkeydown="return false;" Style="text-align: right">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 100%">
        <table style="width: 100%" border="0">
            <tr>
                <td style="width: 580px">
                    <table cellpadding="5px">
                        <tr>
                            <td align="right">วันที่สร้าง</td>
                            <td>
                                <asp:Label ID="lCreateDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">ผู้ที่สร้าง</td>
                            <td>
                                <asp:Label ID="lCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">วันที่แก้ไขล่าสุด</td>
                            <td>
                                <asp:Label ID="lUpdateDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">ผู้ที่แก้ไขล่าสุด</td>
                            <td>
                                <asp:Label ID="lUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>
                        <tr>
                            <td align="right">ราคารวม</td>
                            <td>
                                <asp:TextBox ID="tbTotal" runat="server" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">ส่วนลด</td>
                            <td>
                                <asp:TextBox ID="tbTotalDiscount" runat="server" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">ราคารวมก่อนภาษี</td>
                            <td>
                                <asp:TextBox ID="tbTotalBeforeVat" runat="server" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">ภาษีมูลค่าเพิ่ม</td>
                            <td>
                                <asp:TextBox ID="tbTotalVat" runat="server" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">จำนวนเงินรวมทั้งสิ้น</td>
                            <td>
                                <asp:TextBox ID="tbGrandTotal" runat="server" BackColor="WhiteSmoke" onpaste="return CancelKeyPaste(this)" onkeypress="return false;" onkeydown="return false;" style="text-align: right" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pCancelPurchase" runat="server" Width="250px" BackColor="White" ForeColor="DarkBlue"
        BorderWidth="2" BorderStyle="solid" BorderColor="DarkBlue" Style="z-index: 1;"
        Visible="false">
        <div style="width: 100%; height: 100%; vertical-align: middle; text-align: center;
            font-weight: bold; font-size: 13pt;">
            <br />
            ไม่ดำเนินการสั่งซื้อ
            <br />
            <br />
        </div>
    </asp:Panel>
    <asp:Panel ID="pDeletePO" runat="server" Width="250px" BackColor="White" ForeColor="DarkBlue"
        BorderWidth="2" BorderStyle="solid" BorderColor="DarkBlue" Style="z-index: 1;"
        Visible="false">
        <div style="width: 100%; height: 100%; vertical-align: middle; text-align: center;
            font-weight: bold; font-size: 13pt;">
            <br />
            ยกเลิก
            <br />
            <br />
        </div>
    </asp:Panel>

    <ajaxToolkit:AlwaysVisibleControlExtender ID="avce" runat="server" TargetControlID="pCancelPurchase"
        VerticalSide="Middle" VerticalOffset="10" HorizontalSide="Center" HorizontalOffset="10"
        ScrollEffectDuration=".1" />
    <ajaxToolkit:AlwaysVisibleControlExtender ID="avce2" runat="server" TargetControlID="pDeletePO"
        VerticalSide="Middle" VerticalOffset="10" HorizontalSide="Center" HorizontalOffset="10"
        ScrollEffectDuration=".1" />
    </table>
</asp:Panel>
<center>
<asp:Panel ID="pButtons" runat="server">
    <table>
        <tr>
            <td>
                <asp:Button ID="bCancelPurchase" runat="server" Text="ไม่ดำเนินการสั่งซื้อ"
                    style="display:none" SkinID="ButtonMiddleLong" onclick="bCancelPurchase_Click"
                    OnClientClick="return confirm('ต้องการไม่ดำเนินการสั่งซื้อหรือไม่');" ClientIDMode="Static"  />
            </td>
            <td>
                <asp:Button ID="bSave" runat="server" Text="บันทึก" OnClientClick="return bSave_Click();" onclick="bSave_Click" />
            </td>
            <td>
                <asp:Button ID="bDeletePO" runat="server" Text="ลบข้อมูล" OnClientClick="return confirm('ต้องการลบรายการนี้หรือไม่?');" Visible="false" onclick="bDeletePO_Click" />
            </td>
            <td>
                <asp:Button ID="bPrintPO" runat="server" Text="Print PO" Visible="false" onclick="bPrintPO_Click" />
            </td>
            <td>
                <asp:Button ID="bCancel" runat="server" Text="ยกเลิก" onclick="bCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
</center>

<script type="text/javascript">
    $(document).ready(function () {

        // กำหนดหน้าต่างป๊อปอัพเมื่อคลิกปุ่ม "รายการสินค้า"
        $('#<%= bProductItem.ClientID %>').click(function () {
            var rblItemSelected = $('#<%= rblItem.ClientID %> input:checked').val();
            var rblPoTypeSelected = $('#<%= rblPOType.ClientID %> input:checked').val();

            //สั่งซื้อ
            if (rblPoTypeSelected == "1") {
                if (rblItemSelected == "1") {
                    open_popup('pop_ProductReOrderPointSelect2.aspx?main=po&type=purchase', 850, 400, 'popReorderPoint', 'yes', 'yes', 'yes');
                } else if (rblItemSelected == "2") {
                    open_popup('pop_ProductSelect2.aspx?main=po&type=purchase', 850, 400, 'popProduct', 'yes', 'yes', 'yes');
                } else if (rblItemSelected == "3") {
                    open_popup('pop_PRSelect2.aspx?main=po&prtype=1', 850, 400, 'popPR', 'yes', 'yes', 'yes');
                }
            }
            //สั่งจ้าง
            else if (rblPoTypeSelected == "2") {
                if (rblItemSelected == "1") {
                    open_popup('pop_ProductReOrderPointSelect2.aspx?main=po&type=hire', 850, 400, 'popReorderPoint', 'yes', 'yes', 'yes');
                } else if (rblItemSelected == "2") {
                    open_popup('pop_ProductSelect2.aspx?main=po&type=hire', 850, 400, 'popProduct', 'yes', 'yes', 'yes');
                } else if (rblItemSelected == "3") {
                    open_popup('pop_PRSelect2.aspx?main=po&prtype=2', 850, 400, 'popPR', 'yes', 'yes', 'yes');
                }
            }

            return false;
        });

        $('#<%= btnRefreshI.ClientID %>').css('display', 'none');
    });

    function bSave_Click() {

        var $gvPurchase = document.getElementById('<%= gvPurchase.ClientID %>');
        var $gvHire = document.getElementById('<%= gvHire.ClientID %>');
        var $bSave = document.getElementById('<%= bSave.ClientID %>');
        var $bDeletePO = document.getElementById('<%= bDeletePO.ClientID %>');

        var $supplier = document.getElementById('ContentPlaceHolder1_POControl1_ucDdlSupplier_ddlSupplier');

        if ($supplier.options.selectedIndex == 0) {
            alert('กรุณาเลือกผู้จัดจำหน่าย/ผู้ผลิต');
            return false;
        }

        if ($gvPurchase != null && $gvPurchase.rows.length > 1) {
            
        } else if ($gvHire != null && $gvHire.rows.length > 1) {
            var $gvForm2 = document.getElementById('<%= gvForm2.ClientID %>');
            if ($gvForm2 != null && $gvForm2.rows.length > 1) {

                for (i = 1; i < $gvForm2.rows.length - 1; ++i) {
                    if ($gvForm2.rows[i].cells[1].getElementsByTagName('select')[0].options.selectedIndex == 0) {
                        alert('กรุณาเลือกกลุ่มค่าใช้จ่าย');
                        return false;
                    }

                    if ($gvForm2.rows[i].cells[2].getElementsByTagName('select')[0].options.selectedIndex == 0) {
                        alert('กรุณาเลือกบัญชี');
                        return false;
                    }

                    var percent = $gvForm2.rows[i].cells[3].getElementsByTagName('input')[0].value;
                    var percent = percent.trim();
                    if (percent == '0') {
                        alert('จำนวนเปอร์เซ็นต์ต้องไม่เท่ากับ 0');
                        return false;
                    }

                    if (percent == '') {
                        alert('กรุณาระบุจำนวนเปอร์เซ็นต์');
                        return false;
                    }
                }

                var percentSum = $gvForm2.rows[$gvForm2.rows.length - 1].cells[3].getElementsByTagName('input')[0].value;
                if (percentSum != 100) {
                    alert('อัตราส่วนรวมต้องเท่ากับ 100%');
                    return false;
                }
            }

            for (i = 1; i < $gvHire.rows.length - 1; ++i) {
                if (typeof ($gvHire.rows[i].cells[2]) != "undefined") {
                    if ($gvHire.rows[i].cells[2].getElementsByTagName('select')[0].options.selectedIndex == 0) {
                        alert("กรุณาเลือกหน่วยนับ");
                        return false;
                    }
                }
            }
        } else {
            alert('กรุณาเพิ่มรายการสินค้าก่อนบันทึก');
            return false;
        }

        return true;
    }
</script>