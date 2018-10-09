<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="POMgt.aspx.cs" Inherits="GPlus.PRPO.POMgt" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/POControl.ascx" TagName="POControl" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805"  style="font-family: Tahoma; font-size: 12px">
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
                                <asp:ListItem Text="Stock" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Asset" Value="1"></asp:ListItem>
                                <asp:ListItem Text="ทั้งหมด" Value="" ></asp:ListItem>
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
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" OnClick="btnAdd_Click" />
                <asp:GridView ID="gvPO" runat="server" Width="100%" AutoGenerateColumns="false" OnRowCommand="gvPO_RowCommand"
                    OnRowDataBound="gvPO_RowDataBound" OnSorting="gvPO_Sorting" AllowSorting="true" SkinID="GvLong">
                    <HeaderStyle Height="30" />
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="พิมพ์" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="เลขที่ PO" DataField="PO_Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110" />
                        <asp:BoundField HeaderText="ประเภท" DataField="PO_Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50" />
                        <asp:BoundField HeaderText="วันที่ออก PO" DataField="PO_Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80" />
                        <asp:BoundField HeaderText="ยอดเงิน" DataField="Net_Amonut" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField HeaderText="ผู้สร้าง PO" DataField="Create_By_FullName" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ฝ่าย/ทีม" DataField="Dep_Name" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CommandName="Upd" Visible="false" CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:LinkButton ID="btnUpload" runat="server" Text="" style="display:none;"></asp:LinkButton>
                <asp:ModalPopupExtender ID="mpeUpload" runat="server" TargetControlID="btnUpload"
                    PopupControlID="pnlBrowse" BackgroundCssClass="modalBackground" CancelControlID="btnBrowseCancel"
                    DropShadow="true" />
                <asp:Panel ID="pnlBrowse" runat="server" Width="600" BackColor="White" Style="display: none">
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                เลือกไฟล์
                            </td>
                            <td>
                                <asp:HiddenField ID="hdID" runat="server" />
                                <asp:FileUpload ID="fudFile" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btnBrowseSave" runat="server" Text="บันทึก" OnClick="btnBrowseSave_Click"
                                    CausesValidation="false" />&nbsp;
                                <asp:Button ID="btnBrowseCancel" runat="server" Text="ยกเลิก" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
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
    <%--<asp:UpdatePanel ID="pnlDetail" runat="server" Visible="false">
        <ContentTemplate>--%>
        <table cellpadding="0" cellspacing="0" width="805" style="font-family: Tahoma; font-size: 12px">
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
<%--        </ContentTemplate> 
    </asp:UpdatePanel>--%>
    </asp:Panel>

    <script type="text/javascript">
     
     var first = true;

     function txtPOCodeSearchChange()
     {
         if (first) {
             var $dtStart = document.getElementById('<%= ccFrom.ClientID %>');
             var $dtEnd = document.getElementById('<%= ccTo.ClientID %>');
             $dtStart.value = "";
             $dtEnd.value = "";
             first = false;
         }
        
     }

    </script>

</asp:Content>
