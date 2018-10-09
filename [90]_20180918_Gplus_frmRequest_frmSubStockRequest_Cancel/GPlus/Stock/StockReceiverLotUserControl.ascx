<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="StockReceiverLotUserControl.ascx.cs"
    Inherits="GPlus.Stock.StockReceiverLotUserControl" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<asp:Panel ID="pnlLot" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 145px" align="right">
                Lot No
            </td>
            <td style="width: 80px" align="center">
                <asp:TextBox ID="txtLotNo" AutoPostBack="false" runat="server" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);"
                    Width="60" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุ Lot No"
                    ControlToValidate="txtLotNo" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                    TargetControlID="RequiredFieldValidator1">
                </asp:ValidatorCalloutExtender>--%>
            </td>
            <td style="width: 80px" align="right">
                จำนวนที่รับ
            </td>
            <td style="width: 100px" align="center">
                <asp:TextBox ID="txtReceiveNumber" AutoPostBack="false" Width="50" runat="server"
                    Style="text-align: right;" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="rfvtxtLotNo1" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                    ControlToValidate="txtReceiveNumber" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="exrfvtxtLotNo1" runat="server" Enabled="True" TargetControlID="rfvtxtLotNo1"></asp:ValidatorCalloutExtender>--%>
            </td>
            <td style="width: 100px" align="right">
                Expire date
            </td>
            <td style="width: 120px" align="center">
                <uc2:CalendarControl ID="txtExpireDate" onKeyPress="return (event.keyCode!=13);"
                               AutoPostBack="false" Width="100" runat="server" Enabled="False"></uc2:CalendarControl>
            </td>
            <td align="right">
                <asp:Button ID="btnAddRow" runat="server" Text="เพิ่มที่เก็บ" 
                    OnClick="BtnAddRowClick" CausesValidation="false" />
                <asp:Button ID="btnDeleteRow" runat="server" CausesValidation="false" 
                    OnClick="BtnDeleteRowClick" Text="ลบที่เก็บ" />
                <asp:HiddenField ID="hdIsAdd" runat="server" Value="0" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table>
                    <tr>
                        <td style="width: 80px">
                            <asp:HiddenField ID="hdLotId" runat="server" />
                        </td>
                        <td style="width: 140px" align="right">
                            รหัส Barcode จาก Supplier
                        </td>
                        <td style="width: 100px" align="right">
                            <asp:TextBox ID="txtSupBarcode" Width="80" AutoPostBack="false" runat="server" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td style="width: 130px" align="center">
                            จำนวนที่พิมพ์ Barcode MTL
                        </td>
                        <td style="width: 100px" align="left">
                            <asp:TextBox ID="txtPrintCount" AutoPostBack="false" runat="server" Width="80" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                        </td>
                        <td align="left">
                            ใบ
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="7" style="width: 100%">
                <center>
                    <asp:GridView ID="gvStk" runat="server" AutoGenerateColumns="False" Width="80%" OnRowCommand="GvStkRowCommand"
                        OnRowDataBound="GvStkRowDataBound">
                        <Columns>

                            <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" HeaderStyle-Width="10%"
                                ItemStyle-HorizontalAlign="Center" >
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Qty_Location" HeaderText="จำนวนที่เก็บ" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" 
                                HeaderText="จำนวนที่เก็บ">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdRowID" runat="server" />
                                    <asp:TextBox ID="txtEachUnitNumber" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);" runat="server" CausesValidation="false" 
                                        EnableViewState="true" Style="text-align: right;" Width="80%"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="68%" 
                                HeaderText="สถานที่เก็บ">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddLocationList" runat="server" 
                                        DataTextField="Location_Name" DataValueField="Location_ID" Width="90%">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="68%" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Location_Name" HeaderText="สถานที่เก็บ" />


                   
                        </Columns>
                    </asp:GridView>
                </center>
            </td>
        </tr>
    </table>
    <hr />
</asp:Panel>
