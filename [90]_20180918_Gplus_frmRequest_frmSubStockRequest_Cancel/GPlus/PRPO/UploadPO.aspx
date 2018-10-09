<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadPO.aspx.cs" Inherits="GPlus.PRPO.UploadPO" %>

<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../Script/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager runat="server" ID="ScriptManager1" />
        <center>
            <table cellpadding="0" cellspacing="0" width="805">
                <tr>
                    <td class="tableHeader" align="left">
                        Upload ใบสั่งซื้อ
                    </td>
                </tr>
                <tr>
                    <td class="tableBody">
                        <table border="0" cellpadding="5">
                            <tr>
                                <td>
                                    เลขที่ใบสั่งซื้อ
                                </td>
                                <td>
                                   <asp:TextBox ID="txtPoCode" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td> 
                                </td>
                                 <td>
                                </td>
                            </tr>
                        </table>
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                       <ContentTemplate>
                        <div id="header" style="padding: 2px">
		                    <span style="width: 80px">&nbsp;เลือกไฟล์&nbsp;</span>
		                    <span id="files" style="display: inline">
                                <input type="file" name="fileUploader" onchange="FileChange();" id="fileUploader" runat="server" />
                                <asp:LinkButton ID="btnUploadAuto" runat="server" Text="" style="display:none;"
                                 onclick="btnUploadAuto_Click" ClientIDMode="Static"></asp:LinkButton>
                            </span>
	                    </div>
                        <br />
                        
                        <asp:GridView runat="server" ID="gvUploadPO" 
                            AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvUploadPO_RowDataBound" OnRowCommand="gvUploadPO_RowCommand"
                            OnRowCreated="gvUploadPO_RowCreated" OnRowDeleting = "gvUploadPO_RowDeleting" SkinId="GvLong">
                            <Columns>
                                <asp:TemplateField HeaderText="ชื่อไฟล์" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnFileName" runat="server" Text='<%# Eval("Upload_FileCut") %>'
                                            CommandName="View"
                                        >
                                        </asp:LinkButton>
                                        <asp:HiddenField ID="hdFileName" runat="server"/>
                                        <asp:HiddenField ID="hdFileType" runat="server"/>
                                        <asp:HiddenField ID="hdPoUpLoadID" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ลบ" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ToolTip="ลบ" ImageUrl="~/images/Commands/delete.png" />
                                        </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Upload_Date" HeaderText="วันที่ Upload" 
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Download_Count" HeaderText="จำนวนครั้ง" 
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Latest_Download_Date" HeaderText="วันที่ล่าสุด"
                                    ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="Latest_Download_by" HeaderText="ชื่อ"
                                    ItemStyle-HorizontalAlign="Center"/>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div id="divBtn" runat="server">
                            <asp:Button ID="btnSubmit" runat="server" Text="บันทึก" OnClientClick="return btnSendClick(this);" OnClick="btnSubmit_Click" />
                            &nbsp; &nbsp; &nbsp; 
                            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />   
                        </div>
                    </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="fileUploader" />
                            <asp:PostBackTrigger ControlID="btnUploadAuto" />
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                            <asp:PostBackTrigger ControlID="btnCancel" />
                        </Triggers>
                    </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="tableFooter"></td>
                </tr>
            </table>
        </center>
    </form>
   
   <script type="text/javascript">
       function FileChange(filename) 
       {
           document.getElementById('<%= btnUploadAuto.ClientID %>').click();
       }

       function btnSendClick(button) {
           document.getElementById('<%= btnSubmit.ClientID %>').onclick = function () { return false; };
           return true;
       }
   </script>

</body>
</html>
