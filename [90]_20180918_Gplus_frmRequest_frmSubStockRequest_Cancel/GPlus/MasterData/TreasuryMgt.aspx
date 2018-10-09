<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true"
    CodeBehind="TreasuryMgt.aspx.cs" Inherits="GPlus.MasterData.TreasuryMgt" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาคลังสินค้า
            </td>
        </tr>
        <tr>
            <td class="tableBody">
                <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัส
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtStockCodeSearch" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="width: 130px;" align="right">
                            ชื่อคลังสินค้า
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtStockNameSearch" runat="server" Width="250" MaxLength="100"></asp:TextBox>
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
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" CausesValidation="False"
                                OnClick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnAdd" runat="server" Text="เพิ่มข้อมูล" CausesValidation="False"
                    OnClick="btnAdd_Click" />
                <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" OnRowCommand="gvStock_RowCommand" OnRowDataBound="gvStock_RowDataBound"
                    OnSorting="gvStock_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"
                                    ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="รหัสคลังสินค้า" DataField="Stock_Code" SortExpression="Stock_Code"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ชื่อ" DataField="Stock_Name" SortExpression="Stock_Name"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="ชนิดคลัง" DataField="Stock_Type" SortExpression="Stock_Type"
                            ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField HeaderText="ระดับคลังย่อย" DataField="LevelStk_Desc" SortExpression="LevelStk_Desc"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="สถานะ" DataField="Stock_Status" SortExpression="Stock_Status"
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
                            <td style="width: 130px;" align="right">
                                รหัสคลังสินค้า<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStockCode" runat="server" Width="190" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัสคลังสินค้า"
                                    ControlToValidate="txtStockCode" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ชื่อคลัง<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStockName" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุชื่อคลัง"
                                    ControlToValidate="txtStockName" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                ชนิดของคลัง<span style="Color:Red">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStockType" runat="server" Width="155">
                                    <asp:ListItem Text="เลือกชนิดของคลัง" Value=""></asp:ListItem>
                                    <asp:ListItem Text="คลังหลัก" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="คลังย่อย" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุชนิดของคลัง"
                                    ControlToValidate="ddlStockType" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender" 
                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                </asp:ValidatorCalloutExtender>
                            </td>
                            <td style="width: 130px;" align="right">
                                ระดับคลังย่อย
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStockLevel" runat="server" Width="155" DataTextField="LevelStk_Desc" DataValueField="LevelStk_ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                                เบิกสินค้าจากคลัง
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFromMainStock" runat="server" Width="155" DataValueField="Stock_ID" DataTextField="Stock_Name">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;" align="right">
                                เบิกสินค้าระหว่างคลังย่อย
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFromSubStock" runat="server" Width="155" DataTextField="LevelStk_Desc" DataValueField="LevelStk_Id">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="right">
                            </td>
                            <td>
                                
                            </td>
                            <td colspan="2" align="center">
                                <asp:CheckBox ID="chkPack" runat="server" Text="แตก Pack (รับสินค้า)" />&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkApprove" runat="server" Text="ต้องผ่านการอนุมัติ" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td colspan="2" style=" padding-left:95px;">
                                <%--<fieldset style="width:250px;">
                                <legend>ประเภทคลัง</legend>--%>
                                <table style="border:1px; border-style:solid; border-color:Gray; width:250px;">
                                    <tr>
                                        <td colspan = "4">
                                        <asp:CheckBoxList ID="ChlTempStk_Flag" runat="server" RepeatLayout="Flow">
                                            <asp:ListItem onclick="singleitemcheck(this);">  คลังวัตถุดิบ  </asp:ListItem>
                                            <asp:ListItem onclick="singleitemcheck(this);">  โรงพิมพ์  </asp:ListItem>
                                        </asp:CheckBoxList>
                        
                                        </td>
                                    </tr>
                                </table>
                               <%-- </fieldset>--%>
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
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False"
                            OnClick="btnCancel_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </asp:Panel>

    <script type="text/javascript">
        function singleitemcheck(ch) {
            var chkList = ch.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != ch && ch.checked) {
                    chks[i].checked = false;
                }
            }
        }
    </script>
</asp:Content>
