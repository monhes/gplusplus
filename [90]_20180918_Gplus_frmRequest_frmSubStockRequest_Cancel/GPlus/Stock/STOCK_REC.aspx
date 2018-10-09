<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="STOCK_REC.aspx.cs" Inherits="GPlus.Stock.StockRec" MasterPageFile="../MasterPage/Main.Master"%>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../UserControls/CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
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
                        <td style="width: 130px;" align="right"/>
                        <td>
                            <asp:RadioButtonList ID="rdStockType" runat="server" repeatdirection="Horizontal"  onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);">
                                <asp:ListItem Text="รอรับเข้าคลัง" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="รับเข้าคลังแล้ว" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 130px;" align="right">
                            คลัง
                        </td>
                        <td>
                            <asp:DropDownList ID="cbStock" runat="server" Width="155" DataValueField="Stock_ID" DataTextField="Stock_Name"  onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            เลขที่รับเข้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtPRCodeSearch" runat="server" onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
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
                            <asp:TextBox ID="txtPOCodeSearch" runat="server" onKeyPress="return (event.keyCode!=13);" onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
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
<%--                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" OnClick="btnAdd_Click" />
                <asp:Button ID="btnAddProduct" runat="server" SkinID="ButtonMiddle" Text="ขอรหัสสินค้าใหม่"
                    PostBackUrl="~/PRPO/ProductCodeMgt.aspx" />--%>
                <asp:GridView ID="gvStk" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="GvStkRowCommand"
                    OnRowDataBound="GvStkRowDataBound" OnSorting="GvStkSorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" SortExpression="rownumber" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="เลขที่รับเข้า" DataField="Receive_Stk_No" SortExpression="Receive_Stk_No" />
                        <asp:BoundField HeaderText="วันที่รับเข้า" DataField="Receive_Date" SortExpression="Receive_Date" />
                        <asp:BoundField HeaderText="เลขที่PO" DataField="PO_Code" SortExpression="PO_Code" />
                        <asp:BoundField HeaderText="วันที่ส่งซื้อ" DataField="PO_Date" SortExpression="PO_Date" />
                        <asp:BoundField HeaderText="ยอดเงินในPO" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" DataField="Net_Amonut" SortExpression="Total_Price" />
                        <asp:BoundField HeaderText="ทีมงาน" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField=""  />
                        <asp:BoundField HeaderText="สถานะ" />
                        <asp:TemplateField HeaderText="พิมพ์">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" class="tableBody">
                <uc1:PagingControl ID="PagingControl1" runat="server" Visible="false" />
            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>
 </asp:Content>
