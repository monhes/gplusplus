<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" 
CodeBehind="AmphurMgt.aspx.cs" Inherits="GPlus.MasterData.AmphurMgt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 286px;
        }
        .style2
        {
            width: 151px;
        }
        .style3
        {
            width: 233px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาอำเภอ
            </td>
        </tr>
        <tr>
            <td class="tableBody">
            <center>
                 <table width="600px" style="border:1px; border-style:solid; border-color:Gray; ">
                 <tr><td></td></tr>
                    <tr>
                        <td style="width: 500px;" align="center">
                            ชื่อจังหวัด&nbsp;&nbsp;
                       
                            <asp:DropDownList ID="ddlProvince" runat="server"  Width="190" MaxLength="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 500px;"  align="center">
                            ชื่ออำเภอ/เขต&nbsp;&nbsp;
                        
                            <asp:TextBox ID="txtAmphur" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" onclick="btnSearch_Click" 
                               />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click"  />
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    </table>
                </center>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclick="btnAdd_Click" />
                <asp:GridView ID="gvAmphur" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" onrowcommand="Amphur_RowCommand" 
                    onrowdatabound="Amphur_RowDataBound" onsorting="Amphur_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="จังหวัด" DataField="ProvinceName" SortExpression="ProvinceName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="อำเภอ/เขต" DataField="AmphurName" SortExpression="AmphurName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Amphur_Status" SortExpression="Amphur_Status"
                            ItemStyle-HorizontalAlign="Center" />
                          <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" 
                            ItemStyle-HorizontalAlign="Center" >
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                            ItemStyle-HorizontalAlign="Left" >
                        </asp:BoundField>
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
    <a name="panel"></a>
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
                            <td align="right" class="style2">
                                ชื่ออำเภอ/เขต<span style="Color:Red">*</span>
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="tbAmphurName" runat="server" Width="200" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุชื่ออำเภอ/เขต"
                                    ControlToValidate="tbAmphurName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        
                            <td style="width: 130px;" align="right">
                                รหัส<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="tbAmphurCode" runat="server" MaxLength="10" Width="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุรหัสอำเภอ"
                                    ControlToValidate="tbAmphurCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                           <td  align="right">
                                ชื่อจังหวัด
                            </td>
                            <td>
                            <asp:DropDownList ID="ddllProvince" runat="server"  Width="206px" MaxLength="100" >
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style2">
                                สถานะการใช้งาน
                            </td>
                            <td class="style3">
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
                            <td align="right" class="style2">
                                วันที่สร้าง
                            </td>
                            <td class="style3">
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
                            <td align="right" class="style2">
                                วันที่แก้ไขล่าสุด
                            </td>
                            <td class="style3">
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
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" 
                            onclick="btnSave_Click"  />
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