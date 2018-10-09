<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierDropdownControl.ascx.cs" Inherits="GPlus.UserControls.SupplierDropdownControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<table>
    <tr>
        <td>
            <asp:DropDownList runat="server" ID="ddlSupplier" Width="180px"></asp:DropDownList>
        </td>
        <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุผู้จำหน่าย/ผู้ผลิต"
                    ControlToValidate="ddlSupplier" ForeColor="Red">*</asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                   runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1"></ajaxToolkit:ValidatorCalloutExtender></td>
        <td><asp:ImageButton ID="btnSupplierSearch" runat="server" ImageUrl="~/images/Commands/view.png"/></td>
    </tr>
</table>