function autotime(textb) {
    var tvalue = textb.value;
    var mess = '';
    var isFormat = false;
    var temp0, temp1;
    if (tvalue.length != 0) {
        if (tvalue.substr(0, 1) != 'H') {
            if (tvalue.length == 5) {
                if (tvalue.charAt(2) == ':') {
                    var temp = new Array();
                    temp = tvalue.split(':');
                    temp0 = temp[0];
                    temp1 = temp[1];
                    isFormat = IsHHMM(temp0, temp1);
                }
                else {
                    isFormat = false;
                }
            }
            else if (tvalue.length == 4) {
                if (tvalue.charAt(1) == ':') {
                    tvalue = '0' + tvalue;
                    var temp = new Array();
                    temp = tvalue.split(':');
                    temp0 = temp[0];
                    temp1 = temp[1];
                    isFormat = IsHHMM(temp0, temp1);
                }
                else {
                    temp0 = tvalue.substr(0, 2);
                    temp1 = tvalue.substr(2, 2);
                    isFormat = IsHHMM(temp0, temp1);
                }
            }
            else if (tvalue.length == 3) {
                tvalue = '0' + tvalue;
                temp0 = tvalue.substr(0, 2);
                temp1 = tvalue.substr(2, 2);
                isFormat = IsHHMM(temp0, temp1);
            }
            else {
                isFormat = false;
            }
            outPutHHMM(temp0, temp1, textb, isFormat);
            //document.form1.TextM.value = mess;
        }
        else {
            textb.value = 'HH:MM';
        }
    }
}

function autotime_blank(textb) {
    autotime(textb);
    if (textb.value == 'HH:MM') {
        textb.value = '';
    }
}

function IsNumeric(strString)
//  check for valid numeric strings	
{
    var strValidChars = "0123456789";
    var strChar;
    var blnResult = true;

    if (strString.length == 0) return false;

    //  test strString consists of valid characters listed above
    for (i = 0; i < strString.length && blnResult == true; i++) {
        strChar = strString.charAt(i);
        if (strValidChars.indexOf(strChar) == -1) {
            blnResult = false;
        }
    }
    return blnResult;
}

function IsHHMM(strH, strM) {
    if (IsNumeric(strH.toString()) == true && IsNumeric(strM.toString()) == true && strH.length == 2 && strM.length == 2) {
        if (parseInt(strH) < 24 && parseInt(strM) < 60) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}
function outPutHHMM(strH, strM, textb, isFormat) {
    if (isFormat == true) {
        textb.value = strH + ':' + strM;
    }
    else {
        mess = 'incorrect fotmat';
        textb.value = 'HH:MM';
        textb.select();
        textb.focus();
    }
}

function check_st_en() {
    var myTable = document.all("gv_data");
    for (var i = 1; i < myTable.rows.length; i++) //setting the incrementor=0, but if you have a header set it to 1
    {
        //        var myTextbox = myTable.rows[i].getElementsByTagName("input");
        var myTextbox = myTable.rows[i].getElementsByTagName("input");

        //var myTextbox2 = myTable.rows[i].getElementsByTagName("text");
        if (myTextbox.length > 0) {
            var tval1 = myTextbox[1].value;
            var tval2 = myTextbox[2].value;
            if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
                if ((isNaN(myTextbox[1].value) || myTextbox[1].value != "") && (isNaN(myTextbox[2].value) || myTextbox[2].value != "")) {
                    var str1 = myTextbox[1].value;
                    var str2 = myTextbox[2].value;
                    var temp1 = str1.substr(0, 2) + str1.substr(3, 2);
                    var temp2 = str2.substr(0, 2) + str2.substr(3, 2);

                    var fromfloat = parseFloat(temp1);
                    var tofloat = parseFloat(temp2);
                    if (fromfloat > tofloat) {
                        myTextbox[2].value = 'HH:MM';
                        myTextbox[2].select();
                        myTextbox[2].focus();
                    }
                    else {   //not
                        //                    myTextbox[1].value = fromfloat.toString() + ':' + tofloat.toString();
                        //                    myTextbox[2].value = fromfloat.toString() + ':' + tofloat.toString();
                    }
                }
                else if ((isNaN(myTextbox[1].value) || myTextbox[1].value == "") && (isNaN(myTextbox[2].value) || myTextbox[2].value != "")) {
                    myTextbox[1].value = 'HH:MM';
                    myTextbox[1].select();
                    myTextbox[1].focus();
                }
                else if ((isNaN(myTextbox[1].value) || myTextbox[1].value != "") && (isNaN(myTextbox[2].value) || myTextbox[2].value == "")) {
                    myTextbox[2].value = 'HH:MM';
                    myTextbox[2].select();
                    myTextbox[2].focus();
                }
                else {
                    myTextbox[1].value = 'HH:MM';
                    myTextbox[2].value = 'HH:MM';
                    myTextbox[1].select();
                    myTextbox[1].focus();
                }
            }
        }
    }
}

