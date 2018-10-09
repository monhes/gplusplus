// JScript File

function textonlyAZaz(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\ba-zA-Z\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}
function textonlyGAEdash09_bak(e,textb){
    /*  G-Axx-xxxx-Exxxxxxx-xx  */
    bool = false
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    if(code>=37&&code<=40){
        bool= true;
    }else{
        var character = String.fromCharCode(code);
        var AllowRegex  = /^[\bgaeGAE\-0-9\s]$/;
        if (AllowRegex.test(character)) {
            bool= true;
        }  
    }   
    if(bool){
        if(trim(textb.value).length==0){
            //textb.value = 'G-A';
            insertAtCursor(textb,'G-A');
            //moveToEnd(textb);
        }else if(trim(textb.value).length==5||trim(textb.value).length==19){
            //textb.value += '-';
            insertAtCursor(textb,'-');
            //moveToEnd(textb);
        }else if(trim(textb.value).length==10){
            insertAtCursor(textb,'-E');
            //textb.value += '-E';
            //moveToEnd(textb);
        }
    }
    
    return bool;
}

function textonlyGAEdash09(e,textb){
    bool = false
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    if((code>=96&&code<=105)||(code>=48&&code<=57)){
        beforePosition = GetCursorLocation(textb);
        afterPos = beforePosition;
        beforeLenght = textb.value.length;
        
        newStr = textb.value.replace(/[^0-9]/g, "");
        if(newStr.length>=0){
            newStr = 'G-A'+newStr;
            if(beforePosition<=3){
                afterPos=3;
            }
        }
        if(newStr.length>=5){
            buf = newStr;
            newStr = buf.substr(0,5)+'-'+buf.substr(5);
            if(beforePosition==5){
                afterPos+=1;
            }
        }
        if(newStr.length>=10){
            buf = newStr;
            newStr = buf.substr(0,10)+'-E'+buf.substr(10);
            if(beforePosition==10){
              afterPos+=2;
            }
        }
        if(newStr.length>=19){
            buf = newStr;
            newStr = buf.substr(0,19)+'-'+buf.substr(19);
            if(beforePosition==19){
                afterPos+=1;
            }
        }
        textb.value = newStr;
        
        moveCursor(textb,afterPos);
       


        bool= true;
    } 
    if((code>=37&&code<=40)||code==46||code==8){
        bool= true;
    }
    

    return bool;
}

function CardNOonkeyUp(e,textb){
    textonlyGAEdash09(e,textb);
    bool = false
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    if(code==8){
        beforePosition = GetCursorLocation(textb);
        afterPos = beforePosition;
        beforeLenght = textb.value.length;
        newStr = textb.value.replace(/[^0-9]/g, "");
         
        if(newStr.length>=0){
            newStr = 'G-A'+newStr;
            if(beforePosition<=3){
                Text+=newStr+'<br>';
                afterPos=3;
            }
        }
        if(newStr.length>=5){
            Text+=newStr+'<br>';
            buf = newStr;
            
            newStr = buf.substr(0,5);
            if(trim(buf.substr(5))!=""){
                newStr += '-'+buf.substr(5);
            }else
            if(beforePosition==6){
                afterPos-=1;
            }
        }
        if(newStr.length>=10){
            Text+=newStr+'<br>';
            buf = newStr;
            newStr = buf.substr(0,10);
            if(trim(buf.substr(10))!=""){
                newStr += '-E'+buf.substr(10);
            }else
            if(beforePosition==12){
              afterPos-=2;
            }
        }
        if(newStr.length>=19){
            Text+=newStr+'<br>';
            buf = newStr;
            newStr = buf.substr(0,19);
            if(trim(buf.substr(19))!=""){
                newStr += '-'+buf.substr(19);
            }else
            if(beforePosition==20){
                afterPos-=1;
            }
        }
        textb.value = newStr;
        moveCursor(textb,afterPos);
    }
    if(code==46){
        beforePosition = GetCursorLocation(textb);
        newStr = textb.value.replace(/[^0-9]/g, "");
         
        if(newStr.length>=0){
            newStr = 'G-A'+newStr;
        }
        if(newStr.length>=5){
            Text+=newStr+'<br>';
            buf = newStr;
            
            newStr = buf.substr(0,5);
            if(trim(buf.substr(5))!=""){
                newStr += '-'+buf.substr(5);
            }
        }
        if(newStr.length>=10){
            Text+=newStr+'<br>';
            buf = newStr;
            newStr = buf.substr(0,10);
            if(trim(buf.substr(10))!=""){
                newStr += '-E'+buf.substr(10);
            }
        }
        if(newStr.length>=19){
            Text+=newStr+'<br>';
            buf = newStr;
            newStr = buf.substr(0,19);
            if(trim(buf.substr(19))!=""){
                newStr += '-'+buf.substr(19);
            }
        }
        textb.value = newStr;
        moveCursor(textb,beforePosition);
    }
}

