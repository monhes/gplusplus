<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="DepartmentMgt.aspx.cs" Inherits="GPlus.MasterData.DepartmentMgt"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3 {
            width: 250px;
        }

        .style4 {
        }

        .style5 {
            width: 78px;
        }
        .auto-style2 {
            width: 238px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">ค้นหาหน่วยงาน
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">รหัสหน่วยงาน
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDepartmentCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">ฝ่าย
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDivNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">ทีม
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDepNameSearch" runat="server" Width="190" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">สถานะ
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
                        <td colspan="6" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvDepartment" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                    Width="100%" OnRowCommand="gvDepartment_RowCommand" OnRowDataBound="gvDepartment_RowDataBound"
                    OnSorting="gvDepartment_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi"
                                    CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัส" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ฝ่าย" DataField="" SortExpression="" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ทีม" DataField="" SortExpression="" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="เบิกของจากคลัง" DataField="Stock_Name" HeaderStyle-Width="100px" SortExpression="Stock_Name" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="สถานะ" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
                <uc1:PagingControl ID="PagingControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tableFooter"></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <asp:HiddenField ID="hdOrgId" runat="server" />
                    <asp:HiddenField ID="HdDiv" runat="server" />
                    <asp:HiddenField ID="HdDep" runat="server" />
                    <asp:HiddenField ID="txtItemDivName" runat="server" />
                    <asp:HiddenField ID="TxtItemDepName" runat="server" />
                    <asp:HiddenField ID="hdID" runat="server" />
                    <table width="100%">
                        <tr>
                            <td style="color: Red;" align="right" class="auto-style2"></td>
                            <td align="left" colspan="4">
                                <fieldset>
                                    <legend>กำหนด</legend>
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                        <asp:ListItem Text="ฝ่าย" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="ทีม" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">รหัสฝ่าย<span style="color: Red">*</span>
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="txtDivCode" runat="server" Width="100" MaxLength="9"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัสฝ่าย"
                                    ControlToValidate="txtDivCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td align="right" class="style5">&nbsp;</td>
                            <td class="style3">&nbsp;</td>
                            <td rowspan="5" valign="top">
                                <%--
                                <table>
                                    <tr>
                                        <th>ตึก</th>
                                        <th>ชั้น</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlBuilding" runat="server" AutoPostBack="true" DataTextField="Building_Name" 
                                                DataValueField="Building_ID" onselectedindexchanged="ddlBuilding_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBuildingFloor" runat="server" DataTextField="Building_Floor_Desc" 
                                                DataValueField="Building_FloorId"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                --%>
                                <%--Green Edit--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnAddBuildingFloor" runat="server" Text="เพิ่ม" OnClick="BtnAddBuildingFloor_Click" />
                                        <asp:Button ID="BtnDelBuildingFloor" runat="server" Text="ลบ" OnClick="BtnDelBuildingFloor_Click" />
                                        <asp:Table ID="Table1" runat="server" Width="166px">
                                            <asp:TableHeaderRow>
                                                <asp:TableHeaderCell Text="ตึก"></asp:TableHeaderCell>
                                                <asp:TableHeaderCell Text="ชั้น"></asp:TableHeaderCell>
                                            </asp:TableHeaderRow>
                                        </asp:Table>
                                        <asp:Table ID="TblBuildingFloor" runat="server"></asp:Table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- End Green Edit --%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">ชื่อฝ่าย<span style="color: Red">*</span>
                            </td>
                            <td colspan="3" align="left">
                                <asp:TextBox ID="txtDivName" runat="server" MaxLength="100" Width="320px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtDivName" ErrorMessage="กรุณาระบุชื่อฝ่าย" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">รหัสฝ่าย/ชื่อฝ่าย<span style="color: Red">*</span>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="ddlDiv" runat="server" MaxLength="100" Width="400px"
                                    BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
                                </td>
                                <td align="left" >
                                <asp:ImageButton ID="btnDep" runat="server" ImageUrl="~/images/Commands/view.png" Enabled="false" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="ddlDiv" ErrorMessage="กรุณาระบุรหัสฝ่าย/ชื่อฝ่าย"
                                    ForeColor="Red">*</asp:RequiredFieldValidator>&nbsp;
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                    Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">รหัสทีม<span style="color: Red">*</span>
                                
                                &nbsp;
                            </td>
                            <td class="style4">
                                <asp:TextBox ID="txtDepCode" runat="server" Width="100" MaxLength="9"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="กรุณาระบุรหัสทีม"
                                    ControlToValidate="txtDepCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td align="right" class="style5">&nbsp;</td>
                            <td class="style3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">ชื่อทีม<span style="color: Red">*</span>
                            </td>
                            <td class="style4" colspan="3">
                                <asp:TextBox ID="txtDepName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txtDepName" ErrorMessage="กรุณาระบุชื่อทีม" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator5_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">เบิกสินค้าจากคลัง
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="ddlFromMainStock" runat="server" Width="155" DataValueField="Stock_ID"
                                    DataTextField="Stock_Name">
                                </asp:DropDownList>
                            </td>
                            <td class="style5"></td>
                            <td colspan="2" align="left">
                                <asp:CheckBox ID="chkNotApprove" runat="server" Text="เบิกโดยไม่ต้องอนุมัติ" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">สถานะการใช้งาน
                            </td>
                            <td class="style4">
                                <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Active" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Inactive"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td align="right" class="style5"></td>
                            <td class="style3"></td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">วันที่สร้าง
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
                            </td>
                            <td align="right" class="style5">ผู้ที่สร้าง
                            </td>
                            <td class="style3">
                                <asp:Label ID="lblCreateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style2">วันที่แก้ไขล่าสุด
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblUpdatedate" runat="server"></asp:Label>
                            </td>
                            <td align="right" class="style5">ผู้ที่แก้ไขล่าสุด
                            </td>
                            <td class="style3">
                                <asp:Label ID="lblUpdateBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter"></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
