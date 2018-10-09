<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetStockItemUserControl.ascx.cs" Inherits="GPlus.Stock.SetStockItemUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<style type="text/css">
    .style2
    {
        width: 106px;
    }
    .style3
    {
        width: 81px;
    }
</style>
<asp:Panel ID="pnlLot" runat="server">
    <script type="text/javascript" language="javascript">
        function InvokePop() {
            // to handle in IE 7.0          
            if (window.showModalDialog) {
                retVal = window.showModalDialog("SetStockItemSearchPopup.aspx?ContentId=" + '<%= this.ClientID %>', '', "unadorned:yes;dialogHeight:400px;dialogWidth:600px;resizable:no;scrollbars:yes");
                __doPostBack('__Page', '');
            }
        }
        function __doPostBack(eventTarget, eventArgument) {
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        }

    </script>
    <table cellpadding="0" cellspacing="0">
        <tr>
             <td class="style3" colspan = "10" style=" padding-bottom:10px">
                รหัส Barcode จาก Supplier &nbsp;&nbsp;&nbsp;&nbsp;
            <%--</td>
            <td colspan = "9">  --%>
                <asp:TextBox ID="txtBarcode" runat="server" Width="254px" 
                    style="margin-left: 0px" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style3">
                รหัสสินค้า&nbsp;&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtItemCode" Enabled="false" AutoPostBack="false" 
                    runat="server" MaxLength="15" Width="114px"></asp:TextBox>
            </td>
            <td>
                &nbsp;&nbsp;รายการ&nbsp;&nbsp;
            </td>
            <td>
                 <asp:HiddenField ID="txtItemId" runat="server" />
                 <asp:TextBox ID="txtItemName" Enabled="false" AutoPostBack="false" 
                     runat="server" Width="180px"></asp:TextBox>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtItemName" runat="server" ErrorMessage="กรุณาเลือกรายการ" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
            </td>
            <%--<td>
                <asp:Button ID="btnFindItem" runat="server" Text="ค้นหา" Width="30" OnClientClick="javascript:InvokePop();" />
                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1"></asp:ValidatorCalloutExtender> 
            </td>--%>
            <td>
            <asp:ImageButton ID="btnSelect1" runat="server" ImageUrl="~/images/Commands/view.png" OnClientClick="javascript:InvokePop();"/>
            </td>
             <td>
                &nbsp;&nbsp;หน่วย
            </td>
            <td style="width:130px" align="center">
                <asp:DropDownList ID="ddPack" AutoPostBack="true" runat="server" Width="100" DataTextField="Package_Name" DataValueField="Pack_ID" OnSelectedIndexChanged="ChangeUnit"></asp:DropDownList>
            </td>
            <td>
                จำนวนรวม&nbsp;&nbsp;
            </td>
            <td>
                <asp:TextBox ID="txtItemCount" CausesValidation="false" Width="50" runat="server" style="text-align:right;"
                    onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)">1</asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtItemCount" runat="server" ErrorMessage="กรุณาระบุจำนวนที่มากกว่า 0"
                    ControlToValidate="txtItemCount" ForeColor="Red" >*</asp:RequiredFieldValidator>
                <asp:CompareValidator id="CompareFieldValidator1" runat="server"
                                                    ForeColor="Red"
                                                    ControlToValidate="txtItemCount"
                                                    ValueToCompare="1"
                                                    Type="Integer"
                                                    Operator="GreaterThanEqual"
                                                    ErrorMessage="กรุณาระบุจำนวนที่มากกว่า 0">
                                                    *
                </asp:CompareValidator>
                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" TargetControlID="CompareFieldValidator1"></asp:ValidatorCalloutExtender> 
                <asp:ValidatorCalloutExtender ID="exrtxtItemCount" runat="server" Enabled="True" TargetControlID="rfvtxtItemCount"></asp:ValidatorCalloutExtender>                                       
            </td>
            
        </tr>
    </table>
</asp:Panel>