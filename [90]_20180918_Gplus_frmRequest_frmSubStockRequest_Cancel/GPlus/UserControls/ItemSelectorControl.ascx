<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemSelectorControl.ascx.cs"
    Inherits="GPlus.UserControls.ItemSelectorControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../Script/default.css" rel="stylesheet" type="text/css" />
<script src="../Script/custom.js" type="text/javascript"></script>
<script type="text/javascript">
    function CheckValidData() {
        if (document.getElementById("hdStatus").value == "0" && trim(document.getElementById("txtProduct").value) != "") {
            alert("คุณเลือกรายการไม่ถูกต้อง");
            document.getElementById("txtProduct").value = "";
        }
    }

    function SelectValue() {
        document.getElementById("hdStatus").value = "1";
        GPlus.autocomplete.GetItemID(document.getElementById("txtProduct").value, callback);
    }

    function AssignStatus() {
        document.getElementById("hdStatus").value = "0";
    }

    function callback(msg) {
        document.getElementById("hdItemID").value = msg;
    }
</script>
<asp:HiddenField ID="hdItemID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdStatus" runat="server" Value="0" ClientIDMode="Static" />
<asp:TextBox ID="txtProduct" runat="server" Width="190" autocomplete="off" ClientIDMode="Static"
    onblur="CheckValidData();" onkeypress="AssignStatus();"></asp:TextBox>
<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="กรุณาระบุวัสดุอุปกรณ์"
    ControlToValidate="txtProduct" ForeColor="Red" Enabled="false">*</asp:RequiredFieldValidator>
<asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
</asp:ValidatorCalloutExtender>
<asp:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx" ID="autoComplete1"
    TargetControlID="txtProduct" ServicePath="~/autocomplete.asmx" ServiceMethod="GetItemCompletionList"
    MinimumPrefixLength="2" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" DelimiterCharacters=";, :"
    OnClientItemSelected="SelectValue">
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
