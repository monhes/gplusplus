<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True"
    CodeBehind="SetStock.aspx.cs" Inherits="GPlus.Stock.SetStock" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register src="../UserControls/ItemControl2.ascx" tagname="ItemControl2" tagprefix="uc3" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 128px;
        }
        .style4
        {
            width: 130px;
            height: 19px;
        }
        .style5
        {
            height: 19px;
        }
        .style6
        {
            width: 103px;
            height: 19px;
        }
        .style7
        {
            width: 132px;
        }
        .style8
        {
            height: 19px;
            width: 132px;
        }
        .style9
        {
            width: 103px;
        }
    </style>
       <script type="text/javascript">
           function isNumericKey(e) {
               var charInp = window.event.keyCode;

               if (charInp > 31 && (charInp < 48 || charInp > 57)) {
                   return false;
               }
               return true;
           }
    </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ข้อมูล Set Stock และ ตรวจนับ</td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%" cellspacing="10">
                    <tr>
                        <td style="width: 130px;" align="right">
                           ชื่อคลัง : 
                        </td>
                        <td colspan="3" align="left">
                             <asp:DropDownList ID="ddlStock" runat="server" Width="295px" 
                                 DataTextField="StockType_Name" DataValueField="StockType_ID" Height="23px">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="4" align="left" style=" padding-left:35px">
                            <asp:RadioButtonList ID="rdbMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbMode_SelectedIndexChanged">
                                <asp:ListItem Text="ตรวจนับผ่านทาง Web" Value="W" Selected="True"></asp:ListItem> 
                                <asp:ListItem Text="ตรวจนับผ่านทาง Mobile" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Set Stock" Value="SS"></asp:ListItem>
                                <asp:ListItem Text="ทั้งหมด" Value="All"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                           เลขที่ใบตรวจนับ : 
                        </td>
                        <td colspan="3" align="left">
                           <asp:TextBox ID="txtTransactionNOsearch" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                        </td>

                    </tr>

                    <tr>
                        <td style="width: 130px;" align="right">
                           วันที่บันทึกตั้งแต่ : 
                        </td>
                        <td class="style3">
                           <uc2:calendarcontrol ID="dtCreateStart" runat="server" />
                        </td>
                        <td style="width: 60px;" align="right">
                            ถึงวันที่ : 
                        </td>
                        <td>
                            <uc2:calendarcontrol ID="dtCreateStop" runat="server" />
                        </td>
                    </tr>
                    <tr>
                    <td style="width: 130px;" align="right">
                        ประเภทการตรวจนับ :
                    </td>
                    <td colspan="3">
                               <asp:CheckBox ID="chkCntYear" runat="server" Text="ประจำปี" />&nbsp;&nbsp;&nbsp;
                               <asp:CheckBox ID="chkCntMonth" runat="server" Text="ประจำเดือน" />&nbsp;&nbsp;&nbsp;
                               <asp:CheckBox ID="chkCntDay" runat="server" Text="ประจำวัน" />&nbsp;&nbsp;&nbsp;
                    </td>
                    </tr>

                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>

                </table>


                <asp:GridView ID="gvSetStock" runat="server" AutoGenerateColumns="false" Width="100%" AllowSorting="true" OnRowDataBound="gvSetStock_RowDataBound"  onsorting="gvSetStock_Sorting"  OnRowCommand="gvSetStock_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="ลำดับที่"  DataField="Row" SortExpression="Row"
                            ItemStyle-HorizontalAlign="Center" />--%>
                        <asp:BoundField HeaderText="เลขที่บันทึก" DataField="Transaction_No" SortExpression="Transaction_No"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่ตรวจนับ" DataField="Transaction_Date" SortExpression="Transaction_Date"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="คลังสินค้า" DataField="Stock_Name" SortExpression="Stock_Name" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ประเภทการตรวจนับ" DataField="Transaction_Sub_Other" SortExpression="Transaction_Sub_Other"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้ตรวจนับ" DataField="TransBy_Name" SortExpression="TransBy_Name" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField="Div_Name" SortExpression="Div_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField HeaderText="ทีม" DataField="Dep_Name" SortExpression="Dep_Name"
                            ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField HeaderText="สถานะ" DataField="" SortExpression=""
                            ItemStyle-HorizontalAlign="Left"/>
                        <%--<asp:TemplateField HeaderText="พิมพ์" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ToolTip="พิมพ์" ImageUrl="~/images/Commands/print.png" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
                <uc1:pagingcontrol ID="PagingControl1" runat="server" />


            </td>
        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
    </table>

    <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                   SkinID="ButtonMiddle" OnClick="btnAdd_Click" /> 
    
    <br />
    
    <br />
    
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
<%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียดการตรวจนับ Stock
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:HiddenField ID="hdTransHead_ID" runat="server" />
                    <table width="100%">
                        <tr>
                        <td style="width: 130px;" align="right">
                              เลขที่บันทึก 
                        </td>
                        <td class="style7">
                             <asp:TextBox ID="txtTransactionNO" runat="server" MaxLength="100" Width="190" Enabled="false"></asp:TextBox>
                        </td>
                        <td align="right" class="style9">
                              วันที่ตรวจนับ 
                        </td>
                        <td>
                             <uc2:calendarcontrol ID="dtTransDate" runat="server" />
                        </td>
                        </tr>
                        <tr>
                        <td style="width: 130px;" align="right">
                              ประเภทการตรวจนับ 
                        </td>
                        <td colspan="2">
                            <fieldset style="width:310px">
                                <%--<legend></legend>--%>
                                 <asp:RadioButtonList ID="rdbCntStockType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="ประจำปี" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="ประจำเดือน"></asp:ListItem>
                                    <asp:ListItem Text="ประจำวัน"></asp:ListItem>
                                    <asp:ListItem Text="Set Stock"></asp:ListItem>
                                </asp:RadioButtonList>
                            </fieldset>
                        </td>
                        <td>
                            <asp:FileUpload ID="FileImport" runat="server" />
                             <asp:Button ID="btnImport" runat="server" Text="Import" onclick="btnImport_Click" />
                        </td>
                        </tr>
                        
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อผู้ตรวจนับ
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TxtTransBy" runat="server" MaxLength="100" Width="190" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        <td style="width: 130px;" align="right">
                           ฝ่าย : 
                        </td>
                        <td>
                             <asp:DropDownList ID="drpDivision" runat="server" Width="195px" 
                                 DataTextField="Division_Name" DataValueField="Division_ID" Height="23px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                           ทีมงาน : 
                        </td>
                        <td>
                             <asp:DropDownList ID="drpDepartment" runat="server" Width="195px" 
                                 DataTextField="Department_Name" DataValueField="Department_ID" Height="23px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <div id = "showPartDiff" runat="server">
                        <td align="right">
                            รูปแบบการแสดง : 
                        </td>
                        <td colspan = "2">
                            <asp:RadioButtonList ID="RdbShowDiff" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="RdbShowDiff_SelectedIndexChanged">
                                <asp:ListItem Text="แสดงเฉพาะส่วนต่าง"></asp:ListItem>
                                <asp:ListItem Text="แสดงทั้งหมด" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                       </td>
                       </div>
                    </tr>
                    <tr>
                        <td style=" padding-top:10px; display: none; ">
                            <asp:Button ID="btnAddMoreSetStk" runat="server" Text="เพิ่มรายการ Set Stock"
                                SkinID="ButtonMiddleLong" OnClick="btnAddMoreSetStk_Click" Visible="false" />
                        </td>
                    </tr>
                    

                        
                    </table>
                   
                    <br />
                    <br />

                 <asp:LinkButton ID="btnRefreshItem" runat="server" Text="Refresh" CausesValidation="false"
                                    OnClick="btnRefreshItem_Click" Style="display: none;"></asp:LinkButton>

                <asp:LinkButton ID="btnAdjustRefreshItem" runat="server" Text="Refresh" CausesValidation="false"
                                    OnClick="btnAdjustRefreshItem_Click" Style="display: none;"></asp:LinkButton>
                

                <div id = "Show_Gv_SumExcel" visible = "false" runat="server">
                    <center>
                    <asp:GridView ID="Gv_SumExcel" runat="server" AutoGenerateColumns="False" 
                            Width="100%" AllowSorting="True" onsorting="Gv_SumExcel_Sorting"  >
                    <Columns>
                       <%-- <asp:BoundField HeaderText="ลำดับที่" 
                            ItemStyle-HorizontalAlign="Center" />--%>
                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="Inv_ItemCode" SortExpression="Inv_ItemCode"
                            ItemStyle-HorizontalAlign="Center" >
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ขื่อสินค้า" DataField="Item_Search_Desc" SortExpression="Item_Search_Desc"
                            ItemStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="หน่วย" DataField="Pack_Description" SortExpression="Pack_Description"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" 
                            DataFormatString="{0:d}" >
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="จำนวนนับ" DataField="Summary_Qty" SortExpression="Summary_Qty"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0}" >
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="มูลค่า" DataField="Summary_Cost" SortExpression="Summary_Cost" 
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:###,##0.00}" >
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="เหตุผล" DataField="" SortExpression=""
                            ItemStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                    </Columns>
