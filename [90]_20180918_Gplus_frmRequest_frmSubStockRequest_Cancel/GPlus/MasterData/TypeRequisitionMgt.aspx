<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="TypeRequisitionMgt.aspx.cs" Inherits="GPlus.MasterData.TypeRequisitionMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/ItemOrgStructControl2.ascx" TagName="ItemOrgStructControl2" TagPrefix="uc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาฝ่ายเจ้าของแต่ละประเภทวัสดุ-อุปกรณ์ และกลุ่มผู้ใช้
            </td>
        </tr>
        <tr>
            <td class="tableBody">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
              <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaterialTypeSearch" runat="server" Width="195" DataTextField="MaterialType_Name"
                                DataValueField="MaterialType_ID" OnSelectedIndexChanged="ddlMaterialTypeSearch_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 60px;" align="center">
                            กลุ่มผู้ใช้
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserGroupSearch" runat="server" Width="195" DataTextField="UserGroup_Name"
                                    DataValueField="UserGroup_Id">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan = "4">
                            <uc2:ItemOrgStructControl2 ID="orgCtrlSearch" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" 
                                onclick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" 
                                CausesValidation="False" onclick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
             </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSearch" />
                </Triggers>
             </asp:UpdatePanel>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" 
                    CausesValidation="False" onclick="btnAdd_Click" />
                <asp:GridView ID="gvTypeRequisition" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" onrowcommand="gvTypeRequisition_RowCommand" 
                    onrowdatabound="gvTypeRequisition_RowDataBound" onsorting="gvTypeRequisition_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ประเภทวัสดุ - อุปกรณ์" DataField="Cat_Name" SortExpression="Cat_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="กลุ่มผู้ใช้" DataField="Type_Name" SortExpression="Type_Name"
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader">
                    รายละเอียด
                </td>
            </tr>
            <tr>
                <td class="tableBody">
                    <table width="100%">
                        <tr>
                            <td style="width: 130px;" align="right">
                                ประเภทวัสดุอุปกรณ์
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" DataTextField="MaterialType_Name"
                                    DataValueField="MaterialType_ID" OnSelectedIndexChanged="ddlMaterialType_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                กลุ่มผู้ใช้
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUserGroup" runat="server" Width="195" DataTextField="UserGroup_Name"
                                        DataValueField="UserGroup_Id">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <uc2:ItemOrgStructControl2 ID="orgCtrl" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan = "2" align="left">
                                <asp:Button runat="server" ID="btnAddOrg" Text="เพิ่ม" OnClick="btnAddOrgClick"/>&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnDeleteOrg" Text="ลบ" OnClick="btnDeleteOrgClick"/>
                            </td>
                            <td>
                            
                            </td>
                            <td>
                                 
                            </td>
                        </tr>
                        <tr>
                            <td colspan= "4">
                                <asp:GridView ID="gvOrg" runat="server" AutoGenerateColumns="false" Width="100%"
                                    AllowSorting="true" onrowdatabound="gvOrg_RowDataBound" onsorting="gvOrg_Sorting">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                <asp:HiddenField ID="hdOrgStuct" runat="server" />
                                                <asp:HiddenField ID="hdTrypeReq_ID" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="รหัสฝ่าย" DataField="Div_Code" SortExpression="Div_Code"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="รหัสทีม" DataField="Dep_Code" SortExpression="Dep_Code"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="ชื่อฝ่าย" DataField="Div_Name" SortExpression="Div_Name"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="ชื่อทีม" DataField="Dep_Name" SortExpression="Dep_Name"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="วันที่สร้าง" SortExpression="Create_Date" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ผู้สร้าง" DataField="FullName_Create_By" SortExpression="FullName_Create_By"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" SortExpression="Update_Date" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="ผู้แก้ไขล่าสุด" DataField="FullName_Update_By" SortExpression="FullName_Update_By"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>                       
                            </td>
                        </tr>
                        <%--<tr>
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
                        </tr>--%>
                        <tr>
                        <td colspan="4">
                            
                            
                        </td>
                        </tr>
                       <%-- <tr>
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
                        </tr>--%>
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
