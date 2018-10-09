<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="HistoryPurchaseControl2.ascx.cs"
    Inherits="GPlus.UserControls.HistoryPurchaseControl2" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc3" %>
<%@ Register Src="ItemControl2.ascx" TagName="ItemControl2" TagPrefix="uc2" %>
<%@ Register src="ItemControlSupplier.ascx" tagname="ItemControlSupplier" tagprefix="uc4" %>

<style type="text/css">
    .style3
    {
        width: 185px;
    }
</style>

<table cellpadding="0" cellspacing="0" width="805">
    <tr>
        <td class="tableHeader">
            ข้อมูลการซื้อสินค้าจาก Supplier
        </td>
    </tr>
    <tr>
        <td class="tableBody">
         <center>
                 <table width="700px" style="border:1px; border-style:solid; border-color:Gray; ">
                 <tr><td></td></tr>
                    <tr>
                        <td colspan="3" align="center">
                        <uc4:ItemControlSupplier ID="ItemControlSupplier" runat="server" />
                   </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                        รายการสินค้า
                    
                        <asp:TextBox ID="txtItem" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancelSearch_Click" />
                    </td>
                </tr>
            </table>
            </center>
            <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล"  CausesValidation="False" 
                OnClick="btnAdd_Click"  />
             
            <asp:HiddenField ID="hid_click_from" runat="server" /> 
                          
            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowSorting="True" OnRowCommand="gvHistory_RowCommand" OnRowDataBound="gvHistory_RowDataBound"
                OnSorting="gvHistory_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="รายละเอียด">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                CausesValidation="false"></asp:LinkButton>
                            <asp:HiddenField ID="hdIID" runat="server" />
                            <asp:HiddenField ID="hdItemID" runat="server" />
                            <asp:HiddenField ID="hdPackID" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="รายการสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                        ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="หน่วย" DataField="Pack_Name" SortExpression="Pack_Name" />
                    <asp:BoundField HeaderText="ราคาต่อหน่วยที่ซื้อคร้ังสุดท้าย" DataField="Purchase_Price_Unit"
                        SortExpression="Purchase_Price_Unit" ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="วันที่ซื้อครั้งสุดท้าย" DataField="Purchase_Date" SortExpression="Purchase_Date"
                        ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ราคาต่อหน่วยล่าสุด" DataField="Propose_Price_Unit" SortExpression="Propose_Price_Unit" />
                    <asp:BoundField HeaderText="ส่วนลด%" DataField="LPur_TradeDiscount_Percent" SortExpression="LPur_TradeDiscount_Percent" />
                    <asp:BoundField HeaderText="ส่วนลด" DataField="LPur_TradeDiscount_Amount" SortExpression="LPur_TradeDiscount_Amount" />
                    <asp:BoundField HeaderText="ของแถม" />
                    <asp:BoundField HeaderText="หน่วย" DataField="Fee_Pack_Name" SortExpression="Fee_Pack_Name" />
                    <asp:BoundField HeaderText="ID"  DataField="Inv_ItemID" Visible="False" />
                </Columns>
            </asp:GridView>
            <uc1:PagingControl ID="PagingControl1" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="tableFooter">
        </td>
    </tr>
