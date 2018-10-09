<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDistribute.aspx.cs" Inherits="GPlus.Stock.frmDistribute" MasterPageFile="../MasterPage/Main.Master"%>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="../UserControls/CalendarControl.ascx" tagname="CalendarControl" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
        <style type="text/css">
            .hiddencol
            {
                display: none;
            }
        </style>
     <script language="javascript" type="text/javascript">
         function ConfrimationSave(message) {
             if (confirm(message) == true) {
                 __doPostBack('<%= this.ClientID %>', "true");
                 return true;
             }
             return false;
         }
         function ConfrimationCancel() {
             if (confirm('คุณต้องการยกเลิกตารางจ่ายนี้หรือไม่?') == true) {
                 return true;
             }
             return false;
         }
         function OnLimitDateSelectionChange() {
             var datepicker = null;
             var isNolimitChecked = document.getElementById('<%= this.rdNoLimitToDate.ClientID %>').checked;
             var isLimitChecked = document.getElementById('<%= this.rdLimitToDate.ClientID %>').checked;
             if (isNolimitChecked) {
                 datepicker = document.getElementById('<%= this.ccLimitDate.ClientID %>');
                 datepicker.disabled = true;
             }
             else if (isLimitChecked) {
                 datepicker = document.getElementById('<%= this.ccLimitDate.ClientID %>');
                 datepicker.disabled = false;
             }
         }
     </script>
     <table cellpadding="0" cellspacing="0" width="805">
         <tr>
            <td class="tableHeader">ตารางการจ่ายสินค้าจากคลังใหญ่</td>
         </tr>
         <tr>
             <td class="tableBody">
                 <table cellpadding="5" cellspacing="0" width="100%">
                     <tr>
                         <td colspan="2">
                             <fieldset>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width:160px" align="right">ตารางการจ่ายช่วงวันที่</td>
                                        <td>
                                            <table cellpadding="0" cellspacing="4">
                                                <tr>
                                                    <td><uc3:CalendarControl ID="ccSearchFrom" runat="server" /></td>
                                                    <td>-</td>
                                                    <td><uc3:CalendarControl ID="ccSearchTo" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>คลัง</td>
                                        <td style="width:150px" align="left">
                                            <asp:DropDownList runat="server" ID="ddStock" Width="145px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:160px" align="right">สถานะ</td>
                                        <td>
                                            <table cellpadding="0" cellspacing="4">
                                                <tr>
                                                    <td style="width:105px"><asp:DropDownList runat="server" ID="ddSearchDSStatus" Width="100px" /></td>
                                                    <td><asp:CheckBox runat="server" ID="chkOrgDistrubution" Text="แจงหน่วยงาน" Enabled="false" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" OnClick="BtnSearchClick" />&nbsp;
                                            <asp:Button ID="btnSeachCancel" runat="server" Text="ยกเลิก" OnClick="BtnSeachCancelClick" />
                                        </td>
                                    </tr>
                                </table>
                             </fieldset>
                         </td>
                     </tr>
                     <tr>
                         <td colspan="2">
                             <asp:Button ID="btnCreate" SkinID="ButtonMiddleLong" runat="server" Text="สร้างตารางการจ่าย" OnClick="BtnCreateClick" />
                         </td>
                     </tr>
                     <tr>
                         <td colspan="2">
                             <asp:HiddenField runat="server" ID="hfScheduleId" />
                             <asp:GridView runat="server" ID="gvSchedule" AutoGenerateColumns="false" OnRowDataBound="GvScheduleRowDataBound" OnRowCommand="GvScheduleRowCommand" Width="100%">
                                 <Columns>
                                     <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                         <ItemTemplate>
                                             <asp:LinkButton ID="btnSelectSchedule" runat="server" CommandName="Select" Text="รายละเอียด" Visible="false" />
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:BoundField HeaderText="ลำดับ" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" />
                                     <asp:BoundField HeaderText="วันที่เริ่มต้น-วันที่สิ้นสุด" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="160px"/>
                                     <asp:BoundField HeaderText="หน่วยงาน/คลัง" ItemStyle-HorizontalAlign="left" />
                                     <%--<asp:BoundField HeaderText="คลัง" DataField="Stock_Name" ItemStyle-HorizontalAlign="Center" />--%>
                                     <asp:BoundField HeaderText="วันที่สร้าง" DataField="Create_Date" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Center"/>
                                     <asp:BoundField HeaderText="สถานะ" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"/>
                                 </Columns>
                             </asp:GridView>
                         </td>
                     </tr>
                     <tr>
                         <td style="width:285px" align="left">
                             <asp:Label runat="server" ID="lbScheduleDayText" BackColor="#99ccff" Height="16px" Width="280px" Visible="false" />
                         </td>
                         <td align="left">
                             <asp:Button runat="server" SkinID="ButtonMiddleLongLong" Text="เพิ่มวันจ่ายตารางการจ่ายช่วงนี้" ID="btnAddScheduleDay" Visible="false" OnClick="BtnAddScheduleDayClick" />
                         </td>
                     </tr>
                     <tr>
                         <td colspan="2">
                             <asp:HiddenField ID="hffDayNumber" runat="server" />
                             <asp:GridView runat="server" ID="gvScheduleWithBuilding" AutoGenerateColumns="false" 
                                    OnRowDataBound="GvScheduleWithBuildingRowDataBound" 
                                    OnRowCommand="GvScheduleWithBuildingRowCommand" Width="100%">
                                 <Columns>
                                     <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                         <ItemTemplate>
                                             <asp:LinkButton ID="btnSelectScheduleDetail" runat="server" CommandName="Select" Text="รายละเอียด" Visible="false" />
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:BoundField HeaderText="ลำดับ" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" />
                                     <asp:BoundField HeaderText="วัน" ItemStyle-HorizontalAlign="Center"/>
                                     <asp:BoundField HeaderText="หน่วยงาน/คลัง" ItemStyle-HorizontalAlign="left" />
                                     <%--<asp:BoundField HeaderText="คลัง" DataField="Stock_Name" ItemStyle-HorizontalAlign="Center" />--%>
                                     <asp:BoundField HeaderText="ตึก" DataField="Building_Name" ItemStyle-HorizontalAlign="Center"/>
                                     <asp:BoundField HeaderText="ชั้น" DataField="Building_FloorId" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"/>
                                 </Columns>
                             </asp:GridView>
                         </td>
                     </tr>
                     <tr>
                         <td colspan="2">
                             
                             <asp:Panel runat="server" ID="pnDSDetail" Visible="false">
                                 <fieldset>
                                     <legend>รายละเอียดตารางการจ่าย</legend>
                                     <table cellpadding="0" cellspacing="0" width="100%">
                                         <tr>
                                             <td style="width:160px" align="right">ตารางการจ่ายช่วงวันที่</td>
                                             <td>
                                                 <table cellpadding="0" cellspacing="4">
                                                     <tr>
                                                         <td>
                                                            <uc3:CalendarControl ID="ccDSFrom" runat="server" />
                                                         </td>
                                                         <td>ถึงวันที่</td>
                                                         <td>
                                                             <fieldset>
                                                                 <table cellpadding="0" cellspacing="0">
                                                                     <tr>
                                                                         <td >
                                                                            <asp:RadioButton runat="server" ID="rdNoLimitToDate" GroupName="gToDate" Text="ไม่มีกำหนดวันสิ้นสุด" />&nbsp;
                                                                         </td>
                                                                         <td>
                                                                            <asp:RadioButton runat="server" ID="rdLimitToDate" GroupName="gToDate" Text="ระบุวันที่สิ้นสุด" />&nbsp;&nbsp;
                                                                         </td>
                                                                         <td>
                                                                             <uc3:CalendarControl runat="server" ID="ccLimitDate"  />
                                                                         </td>
                                                                     </tr>
                                                                 </table>
                                                             </fieldset>
                                                         </td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                            
                                             <td style="width:160px" align="right">ระบุวันที่ทำการจ่าย</td>
                                             <td>
                                                <a name="mark"></a>
                                                 <fieldset>
                                                    <asp:RadioButtonList ID="rdDateOfWeek" runat="server" RepeatDirection="Horizontal" Width="100%">
                                                         <asp:ListItem Text="วันจันทร์" Value="1" />
                                                         <asp:ListItem Text="วันอังคาร" Value="2" />
                                                         <asp:ListItem Text="วันพุธ" Value="3" />
                                                         <asp:ListItem Text="วันพฤหัสบดี" Value="4" />
                                                         <asp:ListItem Text="วันศุกร์" Value="5" />
                                                         <asp:ListItem Text="วันเสาร์" Value="6" />
                                                         <asp:ListItem Text="วันอาทิตย์" Value="0" />
                                                    </asp:RadioButtonList>
                                                 </fieldset>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2"> &nbsp;
                                                 &nbsp;&nbsp;&nbsp;
                                                 <table cellpadding="0" cellspacing="0" width="100%">
                                                     <tr>
                                                         <td style="width:50px"></td>
                                                         <td>
                                                             <fieldset>
                                                                 <legend>สำนักงานใหญ่</legend>
                                                                 <table>
                                                                     <tr>
                                                                         <td style="width:100px" align="right">ตึก</td>
                                                                         <td style="width:155px">
                                                                             <asp:DropDownList ID="ddBuilding" runat="server" Width="150" OnSelectedIndexChanged="DdBuildingSelectedIndexChanged" AutoPostBack="true" />
                                                                         </td>
                                                                         <td>ชั้น</td>
                                                                          <td style="width:55px">
                                                                             <asp:DropDownList ID="ddBuildinfFloor" runat="server" Width="150" />
                                                                         </td>
                                                                     </tr>
                                                                     <tr>
                                                                        <td style="width:100px" align="right">รหัสฝ่าย</td>
                                                                        <td><asp:TextBox runat="server" ID="tbDivCode"></asp:TextBox></td>
                                                                        <td>รหัสทีม</td>
                                                                        <td><asp:TextBox runat="server" ID="tbDepCode"></asp:TextBox></td>
                                                                     </tr>
                                                                 </table>
                                                             </fieldset>
                                                         </td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">
                                                 &nbsp;
                                                 <table cellpadding="0" cellspacing="0" width="100%">
                                                     <tr>
                                                         <td style="width:50px"></td>
                                                         <td style="width:150px">
                                                              <fieldset>
                                                                 <asp:RadioButtonList RepeatDirection="Horizontal" ID="rdSearchNameType" runat="server">
                                                                     <asp:ListItem Text="หน่วยงาน" Value="1" />
                                                                     <asp:ListItem Text="คลัง" Value="2" />
                                                                 </asp:RadioButtonList>
                                                             </fieldset>
                                                         </td>
                                                         <td style="width:30px" align="center">
                                                             ชื่อ
                                                         </td>
                                                         <td style="width:155px" align="left">
                                                             <asp:TextBox ID="tbSearchName" runat="server" Width="150px" />
                                                         </td>
                                                         <td align="left">
                                                            
                                                             <asp:Button runat="server" ID="btnSearchDS" Text="ค้นหา" OnClick="BtnSearchDSClick" />
            
                                                         </td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">&nbsp;
                                                 <asp:Panel ID="pnDaySelector" runat="server" Visible="false">
                                                     <table cellpadding="0" cellspacing="2" width="100%">
                                                         <tr>
                                                             <td style="width:50px" />
                                                             <td  style="width:255px"><asp:Label ID="lb1" runat="server" BackColor="#99ccff" Text="แสดงข้อมูลหน่วยงาน" Width="250px" Height="20px" /></td>
                                                             <td style="width:30px">

                                                             </td>
                                                             <td><asp:Label ID="lbDisplayDayOfWeek" runat="server" BackColor="#99ccff" Text="เลือกวัน" Width="100%" Height="20px"/></td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width:50px" />
                                                             <td valign="top">
                                                                 <asp:GridView ID="gvOrg" AutoGenerateColumns="false" runat="server" 
                                                                     ShowHeaderWhenEmpty="true" Width="100%">
                                                                     <Columns>
                                                                         <asp:BoundField DataField="OrgStruc_Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                         <asp:BoundField HeaderText="รหัสฝ่าย" DataField="Div_Code" />
                                                                         <asp:BoundField HeaderText="รหัสทีม" DataField="Dep_Code" />
                                                                         <asp:TemplateField HeaderStyle-Width="15px" >
                                                                             <ItemTemplate>
                                                                                 <asp:CheckBox ID="chkSelectOrg" runat="server" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                         <asp:BoundField  HeaderText="หน่วยงาน" DataField="Description"/>
                                                                     </Columns>
                                                                 </asp:GridView>
                                                             </td>
                                                             <td style="width:35px" rowspan="2" align="center" valign="top">
                                                                 <asp:Button ID="btnIn" SkinID="ButtonSmall" runat="server" Text=">" OnClick="BtnInClick" />
                                                                 <asp:Button ID="btnOut" SkinID="ButtonSmall" runat="server" Text="<" OnClick="BtnOutClick" />
                                                             </td>
                                                             <td rowspan="2" valign="top">
                                                                 <asp:GridView ID="gvResult" AutoGenerateColumns="false" runat="server" ShowHeaderWhenEmpty="true" Width="100%" OnRowDataBound="GvResultRowDataBound">
                                                                     <Columns>
                                                                         <asp:BoundField DataField="Stock_ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                         <asp:BoundField DataField="OrgStruc_Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                         <asp:BoundField HeaderText="รหัสฝ่าย" DataField="Div_Code" />
                                                                         <asp:BoundField HeaderText="รหัสทีม" DataField="Dep_Code" />
                                                                         <asp:TemplateField HeaderStyle-Width="15px" >
                                                                             <ItemTemplate>
                                                                                 <asp:CheckBox ID="chkSelect" runat="server" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                         <asp:BoundField DataField="Stock_ID" Visible="false" />
                                                                         <asp:BoundField DataField="OrgStruc_Id" Visible="false" />
                                                                         <asp:BoundField HeaderText="หน่วยงาน" DataField="Org_Desc" />
                                                                         <asp:BoundField HeaderText="คลัง" DataField="Stock_Name" />
                                                                         <asp:TemplateField HeaderStyle-Width="100px" HeaderText="ความถี่ในกาจ่าย" >
                                                                             <ItemTemplate>
                                                                                 <asp:DropDownList runat="server" ID="ddFreq" Width="100%" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>

                                                                       
                                                                     </Columns>
                                                                 </asp:GridView>
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width:50px" />
                                                             <td valign="top">
                                                                 <asp:GridView ID="gvStock" AutoGenerateColumns="false" runat="server" ShowHeaderWhenEmpty="true" Width="100%" >
                                                                     <Columns>
                                                                         <asp:BoundField DataField="Stock_ID" ItemStyle-CssClass="hiddenField" HeaderStyle-CssClass="hiddenField"/>
                                                                         <asp:TemplateField HeaderStyle-Width="15px" >
                                                                             <ItemTemplate>
                                                                                 <asp:CheckBox ID="chkSelectStock" runat="server" />
                                                                             </ItemTemplate>
                                                                         </asp:TemplateField>
                                                                         <asp:BoundField HeaderText="คลัง" DataField="Stock_Name" />
                                                                     </Columns>
                                                                 </asp:GridView>
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td colspan="4" align="center">
                                                                 <table cellpadding="0" cellspacing="2" width="80%">
                                                                     <tr>
                                                                         <td style="width:100px" align="right">สถานะการใช้งาน</td>
                                                                         <td colspan="4">
                                                                             <asp:RadioButtonList ID="rdScheduleDayStatus" runat="server" RepeatDirection="Horizontal">
                                                                                 <asp:ListItem Text="ใช้งาน" Value="1" />
                                                                                 <asp:ListItem Text="ไม่ใช้งาน" Value="0" />
                                                                             </asp:RadioButtonList>
                                                                         </td>
                                                                     </tr>
                                                                     <tr>
                                                                         <td style="width:100px" align="right">วันที่สร้าง</td>
                                                                         <td style="width:155px" align="left">
                                                                             <asp:TextBox ID="tbCreateDate" runat="server" Width="150px" Enabled="false" />
                                                                         </td>
                                                                         <td style="width:60px" align="center">ผู้ที่สร้าง</td>
                                                                         <td align="left">
                                                                             <asp:TextBox ID="tbCreateBy" runat="server" Width="150px" Enabled="false" />
                                                                         </td>
                                                                     </tr>
                                                                     <tr>
                                                                         <td style="width:100px" align="right">วันที่แก้ไข</td>
                                                                         <td style="width:155px" align="left">
                                                                             <asp:TextBox ID="tbUpdateDate" runat="server" Width="150px" Enabled="false" />
                                                                         </td>
                                                                         <td style="width:60px" align="center">ผู้ที่แก้ไข</td>
                                                                         <td align="left">
                                                                             <asp:TextBox ID="tbUpdateBy" runat="server" Width="150px" Enabled="false" />
                                                                         </td>
                                                                     </tr>
                                                                 </table>
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td colspan="4" align="center">
                                                                 <asp:Button ID="btnSave" SkinID="ButtonMiddle" runat="server" Text="บันทึก" OnClick="BtnSaveClick" />&nbsp;
                                                                 <asp:Button ID="btnDelete" SkinID="ButtonMiddle" runat="server" Text="ลบตารางจ่าย" OnClick="BtnDeleteClick" OnClientClick="return ConfrimationCancel();" />&nbsp;
                                                                 <asp:Button ID="btnCancel" SkinID="ButtonMiddle" runat="server" Text="ยกเลิก" OnClick="BtnCancelClick" />
                                                             </td>
                                                         </tr>
                                                     </table>
                                                 </asp:Panel>
                                             </td>
                                         </tr>
                                     </table>
                                 </fieldset>
                                 </table>
                             </asp:Panel>
                         </td>
                     </tr>
                 </table>
             </td>
         </tr>
         <tr>
             <td class="tableFooter" />
         </tr>
    </table>
</asp:Content>