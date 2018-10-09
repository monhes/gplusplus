<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="MgtStockCardMovment.aspx.cs" Inherits="GPlus.Stock.MgtStockCardMovment" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register src="../UserControls/ItemControl2.ascx" tagname="ItemControl2" tagprefix="uc3" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ความเคลื่อนไหวของสินค้าคงคลัง</td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%" cellspacing="10">
                    <tr>
                        <td style="width: 130px;" align="right">
                           ชื่อคลัง : <span style="color: Red">*</span>
                        </td>
                        <td colspan="3" align="left">
                             <asp:DropDownList ID="ddlStock" runat="server" Width="295px" 
                                 DataTextField="StockType_Name" DataValueField="StockType_ID" Height="23px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุ Supplier"
                                ControlToValidate="ddlStock" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                            </asp:ValidatorCalloutExtender>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                           วันที่ : <span style="color: Red">*</span>
                        </td>
                        <td class="style3">
                           <uc2:calendarcontrol ID="dtCreatePOStart" runat="server" />
                        </td>
                        <td style="width: 60px;" align="right">
                            ถึงวันที่ : <span style="color: Red">*</span>
                        </td>
                        <td>
                            <uc2:calendarcontrol ID="dtCreatePOStop" runat="server" />
                        </td>
                    </tr>
                    <tr>
                    <td style="width: 130px;" align="right">
                        รายการวัสดุอุปกรณ์ : <span style="color: Red">*</span>
                    </td>
                    <td colspan="3">
                        <uc3:itemcontrol2 ID="ItemControl2" runat="server"/>
                    </td>
                    </tr>
                    
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>

                <asp:GridView ID="gvMovement" class="GridHeader2" runat="server" 
                    AutoGenerateColumns="False" Width="100%" AllowSorting="True"
                     OnRowDataBound="gvMovement_RowDataBound" 
                    OnRowCreated="gvMovement_RowCreated" SkinId="GvLong">
                    <Columns>
                        <asp:BoundField HeaderText="วันที่ เวลา" DataField="Transaction_Date" SortExpression="Transaction_Date"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ยอดยกมา" DataField="Income_Balance"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                        <asp:BoundField HeaderText="รับเข้า"  DataField="Recieve_Qty"  itemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0}"/>
                        <asp:BoundField HeaderText="จ่ายออก" DataField="Pay_Qty"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0}"/>
                        <asp:BoundField HeaderText="จำนวนเงิน" DataField="Amount"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0.0000}"/>
                        <asp:BoundField HeaderText="คงเหลือ" DataField="Total"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:###,##0}"/>
                        <asp:BoundField HeaderText="อ้างอิงเลขที่บันทึก" DataField="Reference_Transaction_No" SortExpression="Reference_Transaction_No"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="อ้างอิงเลขที่เอกสาร" DataField="Reference_Document_No" SortExpression="Reference_Document_No"
                            ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField HeaderText="หมายเหตุ" 
                            ItemStyle-HorizontalAlign="Right" DataField="Remark" SortExpression="Remark"/>
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

    
    <br />
    
    <br />
    
   
</asp:Content>

