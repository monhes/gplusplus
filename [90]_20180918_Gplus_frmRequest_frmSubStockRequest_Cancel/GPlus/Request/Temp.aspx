<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="Temp.aspx.cs" Inherits="GPlus.images.Temp" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 275px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlDetail" runat="server" Visible="true">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    Temp
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                          <td align="right" class="style1">
                            Pay_ID : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPay_ID" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Pay_ID2 : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPay_ID2" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Pay_ID3 : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPay_ID3" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Pay_ID4 : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPay_ID4" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Pay_ID5 : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPay_ID5" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Inv_ItemID : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtInv_ItemID" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Pack_ID : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtPack_ID" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td align="right" class="style1">
                            Unit_Price : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtUnit_Price" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                          </td>
                        </tr>
                         <tr>
                          <td align="right" class="style1">
                            Confirm Text : 
                          </td>
                          <td align="left">
                            <asp:TextBox ID="txtConfirm" runat="server" Width="190" MaxLength="100"></asp:TextBox>
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
</asp:Content>
