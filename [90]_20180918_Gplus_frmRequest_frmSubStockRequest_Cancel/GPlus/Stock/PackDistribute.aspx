<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="PackDistribute.aspx.cs" Inherits="GPlus.Stock.PackDistribute" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register src="../UserControls/ItemControlPackBig.ascx" tagname="ItemControl2" tagprefix="uc3" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 128px;
        }
    </style>


   <script type="text/javascript">
       function isNumericKey(e) {
           var charInp = window.event.keyCode;

           if (charInp > 31 && (charInp < 48 || charInp > 57)) {
               return false;
           }
           return true;
       }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                บันทึกรับการแตก Pack สินค้า
            </td>
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
                                 DataTextField="StockType_Name" DataValueField="StockType_ID" Height="23px" OnSelectedIndexChanged="ddlStock_SelectedIndexChanged" AutoPostBack="true">
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
                            รายการวัสดุอุปกรณ์ : <span style="color: Red">*</span>
                        </td>
                        <td colspan="3">
                            <uc3:itemcontrol2 ID="ItemControl2" runat="server"/>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnRefreshI" runat="server" Text="Refresh" CausesValidation="false"
                             OnClick="btnRefreshI_Click" Style="display: none;"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            จำนวนในคลัง :
                        </td>
                        <td>
                           <%--<asp:TextBox ID="txt_InOnhand" runat="server" onkeypress="return isNumericKey(event);" Width="120px" Enabled="false" />--%>
                           <asp:Label ID="lb_InOnhand" runat="server" ForeColor="Green" Font-Bold Width="120px" />
                        </td>
                        <td style="width: 80px;" align="right">
                            จำนวนที่แบ่ง : <span style="color: Red">*</span>
                        </td>
                        <td>
                           <asp:TextBox ID="txt_InReceive" runat="server" onkeypress="return isNumericKey(event);" Width="120px" OnTextChanged="txt_InReceive_TextChanged" AutoPostBack="true" />
                        </td>
                        <td style="width: 120px;">
                        </td>
                    </tr>
                </table>
               <asp:Panel runat="server" ID="pnPack" Visible="false">
                <asp:GridView ID="gvPackList" runat="server" Width="100%"
                        OnRowDataBound="gvPackList_RowDataBound" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="ลำดับ" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Pack_Description" HeaderText="ขนาดบรรจุ" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="จำนวนในคลัง">
                                <ItemTemplate>
                                        <%--<asp:TextBox ID="txt_OnhandQty" runat="server"  Enabled="false" 
                                        Width="90px"></asp:TextBox>--%>
                                        <asp:Label ID="lb_OnhandQty" runat="server" Width="120px" />
                                        <asp:HiddenField ID="hdID" runat="server" />
                                        <asp:HiddenField ID="hdPackID" runat="server" />
                                        <asp:HiddenField ID="hdOnhandQty" runat="server" />
                                        <asp:HiddenField ID="hdPackContent" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนที่รับ">
                                <ItemTemplate>
                                        <asp:TextBox ID="txt_ReceiveQty" runat="server"  Enabled="true" onkeypress="return isNumericKey(event);" 
                                        Width="90px" OnTextChanged = "txt_ReceiveQty_textChanged" AutoPostBack="true"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนที่ได้">
                                <ItemTemplate>
                                        <%--<asp:TextBox ID="txt_SumQty" runat="server"  Enabled="false"  onkeypress="return isNumericKey(event);"
                                        Width="120px"></asp:TextBox>--%>
                                        <asp:Label ID="lb_SumQty" runat="server" Width="120px" />
                                </ItemTemplate>
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="จำนวนรวม">
                                <ItemTemplate>
                                        <%--<asp:TextBox ID="txt_TotalQty" runat="server"  Enabled="false" onkeypress="return isNumericKey(event);"
                                        Width="120px"></asp:TextBox>--%>
                                        <asp:Label ID="lb_TotalQty" runat="server" Width="120px" />
                                </ItemTemplate>
                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                 <table cellspacing="10" width="100%">
                    <tr>
                        <td colspan="5" align="center"> 
                            <uc1:PagingControl runat="server" ID="pagingControlReqList" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="บันทึก" CausesValidation="False" OnClick="btnSave_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancelSearch_Click" />
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
    </ContentTemplate>
 </asp:UpdatePanel>

    <br />
    
    <br />
    
   
</asp:Content>

