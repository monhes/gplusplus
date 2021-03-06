﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestReportPay.aspx.cs" Inherits="GPlus.PRPO.RequestReportPay" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl2.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
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
                <table width="1002" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 201px;" align="left" valign="middle">
                            <img src="../images/logo_Muangthai.png" />
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td style="width: 799px;" align="right" valign="middle">
                            <img src="../images/logo_gplus.png" />
                        </td>
                    </tr>
                    <%--<tr>
                <td></td>
                <td colspan="2" align="right" style="font-size:11pt;">
                    ยินดีต้อนรับ คุณ <asp:Label ID="lblUser" runat="server"></asp:Label>
                    &nbsp;<asp:HyperLink ID="hplLogout" runat="server" Text="ออกจากระบบ" NavigateUrl="~/Default.aspx"></asp:HyperLink>
                </td>
            </tr>--%>
                    <tr align="left" valign="top">
                        <td colspan="3" align="center">
                            <asp:GridView SkinID="Grid2" ID="gv_StockPay" runat="server" CellSpacing="0"
                                    CellPadding="0" AutoGenerateColumns="false" EnableModelValidation="True" Width="600px"
                                    GridLines="None" OnRowCommand="gv_StockPay_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="รายละเอียด">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Detail"
                                                CommandArgument='<%# string.Format("{0}|{1}|{2}", Eval("Pay_Id"), Eval("Pay_Date"), Eval("status")) %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ลำดับที่">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex + 1) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="วันที่จ่าย">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Pay_Date" runat="server" Text='<%# Eval("Pay_Date","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"  Width="80px"/> 
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ชื่อผู้จ่าย">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Pay_By" runat="server" Text='<%# Eval("Pay_By") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ผู้รับ">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Div" runat="server" Text='<%# (Eval("Receive_By")==System.DBNull.Value)?"-":Eval("Receive_By") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="150px" HorizontalAlign="Left"  />
                                    </asp:TemplateField>
                                    
                                    <%--<asp:TemplateField HeaderText="สถานะ">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_status" runat="server" Text='<%# (Eval("status").ToString()=="1")?"รับแล้ว":Eval("status").ToString()=="0"?"จ่ายแล้ว":Eval("status").ToString()=="2"?"ยกเลิกจ่าย":"" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="พิมพ์<br>ใบเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png"
                                                 CommandName="Print" CommandArgument='<%# Eval("Pay_Id") %>' CausesValidation="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:Panel ID="Control_Panel_StockPayItem" runat="server" Visible="false">
                                <br />
                                <br />
                                <div style="width:700px; background-color: #EC467E; height: 20px; text-align: center;">
                                    <table width="100%" border="0" style="text-align: center; font-family: Tahoma, Geneva, sans-serif;">
                                        <tr style="text-align: center; color: #FFFFFF">
                                            <td colspan="4">
                                                <b>รายการจ่ายวันที่ &nbsp;
                                                    <asp:Label ID="lbl_date" runat="server" Text="Label"></asp:Label>
                                                   &nbsp;เวลา&nbsp;  
                                                    <asp:Label ID="lbl_time" runat="server" Text="Label"></asp:Label>&nbsp; น.
                                                </b>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                
                                <asp:GridView SkinID="Grid2" ID="gv_StockPayItem" runat="server" CellSpacing="0"
                                    CellPadding="0" AutoGenerateColumns="false" EnableModelValidation="True" Width="700px"
                                    GridLines="None" OnRowCreated="gv_StockPayItem_RowCreated" OnRowDataBound = "gv_StockPayItem_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ลำดับที่">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex + 1) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="รหัสสินค้า">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_item_no" runat="server" Text='<%# Eval("Inv_ItemCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ชื่อสินค้า">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_item_name" runat="server" Text='<%# Eval("Item_Search_Desc") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="170px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="หน่วย">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_pack_name" runat="server" Text='<%# Eval("Pack_Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ราคา<br>ต่อหน่วย">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_unit_price" runat="server" Text='<%# Eval("Unit_Price","{0:###,###,###.00}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เบิก">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Order_Qty" runat="server" Text='<%# Eval("Order_Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จ่ายสะสม">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Pay_Qty" runat="server" Text='<%# Eval("Pay_Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="คงค้าง">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Remain_Qty" runat="server" Text='<%# Eval("Remain_Qty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จำนวนจ่าย<br>ครั้งนี้">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Qty" runat="server" Text='<%# Eval("Pay_Quantity") %>'></asp:Label>
                                                <asp:HiddenField ID="hdRemarkOrg" runat="server"/>
                                                <asp:HiddenField ID="hdRemarkStock" runat="server"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" HeaderText="หมายเหตุคลัง" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="" HeaderText="หมายเหตุหน่วยงาน" ItemStyle-HorizontalAlign="Center" />                  
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table width="100%">
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Button ID="btn_Close" runat="server" Text="ปิด" SkinID="ButtonMiddle" 
                                            onclick="btn_Close_Click"  />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hid_Request_Id" runat="server" />
                            <asp:HiddenField ID="hid_Pay_Id" runat="server" />
                            <asp:HiddenField ID="hid_Stock_Id_Pay" runat="server" />
                            <asp:HiddenField ID="hid_Summary_ReqId" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    </form>
</body>
</html>
