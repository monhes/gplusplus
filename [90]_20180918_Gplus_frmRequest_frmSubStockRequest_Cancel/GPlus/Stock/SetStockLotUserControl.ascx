<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SetStockLotUserControl.ascx.cs"
    Inherits="GPlus.Stock.SetStockLotUserControl" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<style type="text/css">
    .style1
    {
        width: 55px;
    }
</style>
<asp:Panel ID="pnlLot" runat="server">
<script type="text/javascript" language="javascript">
    function InvokePop() {
        alert("test");
    }
</script>

    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 145px" align="right">
                Lot No
            </td>
            <td style="width: 80px" align="center">
                <asp:TextBox ID="txtLotNo" AutoPostBack="true" runat="server" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" MaxLength="7"
                    Width="60" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="ChangeLotNo"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุ Lot No"
                    ControlToValidate="txtLotNo" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                    TargetControlID="RequiredFieldValidator1">
                </asp:ValidatorCalloutExtender>
            </td>
            <td style="width: 80px" align="right">
                จำนวนที่รับ
            </td>
            <td style="width: 100px" align="center">
                <asp:TextBox ID="txtReceiveNumber" AutoPostBack="true" Width="50" runat="server"
                    Style="text-align: right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="ChangeReceiveNumber"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="rfvtxtLotNo1" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                    ControlToValidate="txtReceiveNumber" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="exrfvtxtLotNo1" runat="server" Enabled="True" TargetControlID="rfvtxtLotNo1"></asp:ValidatorCalloutExtender>--%>
            </td>
            <td style="width: 100px" align="right">
                Expire date
            </td>
            <td style="width: 120px" align="center">
                <uc2:CalendarControl ID="txtExpireDate" AutoPostBack="false" Width="100" runat="server"></uc2:CalendarControl>
            </td>
            <td align="right">
                <asp:Button ID="btnAddRow" runat="server" Text="เพิ่มที่เก็บ" OnClick="BtnAddRowClick" CausesValidation="false" />
                <asp:Button ID="btnDelete" runat="server" Text="ลบ Lot" 
                    CausesValidation="false" onclick="btnDelete_Click" />
                <asp:HiddenField ID="hdIsAdd" runat="server" Value="0" />
            </td>
        </tr>
          <tr>
            <td colspan="4">
                <table>
                    <tr>
                        <td style="width: 55px">
                        </td>
                        <td style="width: 140px" align="right">
                            ราคาต่อหน่วย
                        </td>
                        <td  align="right">
                            <asp:TextBox ID="txtAvgCost" Width="135px" AutoPostBack="false" 
                                runat="server" Enabled = "false"></asp:TextBox>
                            &nbsp;&nbsp;
                            บาท
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td style="width: 130px" align="center">
                            มูลค่ารวม
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPrice" Width="135px" AutoPostBack="false" runat="server"></asp:TextBox>
                        </td>
                        <td align="left">
                            บาท
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table>
                    <tr>
                        <td class="style1">
                        </td>
                        <td style="width: 140px" align="right">
                            รหัส Barcode
                        </td>
                        <td style="width: 100px" align="right">
                            <asp:TextBox ID="txtSupBarcode" Width="190px" AutoPostBack="false" 
                                runat="server" onKeyPress="return (event.keyCode!=13);"
                                onKeyUp="return (event.keyCode!=13);" Enabled = "false"></asp:TextBox>
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
                            <asp:TextBox ID="txtPrintCount" AutoPostBack="false" runat="server" Width="80" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
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
                    <asp:GridView ID="gvStk" runat="server" AutoGenerateColumns="false" Width="80%" OnRowCommand="GvStkRowCommand"
                        OnRowDataBound="GvStkRowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="ลำดับ" DataField="rownumber" HeaderStyle-Width="10%"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="จำนวนที่เก็บ" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdRowID" runat="server" />
                                    <asp:TextBox ID="txtEachUnitNumber" Width="80%" Style="text-align: right;" runat="server"
                                        CausesValidation="false" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                        onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="สถานที่เก็บ" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="68%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddLocationList" DataTextField="Location_Name" Width="90%" DataValueField="Location_ID"
                                        runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Del" Text="ลบที่เก็บ" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </center>
            </td>
        </tr>
    </table>
    <hr />
</asp:Panel>