</table>
<br />
<asp:Panel ID="pnlDetail" runat="server" Visible="false">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายละเอียด
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <asp:HiddenField ID="hdID" runat="server" />
                <table width="100%">
                    <tr>
                        <td class="style98" align="right">
                            สินค้า<span style="color: Red">*</span>
                        </td>
                        <td colspan="3">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                              <td>
                            <asp:TextBox runat="server" ID="tbItemCode" Width="85" BackColor="WhiteSmoke" 
                onkeypress="return false;"></asp:TextBox>
                             </td>
                             <td>
                            <asp:TextBox runat="server" ID="tbItemName" Width="200" 
                BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
                            </td>
                            <td>
                            <asp:TextBox runat="server" ID="tbUnit"  BackColor="WhiteSmoke" 
                 onkeypress="return false;"></asp:TextBox>
                            </td>
                            <td>
                            <asp:ImageButton runat="server" ID="btnSelect" ImageUrl="~/images/Commands/view.png"/>
                            </td>
                            <td>
                            <asp:HiddenField ID="hfItemID" runat="server" />
                            <asp:HiddenField ID="hfPackID" runat="server" /></td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style98" align="right" >
                            ราคาซื้อต่อหน่วยล่าสุด</td>
                        <td>
                            <asp:TextBox ID="txtUnitPrice" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)"
                                Style="text-align: right" MaxLength="12"></asp:TextBox>
                        </td>
                        <td class="style98" align="right" >
                            วันที่ซื้อล่าสุด
                        </td>
                        <td>
                            <uc3:CalendarControl ID="ccPurchaseDate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%" style="border:1px; border-style:solid; border-color:Gray; ">
                             
                                <tr>
                                    <td align="right"class="style94">ส่วนลดการค้า</td>
                                    <td class="style97" >
                                        <asp:TextBox ID="txtLPurTradeP" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="12" Width="62px"></asp:TextBox>%
                                         <asp:TextBox ID="txtLPurTradeA" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="12"  Width="73px"></asp:TextBox>บาท
                                    </td>
                                    
                                    <td class="style98">ของแถม
                                        <asp:TextBox ID="txtFee" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                        onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)"  onpaste="return CancelKeyPaste(this)"
                                        Style="text-align:right" Width="72px"  MaxLength="12"></asp:TextBox>
                                        
                                    </td>
                                     <td class="style3">
                                   
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                             หน่วย
                                                <asp:DropDownList ID="ddlLPurPack" runat="server" width="135px"  DataTextField="Description" 
                                                DataValueField="Pack_Id" AutoPostBack="True" >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                      </td>  
                                     <td align="center" >
                            <table style=" width:100px;">
                             <tr> 
                             
                         <td >
                             <asp:RadioButtonList ID="rdbVatUnitType" runat="server" Width="140px" 
                                 RepeatLayout="Flow">
                              <asp:ListItem Text="Include Vat"></asp:ListItem>
                              <asp:ListItem Text="Exclude Vat" Selected="True"></asp:ListItem>
                                 
                             </asp:RadioButtonList>
                            
                        </td>
                    </tr>
                    </table>
                            </td>
                                    </tr>
                            </table>
                            </td>
                             </tr>
                              <tr>
                              <td align="right" class="style39">
                        </td>
                        <td align="right" class="style88">
                          </td>
                        <td colspan="2">
                            <table style="width:350px;">
                             <tr>
                             
                         <td align="right" class="style87">
                          
                            </td >
                             <td style="width:110px;" align="right">
                            
                        </td>
                        
                    </tr>
                    </table>
                            </td>
                             </tr>
                     <tr>
                      <td align="right" class="style39">
         
                            ราคาที่ Supplier เสนอต่อหน่วย
                        </td>
                        <td class="style88">
                            <asp:TextBox ID="txtProposePriceUnit" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>
                        </td>
                        <td align="right" class="style44">
                            วันที่เสนอล่าสุด</td>
                        <td class="style38">
                            <uc3:CalendarControl ID="ccProposeDate" runat="server" />
                        </td>
                    </tr>
                   
                    <tr>
                        <td align="right" class="style39">
                            สถานะการใช้งาน
                        </td>
                        <td class="style88">
                            <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Inactive"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right" class="style44">
                        </td>
                        <td class="style38">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style39">
                            วันที่สร้าง
                        </td>
                        <td class="style88">
                            <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                        </td>
                        <td align="right" class="style44">
                            ผู้ที่สร้าง
                        </td>
                        <td class="style38">
                            <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style39">
                            วันที่แก้ไขล่าสุด
                        </td>
                        <td class="style88">
                            <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                        </td>
                        <td align="right" class="style44">
                            ผู้ที่แก้ไขล่าสุด
                        </td>
                        <td class="style38">
                            <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False"
                        OnClick="btnCancel_Click" />
                </center>
            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnRefreshSelect" 
                                    runat="server"
                                    style="display:none;" 
                                    ClientIDMode="Static" 
                                    OnClick="btnRefreshSelect_Click"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script type="text/javascript">
                        document.getElementById('btnRefreshSelect').style.display = 'none';
                   </script>
</asp:Panel>

