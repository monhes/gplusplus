<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="CalendarPayGoodsTable.aspx.cs" Inherits="GPlus.MasterData.CalendarPayGoodsTable" %>

<%@ Register Src="~/UserControls/CalendarPayGoodsControl.ascx" TagName="CalendarPayGoodCtrl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805" border="0">
        <tr>
            <td class="tableHeader">
                ค้นหา
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table border="0" width="100%">
                    <tr>
                        <td style="text-align: right">ตั้งแต่เดือน/ปี
                            <asp:TextBox Width="30px" runat="server" style="text-align: right" ID="txtMonthFrom"></asp:TextBox> 
                            / <asp:TextBox Width="50px" runat="server" ID="txtYearFrom"></asp:TextBox>
                        <td style="text-align: left">ถึงเดือน/ปี
                            <asp:TextBox Width="30px" runat="server" style="text-align: right" ID="txtMonthTo"></asp:TextBox> 
                            / <asp:TextBox Width="50px" runat="server" ID="txtYearTo"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />
                            <asp:Button runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"><asp:Button runat="server" Text="เพิ่มข้อมูล" OnClick="btnAddData_Click" ID="btnAddData" /></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView runat="server" ID="gvPayGoodsCalendar" AutoGenerateColumns="false" 
                                OnRowDataBound="gvRowDataBound"
                                OnRowCommand="gvRowCommand" Width="100%">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Detail"
                                             CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                             ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Calendar_Year" HeaderText="ปี" 
                                        HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Month_Number" HeaderText="เดือน" />
                                    <asp:BoundField DataField="Week_Seq" HeaderText="สัปดาห์ที่"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DateRange" HeaderText="ช่วงวันที่" />
                                    <asp:BoundField DataField="Cancel_Status" HeaderText="สถานะ" ItemStyle-HorizontalAlign="center" />
                                    <asp:BoundField DataField="Update_Date" HeaderText="วันที่แก้ไขล่าสุด" ItemStyle-HorizontalAlign="center" />
                                    <asp:BoundField DataField="Update_By" HeaderText="ผู้แก้ไขล่าสุด" />
                                </Columns>
                            </asp:GridView>
                            <uc1:PagingControl ID="PagingControl1" runat="server" />
                        </td>
                    </tr>
                    

                </table>
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
                    <uc1:CalendarPayGoodCtrl ID="CalendarPayGoodCtrl" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
</asp:Content>
