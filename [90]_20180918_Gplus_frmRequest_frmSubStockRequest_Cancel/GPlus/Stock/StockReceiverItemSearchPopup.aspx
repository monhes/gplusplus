<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceiverItemSearchPopup.aspx.cs" Inherits="GPlus.Stock.StockReceiverItemSearchPopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"/>
    <title></title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;" onload="javascript:OnLoadBody();">
    <form id="form1" runat="server">
        <script type="text/javascript" language="javascript">
            function OnLoadBody() {
                self.resizeTo(600, 400);
                window.outerHeight = 400;
                window.outerWidth = 600;
            }
            function CloseDialog() {
                window.close();
            }
            function CheckOne(obj) {
                var grid = obj.parentNode.parentNode.parentNode;
                var inputs = grid.getElementsByTagName("input");
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].type == "checkbox") {
                        if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                            inputs[i].checked = false;
                        }
                    }
                }
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
                                ประเภท
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlMaterialTypeSearch" runat="server" Width="195" DataTextField="MaterialType_Name" DataValueField="MaterialType_ID">
                                </asp:DropDownList>
                            </td>
                             <td style="width: 100px;" align="right">
                                รหัสเดิม(AS400)
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtOldCodeSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
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
                    AllowSorting="true" OnRowCommand="gvMaterial_RowCommand" OnRowDataBound="gvMaterial_RowDataBound"
                    OnSorting="gvMaterial_Sorting">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnSelected" CommandName="Edi" Text="เลือก" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ชื่อ" DataField="Inv_ItemName" SortExpression="Inv_ItemName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ขนาดและคุณลักษณะ" DataField="Inv_Attrbute" SortExpression="Inv_Attrbute"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ประเภท" DataField="Cat_Name" SortExpression="Cat_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มผู้ใช้งาน" DataField="Type_Name" SortExpression="Type_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มอุปกรณ์" DataField="SubCate_Name" SortExpression="SubCate_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Asset_Status" SortExpression="Asset_Status"
                            ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />
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
