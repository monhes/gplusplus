<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_ProductHelp.aspx.cs" Inherits="GPlus.PRPO.pop_ProductHelp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
    <script src="../Script/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
    <ajaxtoolkit:toolkitscriptmanager runat="server" ID="ScriptManager1">
        <%--<Services>
            <asp:ServiceReference Path="~/autocomplete.asmx" />
        </Services>--%>
    </ajaxtoolkit:toolkitscriptmanager>
    <center>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="805">
                    <tr>
                        <td class="tableHeader" align="left">
                            ระบุรายการสินค้า
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBody">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <table width="90%">
                                            <tr>
                                                <td width="35%" align="right">
                                                    รหัสสินค้า
                                                </td>
                                                <td width="55%" align="left">
                                                    <asp:TextBox ID="txtProductCode" runat="server" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <%--<td width="10%" align="right">
                                                    &nbsp;
                                                </td>
                                                <td width="35%" align="left">
                                                    &nbsp;
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    ชื่อสินค้า
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtProductName" runat="server" Width="190" MaxLength="100"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <%--<td align="right">
                                                    หน่วยนับ
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlUnit" runat="server" Width="185">
                                                    </asp:DropDownList>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" />
                                                    <asp:HiddenField ID="hid_Stock_id" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div id="div_item" runat="server" visible="false">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="false" Width="780px"
                                                  OnRowCommand="gvItem_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <%--<HeaderTemplate>
                                                            <asp:CheckBox ID="chkDH" runat="server" />
                                                        </HeaderTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnChoose" runat="server" Text="เลือก" CommandName="Choose"
                                                                CommandArgument='<%# Eval("inv_Itemcode")+ "," + Eval("Item_Search_Desc") + "," + Eval("Pack_Description") %>' CausesValidation="false"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="รหัส" DataField="inv_Itemcode" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                                    <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Item_Search_Desc" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="520px" />
                                                    <asp:BoundField HeaderText="หน่วย" DataField="Pack_Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <uc1:PagingControl ID="PagingControl1" runat="server" />
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td align="center">
                                            <asp:Button ID="btnOK" runat="server" Text="ตกลง" />
                                            <asp:Button ID="btnCancel_Close" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;" />
                                        </td>
                                    </tr>--%>
                                </table>
                                <br />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td class="tableFooter">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    </form>
</body>
</html>
