<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="InOutStkMgt.aspx.cs" Inherits="GPlus.MasterData.InOutStkMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาประเภทการรับเข้า - จ่ายออกกรณีอื่นๆ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 85px;" align="right">
                            รายละเอียด
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txrReason_DesSearch" runat="server" Width="200"></asp:TextBox>
                        </td>
                        <td style=" padding-left:15px;">
                            <table style="border:1px; border-style:solid; border-color:Gray; width:250px;">
                               <tr>
                                   <td>
                                    <asp:RadioButtonList ID="rdbInOutStkTypeSearch" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="ประเภทการรับเข้า" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="ประเภทการจ่ายออก"></asp:ListItem>
                                    </asp:RadioButtonList>
                                  </td>
                                </tr>
                            </table>
                            </td>
                        <td style="width: 50px;" align="right">
                            สถานะ
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="155">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclick="btnAdd_Click" />
                <asp:GridView ID="gvInOutStk" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" onrowcommand="gvInOutStk_RowCommand" 
                    onrowdatabound="gvInOutStk_RowDataBound" onsorting="gvInOutStk_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รายละเอียด" DataField="Reason_Description" SortExpression="Reason_Description"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="InOutStk_Status" SortExpression="InOutStk_Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
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
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td></td>
                            <td colspan="3">
                            <table style="border:1px; border-style:solid; border-color:Gray; width:250px;">
                               <tr>
                                 <td colspan = "4">
                                    <asp:RadioButtonList ID="rdbInOutStkType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="ประเภทการรับเข้า" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="ประเภทการจ่ายออก"></asp:ListItem>
                                    </asp:RadioButtonList>
                                  </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                รายละเอียด<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReason_Desc" runat="server" Width="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรายละเอียด"
                                    ControlToValidate="txtReason_Desc" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td colspan="2" align="left">
                                <asp:CheckBox ID="chkIsCal_Avgcost" Text=" คำนวณราคาเฉลี่ย เมื่อรับเข้า - จ่ายออก" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 130px;" align="right">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่สร้าง
                            </td>
                            <td>
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td style="width: 130px;" align="right">
                                ผู้ที่แก้ไขล่าสุด
                            </td>
                            <td>
                                <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" onclick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                            CausesValidation="False" onclick="btnCancel_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
