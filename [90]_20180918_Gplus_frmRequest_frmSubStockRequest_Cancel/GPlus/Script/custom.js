function open_popup(URL, MyWeight, MyHeight, windowName, showScroll, resizeAble, statusbar) {
    if (windowName == null) windowName = "displayWindow";
    if (showScroll == null) showScroll = "yes";
    if (resizeAble == null) resizeAble = "yes";
    if (statusbar == null) statusbar = "no";

    msgWindow = window.open(URL, windowName, "location=no,menubar=no,scrollbars=" + showScroll + ",resizable=" + resizeAble + ",status=" + statusbar + ",left=" + (screen.width - MyWeight) / 2 + "," +
					"top=" + (screen.height - MyHeight) / 2 + "," +
					"width=" + MyWeight + ",height=" + MyHeight);

    if(msgWindow)
        msgWindow.focus();
}
function open_popup_full(aURL, aWinName) {
    var wOpen;
    var sOptions;

    sOptions = 'location=no,status=yes,menubar=no,scrollbars=yes,resizable=yes,toolbar=no';
    sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString();
    sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString();
    sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0';

    wOpen = window.open('', aWinName, sOptions);
    wOpen.location = aURL;
    wOpen.focus();
    wOpen.moveTo(0, 0);
    wOpen.resizeTo(screen.availWidth, screen.availHeight);
    if(wOpen)
        wOpen.focus();
}

function open_dialog(URL, intWidth, intHeight, showScroll, args, resizeAble) {
    if (!showScroll) showScroll = "yes";
    if (!resizeAble) resizeAble = "no";
    var oRet = window.showModalDialog(URL, args, 'status:no;dialogWidth:' + intWidth + 'px;dialogHeight:' + intHeight + 'px;dialogHide:true;help:no;scroll:' + showScroll + ';resizable:' + resizeAble)
    return oRet;
}
function trim(strTmp) {
    return strTmp.replace(/^\s*|\s*$/g, "");
}
//--    sg_HookupEvent(document, "onmousedown", "sg_ToolsHideSubItem('"+ strID +"')");
function HookupEvent(control, eventType, functionPrefix) {
    var ev;
    eval("ev = control." + eventType + ";");
    if (typeof (ev) == "function") {
        ev = ev.toString();
        ev = ev.substring(ev.indexOf("{") + 1, ev.lastIndexOf("}"));
    }
    else {
        ev = "";
    }
    var func;
    if (navigator.appName.toLowerCase().indexOf('explorer') > -1) {
        func = new Function(ev + " " + functionPrefix);
    }
    else {
        func = new Function("event", ev + " " + functionPrefix);
    }
    eval("control." + eventType + " = func;");
}

function CancelKeyPaste(sender) {
    event.returnValue = false;
    return false;
}

function NumberBoxKeyPress(event, dp, dc, n) {
    var canEdit = false;
    var myString = new String(event.srcElement.value);
    var pntPos = myString.indexOf(String.fromCharCode(dc));
    if (pntPos > 13)
        return false;
    var keyChar = window.event.keyCode;
    var varDecimal = myString.length - pntPos - 1;
    if ((keyChar < 48) || (keyChar > 57)) {
        if (keyChar == dc) {
            if ((pntPos != -1) || (dp < 1)) {
                return false;
            }
        }
        else
            if (((keyChar == 45) && (!n)) || (keyChar != 45))
                return false;
    }
    else {
        if (pntPos > 0 && varDecimal > dp && !canEdit)
            return false;
    }
    canEdit = false;
    return true;
}

function NumberBoxKeyUp(event, dp, dc, n) {
    var myString = new String(event.srcElement.value);
    var keyChar = window.event.keyCode;
    var posIndex = myString.lastIndexOf(String.fromCharCode(45));
    var strOne = myString.substring((0), 1);
    var strAfterOne = myString.substring((1), myString.length);
    var strExpr = /-/gi;
    if ((posIndex > 0) && ((keyChar == 109) || (keyChar == 189))) { event.srcElement.value = (strOne + strAfterOne.replace(strExpr, '')); }
}

function cancelBack() {
    if ((event.keyCode == 8 ||
           (event.keyCode == 37 && event.altKey) ||
           (event.keyCode == 39 && event.altKey))
            &&
           (event.srcElement.form == null || event.srcElement.isTextEdit == false)
          ) {
        event.cancelBubble = true;
        event.returnValue = false;
    }
}


function FormatCurrency(id) {
    var num = id.value.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    id.value = (((sign) ? '' : '-') + num + '.' + cents);
}

function GetNum(objName) {
    var num = document.getElementById(objName).value.toString().replace(/\$|\,/g, '');
    if (trim(num) == "") num = 0;
    else num = num * 1;
    return num;
}


function formatBaht(val) {
    if (val == "" || val == null || val == "NULL") 
    {
        if (val == 0)
            return "0.00";
        return val;
    }

    //Split Decimals
    var arrs = val.toString().split(".");
    //Split data and reverse
    var revs = arrs[0].split("").reverse().join("");
    var len = revs.length;
    var tmp = "";
    for (i = 0; i < len; i++) 
    {
        if (i > 0 && (i % 3) == 0) 
        {
            tmp += "," + revs.charAt(i);
        } 
        else 
        {
            tmp += revs.charAt(i);
        }
    }

    //Split data and reverse back
    tmp = tmp.split("").reverse().join("");
    //Check Decimals
    if (arrs.length > 1 && arrs[1] != undefined) 
    {
        var p = val.toFixed(2).split(".");
        tmp += "." + p[1];
    }
    else tmp += ".00";
    return tmp;
}
//function formatBaht(num) {
//    var p = num.toFixed(4).split(".");
//    return p[0].split("").reverse().reduce(function (acc, num, i, orig) {
//        return num + (i && !(i % 3) ? "," : "") + acc;
//        }, "") + "." + p[1];
//}