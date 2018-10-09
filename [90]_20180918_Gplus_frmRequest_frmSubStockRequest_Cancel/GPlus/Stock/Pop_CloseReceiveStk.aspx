<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pop_CloseReceiveStk.aspx.cs" Inherits="GPlus.Stock.Pop_CloseReceiveStk" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
        <center>
            <table cellpadding="0" cellspacing="0" width="805">
                <tr>
                    <td class="tableHeader" align="left">
                        ปิดการรับสินค้า
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table border="0" cellpadding="5">
                            <tr>
                                <td>
                                    เลขที่ใบสั่งซื้อ
                                </td>
                                <td>
                                   <asp:TextBox ID="txtPOCode" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td> 
                                    วันที่ออกใบสั่งซื้อ
                                </td>
                                 <td>
                                    <asp:TextBox ID="txtPODate" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td> 
                                    รับสินค้าครั้งสุดท้าย
                                </td>
                                 <td>
                                    <asp:TextBox ID="txtLastRecDate" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView runat="server" ID="gvCloseRec" 
                            AutoGenerateColumns="false" Width="100%" 
                            OnRowDataBound="gvCloseRec_RowDataBound" 
                        >
                            <Columns>
                                <asp:BoundField DataField="Inv_ItemCode" HeaderText="รหัสสินค้า" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100"/>
                                <asp:BoundField DataField="Item_Search_Desc" HeaderText="ชื่อสินค้า"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" />
                                <asp:BoundField DataField="Pack_Description" HeaderText="หน่วยนับ" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100"/>
                                <asp:BoundField DataField="Unit_Quantity" DataFormatString="{0:N0}" 
                                    HeaderText="จำนวนที่สั่ง" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="Unit_Quantity">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Receive_Quantity" HeaderText="จำนวนรับ"  DataFormatString="{0:N0}" 
                                    ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Remain_Quantity" HeaderText="จำนวนค้างรับ"  DataFormatString="{0:N0}" 
                                    ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Net_Amount" DataFormatString="{0:N0}" 
                                    HeaderText="จำนวนเงินที่วางบิล" ItemStyle-HorizontalAlign="Right" 
                                    SortExpression="Unit_Quantity">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" HeaderText="ปิดการรับ">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkClose" runat="server" />
                                        <asp:HiddenField ID="hdRemarkClose" runat="server"/>
                                        <asp:HiddenField ID="hdPOItem_ID" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" HeaderText="หมายเหตุ">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="CommentClose" ImageUrl="~/images/application-view-list.png"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ผู้บันทึก/วันที่เวลา" 
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100"/>

                            </Columns>
                        </asp:GridView>
                        <uc1:PagingControl ID="PagingControl1" runat="server" />

                        <br />
                         <asp:Button ID="btnSave" runat="server" Text="บันทึก" onclick="btnSave_Click" />
                         &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                            OnClientClick="window.close();return false;"/>
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
