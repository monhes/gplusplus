<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="GPlus.WebForm1" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Script/default.css" rel="stylesheet" type="text/css" />
    <script src="../Script/custom.js" type="text/javascript"></script>
    <script src="autocomplete.asmx/js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckValidData() {
            if (document.getElementById("hdStatus").value == "0" && trim(document.getElementById("txtProduct").value) != "") {
                alert("คุณเลือกรายการไม่ถูกต้อง");
                document.getElementById("txtProduct").value = "";
            }
        }

        function SelectValue() {
            document.getElementById("hdStatus").value = "1";
            GPlus.autocomplete.HelloWorld(callback);
        }
        function callback(msg) {
            alert(msg);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="scMain" runat="server">
        <Services>
            <asp:ServiceReference Path="~/autocomplete.asmx" />
        </Services>
    </asp:ToolkitScriptManager>
    <asp:HiddenField ID="hdStatus" runat="server" Value="0" />
    <asp:TextBox ID="txtPw" runat="server" TextMode="Password" AutoCompleteType="None" autocomplete="off"></asp:TextBox>
    <asp:TextBox ID="txtProduct" runat="server" Width="190" autocomplete="off" ClientIDMode="Static" onblur="CheckValidData();"></asp:TextBox>
    <asp:AutoCompleteExtender
                runat="server" 
                BehaviorID="AutoCompleteEx"
                ID="autoComplete1" 
                TargetControlID="txtProduct"
                ServicePath="autocomplete.asmx" 
                ServiceMethod="GetItemCompletionList"
                MinimumPrefixLength="2" 
                CompletionInterval="1000"
                EnableCaching="true"
                CompletionSetCount="20"
                CompletionListCssClass="autocomplete_completionListElement" 
                CompletionListItemCssClass="autocomplete_listItem" 
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                DelimiterCharacters=";, :" OnClientItemSelected="SelectValue">
                <Animations>
                    <OnShow>
                        <Sequence>
                            <%-- Make the completion list transparent and then show it --%>
                            <OpacityAction Opacity="0" />
                            <HideAction Visible="true" />
                            
                            <%--Cache the original size of the completion list the first time
                                the animation is played and then set it to zero --%>
                            <ScriptAction Script="
                                // Cache the size and setup the initial size
                                var behavior = $find('AutoCompleteEx');
                                if (!behavior._height) {
                                    var target = behavior.get_completionList();
                                    behavior._height = target.offsetHeight - 2;
                                    target.style.height = '0px';
                                }" />
                            
                            <%-- Expand from 0px to the appropriate size while fading in --%>
                            <Parallel Duration=".4">
                                <FadeIn />
                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                            </Parallel>
                        </Sequence>
                    </OnShow>
                    <OnHide>
                        <%-- Collapse down to 0px and fade out --%>
                        <Parallel Duration=".4">
                            <FadeOut />
                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                        </Parallel>
                    </OnHide>
                </Animations>
            </asp:AutoCompleteExtender>
    </form>
</body>
</html>