<%--                 <EmptyDataTemplate>
                    </EmptyDataTemplate>--%>
                </asp:GridView>
                <uc1:pagingcontrol ID="PagingControl2" runat="server" />

          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <asp:GridView ID="gvAdjust_Stock" runat="server" 
                CellSpacing="0" CellPadding="0"
                AutoGenerateColumns="false" EnableModelValidation="True" Width="100%" 
                GridLines="None" onrowdatabound="gvAdjust_Stock_RowDataBound">
                <Columns>
                    <%--<asp:TemplateField HeaderText="ลำดับที่">
                        <ItemTemplate>
                            <asp:Label ID="lbl_no" runat="server" Text='<%# ( Container.DataItemIndex + 1) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>--%>
                   <%-- <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="รหัสสินค้า">
                        <ItemTemplate>
                            <asp:Label ID="lbl_item_code" runat="server" Text='<%# Eval("Inv_ItemCode") %>'></asp:Label>
                            <asp:HiddenField ID="hd_ItemId" value='<%# Eval("Inv_ItemID") %>' runat="server" />
                            <asp:HiddenField ID="hd_PackId" value='<%# Eval("Pack_ID") %>' runat="server" />
                            <asp:HiddenField ID="hd_AvgCost" value='<%# Eval("Avg_Cost") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ชื่อสินค้า">
                        <ItemTemplate>
                            <asp:Label ID="lbl_item_name" runat="server" Text='<%# Eval("Item_Search_Desc") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                    </asp:TemplateField>
                                                    
                    <asp:TemplateField HeaderText="หน่วย">
                        <ItemTemplate>
                            <asp:Label ID="lbl_pack_name" runat="server" Text='<%# Eval("Pack_Description") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="จำนวนตามระบบ">
                        <ItemTemplate>
                            <asp:Label ID="lbl_onHand_Qty" runat="server" Text='<%# Eval("OnHand_Qty","{0:###,##0}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Right" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="จำนวนนับ">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_cnt" runat="server" AutoPostBack="true" onkeypress="return isNumericKey(event);"   Width="50px" Text='<%# Eval("Cnt_Qty") %>' OnTextChanged="txt_cnt_TextChanged"></asp:TextBox>
                            <%--<asp:Label ID="lbl_onHand_cnt" runat="server" Text='<%# Eval("Cnt_Qty","{0:###,##0}")%>'></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ส่วนต่าง" ControlStyle-ForeColor="Red">
                        <ItemTemplate>
                            <%--<asp:TextBox ID="txt_diif" runat="server" AutoPostBack="true" onkeypress="return isNumericKey(event);"  Width="50px" Text='<%# Eval("Diff") %>' Enabled="false"></asp:TextBox>--%>
                            <asp:Label ID="lbl_onHand_diff" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Right"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Adjust">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_Adjust" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="มูลค่า">
                        <ItemTemplate>
                            <%--<asp:Label ID="lbl_summary" AutoPostBack="false" runat="server" Text='<%# Eval("Qty_Amount","{0:###,##0.0000}")%>'></asp:Label>--%>
                            <asp:Label ID="lbl_summary" AutoPostBack="false" runat="server"></asp:Label>
                        </ItemTemplate >
                        <ItemStyle Width="80px" HorizontalAlign="Right"  />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="เหตุผล">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_Reason" runat="server" AutoPostBack="false"  Width="100px" Text='<%# Eval("Reason") %>'></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>  
                </Columns>
            </asp:GridView>
                <uc1:pagingcontrol ID="PagingControl3" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
                </center>
                <center>
                        <table width="750">
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่สร้าง
                            </td>
                            <td style="width: 130px;" align="left">
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่สร้าง
                            </td>
                            <td style="width: 130px;" align="left">
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td style="width: 130px;" align="left">
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่แก้ไขล่าสุด
                            </td>
                            <td style="width: 130px;" align="left">
                                <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        </table>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                 </center>
                </div>
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