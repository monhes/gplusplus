<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="PRMgt.aspx.cs" Inherits="GPlus.PRPO.PRMgt" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PRControl.ascx" TagName="PRControl" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805" style="font-family: Tahoma; font-size: 12px">
        <tr>
            <td class="tableHeader">
                ค้นหาข้อมูลใบสั่งซื้อ (PR)
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            เลขที่ PR
                        </td>
                        <td>
                            <asp:TextBox ID="txtPRCodeSearch" runat="server" ></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ประเภท
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPRType" runat="server">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="ขอซื้อ" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ขอจ้าง" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่การสร้าง PR
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
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" OnClick="btnAdd_Click" />
                <asp:Button ID="btnAddProduct" runat="server" SkinID="ButtonMiddleLong" Text="ขอรหัสสินค้าใหม่"
                    PostBackUrl="~/PRPO/ProductCodeMgt.aspx" />
                <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="false" AllowSorting="true" Width="100%" OnRowCommand="gvPR_RowCommand"
                    OnRowDataBound="gvPR_RowDataBound" OnSorting="gvPR_Sorting" SkinID="GvLong">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Height="30" HeaderStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="พิมพ์" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="เลขที่ PR" DataField="PR_Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ประเภท" DataField="PR_Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่ขอซื้อ PR" DataField="Request_Date" HeaderStyle-Width="80" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ยอดเงินของ PR" DataField="Net_Amonut" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="ผู้สร้างPR" DataField="Create_By" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ฝ่าย/ทีม" DataField="Dep_Name" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
        <table cellpadding="0" cellspacing="0" width="805" style="font-family: Tahoma; font-size: 12px">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <uc3:PRControl ID="PRControl1" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
