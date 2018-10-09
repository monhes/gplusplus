<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="True" 
    CodeBehind="BuildingFloorMgt.aspx.cs" Inherits="GPlus.MasterData.BulingFloorMgt" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table cellpadding="0" cellspacing="0" width="805">
        <tr>
            <td class="tableHeader">
                ค้นหาชั้นของตึก/อาคาร (BuildingFloor)
            </td>
        </tr>
        <tr>
            <td class="tableBody">
            <center>
                 <table width="600px" style="border:1px; border-style:solid; border-color:Gray; ">
                 <tr><td></td></tr>
                    <tr>
                        <td style="width: 500px;" align="center">
                             ตึก/อาคาร&nbsp;
                       
                            <asp:DropDownList ID="ddlBuilding" runat="server" Width="150" MaxLength="100"
                                DataTextField="Building_Name" DataValueField="Building_ID">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 500px;"  align="center">
                                 ชั้น&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                           <asp:TextBox ID="txtFloor" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" 
                                Width="57px" onclick="btnSearch_Click" />
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnCancelSearch" runat="server" Text="ยกเลิก" Width="55px" 
                                 onclick="btnCancelSearch_Click" />
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    </table>
                </center>
                 <asp:Button ID="btnAdd" runat="server" Height="32" Text="เพิ่มข้อมูล" 
                                 Width="87" onclick="btnAdd_Click" />
                <asp:GridView ID="gvBuilding" runat="server" AutoGenerateColumns="false" Width="100%"
                    AllowSorting="true" onrowcommand="gvBuilding_RowCommand" 
                    onrowdatabound="gvBuilding_RowDataBound" >
                <Columns>
                    <asp:TemplateField HeaderText="รายละเอียด" >
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDetail" runat="server" Text="รายละเอียด" CommandName="Edi" CausesValidation="false"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Building_Id" Visible="False" />
                    <asp:BoundField HeaderText="รหัสตึก/อาคาร" DataField="Building_Code" 
                        ItemStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ชื่อตึก/อาคาร" DataField="Building_Name" 
                        ItemStyle-HorizontalAlign="Left" >
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>                  
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
                        <td style="width: 1200px;"  align="center">
                                ตึก/อาคาร&nbsp;
                            
                                <asp:DropDownList ID="ddlBuilding1" runat="server" Width="150px"
                                DataTextField="Building_Name" DataValueField="Building_ID" 
                                    onselectedindexchanged="ddlBuilding1_SelectedIndexChanged" 
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        
                        </tr>
                       <tr>
                        <td style="width: 1200px;"  align="left">
                               <asp:Button ID="btnPlus" runat="server" Text="เพิ่ม" Width="31px" 
                                   onclick="btnPlus_Click" />
                            &nbsp;
                                  <asp:Button ID="btnDelete1" runat="server" Text="ลบ" Width="31px" 
                                    onclick="btnDelete1_Click" /> 
                            </td>
                           
                        </tr>
                        <tr>
                            <td >
                                 <asp:GridView ID="gvBuildingFloor" runat="server" AutoGenerateColumns="False" 
                                    Width="100%" OnRowCommand="gvBuilding_RowCommand" 
                                    OnRowDataBound="gvBuildingFloor_RowDataBound" Height="16px">
                            
                                    <Columns>
                                        <asp:TemplateField HeaderText="ชั้น">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdBuildingFloor" runat="server" />
                                                <asp:TextBox ID="txtFloor" runat="server" Width="150" MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุชั้น"
                                                    ControlToValidate="txtFloor" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="สถานะ">
                                            <ItemTemplate>
                                                <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Text="Active" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="วันที่แก้ไขล่าสุด" 
                                            ItemStyle-HorizontalAlign="Center" SortExpression="Updated_Date">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Updated_By" HeaderText="ผู้แก้ไขล่าสุด" 
                                            ItemStyle-HorizontalAlign="Center" SortExpression="Updated_By">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="วันที่สร้าง" ItemStyle-HorizontalAlign="Center" 
                                            SortExpression="Created_Date">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Created_By" HeaderText="ผู้สร้าง" 
                                            ItemStyle-HorizontalAlign="Center" SortExpression="Created_By">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>

                                      <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                <asp:HiddenField ID="hdBdFloor" runat="server" />         
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                       
                                    </Columns>
                             
                                </asp:GridView>

                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Button ID="btnAdd1" runat="server" 
                                    Text="บันทึก" Width="55px" onclick="btnAdd1_Click" />

                                <asp:Button ID="btnDelete2" runat="server" Text="ยกเลิก" onclick="btnDelete2_Click"/>
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