function GetCursorLocation(CurrentTextBox)
{
    var CurrentSelection, FullRange, SelectedRange, LocationIndex = -1;
    if (typeof CurrentTextBox.selectionStart == "number")
    {
        LocationIndex = CurrentTextBox.selectionStart;
    }
    else if (document.selection && CurrentTextBox.createTextRange)
    {
        CurrentSelection=document.selection;
        if(CurrentSelection){
            SelectedRange=CurrentSelection.createRange();
            FullRange=CurrentTextBox.createTextRange();
            FullRange.setEndPoint("EndToStart", SelectedRange);
            LocationIndex=FullRange.text.length;
        }
    }
    return LocationIndex;
}
function insertAtCursor(myField, myValue) {
    //IE support
    if (document.selection) {
        myField.focus();
        sel = document.selection.createRange();
        sel.text = myValue;
    }
    //MOZILLA/NETSCAPE support
    else if (myField.selectionStart || myField.selectionStart == '0') { 
        var startPos = myField.selectionStart;
        var endPos = myField.selectionEnd;
        myField.value = myField.value.substring(0, startPos)+ myValue + myField.value.substring(endPos, myField.value.length);
    } else {
        myField.value += myValue;
    }
}


function fillCardId(textb){
    if(trim(textb.value)==''){
    textb.value += 'G-A';
    }
    moveToEnd(textb);
    return true;
}

