<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dgItemCatagory.aspx.cs" Inherits="GPlus.Request.dgItemCatagory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>รายการวัสดุ-อุปกรณ์</title>
    <base target="_self"/>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
        <script type="text/javascript" language="javascript">
            function OnLoadBody() {
                self.resizeTo(600, 400);
                window.outerHeight = 300;
                window.outerWidth = 500;
            }
            function CloseDialog() {
                window.close();
            }
        </script>
        <table cellpadding="0" cellspacing="0" width="98%">
            <tr>
                <td class="tableHeader">
                    ค้นหาวัสดุอุปกรณ์
                </td>
            </tr>
             <tr>
                 <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td style="width: 100px;" align="right">
                                รหัส
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtMaterialCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                            </td>
                             <td style="width: 100px;" align="right">
                                ชื่อ
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtMaterialNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px;" align="right">
                                สถานะ
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="195">
                                    <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="BtnSearchClick" />&nbsp;&nbsp;
                                <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                    OnClientClick="javascript:CloseDialog();" />
                            </td>
                        </tr>
                    </table>
                     <asp:HiddenField runat="server" ID="_hfItemId" />
                     <asp:GridView ID="gvMaterial" runat="server" AutoGenerateColumns="false" Width="100%"
                        AllowSorting="true" OnRowCommand="GvMaterialRowCommand" OnRowDataBound="GvMaterialRowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnSelected" CommandName="Edi" Text="เลือก" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="ชื่อสินค้า" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="หน่วยนับ" DataField="Description" SortExpression="Description"
                                ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="จำนวน" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:TextBox runat="server" ID="tbQty" Width="68px" Text="1" style="text-align:right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" />--%>
                                    <asp:TextBox runat="server" ID="tbQty" Width="68px" style="text-align:right;" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" Visible="false" />
                            <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                                ItemStyle-HorizontalAlign="Left" Visible="false" />
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
    </form>
</body>
</html>
