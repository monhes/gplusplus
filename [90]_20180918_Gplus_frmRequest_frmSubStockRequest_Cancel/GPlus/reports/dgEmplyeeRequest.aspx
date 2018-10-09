<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dgEmplyeeRequest.aspx.cs" Inherits="GPlus.Request.dgEmplyeeRequest"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>รายชื่อ</title>
    <base target="_self"/>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/jquery-1.7.2.min.js"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
    <body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;" onload="OnLoadBody();">
        <form runat="server">
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
                        ค้นหาผู้ทำการร้องขอ
                    </td>
                </tr>
                 <tr>
                     <td class="tableBody">
                        <table width="100%">
                            <tr>
                                <td style="width: 100px;" align="right">
                                    ชื่อ
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFname" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                 <td style="width: 100px;" align="right">
                                    สุกล
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLname" runat="server" Width="190" MaxLength="100"></asp:TextBox>
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
                         <asp:HiddenField runat="server" ID="gvEmployeeId" />
                         <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" Width="100%" AllowSorting="true" OnRowCommand="GvEmployeeRowCommand" OnRowDataBound="GvEmployeeRowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnSelected" CommandName="Edi" Text="เลือก" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ชื่อ" DataField="Account_Fname" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="นามสกุล" DataField="Account_Lname" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="ฝ่าย/ทีม" DataField="Description" ItemStyle-HorizontalAlign="Left" />
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
