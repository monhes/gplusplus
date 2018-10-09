<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageListControl.ascx.cs"
    Inherits="GPlus.UserControls.ImageListControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<asp:UpdatePanel ID="updImg" runat="server"><ContentTemplate>--%>
<table>
    <tr>
        <td valign="top">
            <asp:ListBox ID="lstFile" runat="server" Height="160px" Width="180px" AutoPostBack="true"></asp:ListBox>
        </td>
        <td valign="top">
            <asp:Button ID="btnBrowse" runat="server" Text="Browse" CausesValidation="false"  /><br />
            <asp:Button ID="btnView" runat="server" Text="View" CausesValidation="false" OnClick="btnView_Click" /><br />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="false"
                OnClientClick="return confirm('คุณต้องการลบไฟล์นี้หรือไม่?');" OnClick="btnDelete_Click" /><br />
            <asp:Button ID="btnUp" runat="server" Text="Up" CausesValidation="false" OnClick="btnUp_Click" /><br />
            <asp:Button ID="btnDown" runat="server" Text="Down" CausesValidation="false" OnClick="btnDown_Click"
                Style="height: 26px" />
        </td>
        <td style="padding-left:50px;">
            <asp:Image ID="imgPreview" runat="server" Width="150"  />
        </td>
    </tr>
</table>
<asp:LinkButton ID="btnViewShow" runat="server" Visible="false"></asp:LinkButton>
<asp:ModalPopupExtender ID="mpeUpload" runat="server" TargetControlID="btnBrowse"
    PopupControlID="pnlBrowse" BackgroundCssClass="modalBackground" CancelControlID="btnBrowseCancel"
    DropShadow="true" />
<asp:Panel ID="pnlBrowse" runat="server" Width="600" BackColor="White" Style="display: none">
    <table width="100%">
        <tr>
            <td style="width: 130px;" align="right">
                เลือกไฟล์
            </td>
            <td>
                <asp:FileUpload ID="fudFile" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 130px;" align="right">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnBrowseSave" runat="server" Text="บันทึก" OnClick="btnBrowseSave_Click" CausesValidation="false" />&nbsp;
                <asp:Button ID="btnBrowseCancel" runat="server" Text="ยกเลิก" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<%--</ContentTemplate></asp:UpdatePanel>--%>