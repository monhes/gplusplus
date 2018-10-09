<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="TumbonMgt.aspx.cs" Inherits="GPlus.MasterData.TumbonMgt" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/ImageListControl.ascx" TagName="ImageListControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาตำบล
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            ชื่อตำบล
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTumbonNameSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            จังหวัด
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlProvinceNameSearch" runat="server" Width="195" 
                                onselectedindexchanged="ddlProvinceNameSearch_SelectedIndexChanged" 
                                AutoPostBack="True" >
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                            อำเภอ
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlAmphurSearch" runat="server" Width="195" >
                            </asp:DropDownList>
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click"  />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click"
                                />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclick="btnAdd_Click"
                    />
                <asp:GridView ID="gvTumbon" runat="server" AutoGenerateColumns="false" Width="100%" 
                OnRowCommand="gvTumbon_RowCommand" OnRowDataBound="gvTumbon_DataBound" OnSorting="Tumbon_Sorting"
                    AllowSorting="true" >
                   <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ตำบล" DataField="TumbonName" SortExpression="TumbonName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="อำเภอ/เขต" DataField="AmphurName" SortExpression="AmphurName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="จังหวัด" DataField="ProvinceName" SortExpression="ProvinceName"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Status" SortExpression="Status"
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
                           
                            <td style="width: 130px;" align="right">
                                ตำบล<span style="Color:Red">*</span>
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="tbTumbonName" runat="server" Width="200" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุชื่อตำบล"
                                    ControlToValidate="tbTumbonName" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td align="right" class="style2">
                                รหัสตำบล<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="tbTumbonCode" runat="server" MaxLength="10" Width="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุรหัสตำบล"
                                    ControlToValidate="tbTumbonCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                           
                        </tr>
                        <tr>
                          
                           <td  align="right">
                                แขวงย่อย
                            </td>
                            <td>
                           <asp:TextBox ID="tbSUBDST" runat="server" Width="200" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                           <td  align="right">
                                จังหวัด
                            </td>
                            <td>
                            <asp:DropDownList ID="ddllProvince" runat="server"  Width="206px" MaxLength="100" 
                                    AutoPostBack="True" onselectedindexchanged="ddllProvince_SelectedIndexChanged" >
                            </asp:DropDownList>
                            </td>
                            <td  align="right">
                                อำเภอ
                            </td>
                            <td>
                            <asp:DropDownList ID="ddllAmphur" runat="server"  Width="206px" MaxLength="100" >
                            </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                           <td style="width: 130px;" align="right">
                                รหัสไปรษณีย์<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="tbpostcode" runat="server" MaxLength="10" Width="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุรหัสไปรษณีย์"
                                    ControlToValidate="tbpostcode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                   <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
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
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" onclick="btnSave_Click" 
                             />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                            CausesValidation="False" onclick="btnCancel_Click"  />
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