<%@ Page Title="" Language="C#"  AutoEventWireup="true" MasterPageFile="~/MasterPage/Main.Master"
    CodeBehind="SupplierMTL_Balance.aspx.cs" Inherits="GPlus.Stock.SupplierMTL_Balance" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 95px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- <asp:HiddenField ID="hdStockID" runat="server" />--%>
 <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ยอดสินค้าคงเหลือโรงพิมพ์</td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr style=" height:50px">
                        <td align="right" class="style2">
                            เดือน/ปี</td>
                        <td colspan = "4">
                            <asp:DropDownList ID="ddlMonthStart" runat="server" Width="195" DataTextField="ddlMonth_Name" DataValueField="ddlMonth_ID">
                                <%--<asp:ListItem Value="00">เลือกเดือน</asp:ListItem>--%>
                                <asp:ListItem Value="01">มกราคม</asp:ListItem>
                                <asp:ListItem Value="02">กุมภาพันธ์</asp:ListItem>
                                <asp:ListItem Value="03">มีนาคม</asp:ListItem>
                                <asp:ListItem Value="04">เมษายน</asp:ListItem>
                                <asp:ListItem Value="05">พฤษภาคม</asp:ListItem>
                                <asp:ListItem Value="06">มิถุนายน</asp:ListItem>
                                <asp:ListItem Value="07">กรกฎาคม</asp:ListItem>
                                <asp:ListItem Value="08">สิงหาคม</asp:ListItem>
                                <asp:ListItem Value="09">กันยายน</asp:ListItem>
                                <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                                <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                                <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                            </asp:DropDownList>
                            <asp:dropdownlist id="ddlYearStart" runat="server"></asp:dropdownlist>
                        &nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp; 
                        <asp:DropDownList ID="ddlMonthEnd" runat="server" Width="195" DataTextField="ddlMonth_Name" DataValueField="ddlMonth_ID">
                               <%-- <asp:ListItem Value="00">เลือกเดือน</asp:ListItem>--%>
                                <asp:ListItem Value="01">มกราคม</asp:ListItem>
                                <asp:ListItem Value="02">กุมภาพันธ์</asp:ListItem>
                                <asp:ListItem Value="03">มีนาคม</asp:ListItem>
                                <asp:ListItem Value="04">เมษายน</asp:ListItem>
                                <asp:ListItem Value="05">พฤษภาคม</asp:ListItem>
                                <asp:ListItem Value="06">มิถุนายน</asp:ListItem>
                                <asp:ListItem Value="07">กรกฎาคม</asp:ListItem>
                                <asp:ListItem Value="08">สิงหาคม</asp:ListItem>
                                <asp:ListItem Value="09">กันยายน</asp:ListItem>
                                <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                                <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                                <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                            </asp:DropDownList>
                            <asp:dropdownlist id="ddlYearEnd" runat="server"></asp:dropdownlist>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>

                <asp:GridView ID="gvMovementSum" runat="server" AutoGenerateColumns="false" Width="100%" AllowSorting="true" OnRowCommand="gvMovementSum_RowCommand" OnRowDataBound="gvMovementSum_RowDataBound">
                    <%--OnRowCommand="gvMovementSum_RowCommand" OnRowDataBound="gvMovementSum_RowDataBound" OnSorting="gvMovementSum_Sorting">--%>
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="เดือน/ปี" DataField="Month_End_Sum" SortExpression="Month_End_Sum"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ยอดคงเหลือ" DataField="Balance_Amount" SortExpression="Balance_Amount"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่สร้าง" SortExpression="Create_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้ที่สร้าง" DataField="Crt_by" SortExpression="Crt_by"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Mdf_by" SortExpression="Mdf_by"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="StockID" DataField="Stock_ID" SortExpression="Stock_ID"
                            ItemStyle-HorizontalAlign="Left" visible="false"/>
                    </Columns>
                </asp:GridView>
                <uc1:pagingcontrol ID="PagingControl1" runat="server" />


            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>

    <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                   SkinID="ButtonMiddle" OnClick="btnAdd_Click" />
    
    <br />
    
    <br />
    
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    ยอดสินค้าคงเหลือโรงพิมพ์
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                         <tr >
                            <td style="width: 130px;" align="right">
                                เดือน/ปี</td>
                            <td colspan = "3">
                                <asp:DropDownList ID="ddlMonthDetail" runat="server" Width="195" DataTextField="ddlMonth_Name" DataValueField="ddlMonth_ID">
                                    <asp:ListItem Value="00">เลือกเดือน</asp:ListItem>
                                    <asp:ListItem Value="01">มกราคม</asp:ListItem>
                                    <asp:ListItem Value="02">กุมภาพันธ์</asp:ListItem>
                                    <asp:ListItem Value="03">มีนาคม</asp:ListItem>
                                    <asp:ListItem Value="04">เมษายน</asp:ListItem>
                                    <asp:ListItem Value="05">พฤษภาคม</asp:ListItem>
                                    <asp:ListItem Value="06">มิถุนายน</asp:ListItem>
                                    <asp:ListItem Value="07">กรกฎาคม</asp:ListItem>
                                    <asp:ListItem Value="08">สิงหาคม</asp:ListItem>
                                    <asp:ListItem Value="09">กันยายน</asp:ListItem>
                                    <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                                    <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                                    <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                                </asp:DropDownList>
                                <asp:dropdownlist id="ddlYearDetail" runat="server"></asp:dropdownlist>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                               ยอดคงเหลือ : 
                            </td>
                            <td colspan="3">    
                                <asp:TextBox ID="txtBalance" runat="server" Width="175px" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)"
                                    onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)"
                                    Style="text-align: right" MaxLength="12"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุยอดคงเหลือ"
                                    ControlToValidate="txtBalance" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td colspan = "3">
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <%--<tr>
                            <td style="width: 130px;" align="right">
                            </td>
                            <td colspan="3">    
                                <asp:Button ID="btnDelete" runat="server" Text="ลบข้อมูล" OnClick="btnDelete_Click" />
                            </td>
                        </tr>--%>
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
                        <div id="btnChoice" runat="server">
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                        </div>
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

