<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_PrintForm2.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="GPlus.PRPO.pop_PrintForm2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ระบุการขอแบบพิมพ์</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px; font-family: Tahoma; font-size: 12px">
    <form id="form1" runat="server">
    <center>
        <ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" />
        <table cellpadding="0" cellspacing="0" width="805">
            <tr>
                <td class="tableHeader" align="left">
                    ระบุการขอแบบพิมพ์
                </td>
            </tr>
            <tr>
                <td class="tableBody">

                    <asp:HiddenField ID="HfNewItemID" runat="server" />
                    <asp:HiddenField ID="HfNewPackID" runat="server" />

                    <asp:HiddenField ID="HfOldItemID" runat="server" />
                    <asp:HiddenField ID="HfOldPackID" runat="server" />

                    <asp:RadioButtonList ID="RblFormType" runat="server" 
                        RepeatDirection="Horizontal" AutoPostBack="true" 
                        onselectedindexchanged="RblFormType_IndexChanged">
                        <asp:ListItem Text="แบบพิมพ์ใหม่" Value="0"></asp:ListItem>
                        <asp:ListItem Text="แบบพิมพ์เดิม" Selected="True" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>

                    <%-- PnlNew --%>
                    <asp:UpdatePanel runat="server" ID="upPnlNew" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Panel ID="PnlNew" runat="server" Visible="false">
                        <fieldset>
                            <legend>แบบพิมพ์ใหม่</legend>
                            <table width="100%">
                                <tr>
                                    <td align="left" class="style3">
                                        ฝ่าย/ทีม
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:TextBox ID="TbNewDiv" Enabled="false" runat="server" Width="300"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        แบบพิมพ์ใหม่
                                    </td>
                                    <td align="left" style="width: 70px">รหัสแบบพิมพ์</td>
                                    <td align="left">
                                        <asp:TextBox 
                                            ID="TbNewFormPrintCode" 
                                            runat="server"
                                            Enabled="false" Width="80"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width: 70px">&nbsp;&nbsp;ชื่อแบบพิมพ์</td>
                                    <td align="left">
                                        <asp:TextBox 
                                            ID="TbNewFormPrintName" 
                                            runat="server" 
                                            Enabled="false"
                                            Width="250">
                                        </asp:TextBox>
                                    </td>
                                    <td align="left" style="width: 180px">
                                        <asp:ImageButton ID="BtnNewSelect" runat="server" 
                                            ImageUrl="~/images/Commands/view.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        รูปแบบ
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:RadioButtonList ID="RblNewFormat" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ตามที่แนบ" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="กรุณาออกแบบให้" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        กระดาษ
                                    </td>
                                    <td align="left" colspan="5">
                                        ชนิด&nbsp;<asp:TextBox ID="TbNewPaperType" runat="server" MaxLength="100"></asp:TextBox>&nbsp; 
                                        สี&nbsp;<asp:TextBox ID="TbNewPaperColor" runat="server" MaxLength="100"></asp:TextBox>
                                        ความหนา&nbsp;<asp:TextBox ID="TbNewPaperGram" runat="server" MaxLength="100"></asp:TextBox>แกรม
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        ตัวอักษร
                                    </td>
                                    <td align="left" colspan="5">
                                        พิมพ์&nbsp;<asp:TextBox ID="TbNewFontColor" runat="server" MaxLength="100"></asp:TextBox>&nbsp;สี
                                        &nbsp;&nbsp;ขนาด&nbsp;&nbsp;<asp:TextBox ID="TbNewSizeDetail" runat="server" 
                                            MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        ลักษณะแบบพิมพ์
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:RadioButtonList ID="RblNewPrintType" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ฟอร์มคอมพิวเตอร์" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="แผ่น" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="เข้าชุด" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="เข้าเล่ม" Value="1"></asp:ListItem>
                                            <%--<asp:ListItem Text="แผ่นพับ"  Value="4"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        การเบิกใช้
                                    </td>
                                    <td align="left" valign="top" colspan="5">
                                        <asp:RadioButton 
                                            ID="RbNewFormBorrowType1" 
                                            runat="server" 
                                            Text="เบิกใช้ครั้งเดียวหมด จำนวนพิมพ์"
                                            GroupName="Y" 
                                            ClientIDMode="Static" 
                                            onclick="RbNewFormBorrowType1_Click();"/>&nbsp;
                                        <asp:TextBox ID="TbNewBorrowQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>
                                        <asp:DropDownList ID="DdlNewBorrowUnit" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                    </td>
                                    <td align="left" valign="top" colspan="5">
                                        <asp:RadioButton 
                                            ID="RbNewFormBorrowType2" 
                                            runat="server" 
                                            Text="เก็บสต๊อก ปริมาณจะใช้ต่อเดือน"
                                            GroupName="Y" 
                                            ClientIDMode="Static" 
                                            onclick="RbNewFormBorrowType2_Click();"/>&nbsp;
                                        <asp:TextBox ID="TbNewBorrowMonthQuantity" runat="server" Width="50" 
                                            MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            
                                            onKeyUp="OrderQuantity('TbNewBorrowMonthQuantity', 'TbNewBorrowFirstQuantity', 'TbNewUnitQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" 
                                            onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp; เบิกใช้ครั้งแรก
                                        จำนวน&nbsp;<asp:TextBox ID="TbNewBorrowFirstQuantity" runat="server" Width="50" 
                                            MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            
                                            onKeyUp="OrderQuantity('TbNewBorrowMonthQuantity', 'TbNewBorrowFirstQuantity', 'TbNewUnitQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="DdlNewBorrowMonthUnit" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID" >
                                        </asp:DropDownList>
                                        จำนวนสั่ง&nbsp;<asp:TextBox ID="TbNewUnitQuantity" runat="server" Width="50" 
                                            MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="DdlNewBorrowMonthUnitDisabled" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        อื่นๆ
                                    </td>
                                    <td align="left" valign="top" colspan="5">
                                        <asp:TextBox ID="TbNewRemark" runat="server" Width="300" Height="60" 
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<asp:ListItem Text="แผ่นพับ"  Value="4"></asp:ListItem>--%>
                    <asp:UpdatePanel ID="upPnlOld" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <asp:Panel ID="PnlOld" runat="server" Visible="true">
                        <fieldset>
                            <legend>แบบพิมพ์เดิม</legend>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        ฝ่าย/ทีม
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:TextBox ID="TbOldDiv" runat="server"  Width="300" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">แบบพิมพ์เดิม</td>
                                    <td align="left" style="width: 70px">รหัสแบบพิมพ์</td>
                                    <td align="left">
                                        <asp:TextBox
                                            ID="TbOldFormPrintCode" 
                                            runat="server" 
                                            Width="80"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width: 70px">&nbsp;&nbsp;ชื่อแบบพิมพ์</td>
                                    <td align="left">
                                        <asp:TextBox 
                                            ID="TbOldFormPrintName" 
                                            runat="server"
                                            Enabled="false"
                                            Width="250">
                                        </asp:TextBox>
                                    </td>
                                    <td align="left" style="width: 180px">
                                        <asp:ImageButton 
                                            ID="BtnOldSelect" 
                                            runat="server" 
                                            ImageUrl="~/images/Commands/view.png" Width="24px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        การเบิกใช้
                                    </td>
                                    <td align="left" valign="top" colspan="5">
                                        <asp:RadioButton 
                                            ID="RbOldFormBorrowType1" 
                                            runat="server" 
                                            Text="เบิกใช้คร้ังเดียวหมด" 
                                            GroupName="z" 
                                            ClientIDMode="Static" />&nbsp;
                                        <asp:TextBox ID="TbOldBorrowQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="DdlOldBorrowUnit" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID" ></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                    </td>
                                    <td align="left"  colspan="5">
                                        <asp:RadioButton 
                                            ID="RbOldFormBorrowType2" 
                                            runat="server" 
                                            Text="เก็บสต๊อก" 
                                            GroupName="z" 
                                            ClientIDMode="Static" />
                                        &nbsp; ปริมาณจะใช้ต่อเดือน&nbsp;
                                        <asp:TextBox ID="TbOldBorrowMonthQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            
                                            onKeyUp="OrderQuantity('TbOldBorrowMonthQuantity', 'TbOldBorrowFirstQuantity', 'TbOldUnitQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" 
                                            onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>&nbsp;
                                        เบิกใช้ครั้งแรก จำนวน&nbsp;<asp:TextBox ID="TbOldBorrowFirstQuantity" runat="server"
                                            Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            
                                            onKeyUp="OrderQuantity('TbOldBorrowMonthQuantity', 'TbOldBorrowFirstQuantity', 'TbOldUnitQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" 
                                            onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="DdlOldBorrowMonthUnit" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID">
                                        </asp:DropDownList>
                                         จำนวนสั่ง&nbsp;<asp:TextBox ID="TbOldUnitQuantity" runat="server" Width="50"
                                            MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="DdlOldBorrowMonthUnitDisabled" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="left" valign="top">
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="CbOldIsRequestModify" runat="server" 
                                                            Text="ขอแก้ไขเปลี่ยนแปลง" />&nbsp;&nbsp;
                                                        ลักษณะการแก้ไข&nbsp;<asp:TextBox ID="TbOldRequestModifyDesc" runat="server" 
                                                            Width="300"></asp:TextBox>
                                                    </td>
                                                    <%--<td align="right" colspan="2"></td>
                                                    <td align="left"></td>--%>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="CbOldIsFixedContent" runat="server" 
                                                            Text="ข้อความตามตัวอย่างที่แนบ" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="CbOldIsPaper" runat="server" Text="กระดาษ" />
                                                        &nbsp; ชนิด&nbsp;<asp:TextBox ID="TbOldPaperType" runat="server" Width="50" 
                                                            MaxLength="100"></asp:TextBox>&nbsp;
                                                        &nbsp; สี&nbsp;<asp:TextBox ID="TbOldPaperColor" runat="server" Width="50" 
                                                            MaxLength="100"></asp:TextBox>&nbsp;
                                                        &nbsp; ความหนา&nbsp;<asp:TextBox ID="TbOldPaperGram" runat="server" Width="50" 
                                                            MaxLength="100"></asp:TextBox>&nbsp;แกรม
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="CbOldIsFont" runat="server" Text="ตัวอักษร" />
                                                        &nbsp; พิมพ์&nbsp;<asp:TextBox ID="TbOldFontColor" runat="server"></asp:TextBox>&nbsp;สี&nbsp;
                                                    </td>
                                                    <td align="right">ขนาด</td>
                                                    <td align="left"><asp:TextBox ID="TbOldSizeDetail" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="left" style="width: 100px">
                                        ลักษณะแบบพิมพ์
                                    </td>
                                    <td align="left" colspan="5">
                                        <asp:RadioButtonList ID="RblOldPrintType" runat="server" 
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ฟอร์มคอมพิวเตอร์" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="แผ่น" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="เข้าชุด" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="เข้าเล่ม" Value="1"></asp:ListItem>
                                            <%--<asp:ListItem Text="แผ่นพับ"  Value="4"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        อื่นๆ
                                    </td>
                                    <td align="left" valign="top" colspan="5">
                                        <asp:TextBox ID="TbOldRemark" runat="server" Width="300" Height="60" 
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
	                        <tr>
		                        <td style="width: 60px;" align="left" valign="top">
			                        เริ่มใช้
		                        </td>
		                        <td align="left" valign="top">
			                        <div style="float: left">
				                        <asp:RadioButton ID="RbBorrowType0" runat="server" Text="ระบุวันที่" 
					                        GroupName="x" />&nbsp;</div>
			                        <div style="float: left">
				                        <uc2:CalendarControl ID="ccBorrowDate" runat="server" />
			                        </div>
		                        </td>
		                        <td align="left" valign="top">
			                        <asp:RadioButton ID="RbBorrowType1" runat="server" Text="เมื่อแบบพิมพ์เสร็จ" 
				                        GroupName="x" />&nbsp;
		                        </td>
	                        </tr>
	                        <tr>
		                        <td style="width: 60px;" align="left" valign="top">
		                        </td>
		                        <td align="left" valign="top">
			                        <asp:RadioButton ID="RbBorrowType2" runat="server" 
				                        Text="เมื่อแบบพิมพ์เดิมหมดลง" GroupName="x" />&nbsp;
		                        </td>
		                        <td align="left" valign="top">
			                        <asp:RadioButton ID="RbBorrowType3" runat="server" Text="เมื่อแบบพิมพ์ใหม่เสร็จ โดยยกเลิกแบบพิมพ์เดิม"
				                        GroupName="x" />&nbsp;
		                        </td>
	                        </tr>
	                        <tr>
		                        <td style="width: 60px;" align="left" valign="top">
			                        หมายเหตุ
		                        </td>
		                        <td align="left" colspan="3">
			                        <asp:TextBox ID="TbRemark2" runat="server" Width="685px" Height="60px" 
				                        TextMode="MultiLine"></asp:TextBox>
		                        </td>
                                <td></td>
	                        </tr>
                        </table>
                    <asp:Button ID="BtnOK" runat="server" Text="ตกลง" OnClientClick="return BtnOK_Click();" onclick="BtnOK_Click" />
                    <asp:Button ID="BtnCancel" runat="server" Text="ยกเลิก" 
                        OnClientClick="window.close();return false;" />
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </center>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnRefreshSelect" 
                                    runat="server" 
                                    style="display:none;" 
                                    ClientIDMode="Static" 
                                    OnClick="btnRefreshSelect_Click"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    </form>
    <script type="text/javascript">

        function OrderQuantity(tbBorrowMonthQuantity, tbBorrowFirstQuantity, tbUnitQuantity) {
            var borrowMonthQuantity = document.getElementById(tbBorrowMonthQuantity).value;
            var borrowFirstQuantity = document.getElementById(tbBorrowFirstQuantity).value;

            var TbUnitQuantity = document.getElementById(tbUnitQuantity);

            if (borrowMonthQuantity == "") borrowMonthQuantity = 0;
            if (borrowFirstQuantity == "") borrowFirstQuantity = 0;

            borrowMonthQuantity = parseInt(borrowMonthQuantity);
            borrowFirstQuantity = parseInt(borrowFirstQuantity);

            var result = borrowMonthQuantity * 6 + borrowFirstQuantity;

            TbUnitQuantity.value = result.toString();
        }

        function BtnOK_Click() {

            var rbFormTypeNew = document.getElementById('<%= RblFormType.ClientID %>_0').checked;
            var rbFormTypeOld = document.getElementById('<%= RblFormType.ClientID %>_1').checked;

            if (rbFormTypeOld) {
                if (document.getElementById('<%= TbOldFormPrintCode.ClientID %>').value == '') {
                    alert('กรุณาระบุรหัสแบบพิมพ์และขื่อแบบพิมพ์');
                    return false;
                }
            }

            if (rbFormTypeNew) {
                if (document.getElementById('<%= TbNewFormPrintCode.ClientID %>').value == '') {
                    alert('กรุณาระบุรหัสแบบพิมพ์และขื่อแบบพิมพ์');
                    return false;
                }
            }

            return true;
        }

        document.getElementById('btnRefreshSelect').style.display = 'none';
    </script>
</body>
</html>