function moveToEnd(textb){
    if (textb.createTextRange) 
    { 
        var r = (textb.createTextRange());
        r.moveStart('character', (textb.value.length));
        r.collapse(true);
        r.select();
    }
    return true;
}
function moveCursor(textb,num){
    if (textb.createTextRange) 
    { 
        var r = (textb.createTextRange());
//        r.moveStart('character', (textb.value.length));
        r.moveStart('character', (num));
        r.collapse(true);
        r.select();
    }else if (textb.selectionStart || (textb.selectionStart == '0')) { // Mozilla/Netscape…
        textb.selectionStart = num;
        textb.selectionEnd = num;
    }
    return true;
}
function textonlyAZaz09(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\ba-zA-Z0-9\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textonly09(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0123456789\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textonlyDicemal(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0123456789.\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textonlyDate(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0123456789/\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textonlyTime(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0123456789.:\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textonly09_(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0-9-\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textTel(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b0-9#\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function textreadonly(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    var character = String.fromCharCode(code);
    
    var AllowRegex  = /^[\b\s]$/;
    if (AllowRegex.test(character)) return true;     
    return false; 
}

function Length_Text_Validator(txt, Min, Max)
{
    var text = document.getElementById(txt);
    
     if ((trim(text.value).length < Min) || (trim(text.value).length > Max))
    {
        mesg = "กรุณาระบุข้อความในกล่องข้อความอย่างน้อย " + Min + " ตัวอักษร "
        if (Min != Max)
        {
            mesg = mesg + " แต่ไม่เกิน " + Max+ " ตัวอักษร";            
        }
        alert(mesg);
        text.value = '';
        text.focus();
        return (false);
    }    
    return (true);
}

function Length_Text_Validator2(txt, Min, Max)
{
    var text = document.getElementById(txt);
    
    if (text.value.length != 0)
    {
        if ((trim(text.value).length < Min) || (trim(text.value).length > Max))
        {
            mesg = "กรุณาระบุข้อความในกล่องข้อความอย่างน้อย " + Min + " ตัวอักษร "
            if (Min != Max)
            {
                mesg = mesg + " แต่ไม่เกิน " + Max+ " ตัวอักษร";            
            }
            alert(mesg);
            ///text.value = '';
            text.focus();
            return (false);
        }    
    }
    return (true);
}

// Removes leading whitespaces
function LTrim( value ) {	
	var re = /\s*((\S+\s*)*)/;
	return value.replace(re, "$1");	
}
// Removes ending whitespaces
function RTrim( value ) {	
	var re = /((\s*\S+)*)\s*/;
	return value.replace(re, "$1");	
}
// Removes leading and ending whitespaces
function trim( value ) {	
	return LTrim(RTrim(value));	
}

function passwordChanged(txt_name) {   
    var txt = document.getElementById(txt_name);
    var ChrRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
    if (txt.value!="")
    {
        if (ChrRegex.test(txt.value)) 
        {
            //alert("Password ok !");
        }
        else
        {
            alert("กรุณาระบุรหัสผ่านให้ มีทั้งตัวอักษรและตัวเลข !");
            txt.value = "";
            txt.select();
        }
    }
}

function check00(txt_name) 
{
    //alert(txt_name);
    var txt = document.getElementById(txt_name);
    if (txt.value == "00")
    {
        alert("ระบบไม่อนุญาตให้ใช้รหัส '00' !");       
        txt.value = "";
        txt.select(); 
    }
    else if (txt.value == "000")
    {
        alert("ระบบไม่อนุญาตให้ใช้รหัส '000' !");       
        txt.value = "";
        txt.select(); 
    }   
}

function dateDifference(d1,d2) 
{ 
    var strDate1 = d1; 
    var strDate2 = d2;
    datDate1= Date.parse(strDate1); 
    datDate2= Date.parse(strDate2); 
    dateDiff = ((datDate2-datDate1)/(24*60*60*1000)) ;
    return dateDiff;
} 

function CheckDate1(d1, txt)
{ 
    var text = document.getElementById(txt);
    var mySplitResult = d1.split("/");
    var strDateIn = mySplitResult[1] + "/" + mySplitResult[0] + "/" + mySplitResult[2];    
    var now = new Date();    
    var strNow = now.getMonth()+ 1 + "/" + now.getDate() + "/" + now.getFullYear();
    //alert(dateDifference(strNow, strDateIn));
    
    if (dateDifference(strNow, strDateIn) <= 0)
    {        
        alert("กรุณาระบุวันที่ให้ถูกต้อง");
        text.value = '';
        text.focus();
        return (false);
    }
    return (true);
} 

function multiline_lenght(text, Max)
{
    //var text = document.getElementById(txt);
    if (text.value.length >= Max)   
    {
        text.value = text.value.substring(0,Max);
    }
    else
    {
        
    }
}

//##########################format currency
function checkNumeric(objName,minval, maxval,comma,period,hyphen)
{
	var numberfield = objName;
	if (chkNumeric(objName,minval,maxval,comma,period,hyphen) == false)
	{
		numberfield.select();
		numberfield.focus();
		return false;
	}
	else
	{
		return true;
	}
}

function chkNumeric(objName,minval,maxval,comma,period,hyphen)
{
// only allow 0-9 be entered, plus any values passed
// (can be in any order, and don't have to be comma, period, or hyphen)
// if all numbers allow commas, periods, hyphens or whatever,
// just hard code it here and take out the passed parameters
    var checkOK = "0123456789" + comma + period + hyphen;
    var checkStr = objName;
    var allValid = true;
    var decPoints = 0;
    var allNum = "";

    for (i = 0;  i < checkStr.value.length;  i++)
    {
    ch = checkStr.value.charAt(i);
    for (j = 0;  j < checkOK.length;  j++)
        if (ch == checkOK.charAt(j))
            break;
        if (j == checkOK.length)
        {
            allValid = false;
            break;
        }
        if (ch != ",")
            allNum += ch;
    }
    if (!allValid)
    {	
        alertsay = "Please enter only these values \""
        alertsay = alertsay + checkOK + "\" in the this field."
        alert(alertsay);
        checkStr.value='0';
        return (false);
    }

    // set the minimum and maximum
    var chkVal = allNum;
    var prsVal = parseInt(allNum);
    if (chkVal != "" && !(prsVal >= minval && prsVal <= maxval))
    {
        alertsay = "กรุณากรอกข้อมูลเป็นตัวเลขที่มากกว่า "
        alertsay = alertsay + "หรือเท่ากับ \"" + AddCommas(minval) + "\" และต้องน้อยกว่า"
        alertsay = alertsay + "หรือเท่ากับ \"" + AddCommas(maxval) + "\" ."
        alert(alertsay);
        return (false);
    }
}

function FormatCurrency(objNum)
      {
            var num = objNum.value;
            var ent, dec;
            if (num != '' && num != objNum.oldvalue)
            {
                  num = MoneyToNumber(num);
                  if (isNaN(num))
                  {
                        objNum.value = objNum.oldvalue;
                  }
                  else
                  {
                        if (event.keyCode == 190 || !isNaN(num.split('.')[1]))
                        {
                              objNum.value = AddCommas(num.split('.')[0])+'.'+num.split('.')[1];
                        }
                        else
                        {
                              objNum.value = AddCommas(num.split('.')[0]);
                        }
                        objNum.oldvalue = objNum.value;
                  }
            }
      }
      
      function MoneyToNumber(num)
      {
            return (num.replace(/,/g, ''));
            
      }

      function AddCommas(number)
      {
            number = '' + number;
            if (number.length > 3)
            {
                  var mod = number.length % 3;
                  var output = (mod > 0 ? (number.substring(0,mod)) : '');
                  for (i=0 ; i < Math.floor(number.length / 3); i++)
                  {
                        if ((mod == 0) && (i == 0))
                        {
                              output += number.substring(mod + (3 * i), mod + (3 * (i + 1)));
                        }
                        else
                        {
                        output += ',' + number.substring(mod + (3 * i), mod + (3 * (i + 1)));
                        }
                  }
                  return (output);
            }
            else
            {
                  return (number);
            }
      }
//4444444444444444444444444
function NumberToMoney_old(num){
    //alert(num);
    return num.toFixed(2).toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g,'$1,').split('').reverse().join('').replace(/^[\,]/,'');
}
//555555555555555555555555555555
function NumberToMoney(num) {
//parseFloat(MoneyToNumber(this.value))
num = MoneyToNumber(num);
num = parseFloat(num);
//
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
return (((sign)?'':'-') + '' + num + '.' + cents);
}

function NumberToInt(num) {
//parseFloat(MoneyToNumber(this.value))
num = MoneyToNumber(num);
num = parseFloat(num);
//
num = num.toString().replace(/\$|\,/g,'');
if(isNaN(num))
num = "0";
sign = (num == (num = Math.abs(num)));
num = Math.floor(num*100+0.50000000001);
cents = num%100;
num = Math.floor(num/100).toString();
if(cents<10)
cents = "0" + cents;
for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
num = num.substring(0,num.length-(4*i+3))+','+
num.substring(num.length-(4*i+3));
return (((sign)?'':'-') + '' + num );
}
//##########################format currency


//#################### For Check Date Input
// VERSION 1.0
function validateNumKey ()
{
 var inputKey =  event.keyCode;
 var returnCode = true;
 
 if ( inputKey > 47 && inputKey < 58 ) // numbers
 {
  return;
 }
 else
 {
  returnCode = false;
  event.keyCode = 0;
 }
 event.returnValue = returnCode;
}
 
function addDashes2(tbox)
{
 var tb = document.getElementById(tbox);
 
 var currValue = tb.value;
 var a = currValue.split ("/").join("");
 
 if ( a.length > 3 )
    tb.value = a.substr(0,2) + "/" + a.substr(2,2) + "/" + a.substr(4);
 else
    if ( a.length > 1 )
    tb.value = a.substr(0,2) + "/" + a.substr(2)
}

// VERSION 2
function checkDate(tbox) {
    var fld = document.getElementById(tbox);
    var mo, day, yr;
    var entry = fld.value;
    var reLong = /\b\d{1,2}[\/-]\d{1,2}[\/-]\d{4}\b/;
    var reShort = /\b\d{1,2}[\/-]\d{1,2}[\/-]\d{2}\b/;
    var valid = (reLong.test(entry)) || (reShort.test(entry));
    if (valid) {
        var delimChar = (entry.indexOf("/") != -1) ? "/" : "-";
        var delim1 = entry.indexOf(delimChar);
        var delim2 = entry.lastIndexOf(delimChar);
        mo = parseInt(entry.substring(0, delim1), 10);
        day = parseInt(entry.substring(delim1+1, delim2), 10);
        yr = parseInt(entry.substring(delim2+1), 10);
        // handle two-digit year
        if (yr < 100) {
            var today = new Date();
            // get current century floor (e.g., 2000)
            var currCent = parseInt(today.getFullYear() / 100) * 100;
            // two digits up to this year + 15 expands to current century
            var threshold = (today.getFullYear() + 15) - currCent;
            if (yr > threshold) {
                yr += currCent - 100;
            } else {
                yr += currCent;
            }
        }
        var testDate = new Date(yr, mo-1, day);
        if (testDate.getDate() == day) {
            if (testDate.getMonth() + 1 == mo) {
                if (testDate.getFullYear() == yr) {
                    // fill field with database-friendly format
                    fld.value = mo + "/" + day + "/" + yr;
                    return true;
                } else {
                    alert("There is a problem with the year entry.");
                }
            } else {
                alert("There is a problem with the month entry.");
            }
        } else {
            alert("There is a problem with the date entry.");
        }
    } else {
        alert("Incorrect date format. Enter as mm/dd/yyyy.");
    }
    return false;
}