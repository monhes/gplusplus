<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="BuyHistoryMgt.aspx.cs" Inherits="GPlus.MasterData.BuyHistoryMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register src="../UserControls/CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาข้อมูลการซื้อสินค้าจาก Supplier
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            Supplier
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlSupplier" runat="server" DataTextField="Supplier_Name" DataValueField="Supplier_ID" Width="155">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            รายการสินค้า
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaterialNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td  align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvBuyHistory" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvBuyHistory_RowCommand" OnRowDataBound="gvBuyHistory_RowDataBound"
                    OnSorting="gvBuyHistory_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" DataField="Material_Code" SortExpression="Material_Code"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="รายการสินค้า" DataField="Material_Name" SortExpression="Material_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="หน่วย" DataField="PackageName" SortExpression="PackageName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ราคาต่อหน่วยที่ซื้อครั้งสุดท้าย" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่ซื้อครั้งสุดท้าย" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ราคาต่อหน่วยล่าสุด" DataField="PresentPrice" SortExpression="PresentPrice" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ส่วนลด%" DataField="DiscountMarket" SortExpression="DiscountMarket" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ส่วนลด(จำนวนเงิน)" DataField="Price" SortExpression="Price" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ของแถม" DataField="GiveAway" SortExpression="GiveAway" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="หน่วย" DataField="GiveAwayUnit" SortExpression="GiveAwayUnit" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ลงวันที่" DataField="PresentDate" SortExpression="PresentDate" ItemStyle-HorizontalAlign="Left" />
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
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td style="width: 130px; color: Red;" align="right">
                                รหัส
                            </td>
                            <td colspan="2">
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPackage" runat="server" DataTextField="Description" DataValueField="MaterialPackage_ID" Width="155"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุ Package"
                                    ControlToValidate="ddlPackage" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ราคาต่อหน่วยที่ซื้อครั้งสุดท้าย
                            </td>
                            <td>
                                 <asp:TextBox ID="txtLastPrice" runat="server" Width="90" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                     onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>บาท
                            </td>
                            <td style="width: 130px;" align="right">
                                วันที่ซื้อครั้งสุดท้าย
                            </td>
                            <td>
                                <uc3:CalendarControl ID="CalendarControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px; color: Red;" align="right">
                                ราคาต่อหน่วยที่เสนอล่าสุด
                            </td>
                            <td>
                                <asp:TextBox ID="txtPresentPrice" runat="server" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                     onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุราคาต่อหน่วยที่เสนอล่าสุด"
                                    ControlToValidate="txtPresentPrice" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                วันที่เสนอล่าสุด
                            </td>
                            <td>
                                <uc3:CalendarControl ID="CalendarControl2" runat="server" IsRequire="true" />
                            </td>
                        </tr>
                       <tr>
                            <td style="width: 130px;" align="right">
                                ส่วนลดการค้า
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscount" runat="server" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                     onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="5"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                จำนวนเงิน
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscountPrice" runat="server" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                     onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ของแถม
                            </td>
                            <td>
                                <asp:TextBox ID="txtGiveup" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                หน่วย
                            </td>
                            <td>
                                <asp:TextBox ID="txtGiveupUnit" runat="server" MaxLength="50"></asp:TextBox>
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
