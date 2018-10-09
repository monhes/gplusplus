<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_NoPOSelect.aspx.cs" Inherits="GPlus.PRPO.pop_NoPOSelect" %>

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
                        <table border="0" cellpadding="5" width="100%">
                            <tr>
                                <td style="width:100px; text-align:right">
                                    ค้นหาเลขที่ใบ PO
                                </td>
                                <td style="width:100px; text-align:left">
                                   <asp:TextBox ID="txtNoPO" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView runat="server" ID="gvNoPO" AutoGenerateColumns="false" 
                                        OnRowDataBound="gvItem_RowDataBound" Width="100%">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnSelect" runat="server" Text="เลือก"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PO_Code" HeaderText="เลขที่ใบ PO" 
                                                HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagingControl ID="PagingControl1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </form>
</body>
</html>
