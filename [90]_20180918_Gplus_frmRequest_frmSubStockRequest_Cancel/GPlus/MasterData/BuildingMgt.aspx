<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" 
CodeBehind="BuildingMgt.aspx.cs" Inherits="GPlus.MasterData.BuildingMgt" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาอาคาร
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสตึก/อาคาร
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtBuildingid" runat="server"  Width="195"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ชื่อตึก/อาคาร
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtBuildingna" runat="server" Width="190" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
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
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclientclick="btnSearch_Click" onclick="btnSearch_Click1" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclientclick="btnCancelSearch_Click" 
                                onclick="btnCancelSearch_Click1"  />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclientclick="btnAdd_Click" 
                    onclick="btnAdd_Click"  />

                <br />
              
                <asp:GridView ID="gvBuilding" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    Width="754px"  OnRowCommand="gvBuilding_RowCommand" OnRowDataBound="gvBuilding_RowDataBound"
                    OnSorting="gvBuilding_Sorting">
                 <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" 
                                CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัสตึก/อาคาร" DataField="Building_Code" SortExpression="Building_Code"
                            ItemStyle-HorizontalAlign="Center" >
                       <ItemStyle HorizontalAlign="Center"></ItemStyle> </asp:BoundField>
                        <asp:BoundField HeaderText="ชื่อตึก/อาคาร" DataField="Building_Name" SortExpression="Building_Name"
                            ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="สถานะ" DataField="Building_Status" SortExpression="Building_Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" 
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                            ItemStyle-HorizontalAlign="Left" >
                      
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                      <asp:TemplateField HeaderText="ชั้น" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="26">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDetail2" runat="server" ImageUrl="~/images/Commands/new.png" CommandName="Detail" ToolTip="ชั้น" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                       
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
     <a name="pnlDetail"></a>
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
                                รหัสตึก/อาคาร<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuildingcode" runat="server" Width="150" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัส"
                                    ControlToValidate="txtBuildingcode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            </tr>
                            <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อตึก/อาคาร<span style="color: Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuildingname" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุชื่อตึก/อาคาร"
                                    ControlToValidate="txtBuildingname" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
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
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" 
                            onclick="btnSave_Click"  />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" 
                            CausesValidation="False" onclick="btnCancel_Click"
                             />
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