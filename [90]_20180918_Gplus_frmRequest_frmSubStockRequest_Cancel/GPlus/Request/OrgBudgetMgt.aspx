<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="OrgBudgetMgt.aspx.cs" Inherits="GPlus.Request.OrgBudgetMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/ItemOrgStructControl2.ascx" TagName="ItemOrgStructControl2" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            border:1px solid #C0C0C0;
        }
        .style2
        {
            border:1px solid #C0C0C0;
            color : #416285;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
            ค้นหาข้อมูลงบประมาณการจ่ายของหน่วยงาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                 <table width="100%">
                     <tr>
                        <td style="width:55px;">
                        </td>
                        <td colspan="2" align = "left">
                            ปีพุทธศักราช 
                            &nbsp;&nbsp;
                        <%--</td>
                        <td align = "left">--%>
                            <asp:TextBox runat="server" ID="tbYear" Width="145px" 
                                style="text-align:right;" 
                                onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)" 
                                onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" 
                                onpaste="return CancelKeyPaste(this)" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <uc2:ItemOrgStructControl2 ID="orgCtrl" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                 </table>
                 <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvBudget" class="GridHeader2" runat="server" 
                    AutoGenerateColumns="False" Width="100%"
                     OnRowDataBound="gvBudget_RowDataBound" OnRowCommand="gvBudget_RowCommand"
                    OnRowCreated="gvBudget_RowCreated" SkinId="GvLong">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ปีพุทธศักราช" DataField="Budget_Year" SortExpression="Budget_Year"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField="DivName" SortExpression="DivName"
                         ItemStyle-Width="70"  ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ทีมงาน" DataField="DepName" SortExpression="DepName"
                        ItemStyle-Width="70"   ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="เครื่องเขียน" DataField="SUM_budget8" SortExpression="SUM_budget8"
                        ItemStyle-Width="90" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}"/>
                        <asp:BoundField HeaderText="แบบพิมพ์" DataField="SUM_budget9" SortExpression="SUM_budget9"
                        ItemStyle-Width="90" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,##0.00}"/>
                       <asp:BoundField HeaderText="วันที่สร้าง" SortExpression="CrtDate_Budget8" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ผู้ที่สร้าง" DataField="CrtBy_Budget8" SortExpression="CrtBy_Budget8"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="MdfDate_Budget8" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="MdfBy_Budget8" SortExpression="MdfBy_Budget8"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่สร้าง" SortExpression="CrtDate_Budget9" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ผู้ที่สร้าง" DataField="CrtBy_Budget9" SortExpression="CrtBy_Budget9"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="MdfDate_Budget9" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="MdfBy_Budget9" SortExpression="MdfBy_Budget9"
                            ItemStyle-HorizontalAlign="Left" />
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
<br />
<asp:Panel ID="pnlDetail" runat="server" Visible="false">
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
         <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table id="TableBudgetDetail" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2" align = "right">
                            ปีพุทธศักราช 
                            &nbsp;&nbsp;
                            <asp:TextBox runat="server" ID="tbYearDetail" Width="125px"  
                                style="text-align:right;"/>
                        </td>
                        <td colspan="2" align = "right">
                            ชื่อฝ่าย 
                            &nbsp;&nbsp;
                            <asp:TextBox runat="server" ID="tbDivNameDetail" Width="180px" Enabled="false"
                                style="text-align:right;"/>
                        </td>
                         <td colspan="2" align = "left">
                            ชื่อทีม 
                            &nbsp;&nbsp;
                            <asp:TextBox runat="server" ID="tbDepNameDetail" Width="180px" Enabled="false"
                                style="text-align:right;"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table style="border:1px solid #C0C0C0;border-collapse:collapse; width:80%;">
                            <tr style="background-color:#3A8FE3; color:White;">
                                <th style="border:1px solid #C0C0C0; width:30%;" rowspan="2">งบประมาณ</th>
                                <th style="border:1px solid #C0C0C0;" colspan="2">ประเภทวัสดุอุปกรณ์</th>
                            </tr>
                            <tr style="border:1px solid #C0C0C0; background-color:#3A8FE3; color:White;">
                                <td class="style1">
                                    เครื่องเขียน
                                </td>
                                <td class="style1">
                                    แบบพิมพ์
                                </td>
                            </tr>
                            <tr style="border:1px solid #C0C0C0; background-color:#F6F6F6;">
                                <td class="style2">
                                    มกราคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month1" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month1" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    กุมภาพันธ์
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month2" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1"> 
                                    <asp:TextBox ID="tbBdgCate9_Month2" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2">
                                    มีนาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month3" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month3" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    เมษายน
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month4" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month4" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2">
                                    พฤษภาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month5" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month5" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    มิถุนายน
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month6" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month6" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2">
                                    กรกฎาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month7" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month7" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    สิงหาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month8" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month8" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                             <tr style="background-color:#F6F6F6;">
                                <td class="style2">
                                    กันยายน
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month9" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month9" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    ตุลาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month10" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month10" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2">
                                    พฤษจิกายน
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month11" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month11" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#FFD6E4;">
                                <td class="style2">
                                    ธันวาคม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate8_Month12" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="tbBdgCate9_Month12" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="Calculate" AutoPostBack="true"  Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2" style=" font-weight:bold">
                                    รวม
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="Total_tbBdgCate8" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="CalculateTotal_Cate8" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="Total_tbBdgCate9" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" OnTextChanged="CalculateTotal_Cate9" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color:#F6F6F6;">
                                <td class="style2" style=" font-weight:bold">
                                    ยอดยกมาของการเบิกใช้
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="UseAmount_tbBdgCate8" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="UseAmount_tbBdgCate9" runat="server" Width="140" onKeyPress="return NumberBoxKeyPress(event, 4, 46, false)" onKeyUp="return NumberBoxKeyUp(event, 4, 46, false)" onpaste="return CancelKeyPaste(this)" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                        <table style="border-collapse:collapse; width:80%;">
                            <tr>
                                <td style="width:25%" >
                                   สถานะ
                                </td>
                                <td style="width:25%;" align="center">
                                    <asp:RadioButtonList ID="rblCate8_Status" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width:25%"  align="center">
                                   <asp:RadioButtonList ID="rblCate9_Status" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:25%">
                                   วันที่สร้าง
                                </td>
                                <td style="width:25%">
                                    <asp:Label ID="lblCrtDate_Cate8" runat="server"></asp:Label>
                                </td>
                                <td style="width:25%">
                                   <asp:Label ID="lblCrtDate_Cate9" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="width:25%">
                                   ผู้ที่สร้าง
                                </td>
                                <td style="width:25%">
                                    <asp:Label ID="lblCrtBy_Cate8" runat="server"></asp:Label>
                                </td>
                                <td style="width:25%">
                                   <asp:Label ID="lblCrtBy_Cate9" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="width:25%">
                                   วันที่แก้ไขล่าสุด
                                </td>
                                <td style="width:25%">
                                    <asp:Label ID="lblMdfDate_Cate8" runat="server"></asp:Label>
                                </td>
                                <td style="width:25%">
                                   <asp:Label ID="lblMdfDate_Cate9" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td style="width:25%">
                                   ผู้ที่แก้ไขล่าสุด
                                </td>
                                <td style="width:25%">
                                    <asp:Label ID="lblMdfBy_Cate8" runat="server"></asp:Label>
                                </td>
                                <td style="width:25%">
                                   <asp:Label ID="lblMdfBy_Cate9" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </td>
                    </tr>
                    <tr>
                          <td colspan="6" align="center">
                             <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                                &nbsp; &nbsp;
                             <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
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
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Panel>
</asp:Content>
