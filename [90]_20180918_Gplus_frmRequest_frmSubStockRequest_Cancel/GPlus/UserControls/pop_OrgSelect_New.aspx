<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_OrgSelect_New.aspx.cs" Inherits="GPlus.UserControls.pop_OrgSelect_New" %>


<%@ Register Src="PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../Script/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            height: 34px;
        }
    </style>
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
        <center>
            <table cellpadding="0" cellspacing="0" width="805">
                <tr>
                    <td class="tableHeader" align="left">
                        ระบุทีมงานที่ขอซื้อ
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table border="0" cellpadding="5" style="width: 597px">
                            <tr>
                                <td class="auto-style1">
                                    <label runat="server" id="lbDivCode">ค้นหารหัสฝ่าย</label>
                                </td>
                                <td class="auto-style1" align="left">
                                   <asp:TextBox ID="txtDivCode" runat="server"></asp:TextBox>
                                </td>
                                <td class="auto-style1"> 
                                    
                                </td>
                                 <td class="auto-style1">
                                     </td>
                            </tr>
                            <tr>
                                <td>ค้นหาชื่อฝ่าย/ทีม</td>
                                <td colspan="3" align = "left">
                                    <asp:TextBox ID="txtDescription" runat="server" Width="399px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"><asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="btnSearch_Click" /></td>
                            </tr>
                        </table>
                        <asp:GridView runat="server" ID="gvDivDepDisplay" 
                            AutoGenerateColumns="false" Width="100%" OnRowDataBound="gv_RowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSelect" runat="server" Text="เลือก"
                                            CommandName="Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Div_Code" HeaderText="รหัสฝ่าย" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100"/>
                            <%--    <asp:BoundField DataField="Dep_Code" HeaderText="รหัสทีม"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" />--%>
                                <asp:BoundField DataField="Description" HeaderText="ชื่อฝ่าย" Visible="true" />
                                <%--<asp:BoundField DataField="Description" HeaderText="ชื่อทีม" />--%>
                            </Columns>
                        </asp:GridView>
                        <uc1:PagingControl ID="PagingControl1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter"></td>
                </tr>
            </table>
        </center>
    </form>
</body>
</html>
