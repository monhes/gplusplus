<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="POSearch.aspx.cs"
     Inherits="GPlus.PRPO.POSearch" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/POControl.ascx" TagName="POControl" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาข้อมูลการขอซื้อ (PO)
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            เลขที่ PO
                        </td>
                        <td>
                            <asp:TextBox ID="txtPOCodeSearch" runat="server"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ประเภท
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPOType" runat="server">
                                <asp:ListItem Text="ทั้งหมด" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="จัดซื้อ" Value="1"></asp:ListItem>
                                <asp:ListItem Text="จัดจ้าง" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblStockType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Stock" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Asset" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ทั้งหมด" Value="" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่ออก PO
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccFrom" runat="server" />
                        </td>
                        <td style="width: 130px;" align="right">
                            ถึงวันที่
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccTo" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsUpload" runat="server" Text="ยังไม่ Upload PO" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvPO" runat="server" Width="100%" AutoGenerateColumns="false" OnRowCommand="gvPO_RowCommand"
                    OnRowDataBound="gvPO_RowDataBound" OnSorting="gvPO_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="พิมพ์">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="เลขที่PO" DataField="PO_Code" SortExpression="PO_Code" />
                        <asp:BoundField HeaderText="ประเภท" DataField="PO_Type" SortExpression="PO_Type" />
                        <asp:BoundField HeaderText="วันที่ออก PO" DataField="PO_Date" SortExpression="PO_Date" />
                        <asp:BoundField HeaderText="ยอดเงิน" DataField="Net_Amonut" SortExpression="Net_Amonut"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="ผู้สร้างPO" DataField="Create_By_FullName" SortExpression="Create_By" />
                        <asp:BoundField HeaderText="ฝ่าย/ทีม" DataField="Dep_Name" SortExpression="Dep_Name" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" class="tableBody">
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
                    <uc3:POControl ID="POControl1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
