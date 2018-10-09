<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup_ChangePassword.aspx.cs" Inherits="GPlus.Popup_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>เปลี่ยนรหัสผ่าน</title>
    <style type="text/css">
    .tdTitle
    {
        font-size:13px;
        font-weight:bold;    
        
    }
    .tdTitle2
    {
        font-size:15px;
        font-weight:bold; 
        color:#FF0080;   
        
    }
        .style1
        {
            font-size: 13px;
            font-weight: bold;
            width: 156px;
        }
        .style2
        {
            width: 156px;
        }
    
    </style>
</head>
<body style="background-image:url('images/Stock/bg.jpg')">
    <form id="form1" runat="server">
    <div style="text-align:center;">
 
        <table cellpadding="5" style="margin:50px 0px 0px 20px">
            <tr style="height:60px">
                <td class="tdTitle2" colspan="4" align="center">เปลี่ยนรหัสผ่าน</td>
            </tr>
            <tr>
                <td align="right" class="tdTitle">ชื่อผู้ใช้งาน</td>
                <td align="left" colspan="2">
                    <asp:TextBox ID="TbUserName" runat="server" Width="200px"></asp:TextBox></td>
                <td align="left"><asp:Label ID="LbUserName" runat="server" ForeColor="Red" Font-Size="Small" Width="110px"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="tdTitle">รหัสผ่านเดิม</td>
                <td align="left" colspan="2"><asp:TextBox ID="TbOldPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox></td>
                <td align="left"><asp:Label ID="LbOldPassword" runat="server" ForeColor="Red" Font-Size="Small" Width="110px"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="tdTitle">รหัสผ่านใหม่</td>
                <td align="left" colspan="2"><asp:TextBox ID="TbNewPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox></td>
                <td align="left"><asp:Label ID="LbNewPassword" runat="server" ForeColor="Red" Font-Size="Small" Width="110px"></asp:Label></td>
            </tr>
            <tr>
                <td align="right" class="tdTitle">ยืนยันรหัสผ่านใหม่</td>
                <td align="left" colspan="2"><asp:TextBox ID="TbConfirmPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox></td>
                <td align="left"><asp:Label ID="LbConfirmPassword" runat="server" ForeColor="Red" Font-Size="Small" Width="120px"></asp:Label></td>
            </tr>
            <tr>
                <td class="style2"></td>
                <td colspan="2">
                    <asp:Button runat="server" SkinID="LoginButton" ID="BtnLogin" Text="บันทึก" 
                        OnClick="BtnLogin_Click" />
                    <asp:Button runat="server" SkinID="LoginButton" ID="BtnCancel" Text="ยกเลิก" OnClientClick="return cancelClick()"/>
                </td>
            </tr>
        </table>

    </div>
    </form>
</body>
<script type="text/javascript">
    window.onload = function () {
        document.getElementById('<%=TbUserName.ClientID %>').onfocus = function () {
            document.getElementById('<%=LbUserName.ClientID %>').innerText = "";
        }

        document.getElementById('<%=TbOldPassword.ClientID %>').onfocus = function () {
            document.getElementById('<%=LbOldPassword.ClientID %>').innerText = "";
        }

        document.getElementById('<%=TbNewPassword.ClientID %>').onfocus = function () {
            document.getElementById('<%=LbNewPassword.ClientID %>').innerText = "";
        }

        document.getElementById('<%=TbConfirmPassword.ClientID %>').onfocus = function () {
            document.getElementById('<%=LbConfirmPassword.ClientID %>').innerText = "";
        }
    }

    function cancelClick() {
        document.getElementById('<%=LbUserName.ClientID %>').innerText = "";
        document.getElementById('<%=LbOldPassword.ClientID %>').innerText = "";
        document.getElementById('<%=LbNewPassword.ClientID %>').innerText = "";
        document.getElementById('<%=LbConfirmPassword.ClientID %>').innerText = "";

        document.getElementById('<%=TbUserName.ClientID %>').value = "";
        document.getElementById('<%=TbOldPassword.ClientID %>').value = "";
        document.getElementById('<%=TbNewPassword.ClientID %>').value = "";
        document.getElementById('<%=TbConfirmPassword.ClientID %>').value = "";

        return false;
    }

</script>
</html>