function check_st_en_add() {

    var tval1 = document.form1.txt_start_time.value;
    var tval2 = document.form1.txt_end_time.value;
    if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
        if ((isNaN(document.form1.txt_start_time.value) || document.form1.txt_start_time.value != "") && (isNaN(document.form1.txt_end_time.value) || document.form1.txt_end_time.value != "")) {
            var str1 = document.form1.txt_start_time.value;
            var str2 = document.form1.txt_end_time;
            var temp1 = str1.substr(0, 2) + str1.substr(3, 2);
            var temp2 = str2.substr(0, 2) + str2.substr(3, 2);

            var fromfloat = parseFloat(temp1);
            var tofloat = parseFloat(temp2);
            if (fromfloat > tofloat) {
                document.form1.txt_end_time.value = 'HH:MM';
                document.form1.txt_end_time.select();
                document.form1.txt_end_time.focus();
            }
            else {   //not
                //                    myTextbox[0].value = fromfloat.toString() + ':' + tofloat.toString();
                //                    myTextbox[1].value = fromfloat.toString() + ':' + tofloat.toString();
            }
        }
        else if ((isNaN(document.form1.txt_start_time.value) || document.form1.txt_start_time.value == "") && (isNaN(document.form1.txt_end_time.value) || document.form1.txt_end_time.value != "")) {
            document.form1.txt_start_time.value = 'HH:MM';
            document.form1.txt_start_time.select();
            document.form1.txt_start_time.focus();
        }
        else if ((isNaN(document.form1.txt_start_time.value) || document.form1.txt_start_time.value != "") && (isNaN(document.form1.txt_end_time.value) || document.form1.txt_end_time.value == "")) {
            document.form1.txt_end_time.value = 'HH:MM';
            document.form1.txt_end_time.select();
            document.form1.txt_end_time.focus();
        }
        else {
            document.form1.txt_start_time.value = 'HH:MM';
            document.form1.txt_start_time.value = 'HH:MM';
            document.form1.txt_start_time.select();
            document.form1.txt_start_time.focus();
        }
    }
}

