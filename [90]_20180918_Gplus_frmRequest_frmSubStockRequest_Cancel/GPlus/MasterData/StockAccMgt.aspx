<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="StockAccMgt.aspx.cs" Inherits="GPlus.MasterData.StockAccMgt" %>
<%@ Register src="../UserControls/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register src="../UserControls/AccountNameControl.ascx" tagname="AccountNameControl" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 13px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาผู้ใช้งานคลังสินค้า
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            คลังสินค้า
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStock" runat="server" DataTextField="Stock_Name" 
                                DataValueField="Stock_ID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1" colspan="2">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="ผู้ใช้งาน" 
                                HorizontalAlign="Left">
                            <table width="100%">
                            <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อ</td>
                                <td style="width: 130px;" align="right">
                                    <asp:TextBox ID="txtAccFName" runat="server" style="text-align: left"></asp:TextBox>
                                </td>
                            <td style="width: 130px;" align="right">
                                นามสกุล</td>
                                <td style="width: 130px;" align="right">
                                    <asp:TextBox ID="txtAccLName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click"/>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click"/>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclick="btnAdd_Click"/>
                <asp:HiddenField ID="hdStatus" runat="server" />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    AllowSorting="true" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                    OnSorting="GridView1_Sorting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" CommandName="Edi" CommandArgument="Stock_Account_ID" ItemStyle-HorizontalAlign="Center">รายละเอียด</asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                รายละเอียด
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="StAccID" DataField="Stock_Account_ID" Visible="False" SortExpression="Stock_Account_ID"/>
                        <asp:BoundField HeaderText="AccID" DataField="Account_ID" Visible="False" SortExpression="Account_ID"/>
                        <asp:BoundField HeaderText="คลังสินค้า" DataField="Stock_Name" SortExpression="Stock_Name" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="ชื่อ-นามสกุลผู้ใช้งาน" DataField="fullname" SortExpression="fullname" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField HeaderText="สถานะ" DataField="stock_account_status" SortExpression="stock_account_status" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" DataField="update_date" SortExpression="update_date" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="update_by" SortExpression="update_by" ItemStyle-HorizontalAlign="Left"/>
                    </Columns>
                </asp:GridView>
                <uc1:PagingControl ID="PagingControl1" runat="server" />
                <tr><td class="tableFooter"></td></tr>
            </td>
        </tr>
    </table>
    <br />
    <a name="pnlDetail"></a>
    <asp:Panel ID="pnlDetail" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อ-นามสกุลผู้ใช้งาน</td>
                            <td>
                                <uc2:AccountNameControl ID="AccountNameControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                คลังสินค้า 
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStock1" runat="server" DataTextField="Stock_Name" 
                                    DataValueField="Stock_ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 130px;" align="right">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" onclick="btnSave_Click" 
                            style="height: 26px"/>
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                            CausesValidation="False" onclick="btnCancel_Click"/>
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
