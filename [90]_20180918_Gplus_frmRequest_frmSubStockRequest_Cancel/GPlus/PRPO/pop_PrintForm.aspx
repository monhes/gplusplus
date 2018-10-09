<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_PrintForm.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="GPlus.PRPO.pop_PrintForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ระบุการขอแบบพิมพ์</title>
    <script src="../Script/custom.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 104px;
        }
    </style>
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
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
                    <asp:LinkButton ID="btnRefreshSelect" runat="server" Text="" style="display:none;"
                     onclick="btnRefreshSelect_Click" ClientIDMode="Static"></asp:LinkButton>
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                        OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                        <asp:ListItem Text="แบบพิมพ์ใหม่" Value="0"></asp:ListItem>
                        <asp:ListItem Text="แบบพิมพ์เดิม" Selected="True" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Panel ID="pnlNew" runat="server" Visible="false">
                        <fieldset>
                            <legend>แบบพิมพ์ใหม่</legend>
                            <table width="100%">
                                <tr>
                                    <td align="left" class="style1">
                                        ฝ่าย/ทีม
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDepartment" Enabled="false" runat="server" Width="300"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style1">
                                        แบบพิมพ์ใหม่
                                    </td>
                                    <td align="left">
                                        <asp:HiddenField ID="hdID1" runat="server" />
                                        <asp:HiddenField ID="hdPackID1" runat="server" />
                                        รหัสแบบพิมพ์&nbsp;<asp:TextBox ID="txtFormPrintCode" runat="server" 
                                            MaxLength="20" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>&nbsp;
                                        ชื่อแบบพิมพ์&nbsp;<asp:TextBox ID="txtFormPrintName" runat="server" MaxLength="100" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
                                        <asp:ImageButton ID="btnSelect1" runat="server" ImageUrl="~/images/Commands/view.png" />
                                        <%--<asp:LinkButton ID="btnRefreshSelect" runat="server" Text="" style="display:none;"
                                            onclick="btnRefreshSelect_Click" ClientIDMode="Static"></asp:LinkButton>--%>
                                        <asp:HiddenField ID="hdPrice" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style1">
                                        รูปแบบ
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblFormat" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ตามที่แนบ" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="กรุณาออกแบบให้" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style1">
                                        กระดาษ
                                    </td>
                                    <td align="left">
                                        ชนิด&nbsp;<asp:TextBox ID="txtPaperType" runat="server" MaxLength="100"></asp:TextBox>&nbsp; 
                                        สี&nbsp;<asp:TextBox ID="txtPaperColor" runat="server" MaxLength="100"></asp:TextBox>
                                        ความหนา&nbsp;<asp:TextBox ID="txtPaperGram" runat="server" MaxLength="100"></asp:TextBox>แกรม
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style1">
                                        ตัวอักษร
                                    </td>
                                    <td align="left">
                                        พิมพ์&nbsp;<asp:TextBox ID="txtFontColor" runat="server" MaxLength="100"></asp:TextBox>&nbsp;สี
                                        &nbsp;&nbsp;ขนาด&nbsp;&nbsp;<asp:TextBox ID="tbSizeDetailNew" runat="server" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="style1">
                                        ลักษณะแบบพิมพ์
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblPrintType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ฟอร์มคอมพิวเตอร์" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="แผ่น" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="เข้าชุด" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="เข้าเล่ม" Value="1"></asp:ListItem>
                                            <%--<asp:ListItem Text="แผ่นพับ"  Value="4"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="style1">
                                        การเบิกใช้
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButton ID="rblFormBorrowType1" runat="server" Text="เบิกใช้ครั้งเดียวหมด จำนวนพิมพ์"
                                            GroupName="Y" ClientIDMode="Static" onclick="clearBorrowTypeNew(this);"/>&nbsp;
                                        <asp:TextBox ID="txtBorrowQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>
                                        <asp:DropDownList ID="ddlBorrowUnit1" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="style1">
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButton ID="rdbFormBorrowType2" runat="server" Text="เก็บสต๊อก ปริมาณจะใช้ต่อเดือน"
                                            GroupName="Y" ClientIDMode="Static" onclick="clearBorrowTypeNew(this);"/>&nbsp;
                                        <asp:TextBox ID="txtBorrowMonthQuantity" runat="server" Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="calculateOrder('txtBorrowMonthQuantity', 'txtBorrowFirstQuantity', 'txtOredrQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp; เบิกใช้ครั้งแรก
                                        จำนวน<asp:TextBox ID="txtBorrowFirstQuantity" runat="server" Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="calculateOrder('txtBorrowMonthQuantity', 'txtBorrowFirstQuantity', 'txtOredrQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="ddlBorrowTypeUnit2" runat="server" DataTextField="Description" DataValueField="Pack_ID" OnSelectedIndexChanged="ddlBorrowTypeUnit2_IndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        จำนวนสั่ง<asp:TextBox ID="txtOredrQuantity" runat="server" Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="ddlOrederTypeUnit" runat="server" DataTextField="Description" DataValueField="Pack_ID" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="style1">
                                        อื่นๆ
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txtRemark" runat="server" Width="300" Height="60" 
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel ID="pnlOld" runat="server" Visible="true">
                        <fieldset>
                            <legend>แบบพิมพ์เดิม</legend>
                            <table width="100%">
                                <tr>
                                    <td style="width: 130px;" align="left">
                                        ฝ่าย/ทีม
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt1_Department" runat="server"  Width="300" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px;" align="left">
                                        แบบพิมพ์ใหม่
                                    </td>
                                    <td align="left">
                                        <asp:HiddenField ID="hdID2" runat="server" />
                                        <asp:HiddenField ID="hdPackID2" runat="server" />
                                        รหัสแบบพิมพ์&nbsp;<asp:TextBox ID="txt1_FormPrintCode" runat="server" MaxLength="20" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>&nbsp;
                                        ชื่อแบบพิมพ์&nbsp;<asp:TextBox ID="txt1_FormPrintName" runat="server" MaxLength="100" BackColor="WhiteSmoke" onkeypress="return false;"></asp:TextBox>
                                        <asp:ImageButton ID="btnSelect2" runat="server" ImageUrl="~/images/Commands/view.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px;" align="left" valign="top">
                                        การเบิกใช้
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButton ID="rdb1_FormBorrowType1" runat="server" Text="เบิกใช้คร้ังเดียวหมด" GroupName="z" ClientIDMode="Static" onclick="clearBorrowTypeOld(this);"/>&nbsp;
                                        <asp:TextBox ID="txt1_BorrowQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="ddl1_BorrowUnit" runat="server" 
                                            DataTextField="Description" DataValueField="Pack_ID">                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px;" align="left" valign="top">
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:RadioButton ID="rdb1_FormBorrowType2" runat="server" Text="เก็บสต๊อก" GroupName="z" ClientIDMode="Static" onclick="clearBorrowTypeOld(this);"/>&nbsp;
                                        &nbsp;&nbsp;&nbsp; ปริมาณจะใช้ต่อเดือน&nbsp;
                                        <asp:TextBox ID="txt1_MonthQuantity" runat="server" Width="50" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="calculateOrder('txt1_MonthQuantity', 'txtBorrowFirstQuantityOld', 'txt1_OrderQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right" MaxLength="7"></asp:TextBox>&nbsp;
                                        เบิกใช้ครั้งแรก จำนวน<asp:TextBox ID="txtBorrowFirstQuantityOld" runat="server" Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="calculateOrder('txt1_MonthQuantity', 'txtBorrowFirstQuantityOld', 'txt1_OrderQuantity'); return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="ddl1_BorrowUnit2" runat="server" DataTextField="Description" DataValueField="Pack_ID" OnSelectedIndexChanged="ddl1_BorrowUnit2_IndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                         จำนวนสั่ง<asp:TextBox ID="txt1_OrderQuantity" runat="server" Width="50" MaxLength="7" onKeyPress="return NumberBoxKeyPress(event, 0, 46, false)"
                                            onKeyUp="return NumberBoxKeyUp(event, 0, 46, false)" onpaste="return CancelKeyPaste(this)"
                                            Style="text-align: right"></asp:TextBox>&nbsp;
                                        <asp:DropDownList ID="ddl1_OrderUnit2" runat="server" DataTextField="Description" DataValueField="Pack_ID" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" valign="top">
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="chk1_IsRequestModify" runat="server" Text="ขอแก้ไขเปลี่ยนแปลง" />&nbsp;&nbsp;
                                                        ลักษณะการแก้ไข&nbsp;<asp:TextBox ID="tbRequestModifyDesc" runat="server" Width="300"></asp:TextBox>
                                                    </td>
                                                    <%--<td align="right" colspan="2"></td>
                                                    <td align="left"></td>--%>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="chk1_IsFixedContent" runat="server" Text="ข้อความตามตัวอย่างที่แนบ" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:CheckBox ID="chk1_IsPaper" runat="server" Text="กระดาษ" />
                                                        &nbsp; ชนิด&nbsp;<asp:TextBox ID="txt1_PaperType" runat="server" Width="50" MaxLength="100"></asp:TextBox>&nbsp;
                                                        &nbsp; สี&nbsp;<asp:TextBox ID="txt1_PaperColor" runat="server" Width="50" MaxLength="100"></asp:TextBox>&nbsp;
                                                        &nbsp; ความหนา&nbsp;<asp:TextBox ID="txt1_PaperGram" runat="server" Width="50" MaxLength="100"></asp:TextBox>&nbsp;แกรม
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chk1_IsFont" runat="server" Text="ตัวอักษร" />
                                                        &nbsp; พิมพ์&nbsp;<asp:TextBox ID="txt1_FontColor" runat="server"></asp:TextBox>&nbsp;สี&nbsp;
                                                    </td>
                                                    <td align="right">ขนาด</td>
                                                    <td align="left"><asp:TextBox ID="tbSizeDetail" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="width: 130px;" align="left">
                                        ลักษณะแบบพิมพ์
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rbl1_PrintType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="ฟอร์มคอมพิวเตอร์" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="แผ่น" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="เข้าชุด" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="เข้าเล่ม" Value="1"></asp:ListItem>
                                            <%--<asp:ListItem Text="แผ่นพับ"  Value="4"></asp:ListItem>--%>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px;" align="left" valign="top">
                                        อื่นๆ
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="txt1_Remark" runat="server" Width="300" Height="60" 
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 130px;" align="left" valign="top">
                                เริ่มใช้
                            </td>
                            <td align="left" valign="top">
                                <div style="float: left">
                                    <asp:RadioButton ID="rdbBorrowType0" runat="server" Text="ระบุวันที่" GroupName="x" />&nbsp;</div>
                                <div style="float: left">
                                    <uc2:CalendarControl ID="ccNewBorrowDate" runat="server" />
                                </div>
                            </td>
                            <td align="left" valign="top"  colspan="2">
                                <asp:RadioButton ID="rdbBorrowType1" runat="server" Text="เมื่อแบบพิมพ์เสร็จ" GroupName="x" />&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                                <asp:RadioButton ID="rdbBorrowType2" runat="server" Text="เมื่อแบบพิมพ์เดิมหมดลง" GroupName="x" />&nbsp;
                            </td>
                            <td colspan="2" align="left" valign="top">
                                <asp:RadioButton ID="rdbBorrowType3" runat="server" Text="เมื่อแบบพิมพ์ใหม่เสร็จ โดยยกเลิกแบบพิมพ์เดิม"
                                    GroupName="x" />&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;" align="left" valign="top">
                                หมายเหตุ
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRemark2" runat="server" Width="300" Height="60" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnOK" runat="server" Text="ตกลง" onclick="btnOK_Click" />
                    <asp:Button ID="btnCancel1" runat="server" Text="ยกเลิก" OnClientClick="window.close();return false;" />
                </td>
            </tr>
            <tr>
                <td class="tableFooter">
                </td>
            </tr>
        </table>
    </center>
    </form>
    <script type="text/javascript">
        // Begin Green Edit

        function clearBorrowTypeNew(rdb) {
            if (rdb.value == 'rblFormBorrowType1') {
                document.getElementById('txtBorrowMonthQuantity').value = "";
                document.getElementById('txtBorrowFirstQuantity').value = "";
                document.getElementById('txtOredrQuantity').value = "";

                document.getElementById('txtBorrowMonthQuantity').disabled = true;
                document.getElementById('txtBorrowFirstQuantity').disabled = true;
                document.getElementById('ddlBorrowTypeUnit2').disabled = true;
                document.getElementById('txtOredrQuantity').disabled = true;

                document.getElementById('txtBorrowQuantity').disabled = false;
                document.getElementById('ddlBorrowUnit1').disabled = false;
            }
            else if (rdb.value == 'rdbFormBorrowType2') {
                document.getElementById('txtBorrowQuantity').value = "";
                document.getElementById('txtBorrowQuantity').disabled = true;
                document.getElementById('ddlBorrowUnit1').disabled = true;

                document.getElementById('txtBorrowMonthQuantity').disabled = false;
                document.getElementById('txtBorrowFirstQuantity').disabled = false;
                document.getElementById('ddlBorrowTypeUnit2').disabled = false;
                document.getElementById('txtOredrQuantity').disabled = false;
            }
        }


        function clearBorrowTypeOld(rdb) {
            if (rdb.value == 'rdb1_FormBorrowType1') {
                document.getElementById('txt1_MonthQuantity').value = "";
                document.getElementById('txtBorrowFirstQuantityOld').value = "";
                document.getElementById('txt1_OrderQuantity').value = "";

                document.getElementById('txt1_BorrowQuantity').disabled = false;
                document.getElementById('ddl1_BorrowUnit').disabled = false;

                document.getElementById('txt1_MonthQuantity').disabled = true;
                document.getElementById('txtBorrowFirstQuantityOld').disabled = true;
                document.getElementById('txt1_OrderQuantity').disabled = true;
                document.getElementById('ddl1_BorrowUnit2').disabled = true;
            }
            else if (rdb.value == 'rdb1_FormBorrowType2') {
                document.getElementById('txt1_BorrowQuantity').value = "";

                document.getElementById('txt1_BorrowQuantity').disabled = true;
                document.getElementById('ddl1_BorrowUnit').disabled = true;

                document.getElementById('txt1_MonthQuantity').disabled = false;
                document.getElementById('txtBorrowFirstQuantityOld').disabled = false;
                document.getElementById('txt1_OrderQuantity').disabled = false;
                document.getElementById('ddl1_BorrowUnit2').disabled = false;
            }
        }

        function calculateOrder(txtMonthQuantity, txtBorrowFirstQuantity, txtOrderQuantity) {
            var txtMonthQuantity = document.getElementById(txtMonthQuantity).value;
            var txtBorrowFirstQuantity = document.getElementById(txtBorrowFirstQuantity).value;
            var txtOrderQuantity = document.getElementById(txtOrderQuantity);

            if (txtMonthQuantity == "") txtMonthQuantity = 0;
            if (txtBorrowFirstQuantity == "") txtBorrowFirstQuantity = 0;

            txtMonthQuantity = parseInt(txtMonthQuantity);
            txtBorrowFirstQuantity = parseInt(txtBorrowFirstQuantity);

            var result = txtMonthQuantity * 6 + txtBorrowFirstQuantity;

            txtOrderQuantity.value = result.toString();
        }
        // End Green Edit
    </script>
</body>
</html>
