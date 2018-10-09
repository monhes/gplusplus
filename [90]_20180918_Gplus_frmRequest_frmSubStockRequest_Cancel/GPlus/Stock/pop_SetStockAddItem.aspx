<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="pop_SetStockAddItem.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="GPlus.Stock.pop_SetStockAddItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Stock/SetStockItemUserControl.ascx" TagName="StockUserControl" TagPrefix="sctl1" %>
<%@ Register Src="~/Stock/SetStockLotUserControl.ascx" TagName="LotUserControl" TagPrefix="sctl2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ระบุการขอแบบพิมพ์</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <center>
       <ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader" align="left">
                    เพิ่มรายการ Set Stock</td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:Panel ID="pnlAdd" runat="server">
                            <table width="100%">
                               <%-- <tr>
                                    <td style="width: 160px;" align="left">
                                        รหัส Barcode จาก Supplier
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBarcode" runat="server" Width="250" ></asp:TextBox>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="left" colspan = "2">
                                        <sctl1:StockUserControl ID="suControl1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="right" style=" padding:10px 55px 0px 0px">
                                        <asp:Button ID="btnAddLot" runat="server" Text="Add Lot" OnClick="BtnAddLotClick" CausesValidation="false" />
                                    </td>
                                </tr>
                            </table>
                    </asp:Panel>
                    <br />
                    
                    <%--<asp:Button ID="btnOK" runat="server" Text="ตกลง" onclick="btnOK_Click" />
                    <asp:Button ID="btnCancel1" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;" />--%>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
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
                 <asp:Panel ID="panelButton" runat="server" >
                <table cellpadding="0" cellspacing="0"  width="95%">
                    <tr>
                        <td class="tableBody" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="BtnSaveClick" />
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClientClick="window.close();" CausesValidation="false" />
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
</body>
</html>
