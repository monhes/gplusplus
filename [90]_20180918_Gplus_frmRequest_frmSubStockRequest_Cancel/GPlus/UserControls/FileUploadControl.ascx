<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUploadControl.ascx.cs" Inherits="GPlus.UserControls.FileUploadControl" %>
<asp:Panel ID="Panel1" runat="server" Width="100%" Font-Size="X-Small">
    <asp:Image ID="imgFile" runat="server" Visible="false" /><br />
    <asp:LinkButton ID="hplFilePath" runat="server" Visible="False" 
        CausesValidation="false" cv="1" onclick="hplFilePath_Click1"></asp:LinkButton>
    <asp:LinkButton ID="btnDelete" runat="server" Text="[Delete File]" 
        OnClientClick="return confirm('Are you sure delete this file?');" 
        Visible="false" onclick="btnDelete_Click" CausesValidation="false" cv="1"></asp:LinkButton>
    <br />
    <input id="fileUpload" style="width: 230px;" type="file" size="26" name="File1" runat="server" />
    <asp:Button ID="btnUpload" Width="60px" OnClick="btnUpload_Click"
        runat="server" Text="แสดงรูป" CausesValidation="false" cv="1"></asp:Button>
    <asp:Label ID="lblError" Width="100%" runat="server" ForeColor="Red"></asp:Label>
</asp:Panel>