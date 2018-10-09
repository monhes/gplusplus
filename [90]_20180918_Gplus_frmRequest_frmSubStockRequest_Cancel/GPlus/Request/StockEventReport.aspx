<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="StockEventReport.aspx.cs" Inherits="GPlus.Request.StockEventReport"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ItemOrgStructControl2.ascx" TagName="ItemOrgStructControl2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                รายงานการเบิกและเปรียบเทียบ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td colspan="4">
                            <uc1:ItemOrgStructControl2 ID="ItemOrgStructCtrl2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px"></td>
                        <td colspan="2">
                            <fieldset style="width: 100%">
                                <legend>ช่วงเวลาจ่าย</legend>
                                    <table>
                                        <tr>
                                            <td align="left">
                                                <asp:RadioButtonList ID="rblDateType" runat="server">
                                                    <asp:ListItem Value="D">วันที่</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="M">เดือน</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <uc2:CalendarControl ID="ccStartDate" runat="server" />
                                                        </td>
                                                        <td>
                                                            ถึงวันที่
                                                        </td>
                                                        <td>
                                                            <uc2:CalendarControl ID="ccEndDate" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMonth" runat="server" Width="120">
                                                                <asp:ListItem Value="1">มกราคม</asp:ListItem>
                                                                <asp:ListItem Value="2">กุมภาพันธ์</asp:ListItem>
                                                                <asp:ListItem Value="3">มีนาคม</asp:ListItem>
                                                                <asp:ListItem Value="4">เมษายน</asp:ListItem>
                                                                <asp:ListItem Value="5">พฤษภาคม</asp:ListItem>
                                                                <asp:ListItem Value="6">มิถุนายน</asp:ListItem>
                                                                <asp:ListItem Value="7">กรกฎาคม</asp:ListItem>
                                                                <asp:ListItem Value="8">สิงหาคม</asp:ListItem>
                                                                <asp:ListItem Value="9">กันยายน</asp:ListItem>
                                                                <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                                                                <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                                                                <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">
                                                            ปี พ.ศ.
                                                        </td>
                                                        <td><asp:TextBox 
                                                                ID="tbYear" runat="server" Width="50px"
                                                                onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" 
                                                                onpaste="return CancelKeyPaste(this)"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                            </fieldset>
                        </td>
                        <td style="width: 150px">&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <fieldset style="width: 100%">
                                <legend>วัสดุ-อุปกรณ์</legend>
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td align="right">ประเภทวัสดุอุปกรณ์</td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" Width="250"
                                                                DataTextField="Cat_Name" DataValueField="Cate_ID" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" Enabled="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">รหัสวัสดุ - อุปกรณ์</td>
                                                        <td align="left"><asp:TextBox ID="tbItemCode" runat="server" onKeyUp="return chkHideGridview()" ></asp:TextBox></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">รายการสินค้า</td>
                                                        <td align="left"><asp:TextBox ID="tbItemName" runat="server" onKeyUp="return chkHideGridview()"></asp:TextBox></td>
                                                        <asp:LinkButton ID="btnRefreshGv" runat="server" Text="..." 
                                                                    OnClientClick="return true;" ClientIDMode="Static" onclick="btnRefreshGv_Click" style="display:none"></asp:LinkButton>
                                                        <td>
                                                            <asp:CheckBox ID="chkSummary" runat="server" Text=" พิมพ์สรุป " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                            </fieldset>
                        </td>
                    </tr>
                    <%--<tr>
                    <td colspan = "4" align="left">
                        <asp:Label ID="lblList" Text="รายการสินค้า" runat="server"></asp:Label>
                    </td>
                    </tr>--%>
                    <tr>
                    <td colspan = "4">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="gv" style="width: 100%; max-height: 350px; overflow:auto" runat="server" visible="false">
                            <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="false" 
                                    Width="100%" AllowSorting="false" onrowdatabound="gvItem_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkDH" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkD" runat="server" />
                                                <asp:HiddenField ID="hdID" runat="server" />
                                                <asp:HiddenField ID="hdUnitID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Item_Search_Desc" SortExpression="Item_Search_Desc"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="หน่วย" DataField="Pack_Description" SortExpression="Pack_Description"
                                                ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                                </div>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                              <asp:Button ID="bSearch" runat="server" Text="ค้นหา" OnClick="bSearch_Click" />&nbsp;&nbsp;
                              <asp:Button ID="bCancel" runat="server" Text="ยกเลิก" OnClick="bCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <rsweb:ReportViewer ID="ReportViewerSummary" runat="server" Width="100%"
                                Height="400px" Font-Names="Verdana" Font-Size="8pt"
                                InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                                WaitMessageFont-Size="14pt">
                                <LocalReport ReportPath="Request\StockEventReportSummary.rdlc">
                                </LocalReport>
                            </rsweb:ReportViewer>
                            <rsweb:ReportViewer ID="ReportViewer" runat="server" Width="100%"
                                Height="400px" Font-Names="Verdana" Font-Size="8pt"
                                InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                                WaitMessageFont-Size="14pt">
                                <LocalReport ReportPath="Request\StockEventReport.rdlc">
                                </LocalReport>
                            </rsweb:ReportViewer>
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
    
    <script type="text/javascript">
        $(document).ready(function () {
            var $bSearch = $('#<%= bSearch.ClientID %>');

            $bSearch.click(function () {

                var $tbYear = $('#<%= tbYear.ClientID %>');

                if ($tbYear.val() == "") {
                    alert('กรุณาระบุปี พ.ศ.');
                    return false;
                }
            });



            var $cb = $('#<%= gvItem.ClientID %> tr:eq(0)').find('th:eq(0)').find('input:eq(0)');

            var cbState = $cb.is(':checked');

            $cb.click(function () {
                if (cbState) {
                    cbState = false;

                    $.each($('#<%= gvItem.ClientID %> tr'), function () {
                        $(this).find('input').removeAttr('checked');
                    });

                } else {
                    cbState = true;

                    $.each($('#<%= gvItem.ClientID %> tr'), function () {
                        $(this).find('input').attr('checked', 'true');
                    });
                }
            });

        });

        //        function hideGridview() {
        //            //alert("test");
        //            $('#<%= gv.ClientID %>').hide();
        //            return true;
        //        }



        function chkHideGridview() {

            if ($('#<%= tbItemCode.ClientID %>').val() == "" && $('#<%= tbItemName.ClientID %>').val() == "") {

                document.getElementById('btnRefreshGv').click();

            }
            else {
                $('#<%= gv.ClientID %>').hide();
            }



        }


    </script>
</asp:Content>


