<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PRControl.ascx.cs" Inherits="GPlus.UserControls.PRControl" %>
<%@ Register Src="CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<%@ Register Src="POPRAttachControl.ascx" TagName="POPRAttachControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Panel ID="pnlEdit" runat="server" style="font-family: Tahoma; font-size: 12px">
    <table>
        <tr>
            <td>
                <fieldset style="padding: 0px">
                <table style="display:inline" border="0">
                    <tr>
                        <td>
                            <fieldset style="padding:0px">
                            <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rblPRType" 
                                AutoPostBack="true" onselectedindexchanged="rblPRType_SelectedIndexChanged">
                                <asp:ListItem Text="ขอซื้อ" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="ขอจ้าง" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td align="left">เลขที่ใบขอซื้อ&nbsp;&nbsp;<asp:TextBox runat="server" ID="tbPRCode" BackColor="WhiteSmoke" Width="120" ReadOnly="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">วันเวลาที่ขอซื้อ</td><td><uc2:CalendarControl runat="server" ID="ccRequestDate" /></td>
                    </tr>
                    <tr>
                        <td align="right">หน่วยงานที่ขอซื้อ</td>
                        <td><asp:TextBox runat="server" Width="300" ID="tbDivPR" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">ชื่อโครงการ</td><td><asp:DropDownList  runat="server" ID="ddlProject" DataTextField="Project_Name" DataValueField="Project_ID" Enabled="false"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right">วัตถุประสงค์</td>
                        <td><asp:TextBox  runat="server" Width="300" ID="tbObjective" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Ref. PO</td>
                        <td><asp:TextBox  runat="server" ReadOnly="true" BackColor="WhiteSmoke" ID="tbRefPO" Width="300"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">Supplier</td>
                        <td><asp:DropDownList  runat="server" ID="ddlSupplier" DataTextField="Supplier_Name" DataValueField="Supplier_ID"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right">ใบเสนอราคา</td>
                        <td><asp:TextBox  runat="server" ID="tbQuotationCode"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">ลงวันที่</td>
                        <td><uc2:CalendarControl ID="ccQuotationDate" runat="server" /></td>
                    </tr>
                </table>
                </fieldset>
            </td>
            <td>
                <fieldset style="padding: 0px">
                    <legend>เอกสารแนบ</legend>
                    <iframe src="UploadFile.aspx" runat="server" height="210" width="320" frameborder="0"></iframe>
                    <div id="FileId"></div>
                </fieldset>
            </td>
        </tr>
    </table>
    <div>
        <table>
            <tr>
                <td style="width: 430px">
                    <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <fieldset><legend>ข้อมูลแบบพิมพ์</legend>
                            <asp:CheckBox runat="server" Text="มีข้อมูลการขอแบบพิมพ์" ID="cbPrintForm" />&nbsp;&nbsp;
                            <asp:Button runat="server" Text="บันทึก/แก้ไขการขอแบบพิมพ์" SkinID="ButtonMiddleLongLong" ID="bPrintForm" OnClientClick="pop_PrintForm();" />
                        </fieldset>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td></td>
                <td style="width: 350px">
                    <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <fieldset><legend>เลือกรายการ</legend>
                            <asp:Button runat="server" Text="Reorder Point" SkinID="ButtonMiddleLong" Enabled="false" />
                            <asp:Button runat="server" Text="ระบุรายการสินค้า" SkinID="ButtonMiddleLong" ID="bProductSelect" />
                        </fieldset>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>

            </tr>
        </table>
    </div>
        <asp:UpdatePanel runat="server" ID="upRefresh" UpdateMode="Conditional">
    <ContentTemplate>
    <table>
        <tr>
            <td>
                <div>
                    <fieldset>
                        <legend>ส่วนลด</legend>
                        <table>
                            <tr>
                                <td style="width: 100px">
                                    <asp:CheckBox ID="cbTradeDiscountType" runat="server" Text="ส่วนลดการค้า" />
                                </td>
                                <td style="width: 150px">
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
                                    &nbsp;</td>
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
                </Columns>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pHire" runat="server" Visible="false">
            <fieldset>
                <legend>รายการ</legend>
                <asp:Button ID="bAddHireItem" runat="server" Text="เพิ่มรายการ" SkinID="ButtonMiddle" onclick="bAddHireItem_Click" />
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
                                <asp:HiddenField ID="hfInvSpecPurchase" runat="server" />
                                <asp:HiddenField ID="hfPoItemID" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รายการ">
                            <ItemTemplate>
                                <asp:TextBox ID="tbProcureName" runat="server" MaxLength="100" Width="140"></asp:TextBox>
                                <asp:Literal ID="lbInvSpecPurchase" runat="server"></asp:Literal>
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
                                <asp:TextBox ID="tbUnitPrice" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" style="text-align: right" MaxLength="10"></asp:TextBox>
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
    </ContentTemplate>
    </asp:UpdatePanel>
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
    <center>
        <table>
            <tr>
                <td>
                    <asp:Button ID="bSave" runat="server" Text="บันทึก" OnClientClick="return bSave_Click();" OnClick="bSave_Click" />&nbsp;
                    <asp:Button ID="bDelete" runat="server" Text="ลบข้อมูล" OnClick="bDelete_Click" 
                        Visible="false" Width="70px" />&nbsp;
                    <asp:Button ID="bPrint" runat="server" Text="Print PR" Visible="false" />&nbsp;
                    <asp:Button ID="bClose" runat="server" Text="ยกเลิก" CausesValidation="false" OnClick="bClose_Click" />
                </td>
            </tr>
        </table>
    </center>
</asp:Panel>
<asp:Panel ID="pDeletePR" runat="server" Width="250px" BackColor="White" ForeColor="DarkBlue"
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
<ajaxToolkit:AlwaysVisibleControlExtender ID="avce2" runat="server" TargetControlID="pDeletePR"
    VerticalSide="Middle" VerticalOffset="10" HorizontalSide="Center" HorizontalOffset="10"
    ScrollEffectDuration=".1" />

<script type="text/javascript">
    document.getElementById('btnRefreshI').style.display = 'none';

    function pop_PrintForm() {
        open_popup('pop_PrintForm2.aspx', 850, 400, 'popPrintForm', 'yes', 'yes', 'yes');
    }

    function bSave_Click() {
        var $gvPurchase = document.getElementById('<%= gvPurchase.ClientID %>');
        var $gvHire = document.getElementById('<%= gvHire.ClientID %>');
        var $bSave = document.getElementById('<%= bSave.ClientID %>');
        var $bDeletePO = document.getElementById('<%= bDelete.ClientID %>');

//        var $supplier = document.getElementById('<%= ddlSupplier.ClientID %>');

//        if ($supplier.options.selectedIndex == 0) {
//            alert('กรุณาเลือก Supplier');
//            return false;
//        }

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
