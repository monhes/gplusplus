<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GPlus.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .common {
            position: absolute;
            top: 50%;
            left: 50%;
            border-width: 0px;
            border-style: solid;
        }
    </style>
</head>
<body style="background-image:url('Images/Login/new/cloud.jpg'); background-repeat: repeat; margin:0">
    <div style="position:fixed; width:100%; height:28px; background-image:url('Images/Login/new/bar.png'); top:0px; "></div>
    <div class="common" style="background-image:url('Images/logo_Muangthai.png'); width:100px; height:104px; top:40px; margin-left:-460px;"></div>
    <div class="common" style="background-image:url('Images/logo_gplus.png'); width:112px; height:112px; top:40px; margin-left:300px;"></div>
    <form id="form1" runat="server">
    <div class="common" style="background-image: url('Images/Login/new/Login_bg.png'); 
            background-repeat: no-repeat;             
            margin-top: -294px;
            margin-left: -531px;
            width: 1063px;
            height: 587px;">
        <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />

        <div style="background: url('Images/Login/Login_box.png'); background-repeat: no-repeat; margin-left: 345px; margin-top:175px; width:375px; height: 50px;">
            <asp:TextBox ID="txtUserName" runat="server" Height="32" Width="374" Font-Size="21pt"
                BorderStyle="None" BackColor="Transparent"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="กรุณาระบุผู้ใช้งานระบบ"
                ControlToValidate="txtUserName" ForeColor="Red">*</asp:RequiredFieldValidator>
            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                TargetControlID="RequiredFieldValidator2">
            </asp:ValidatorCalloutExtender>
        </div>

        <div style="background: url('Images/Login/Login_box.png'); background-repeat: no-repeat; margin-left:345px; margin-top:65px; width:375px; height: 50px;">
            <asp:TextBox ID="txtPasword" runat="server" Height="32" Width="374" Font-Size="21pt"
                BorderStyle="None" BackColor="Transparent" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="กรุณาระบุรหัสผ่านผู้ใช้งานระบบ"
                ControlToValidate="txtPasword" ForeColor="Red">*</asp:RequiredFieldValidator>
            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                TargetControlID="RequiredFieldValidator1">
            </asp:ValidatorCalloutExtender>
        </div>
        <table style="margin-left:345px; margin-top: 30px; border:0px; border-style:solid; width:350px">
            <tr>
                <td style="width:40px;">

                </td>
                <td style="width: 250px;">
                    <a href="javascript:void(0);" onclick="Popup_ChangePwd(500, 400);">เปลี่ยนรหัสผ่าน</a>
                </td>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="ตกลง" SkinID="LoginButton" OnClick="btnLogin_Click" />
                </td>
                <td style="width: 30px;">
                    &nbsp;
                </td>
                <td style="width: 30px;">
                    <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" SkinID="LoginButton" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <div style="position:fixed; width:100%; height:28px; background-image:url('Images/Login/new/bar2.png'); bottom:0px; "></div>
    <script type="text/javascript" src="Script/custom.js"></script>
    <script type="text/javascript">
        function Popup_ChangePwd(width, height) {

            var screenHeight    = screen.height;
            var screenWidth     = screen.width;

            var popup = window.open
            (
                'Popup_ChangePassword.aspx',
                'ChangePassword',
                "location=no,menubar=no,scrollbars=no,resizable=no,status=no,left=" + (screenWidth - width) / 2 + ",top=" + (screenHeight - height) / 2 + ",width=" + width + ",height=" + height
            );

            if (popup)
                popup.focus();
        }
    </script>
</body>
</html>
