<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ProductCodeMgt.aspx.cs" Inherits="GPlus.PRPO.ProductCodeMgt"  MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register src="../UserControls/ItemControl.ascx" tagname="ItemControl" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 83px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ขอรหัสสินค้าจากการจัดซื้อ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                        </td>
                        <td colspan="3">
                            <asp:CheckBoxList ID="cblStatus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="ขอรหัสสินค้า" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="ให้รหัสสินค้า" Value="1" Selected="True"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            ผู้ขอสร้างรหัส
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRequestBySearch" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            วันที่ขอ
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccRequestFrom" runat="server" />
                        </td>
                        <td style="width: 130px;" align="right">
                            ถึงวันที่
                        </td>
                        <td>
                            <uc2:CalendarControl ID="ccRequestTo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearchMain" runat="server" Text="ค้นหา" OnClick="btnSearchMain_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelMain" runat="server" Text="ยกเลิก" OnClick="btnCancelMain_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="false" Width="100%"
                    OnRowCommand="gvItem_RowCommand" OnRowDataBound="gvItem_RowDataBound" OnSorting="gvItem_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="วันที่-เวลาที่ขอ" DataField="Request_Date" SortExpression="Request_Date" />
                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName" />
                        <asp:BoundField HeaderText="หน่วย" DataField="Package_Name" SortExpression="Package_Name" />
                        <asp:BoundField HeaderText="รหัสที่ขอ" />
                        <asp:BoundField HeaderText="รหัสสินค้าที่จัดซื้อกำหนด" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode" />
                        <asp:BoundField HeaderText="ผู้ขอรหัส" DataField="Request_By" SortExpression="Request_By" />
                        <asp:BoundField HeaderText="จัดซื้อผู้กำหนดรหัส" DataField="Approve_By" SortExpression="Approve_By" />
                        <asp:BoundField HeaderText="สถานะข้อมูล" DataField="Status" SortExpression="Status" />
                    </Columns>
                </asp:GridView>
                <uc1:PagingControl ID="PagingControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <asp:Panel ID="pnlRequest" runat="server">
                    <fieldset>
                    <legend>ค้นหาเพื่อขอรหัส</legend>
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อสินค้า
                            </td>
                            <td style="width:160px;">
                                <asp:TextBox ID="txtProductNameSearch" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSearchProduct" runat="server" Text="ค้นหา" OnClick="btnSearchProduct_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:GridView ID="gvItemName" runat="server" Width="90%" AutoGenerateColumns="false"
                                    AllowSorting="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="หน่วย" DataField="Package_Name" SortExpression="Package_Name"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Supplier" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="ราคาต่อหน่วย" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="btnNewRequest" runat="server" Text="เพิ่ม" Visible="false" OnClick="btnNewRequest_Click" />
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                </asp:Panel>
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
                    ขอรหัสสินค้า
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td align="right" class="style1">
                                ชื่อสินค้า
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                            </td>
                            <td style="width: 130px;" align="right">
                                ขนาดและคุณลักษณะ
                            </td>
                            <td>
                                <asp:TextBox ID="txtAttribute" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style1">
                                ประเภท
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="195" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชนิด
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlForm" runat="server" Width="195">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style1">
                                กลุ่มผุ้ใช้งาน
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Width="195">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                กลุ่มอุปกรณ์
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubCate" runat="server" Width="195">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style1">
                                จำนวนที่สั่ง
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="50" MaxLength="10" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right"></asp:TextBox>
                                &nbsp;หน่วยนับ&nbsp;
                                <asp:DropDownList ID="ddlPackage" runat="server" Width="195" DataTextField="Package_Name"
                                    DataValueField="Pack_ID">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                ขอรหัสหน่วยนับ
                            </td>
                            <td>
                                <asp:TextBox ID="txtPackDesc" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style1">
                                หมายเหตุ
                            </td>
                            <td align="left" valign="top" colspan="3">
                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="300" Height="70"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="ลบข้อมูล" OnClientClick="return confirm('คุณต้องการลบรายการนี้หรือไม่?');"
                                    OnClick="btnDelete_Click" />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlApproveCode" runat="server">
                        <table width="100%">
                            <tr>
                                <%--<td style="width: 130px;" align="right">
                                    รหัสสินค้า
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdItemCode" runat="server" />
                                    <asp:TextBox ID="txtApproveItemCode" runat="server" Width="120" MaxLength="20" onblur="SearchItem();"></asp:TextBox>
                                </td>
                                <td style="width: 130px;" align="right">
                                    ชื่อสินค้า
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApproveItemName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                                </td>--%>
                                <td colspan = "4" >
                                    <asp:HiddenField ID="hdItemCode" runat="server" />
                                    <uc3:itemcontrol ID="ItemControl" runat="server"/>
                                </td>
                               
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:LinkButton id="btnRefresh" runat="server" Text="Refresh" Style="display:none;" 
                                        onclick="btnRefresh_Click"></asp:LinkButton>
                                    <asp:GridView ID="gvPackage" runat="server" AllowSorting="false" 
                                        AllowPaging="false" Width="90%"
                                     AutoGenerateColumns="false" onrowdatabound="gvPackage_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="จำนวน" DataField="Pack_Content" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="หน่วย" DataField="Package_Name" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="หน่วยที่เรียกใช้" DataField="Pack_Name_Base" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="รายละเอียด" DataField="Description" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField HeaderText="ราคาประมาณการ" DataField="Avg_Cost" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField HeaderText="หน่วยย่อยทีสุด" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px;" align="right">
                                    ผู้ให้รหัส
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApproveBy" runat="server" Width="190" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 130px;" align="right">
                                    วันที่-เวลา
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApproveDate" runat="server" Width="190" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnSaveApprove" runat="server" Text="บันทึก" 
                                        onclick="btnSaveApprove_Click" />
                                    <asp:Button ID="btnCancelApprove" runat="server" Text="ยกเลิก" 
                                        CausesValidation="false" onclick="btnCancelApprove_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
