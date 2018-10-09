<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_NoPRSelect.aspx.cs" Inherits="GPlus.UserControls.pop_NoPRSelect" %>


<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
        <center>
            <table cellpadding="0" cellspacing="0" width="805">
                <tr>
                    <td class="tableHeader" align="left">
                        ค้นหาเลขที่ใบ PO
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table border="0" cellpadding="5">
                            <tr>
                                <td>
                                    ค้นหาเลขที่ใบขอซื้อ/ขอจ้าง
                                </td>
                                <td>
                                   <asp:TextBox ID="txtNoPR" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    วิธีการขอซื้อ/ขอจ้าง
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPRType" runat="server">
                                        <asp:ListItem Text="ทั้งหมด" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="ขอซื้อ" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="ขอจ้าง" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:GridView runat="server" ID="gvNoPR" AutoGenerateColumns="false" 
                                        OnRowDataBound="gvItem_RowDataBound" Width="100%">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSelect" runat="server" Text="เลือก"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PR_Code" HeaderText="เลขที่ใบขอซื้อ/ขอจ้าง" />
                            </Columns>
                        </asp:GridView>
                        <uc1:PagingControl ID="PagingControl1" runat="server" />
                    </td>
                </tr>
            </table>
        </center>
    </form>
</body>
</html>
