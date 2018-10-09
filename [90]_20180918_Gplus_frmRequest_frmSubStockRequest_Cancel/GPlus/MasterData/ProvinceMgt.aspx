<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="ProvinceMgt.aspx.cs" Inherits="GPlus.MasterData.ProvinceMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 322px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                จังหวัด
            </td>
        </tr>

        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสจังหวัด
                        </td>
                        <td>
                            <asp:TextBox ID="txtProvinceCode" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ชื่อจังหวัด
                        </td>
                        <td>
                            <asp:TextBox ID="txtProvinceName" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        
                        </tr>
                        <tr>
                        <td style="width: 130px;" align="right">
                            สถานะ
                        </td>
                        <td>
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
                            onclick="btnSearch_Click"/>&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False"  onclick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                 </table>
            
              <asp:Button ID="btnAdd" runat="server" Text="เพิ่มจังหวัด" 
                    CausesValidation="False" onclick="btnAdd_Click" />

                <asp:GridView ID="GvProvince" runat="server" AutoGenerateColumns="False" Width="782px"
                    AllowSorting="True" onrowcommand="Province_RowCommand" 
                    onrowdatabound="Province_RowDataBound" Height="127px" 
                    onsorting="Province_Sorting" >
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัสจังหวัด" DataField="ProvinceCode" SortExpression="ProvinceCode"
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField> 
                        <asp:BoundField HeaderText="ชื่อจังหวัด" DataField="ProvinceName" SortExpression="ProvinceName"  ItemStyle-Width="180"
                            ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="สถานะ" DataField="Province_Status" SortExpression="Province_Status"
                            ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้ที่แก้ไขล่าสุด" DataField="Update_By" SortExpression="Update_By"
                            ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
                <br />
                <uc1:PagingControl ID="PagingControl1" runat="server" />
            </td>

        </tr>
        <tr>
            <td class="tableFooter">
            </td>
        </tr>
 </table>
           <!-- ข้างล่าง=============================================================================================== -->
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
                                รหัสจังหวัด<span style="Color:Red">*</span>
                            </td>
                            <td class="style1">
                                <asp:TextBox ID="txtProvinceCode1" runat="server" MaxLength="100" Width="175px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัสจังหวัด"
                                    ControlToValidate="txtProvinceCode1" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
              <!-- =============================================================================================== -->
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชื่อจังหวัด<span style="Color:Red">*</span>
                            </td>
                            <td class="style1">
                                <asp:TextBox ID="txtProvinceName1" runat="server" MaxLength="100" Width="217px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุชื่อจังหวัด"
                                    ControlToValidate="txtProvinceName1" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
             <!-- =============================================================================================== -->
                        <tr>
                            <td style="width: 130px;" align="right">
                                สถานะการใช้งาน
                            </td>
                            <td class="style1">
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
                            <td class="style1">
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
                            <td class="style1">
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
                          CausesValidation="False"  />
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
