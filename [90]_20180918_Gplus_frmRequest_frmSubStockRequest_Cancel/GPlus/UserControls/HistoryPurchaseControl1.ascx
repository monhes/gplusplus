<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="HistoryPurchaseControl1.ascx.cs"
    Inherits="GPlus.UserControls.HistoryPurchaseControl1" %>
<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register src="ItemControl.ascx" tagname="ItemControl" tagprefix="uc2" %>
<%@ Register src="ItemControlSupplier.ascx" tagname="ItemControlSupplier" tagprefix="uc4" %>
<%@ Register src="CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>

<style type="text/css">
    .style32
    {
        height: 31px;
    }
    .style38
    {
        width: 98px;
    }
    .style39
    {
        width: 371px;
    }
    .style44
    {
        width: 102px;
    }
    .style87
    {
        width: 80px;
    }
    .style88
    {
        width: 253px;
    }
    .style94
    {
        width: 201px;
    }
    .style95
    {
        width: 540px;
    }
    .style97
    {
        width: 271px;
    }
    .style98
    {
        width: 561px;
    }
</style>

<table cellpadding="0" cellspacing="0" width="805">
    <tr>
        <td class="tableHeader">
            บันทึกและสอบถามราคาซื้อจาก Supplier
        </td>
    </tr>
    <tr>
        <td class="tableBody">
            <table width="100%">
                <tr>
                    
                    <td colspan="5" class="style32">
                        <uc2:ItemControl ID="ItemControl1" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="3">
                        <uc4:ItemControlSupplier ID="ItemControlSupplier" runat="server" />
                    </td>
                    
                    <td>
                     Supplier ที่เคยสั่งซื้อ&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlSearchSupplierCount" runat="server" Width="195">
                            <asp:ListItem Text="จำนวนทั้งหมด" Value=""></asp:ListItem>
                            <asp:ListItem Text="3 ราย" Value="3"></asp:ListItem>
                            <asp:ListItem Text="2 ราย" Value="2"></asp:ListItem>
                            <asp:ListItem Text="1 ราย" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td style="width: 80px;" align="right">
         
                </td>
                    <td >
                    ย้อนหลัง&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlSearchBackMonth" runat="server" Width="195">
                            <asp:ListItem Text="1 เดือน" Value="1"></asp:ListItem>
                            <asp:ListItem Text="3 เดือน" Value="3"></asp:ListItem>
                            <asp:ListItem Text="6 เดือน" Value="6"></asp:ListItem>
                            <asp:ListItem Text="1 ปี" Value="12"></asp:ListItem>
                            <asp:ListItem Text="2 ปี" Value="24"></asp:ListItem>
                            <asp:ListItem Text="3 ปี" Value="36"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" onclick="btnSearch_Click1" 
                            />&nbsp;&nbsp;
                        <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                            CausesValidation="False" onclick="btnCancelSearch_Click" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                CausesValidation="False" onclick="btnAdd_Click"/>
            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" 
                Width="100%" AllowSorting="true" onrowcommand="gvHistory_RowCommand" 
                onrowdatabound="gvHistory_RowDataBound" onsorting="gvHistory_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="รายละเอียด">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                CausesValidation="false"></asp:LinkButton>
                            <asp:HiddenField ID="hdIID" runat="server" />
                            <asp:HiddenField ID="hdItemID" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ชื่อ Supplier" DataField="Supplier_Name" SortExpression="Supplier_Name"
                        ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="วันที่ซื้อล่าสุด" DataField="Purchase_Date" SortExpression="Purchase_Date" 
                        ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField HeaderText="หน่วยที่ซื้อ" DataField="Pack_Name" SortExpression="Pack_Name"/>
                    <asp:BoundField HeaderText="ราคาซื้อต่อหน่วย" DataField="Purchase_Price_Unit" SortExpression="Purchase_Price_Unit" 
                        ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="ราคาที่ Supplier เสนอต่อหน่วย" DataField="Propose_Price_Unit" SortExpression="Propose_Price_Unit" 
                        ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="หน่วยที่เสนอ" DataField="Pack_Name" SortExpression="Pack_Name"  />
                    <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status"  />
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
  <a name="pnlDetail"></a>
<asp:Panel ID="pnlDetail" runat="server" Visible="false" Width="805px">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายละเอียด
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <asp:HiddenField ID="hdID" runat="server" />
                <table style="width: 100%">
                    <tr>
                        <td colspan="2">
                            
                            <uc4:ItemControlSupplier ID="ItemControlSupplier1" runat="server" />
                      
                        </td>
                        <td align="right"class="style44" >
                            วันที่ซื้อล่าสุด
                        </td>
                        <td class="style38">
                            <uc3:CalendarControl ID="ccPurchaseDate" runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="4">
                            <table width="100%" style="border:1px; border-style:solid; border-color:Gray; ">
                             <tr>

                        <td align="right" class="style94">
                            หน่วยที่ซื้อล่าสุด
                        </td>
                        <td class="style97">
                            <asp:DropDownList ID="ddlUnit" runat="server" Width="155" 
                                style="margin-right: 1px"></asp:DropDownList>
                        </td>
                        <td class="style98" >
                            ราคาซื้อต่อหน่วยล่าสุด
                       
                            <asp:TextBox ID="txtUnitPrice" runat="server" onKeyPress="return NumberBoxKeyPress(event, 2, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 2, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right; margin-left: 0px;" MaxLength="12" 
                                Width="118px"></asp:TextBox>บาท
                        </td>
                    </tr>
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
                                        หน่วย
                                        <asp:DropDownList ID="ddlLPurPack" runat="server" width="135px" ></asp:DropDownList>
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
                            ลงวันที่
                        </td>
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
</asp:Panel>
