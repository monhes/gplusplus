<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="RequestStockOnHand.aspx.cs" Inherits="GPlus.Request.RequestStockOnHand" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register src="../UserControls/ItemControl4.ascx" tagname="ItemControl4" tagprefix="uc4" %><%--
<%@ Register src="../UserControls/ItemControl3.ascx" tagname="ItemControl3" tagprefix="uc3" %>--%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 222px;
        }
        .style3
        {
            width: 130px;
        }
        #Select1
        {
            width: 189px;
        }
        .style5
        {
            height: 33px;
        }
        .style6
        {
            width: 102px;
        }
        .style7
        {
            width: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                สอบถามจำนวนคงคลัง</td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <%--<tr>
                        <td align="right" class="style3">
                            รายการวัสดุ/อุปกรณ์
                        </td>
                        <td align="left" class="style6">
                            <asp:TextBox ID="txtMaterialTypeCodeSearch" runat="server" Width="199px" 
                                MaxLength="50"  AutoPostBack="true" 
                                OnTextChanged="txtMaterialTypeCodeSearch_TextChanged"></asp:TextBox>
                        </td>
                        <td align="right" class="style7">
                          
                        </td>
                        <td align="left">
                            
                        </td>
                    </tr>--%>
                    <tr>
                    <td style="width: 130px;" align="right">
                        รายการวัสดุอุปกรณ์
                    </td>
                    <td colspan="3">
                        <uc4:itemcontrol4 ID="ItemControl4" runat="server"/>
                    </td>
                    <%--<td align="right" class="style7">
                            หน่วย
                    </td>
                    <td align="left" class="style1">
                        <asp:DropDownList ID="ddlMaterialUnit" runat="server" Width="200" DataTextField="MaterialUnit_Name" DataValueField="MaterialUnit_ID">
                        </asp:DropDownList>
                    </td>--%>
                    </tr>

                    <%--<tr>
                    <td style="width: 130px;" align="right">
                        รายการวัสดุอุปกรณ์
                    </td>
                    <td colspan="3">
                        <uc3:itemcontrol3 ID="ItemControl3" runat="server"/>
                    </td>--%>
                    <%--<td align="right" class="style7">
                            หน่วย
                    </td>
                    <td align="left" class="style1">
                        <asp:DropDownList ID="ddlMaterialUnit" runat="server" Width="200" DataTextField="MaterialUnit_Name" DataValueField="MaterialUnit_ID">
                        </asp:DropDownList>
                    </td>--%>
                    </tr>

                    <%--<tr>
                        <td align="right" class="style3">
                            ชื่อ
                        </td>
                        <td align="left" class="style6">
                             <asp:TextBox ID="txtMaterialNameSearch" runat="server" Width="200" MaxLength="100"></asp:TextBox>
                        </td>
                         <td align="right" class="style7">
                            หน่วย
                        </td>
                        <td align="left" class="style1">
                             <asp:DropDownList ID="ddlMaterialUnit" runat="server" Width="200" DataTextField="MaterialUnit_Name" DataValueField="MaterialUnit_ID">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right" class="style3">
                            คลัง
                        </td>
                        <td align="left" class="style6">
                            <asp:ListBox ID="LBStock" runat="server" Height="90px" Width="292px" 
                                DataTextField="LBStock_Name" DataValueField="LBStock_ID" Rows="5" 
                                SelectionMode="Multiple">
                            </asp:ListBox>
                         </td>
                         <td align="right" class="style7">
                           
                        </td>
                        <td align="left" class="style1">
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center" class="style5">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" 
                    Width="100%" AllowSorting="True"
                    onrowdatabound="gvStock_RowDataBound"
                    onsorting="gvStock_Sorting" >
                    <Columns>
                        <asp:BoundField HeaderText="คลัง" DataField="Stock_Name" SortExpression="Stock_Name"
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ประเภทคลัง" DataField="StockType" SortExpression="StockType"
                            ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="หน่วย" DataField="Pack_Description" SortExpression="Pack_Description"
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="จำนวน" DataField="OnHand_Qty" SortExpression="OnHand_Qty"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:###,###}" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
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
</asp:Content>
