<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempRPT_Routine.aspx.cs" Inherits="GPlus.PRPO.TempRPT_Routine" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title></title>

    <script type="text/javascript">


        function stopKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 8) && (node.type != "text")) { return false; }
        }

        document.onkeypress = stopKey;


    </script>




</head>
<body onkeydown="return stopKey()">

    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Print" />
        <asp:HiddenField ID="hid_SummaryReq_id" runat="server" />
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            GroupTreeImagesFolderUrl="" Height="1237px" 
            ReportSourceID="CrystalReportSource1" ToolbarImagesFolderUrl="" 
            ToolPanelView="None" ToolPanelWidth="200px" Width="1675px" 
            HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" />
        
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="Report_Test_3.rpt">
            </Report>
        </CR:CrystalReportSource>

        <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" 
            GroupTreeImagesFolderUrl="" Height="1237px" 
            ReportSourceID="CrystalReportSource2" ToolbarImagesFolderUrl="" 
            ToolPanelView="None" ToolPanelWidth="200px" Width="1675px" 
            HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" />
        <CR:CrystalReportSource ID="CrystalReportSource2" runat="server">
            <Report FileName="Report_Test_2.rpt">
            </Report>
        </CR:CrystalReportSource>
    
    </div>
    </form>
</body>
</html>