function iscorrect() {
    var myTable = document.all("GridView1");
    for (var i = 1; i < myTable.rows.length - 1; i++) //setting the incrementor=0, but if you have a header set it to 1
    {
        var myTextbox = myTable.rows[i].getElementsByTagName("input");
        if (myTextbox.length > 0) {
            var myTextbox1 = myTextbox[0].value;
            var myTextbox2 = myTextbox[1].value;
            if (myTextbox1.length == 5 && myTextbox2.length == 5) {
                var tval1 = myTextbox[0].value;
                var tval2 = myTextbox[1].value;
                if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
                    if (IsHHMM(tval1.substr(0, 2), tval1.substr(3, 2)) && IsHHMM(tval2.substr(0, 2), tval2.substr(3, 2))) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }

}

function iscorrect_nongrid(text1, text2) {
    var myTextbox1 = document.getElementById(text1);
    var myTextbox2 = document.getElementById(text2);
    var myTextboxValue1 = myTextbox1.value;
    var myTextboxValue2 = myTextbox2.value;
    if (myTextboxValue1.length == 5 && myTextboxValue2.length == 5) {
        if (myTextboxValue1.substr(0, 2) != 'HH' && myTextboxValue2.substr(0, 2) != 'HH') {
            if (IsHHMM(myTextboxValue1.substr(0, 2), myTextboxValue1.substr(3, 2)) && IsHHMM(myTextboxValue2.substr(0, 2), myTextboxValue2.substr(3, 2))) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}

function check_st_en_nongrid(text1, text2) {           //document.form1.TextMessage.value =
    var myTextbox1 = document.getElementById(text1);
    var myTextbox2 = document.getElementById(text2);
    var tval1 = myTextbox1.value;
    var tval2 = myTextbox2.value;
    ////if (tval1!='HH:MM'&& tval2!='HH:MM')
    if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
        if ((isNaN(myTextbox1.value) || myTextbox1.value != '') && (isNaN(myTextbox2.value) || myTextbox2.value != '')) {
            var str1 = myTextbox1.value;
            var str2 = myTextbox2.value;
            var temp1 = str1.substr(0, 2) + str1.substr(3, 2);
            var temp2 = str2.substr(0, 2) + str2.substr(3, 2);

            var fromfloat = parseFloat(temp1);
            var tofloat = parseFloat(temp2);
            if (fromfloat > tofloat) {
                myTextbox2.value = 'HH:MM';
                myTextbox2.select();
                myTextbox2.focus();
            }
            else {   //not
                //                    myTextbox[0].value = fromfloat.toString() + ':' + tofloat.toString();
                //                    myTextbox[1].value = fromfloat.toString() + ':' + tofloat.toString();
            }
        }
        else if ((isNaN(myTextbox1.value) || myTextbox1.value == '') && (isNaN(myTextbox2.value) || myTextbox2.value != '')) {
            myTextbox1.value = 'HH:MM';
            myTextbox1.select();
            myTextbox1.focus();
        }
        else if ((isNaN(myTextbox1.value) || myTextbox1.value != '') && (isNaN(myTextbox2.value) || myTextbox2.value == '')) {
            myTextbox2.value = 'HH:MM';
            myTextbox2.select();
            myTextbox2.focus();
        }
        else {
            myTextbox1.value = 'HH:MM';
            myTextbox2.value = 'HH:MM';
            myTextbox1.select();
            myTextbox1.focus();
        }
    }
}

function check_st_en_nongrid2(text1, text2, dateId1, dateId2) {
    var myTextdate1 = document.getElementById(dateId1);
    var myTextdate2 = document.getElementById(dateId2);
    if (myTextdate1.value != '' && myTextdate2.value != '') {
        if (myTextdate1.value == myTextdate2.value) {
            var myTextbox1 = document.getElementById(text1);
            var myTextbox2 = document.getElementById(text2);
            var tval1 = myTextbox1.value;
            var tval2 = myTextbox2.value;
            ////if (tval1!='HH:MM'&& tval2!='HH:MM')
            if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
                if ((isNaN(myTextbox1.value) || myTextbox1.value != '') && (isNaN(myTextbox2.value) || myTextbox2.value != '')) {
                    var str1 = myTextbox1.value;
                    var str2 = myTextbox2.value;
                    var temp1 = str1.substr(0, 2) + str1.substr(3, 2);
                    var temp2 = str2.substr(0, 2) + str2.substr(3, 2);

                    var fromfloat = parseFloat(temp1);
                    var tofloat = parseFloat(temp2);
                    if (fromfloat > tofloat) {
                        myTextbox2.value = 'HH:MM';
                        myTextbox2.select();
                        myTextbox2.focus();
                    }
                    else {   //not
                        //                    myTextbox[0].value = fromfloat.toString() + ':' + tofloat.toString();
                        //                    myTextbox[1].value = fromfloat.toString() + ':' + tofloat.toString();
                    }
                }
                else if ((isNaN(myTextbox1.value) || myTextbox1.value == '') && (isNaN(myTextbox2.value) || myTextbox2.value != '')) {
                    myTextbox1.value = 'HH:MM';
                    myTextbox1.select();
                    myTextbox1.focus();
                }
                else if ((isNaN(myTextbox1.value) || myTextbox1.value != '') && (isNaN(myTextbox2.value) || myTextbox2.value == '')) {
                    myTextbox2.value = 'HH:MM';
                    myTextbox2.select();
                    myTextbox2.focus();
                }
                else {
                    myTextbox1.value = 'HH:MM';
                    myTextbox2.value = 'HH:MM';
                    myTextbox1.select();
                    myTextbox1.focus();
                }
            }
        }
    }
}

function check_st_en_jang(thistext)//for Bajang's page
{
    var myTable = document.all("gridtime");
    for (var i = 1; i < myTable.rows.length - 1; i++) //setting the incrementor=0, but if you have a header set it to 1
    {
        var myTextbox = myTable.rows[i].getElementsByTagName("input");
        if (myTextbox.length > 0) {
            var tval1 = myTextbox[0].value;
            var tval2 = myTextbox[1].value;
            if (tval1.substr(0, 2) != 'HH' && tval2.substr(0, 2) != 'HH') {
                if ((isNaN(myTextbox[0].value) || myTextbox[0].value != "") && (isNaN(myTextbox[1].value) || myTextbox[1].value != "")) {
                    var str1 = myTextbox[0].value;
                    var str2 = myTextbox[1].value;
                    var temp1 = str1.substr(0, 2) + str1.substr(3, 2);
                    var temp2 = str2.substr(0, 2) + str2.substr(3, 2);

                    var fromfloat = parseFloat(temp1);
                    var tofloat = parseFloat(temp2);
                    if (fromfloat > tofloat) {
                        myTextbox[1].value = 'HH:MM';
                        myTextbox[1].select();
                        myTextbox[1].focus();
                    }
                    else {   //not
                        //                    myTextbox[0].value = fromfloat.toString() + ':' + tofloat.toString();
                        //                    myTextbox[1].value = fromfloat.toString() + ':' + tofloat.toString();
                    }
                }
                else if ((isNaN(myTextbox[0].value) || myTextbox[0].value == "") && (isNaN(myTextbox[1].value) || myTextbox[1].value != "")) {
                    myTextbox[0].value = 'HH:MM';
                    myTextbox[0].select();
                    myTextbox[0].focus();
                }
                else if ((isNaN(myTextbox[0].value) || myTextbox[0].value != "") && (isNaN(myTextbox[1].value) || myTextbox[1].value == "")) {
                    myTextbox[1].value = 'HH:MM';
                    myTextbox[1].select();
                    myTextbox[1].focus();
                }
                else {
                    myTextbox[0].value = 'HH:MM';
                    myTextbox[1].value = 'HH:MM';
                    myTextbox[0].select();
                    myTextbox[0].focus();
                }
            }
        }
    }
}