<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarDependOnMonthYearControl.ascx.cs" Inherits="GPlus.UserControls.CalendarDependOnMonthYearControl" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<script src="../Script/js_calendardateUser2.js" type="text/javascript"></script>
<script src="../Script/js_checkTime.js" type="text/javascript"></script>
<script src="../Script/js_checkDate.js" type="text/javascript"></script>
<script src="../Script/js_checkStr.js" type="text/javascript"></script>

<table cellpadding="0" cellspacing="0" id="tblCalendar" runat="server">
    <tr>
        <td style="text-align: left; width: 80px" align="left" valign="middle">
            <asp:TextBox ID="txtDate" Enabled="true" runat="server" Width="76" MaxLength="13" onblur="if(!isDate(this.value))this.focus();" />
            <asp:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server" 
                TargetControlID="txtDate"
                Mask="99/99/9999"
                MessageValidatorTip="true"
                MaskType="Date"
                DisplayMoney="Left"
                AcceptNegative="Left"
                ErrorTooltipEnabled="True">
            </asp:MaskedEditExtender>
        </td>
        <td align="left">
            <img src="../images/Commands/calendar_selector.gif" onclick="popUpCalendar(this, document.getElementById('<%= txtDate.ClientID %>'), 'dd/mm/yyyy')" style="cursor: pointer;" />
            <%--<asp:ImageButton ID="imgCalendar" runat="server" ImageUrl="~/images/Commands/calendar_selector.gif" style="cursor: pointer;"
                OnClientClick="popUpCalendar(document.getElementById('<%= imgCalendar.ClientID %>'), document.getElementById('<%= txtDate.ClientID %>'), 'dd/mm/yyyy'); retutn false;" />--%>
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" color="red"
                Visible="false" Display="Dynamic" SetFocusOnError="true" ErrorMessage="กรุณาระบุข้อมูล"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
<script type="text/javascript">
    var dtCh = "/";
    var minYear = 2400;
    var maxYear = 2700;

    function setMonth(month) {
        monthChanged = parseInt(month);
    }

    function setYear(year) {
        yearChanged = parseInt(year);
    }

    function isInteger(s) {
        var i;
        for (i = 0; i < s.length; i++) {
            // Check that current character is number.
            var c = s.charAt(i);
            if (((c < "0") || (c > "9"))) return false;
        }
        // All characters are numbers.
        return true;
    }

    function stripCharsInBag(s, bag) {
        var i;
        var returnString = "";
        // Search through string's characters one by one.
        // If character is not in bag, append to returnString.
        for (i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if (bag.indexOf(c) == -1) returnString += c;
        }
        return returnString;
    }

    function daysInFebruary(year) {
        // February has 29 days in any year evenly divisible by four,
        // EXCEPT for centurial years which are not also divisible by 400.
        return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
    }
    function DaysArray(n) {
        for (var i = 1; i <= n; i++) {
            this[i] = 31
            if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
            if (i == 2) { this[i] = 29 }
        }
        return this
    }

    function isDate(dtStr) {
        if (dtStr.replace('__/__/____', '').length == 0)
            return true;

        var daysInMonth = DaysArray(12)
        var pos1 = dtStr.indexOf(dtCh)
        var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
        var strDay = dtStr.substring(0, pos1)
        var strMonth = dtStr.substring(pos1 + 1, pos2)
        var strYear = dtStr.substring(pos2 + 1)
        strYr = strYear
        if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
        if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
        for (var i = 1; i <= 3; i++) {
            if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
        }
        month = parseInt(strMonth)
        day = parseInt(strDay)
        year = parseInt(strYr)
        if (pos1 == -1 || pos2 == -1) {
            alert("กรุณาระบุรูปแบบวันที่เป็น : dd/mm/yyyy")
            return false
        }
        if (strMonth.length < 1 || month < 1 || month > 12) {
            alert("กรุณาระบุเดือนให้ถูกต้อง")
            return false
        }
        if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
            alert("กรุณาระบุวันที่ให้ถูกต้อง")
            return false
        }
        if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
            alert("กรุณาระบุปีตั้งแต่ " + minYear + " ถึง " + maxYear)
            return false
        }
        if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
            alert("กรุณาระบุวันที่ให้ถูกต้อง")
            return false
        }
        return true
    }
</script>