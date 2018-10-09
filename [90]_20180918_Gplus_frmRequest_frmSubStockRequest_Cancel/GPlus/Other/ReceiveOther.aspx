<%@ Page Language="C#"   AutoEventWireup="true" Inherits="Other_ReceiveOther" 
MasterPageFile="~/MasterPage/Main.Master" MaintainScrollPositionOnPostback="true" enableSessionState="true" Codebehind="ReceiveOther.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Site.css" rel="stylesheet" type="text/css" />
    <script src="jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="../Script/default.css" rel="stylesheet" type="text/css" />
     
     
  

     <script language="JavaScript">

         var i_rowstart = 0;
         var i_rowcross = 1;
         var i_totalPage;
         var pagination = "";
         var int_stock = 1;
         var int_lot = 1;
         var ProductID = "";
         var i_total = 0;
         var pay_id = ""; //รับคืนแผนก

         var ValueLot = new Array();


         Number.prototype.formatMoney = function (c, d, t) {
             var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
             return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
         };

         String.prototype.formatMoney = function (c, d, t) {
             return parseFloat(this).formatMoney(c, d, t);
         }


         $().ready(function () {


            // setStockDropDown();

         });


         function convertToDateByString(val) {
             try {

                 if (val == "") {
                     return "";
                 }

                 var check = 0;
                 var num = 0;
                 var str_day = 0;
                 var str_month = 0;
                 var str_year = 0;

                 for (var i = 0; i < val.toString().length; i++) {

                     if (val.substring(i, i + 1) == '/') {

                         if (check == 0) {

                             str_month = val.substring(0, i);
                             if (str_month.length < 2) {
                                 str_month = "0" + str_month;
                             }

                             num = i;
                             check++;
                         }
                         else if (check == 1) {

                             str_day = val.substring(num + 1, i);
                             if (str_day.length < 2) {
                                 str_day = "0" + str_day;
                             }
                             str_year = val.substring(i + 1, i + 5);

                             i = val.toString().length;

                         }
                     }
                 }

                 return str_day + "/" + str_month + "/" + str_year;
             }
             catch (err) {
                 return "";
             }
         }


         function convertToDateServerByString(val) {
             try {

                 if (val == "") {
                     return "";
                 }

                 var check = 0;
                 var num = 0;
                 var str_day = 0;
                 var str_month = 0;
                 var str_year = 0;

                 for (var i = 0; i < val.toString().length; i++) {

                     if (val.substring(i, i + 1) == '/') {

                         if (check == 0) {

                             str_month = val.substring(0, i);
                             if (str_month.length < 2) {
                                 str_month = "0" + str_month;
                             }

                             num = i;
                             check++;
                         }
                         else if (check == 1) {

                             str_day = val.substring(num + 1, i);
                             if (str_day.length < 2) {
                                 str_day = "0" + str_day;
                             }
                             str_year = val.substring(i + 1, i + 5);

                             i = val.toString().length;

                         }
                     }
                 }

                 return str_month + "/" + str_day + "/" + str_year;
             }
             catch (err) {
                 return "";
             }
         }

         function setStockDropDown()
         {
             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetLocation",
                 contentType: "application/json; charset=utf-8",
                 data: "{'str_stock' : '" + $("#ContentPlaceHolder1_drp_searchStock").val() + "' }",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;
                     var str_drp = "";
                     $("#drp_idStock1_1 >option").remove();
                     while (i < int_max) {
                         str_drp = str_drp + "<option value=" + resultH[i].Value + ">" + resultH[i].Text + "</option>";
                         i++;
                     }
                     $("#drp_idStock1_1").append(str_drp);
   
                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }

         Array.prototype.removeByValue = function (val) {//เพิ่ม propotype ของ Array เข้าไปเพื่อใช้ remove ค่าออกด้วย value
             for (var i = 0; i < this.length; i++) {
                 if (this[i] == val) {
                     this.splice(i, 1);
                     break;
                 }
             }
         }


         Array.prototype.CheckByValue = function (val) {//เพิ่ม propotype ของ Array  ค่าออกด้วย value
             var result = false;
             for (var i = 0; i < this.length; i++) {
                 if (this[i] == val) {
                     result = true;
                 }
             }
             return result;
         }


         ////////////////ฟังก์ชั่นสำหรับ open modal popup//////////////////////      
         function revealModal(divModal) {


             window.onscroll = function () {
                 document.getElementById(divModal).style.top = document.body.scrollTop;
             };
             document.getElementById(divModal).style.display = "block";
             document.getElementById(divModal).style.top = document.body.scrollTop;

             //set height of modal background at runtime
             var winH = document.body.offsetHeight;

             document.getElementById('modalBackground').style.height = winH + 'px';

         }





         function hideModal(divModal) {
            
             $('#' + divModal).hide();

             //             if (divModal == "tablePopup") {

             //                 var currentIFrame = $('#iframe_target');
             //                 if (currentIFrame.contents().find("body #hid_popup").val() != "") {
             //                     clearAll();
             //                 }

             //                 doc = document.getElementById("iframe_target");
             //                 if (doc.document) {
             //                     document.iframe_target.document.body.innerHTML = ""; //Chrome, IE
             //                 } else {
             //                     doc.contentDocument.body.innerHTML = ""; //FireFox
             //                 }

             //             }

         }


         function btn_searchOnClick() {
             GetTableSearch();

         }


         function btn_deleteLotStockOnClick(lot) {

             $("#tableLotStock" + $("#hid_Lot" + lot + "").val() + "").remove();
             var sum = $("#hid_Lot" + lot + "").val() - 1;
             $("#hid_Lot" + lot + "").val(sum);



//             $.ajax({
//                 type: "POST",
//                 url: "jsonService.asmx/deleteLotStockLast",
//                 data: "{'str_lotProduct':'" + $("#txt_productID").val() + "' }",
//                 dataType: "json",
//                 contentType: "application/json; charset=utf-8",
//                 success: function (data) {
//                 },
//                 failure: function (msg) {
//                     alert("error");
//                 }

//             });

             // <input  type='button' value='ลบ' name='btn_lotDelete" + obj + "' id='btn_lotDelete" + obj + "' onclick='JavaScript:btn_deleteLotStockOnClick(" + obj + "," + int_stock + ")' />

         }


         function CreateNewRowStock(obj, cStockQty, cStockID, cStockName) {
             var str_table;
             var str_dropDown;
             int_stock = $("#hid_Lot" + obj + "").val();

             int_stock++;
             str_table = "<table id = 'tableLotStock" + int_stock + "'><tr><td align='right'><div id = 'runningStock" + obj + "_" + int_stock + "'>" + int_stock + ".</div><input type='hidden' name='hid_lotStock' id='hid_lotStock" + obj + "_" + int_stock + "' value='" + int_lot + "' /></td>";
             str_table = str_table + "<td> จำนวน : </td><td><input id = 'txt_qtyStock" + obj + "_" + int_stock + "' value = '" + cStockQty + "' name = 'txt_qtyStock' type ='text' style='width:150px' /></td>";
             str_table = str_table + "<td> รหัสสถานที่ : </td><td>";
             //             str_table = str_table + "<td> รหัสสถานที่ : </td><td><input id = 'txt_idStock" + obj + "_" + int_stock + "' name = 'txt_idStock' type ='text' style='width:150px' /></td>";
             //             str_table = str_table + "<td> <input id = 'txt_nameStock" + obj + "_" + int_stock + "' name = 'txt_nameStock' type ='text'  style='width:150px' /> </td></tr></table>";
             str_dropDown = "drp_idStock" + obj + "_" + int_stock;
             str_table = str_table + "<select name=" + str_dropDown + " id=" + str_dropDown + "> ";
             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetLocation",
                 contentType: "application/json; charset=utf-8",
                 data: "{'str_stock' : '" + $("#ContentPlaceHolder1_drp_searchStock").val() + "' }",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;

                     while (i < int_max) {
                         if (resultH[i].Value == cStockID) {
                             str_table = str_table + "<option value=" + resultH[i].Value + " selected>" + resultH[i].Text + "</option>";
                         }
                         else {
                             str_table = str_table + "<option value=" + resultH[i].Value + ">" + resultH[i].Text + "</option>";
                         }
                         i++;

                     }

                     str_table = str_table + "</select></td></tr></table>";
                     $("#detailStockUnder" + obj + "").append(str_table);
                     

                     //                     $("#hid_lotStock" + obj + "_" + int_stock).val(obj);
                     //                     $("#txt_qtyStock" + obj + "_" + int_stock).val(cStockQty);
                     //                     $("#drp_idStock" + obj + "_" + int_stock).val(cStockID);
                     //$("#txt_nameStock" + obj + "_" + int_stock).val(cStockName);

                     $("#hid_Lot" + obj + "").val(int_stock);

                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });



             //$("#txtCetification" + int_Certification).val(cCetification);



         }

         function CreateNewRowLot(obj, cLotNo, cLotQtc, cLotExpire, cLotPrice, cLotTotal) {
             var str_table;
             int_lot++;
             int_stock = 1;
             str_table = "<br/><br/> Lot No. <table ><tr><td align='right'><div id = 'runningLot" + int_lot + "'>" + int_lot + ".</div> <input type='hidden' name='hid_Lot" + int_lot + "' id='hid_Lot" + int_lot + "' value= '1' /> </td>";
             str_table = str_table + "<td> รหัส Barcode : </td><td><input id = 'txt_barcodeDetail" + int_lot + "' name = 'txt_barcodeDetail" + int_lot + "' type ='text' style='width:150px' /></td>";
             str_table = str_table + "<td> Lot No. : </td><td colspan = 5><input id = 'txt_lotDetail" + int_lot + "' value= '" + cLotNo + "' name = 'txt_lotDetail" + int_lot + "' type ='text' style='width:150px' /></td></tr>";
             str_table = str_table + "<tr><td></td><td>จำนวน : </td><td> <input id = 'txt_qtyDetail" + int_lot + "' value = '" + cLotQtc + "' name = 'txt_qtyDetail" + int_lot + "' type ='text' onchange='JavaScript:calTotalLot(\"" + int_lot + "\")'  style='width:150px' /> </td>";
             str_table = str_table + "<td> Expire Date :</td><td>";

             //<td> <input id = 'txt_expireDateDetail" + int_lot + "' name = 'txt_expireDateDetail" + int_lot + "' type ='text'  style='width:150px' /> </td>

             str_table = str_table + "<table id='uc_Expire_tblCalendar" + int_lot + "' cellpadding='0' cellspacing='0'><tr>";
             str_table = str_table + "<td style='text-align: left; width: 80px' align='left' valign='middle'>";
             str_table = str_table + "<input name='txt_expireDateDetail" + int_lot + "' value = '" + cLotExpire + "' type='text' id='txt_expireDateDetail" + int_lot + "' onblur='if(!isDate(this.value))this.focus();' style='width:76px;' />";
             str_table = str_table + "<input type='hidden' name='uc_Expire_tblCalendar" + int_lot + "$txtDate_MaskedEditExtender_ClientState' id='uc_txt_expireDateDetail_txtDate_MaskedEditExtender_ClientState' /></td>";
             str_table = str_table + " <td align='left'><img src='../images/Commands/calendar_selector.gif' onclick='popUpCalendar(this, document.getElementById(\"txt_expireDateDetail" + int_lot + "\"), \"dd/mm/yyyy\")' style='cursor: pointer;' />";
             str_table = str_table + " </td><td></td> </tr> </table>  </td>";




             str_table = str_table + "<td> ราคาต่อหน่วย : </td><td> <input id = 'txt_priceDetail" + int_lot + "' value = '" + cLotPrice + "' name = 'txt_priceDetail" + int_lot + "' type ='text' onchange='JavaScript:calTotalLot(\"" + int_lot + "\")' style='width:150px' /> บาท </td></tr>";
             str_table = str_table + "<tr><td></td><td colspan = 6 align =left> มูลค่ารวม <input id = 'txt_totalDetail" + int_lot + "' value = '" + cLotTotal + "' name = 'txt_totalDetail" + int_lot + "' type ='text'  style='width:150px'  readonly /> บาท </td></tr>";
             str_table = str_table + "<tr><td colspan = 7 > สถานที่เก็บ ";
             str_table = str_table + "<table ><tr><td align='right'><div id = 'runningStock" + int_lot + "_" + int_stock + "'>" + int_stock + ".</div><input type='hidden' name='hid_lotStock' id='hid_lotStock" + int_lot + "_" + int_stock + "' value='" + int_lot + "' /></td>";
             str_table = str_table + "<td> จำนวน : </td><td><input id = 'txt_qtyStock" + int_lot + "_" + int_stock + "'   name = 'txt_qtyStock" + int_lot + "_" + int_stock + "' type ='text' style='width:150px' /></td>";
             str_table = str_table + "<td> รหัสสถานที่ : </td><td>";


             str_dropDown = "drp_idStock" + int_lot + "_" + int_stock;
             str_table = str_table + "<select name=" + str_dropDown + " id=" + str_dropDown + "> ";

             setTimeout(function () {
                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetLocation",
                     contentType: "application/json; charset=utf-8",
                     data: "{'str_stock' : '" + $("#ContentPlaceHolder1_drp_searchStock").val() + "' }",
                     dataType: "json",
                     success: function (data) {
                         var resultH = data.d;
                         var int_max = resultH.length;
                         var i = 0;

                         while (i < int_max) {
                             str_table = str_table + "<option value=" + resultH[i].Value + ">" + resultH[i].Text + "</option>";
                             i++;
                         }

                         str_table = str_table + "</select></td>";


                         str_table = str_table + "<td>  <input  type='button' value='Add สถานที่.' name='btn_addStock' onclick='JavaScript:CreateNewRowStock(\"" + int_lot + "\",\"\",\"\",\"\")' BackColor='Transparent' BorderStyle='None' ";
                         str_table = str_table + " Style='background-image: url(\"../Images/button_mll.png\"); background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 191px;' forecolor='white'  Height='25' ";
                         str_table = str_table + " onmouseout='this.style.backgroundImage = 'url(../Images/button_mll.png)';this.style.color = 'white';' ";
                         str_table = str_table + " onmouseover='this.style.backgroundImage = 'url(../Images/button_mll_over.png)';this.style.color = '#EC467E';' /></td></tr></table><div id = 'detailStockUnder" + int_lot + "'></div>";
                         $("#detailLotUnder").append(str_table);


                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });
             }, 50);



             //$("#txtCetification" + int_Certification).val(cCetification);

         }


         function btn_resetDetailOnClick() {
             hideModal('tablePopupDetail');
         }

         function btn_saveDetailOnClick() {

             if ($("#ContentPlaceHolder1_drp_typeReceive").val() == "9") {

                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetCheckRequest",
                     data: "{'str_itemID':'" + $("#txt_productID").val() + "' , 'str_packID':'" + $("#drp_productPack").val() + "' , 'str_payID':'" + pay_id + "' ,'str_total':'" + $("#txt_total").val() + "' }",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         if (data.d != "") {
                             alert(data.d);
                             return;
                         }
                         else {
                             setTimeout(function () { detailSave(); }, 100);
                         }


                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });

             }
             else {
                 setTimeout(function () { detailSave(); }, 100);
             }

         }

         function detailSave() {



             var validUnit = true;
             var int_i = 1;
             var int_j = 1;
             var int_k = 1;
             var int_stockQty = 0;
             var str_barcodeDetail = "";
             var str_lotDetail = "";
             var str_lotDetailRun = "";
             var str_qtyDetail = "";
             var str_expireDetail = "";
             var str_priceDetail = "";
             var str_totalDetail = "";
             var str_hidLotStock = ""; //ลำดับ Lot
             var str_hidLotStockRun = ""; //ลำดับ Stock
             var str_qtyStock = "";
             var str_idStock = "";
             var str_nameStock = "";
             var str_lotProduct = "";
             var str_lotProductName = "";
             var str_lotProductQty = "";
             var str_lotProductType = "";
             var str_lotProductTypeText = "";


             if ($("#txt_productID").val() == "" || $("#txt_productName").val() == "") {
                 alert("กรุณากรอกสินค้า");
                 return;
             }


             while (int_i <= int_lot) {

                 //str_barcodeDetail += $("#txt_barcodeDetail" + int_i + "").val();
                 str_lotDetail += $("#txt_lotDetail" + int_i + "").val() + ",";
                 str_qtyDetail += $("#txt_qtyDetail" + int_i + "").val() + ",";
                 str_expireDetail += $("#txt_expireDateDetail" + int_i + "").val() + ",";
                 str_priceDetail += $("#txt_priceDetail" + int_i + "").val() + ",";
                 str_totalDetail += $("#txt_totalDetail" + int_i + "").val() + ",";


                 str_lotDetailRun += int_i + ",";



                 while (int_j <= $("#hid_Lot" + int_i + "").val()) {



                     str_hidLotStock += $("#hid_lotStock" + int_i + "_" + int_j + "").val() + ",";
                     str_hidLotStockRun += int_j + ",";
                     str_qtyStock += $("#txt_qtyStock" + int_i + "_" + int_j + "").val() + ",";
                     str_idStock += $("#drp_idStock" + int_i + "_" + int_j + "").val() + ",";
                     str_nameStock += $("#drp_idStock" + int_i + "_" + int_j + "").text() + ",";

                     while (int_k < $("#hid_Lot" + int_i + "").val()) {
                         if ($("#drp_idStock" + int_i + "_" + int_j + "").val() == $("#drp_idStock" + int_i + "_" + int_k + "").val() && (int_k != int_j)) {

                             alert("สถานที่ของสินค้าซ้ำกัน");
                             return;
                         }

                         int_k++;
                     }

                     int_stockQty = parseInt(int_stockQty) + parseInt($("#txt_qtyStock" + int_i + "_" + int_j + "").val()); //

                     int_k = 1; //

                     int_j++;

                 }



                 if (int_stockQty != $("#txt_qtyDetail" + int_i + "").val()) {//
                     alert("จำนวนในสถานที่กับ Lot ไม่ตรงกัน");
                     return;
                 }

                 int_stockQty = 0;

                 int_j = 1;
                 int_i++;
             }

             str_lotProduct = $("#txt_productID").val();
             str_lotProductName = $("#txt_productName").val();
             str_lotProductQty = $("#txt_total").val();
             str_lotProductType = $("#drp_productPack").val();
             str_lotProductTypeText = $("#drp_productPack option:selected").text();
             
             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/SaveDetail",
                 data: "{'str_lotProduct':'" + str_lotProduct + "','str_lotProductName':'" + str_lotProductName + "','str_lotProductQty':'" + str_lotProductQty + "','str_lotProductType':'" + str_lotProductType + "','str_lotProductTypeText':'" + str_lotProductTypeText + "','str_lotDetail':'" + str_lotDetail + "', 'str_lotDetailRun':'" + str_lotDetailRun + "','str_qtyDetail':'" + str_qtyDetail + "','str_expireDetail' :'" + str_expireDetail + "','str_priceDetail' : '" + str_priceDetail + "','str_totalDetail' : '" + str_totalDetail + "','str_hidLotStock' : '" + str_hidLotStock + "','str_hidLotStockRun' : '" + str_hidLotStockRun + "','str_qtyStock' : '" + str_qtyStock + "','str_idStock' : '" + str_idStock + "','str_nameStock' : '" + str_nameStock + "' }",
                 dataType: "json",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     GetTableHeadLot('');
                     $("#detailReceviceTable").hide();
                     clearDetail();
                 },
                 failure: function (msg) {
                     alert("error");
                 }

             });


             hideModal('tablePopupDetail');
         }


         function clearHead() {

             $("#txt_productID").val("");
             $("#txt_productName").val("");
             $("#drp_productPack").val("");
             $("#txt_total").val("");
             $("#detailLotUnder").html("");
             $("#detailStockUnder1").html("");
         }


         function clearDetail() {

             $("#runningLot1").val("");
             $("#txt_barcodeDetail1").val("");
             $("#txt_lotDetail1").val("");
             $("#txt_qtyDetail1").val("");
             $("#txt_expireDateDetail1").val("");
             $("#runningStock1_1").val("");
             $("#txt_qtyStock1_1").val("");
             $("#drp_idStock1_1").val("");
             $("#txt_priceDetail1").val("");
             $("#txt_totalDetail1").val("");
             $("#hid_Lot1").val(1)
             int_stock = 1;
             int_lot = 1;

         }

         function clear() {

             $("#txt_number").val("");
             $("#txt_receiveID").val("");

             var d = new Date();
             var month = d.getMonth() + 1;
             var day = d.getDate();
             var year = d.getFullYear();
             if (year < 2500) {
                 year = year + 543;
             }
             var output = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;
             

             $("#ContentPlaceHolder1_uc_receiveDate_txtDate").val(output);
             $("#ContentPlaceHolder1_drp_typeReceive").val("");
             $("#txt_remark").val("");
             $("#ContentPlaceHolder1_drp_supplier").val("");
             $("#txt_preSend").val("");
             $("#txt_preReceive").val($("#ContentPlaceHolder1_hidUserFName").val() + " " + $("#ContentPlaceHolder1_hidUserLName").val());
             $("#hid_preReceive").val($("#ContentPlaceHolder1_hidUser").val())
             $("#txt_create").val("");
             $("#txt_createDate").val("");
             $("#txt_updateDate").val("");
             $("#txt_update").val("");
             $("#myDetail").html('');

             $("#txt_numberRequest").val("");
             $("#txt_RequestDesc").val("");
             $("#txt_RequestDate").val("");

             $("#trRequest").hide();


         }

         function btn_saveOnClick() {

             if ($("#ContentPlaceHolder1_drp_typeReceive").val() == "") {

                 alert("กรุณาเลือกประเภทการรับ");
                 return;
             }

             if ($("#ContentPlaceHolder1_drp_typeReceive").val() == "9") {
                 if ($("#txt_numberRequest").val() == "" || $("#txt_RequestDesc").val() == "") {
                     alert("กรุณากรอกเลชที่ใบเบิกให้ถูกต้อง");
                     return;
                 }
                 
             }


             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/saveReceive",
                 data: "{'str_receiveID':'" + $("#txt_receiveID").val() + "','str_receiveDate':'" + $("#ContentPlaceHolder1_uc_receiveDate_txtDate").val() + "','str_number':'" + $("#txt_number").val() + "','str_typeReceive':'" + $("#ContentPlaceHolder1_drp_typeReceive").val() + "','str_remark':'" + $("#txt_remark").val() + "','str_supplier':'" + $("#ContentPlaceHolder1_drp_supplier").val() + "','str_preSend':'" + $("#txt_preSend").val() + "','str_preReceive':'" + $("#hid_preReceive").val() + "','str_stock':'" + $("#ContentPlaceHolder1_drp_searchStock").val() + "','str_user':'" + $("#ContentPlaceHolder1_hidUser").val() + "','str_payID' : '" + pay_id + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     alert(resultH);
                     $("#myDetail").html('');
                     GetTableSearch();
                     clear();
                     clearHead();
                     clearDetail();
                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });

         }


         function calTotalLot(Lot) {

             var cal_qty = 0;
             var cal_price = 0;
             var cal_total = 0;
             var int_i = 1;
             var cal = 0;
             i_total = 0;


             if ($("#txt_qtyDetail" + Lot).val() != "") {
                 cal_qty = $("#txt_qtyDetail" + Lot + "").val()
             }
             if ($("#txt_priceDetail" + Lot).val() != "") {
                 cal_price = $("#txt_priceDetail" + Lot + "").val()
             }

             cal_total = cal_qty * cal_price;

             while (int_i <= int_lot) {

                 if ($("#txt_qtyDetail" + int_i + "").val() == "") {
                     cal = 0;
                 }
                 else {
                     cal = $("#txt_qtyDetail" + int_i + "").val();
                 }
                 i_total = parseInt(i_total) +  parseInt(cal);
                 int_i++;
             }


             $("#txt_total").val(i_total);
             $("#txt_priceDetail" + Lot + "").val(cal_price.formatMoney(4, '.', ''))
             $("#txt_totalDetail" + Lot + "").val(cal_total.formatMoney(4, '.', ''));

         }

         function GetTableSearch() {
             var str_table;


             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetSearch",
                 data: "{'str_sort': '','str_rowPer':'10','str_rowStart' :'1','str_dateStart' :'" + $("#ContentPlaceHolder1_uc_searchDateStart_txtDate").val() + "','str_dateEnd' :'" + $("#ContentPlaceHolder1_uc_searchDateEnd_txtDate").val() + "','str_receive' :'" + $("#txt_searchReceiveID").val() + "','str_stock' :'" + $("#ContentPlaceHolder1_drp_searchStock").val() + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;

                     str_table = "";
                     var int_max = resultH.length;
                     var i = 0;
                     //                     for (var i = 0; i < int_max; i++) {

                     //                         alert(resultH[i].productID);
                     //                     }


                     //createPagination(int_max, "H"); //สร้าง pagination First 1 2 3 Last
                     //var int_i = 0; //กำหนดให้ เริ่มต้นวน loop ตั้งแต่ i_rowstart
                     // var i_rowend = (i_rowstart + i_rowperpage) - 1; //คำนวณหา แถวสุดท้ายที่ต้องโชว์

                     if (int_max > 0) {

                         // int_i = i_rowstart;

                         //str_table = "<table id='table-2'><thead><th><div align='center'>ลำดับ</div></th> ";
                         str_table = "<table BackColor='#3A8FE3' style='color: White;width:780px' CellSpacing='1' CellPadding='1'>";
                         str_table = str_table + "<tr class = 'GridHeader' ><th> <div align='center'>รายละเอียด</div></th> ";
                         str_table = str_table + "<th> <div align='center'>ลำดับที่</div></th> ";
                         str_table = str_table + "<th> <div align='center'>เลขที่ใบรับเข้า</div></th> ";
                         str_table = str_table + "<th> <div align='center'>วันที่รับเข้า</div></th> ";
                         str_table = str_table + "<th> <div align='center'>เลขที่อ้างอิง</div></th> ";
                         str_table = str_table + "<th> <div align='center'>แหล่่งที่มา</div></th> ";
                         str_table = str_table + "<th> <div align='center'>ชื่อผู้รับ</div></th> ";
                         str_table = str_table + "<th> <div align='center'>ยอดเงินรวม</div></th> ";
                         str_table = str_table + "<th> <div align='center'>สถานะ</div></th></tr> ";


                         while (i < int_max) {

                             if ((i + 1) % 2) {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #F6F6F6;color: #416285'>";
                             }
                             else {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #FFD6E4;color: #416285'>";

                             }

                             str_table = str_table + "<td><a href='JavaScript:GetHead(\"" + resultH[i].transactionID + "\",\"" + resultH[i].receiveID + "\",\"" + resultH[i].receiveDate + "\",\"" + resultH[i].type + "\",\"" + resultH[i].refNumber + "\",\"" + resultH[i].supplier + "\",\"" + resultH[i].receiveUser + "\",\"" + resultH[i].receiveUserID + "\",\"" + resultH[i].sendUser + "\",\"" + resultH[i].reason + "\",\"" + resultH[i].createBy + "\",\"" + resultH[i].createDate + "\",\"" + resultH[i].updateBy + "\",\"" + resultH[i].updateDate + "\",\"" + resultH[i].stockID + "\",\"" + resultH[i].flag + "\",\"" + resultH[i].pay_id + "\")'>";
                             str_table = str_table + "รายละเอียด</a></td> ";
                             str_table = str_table + "<td>" + (i + 1) + "</td> ";
                             str_table = str_table + "<td>" + resultH[i].receiveID + "</td>";
                             str_table = str_table + "<td>" + convertToDateServerByString(resultH[i].receiveDate) + "</td>";
                             str_table = str_table + "<td>" + resultH[i].refNumber + "</td>";
                             str_table = str_table + "<td>" + resultH[i].supplierName + "</td>";
                             str_table = str_table + "<td>" + resultH[i].receiveUser + "</td>";
                             str_table = str_table + "<td>" + (resultH[i].total).formatMoney(4, '.', '') + "</td>";
                             if (resultH[i].flag == "True") {
                                 str_table = str_table + "<td>ใช้งาน</td></tr>";
                             }
                             else {
                                 str_table = str_table + "<td>ยกเลิก</td></tr>";
                             }


                             i++;
                         }

                         str_table = str_table + "</table>";

                         $("#mySpan").html(str_table);
                         $("#mySpan").show();
                     } // if (int_max > 0)
                     else {
                         alert("ไม่พบข้อมูล");
                         $("#mySpan").html('');

                     }



                 },
                 failure: function (msg) {
                     alert("error");
                 }
             });

         }



         function pagingChangePage() {


         }


         function pagingChangeTotal() {

         }

         //paging
         function doPaging(pageNo) { //เมื่อ click page link จะคำนวณเพื่อหาค่า แถวเริ่มต้น ที่จะโชว์
             // i_rowstart = (i_rowperpage * pageNo) - i_rowperpage;
             currentPage = pageNo;
             i_rowstart = (pageNo - 1) * i_rowperpage;
             doCallAjax(checkSort);
         }

         function crossPaging(value) {
             i_rowcross = i_rowcross + value;
             i_rowstart = (i_rowperpage * i_rowcross) - i_rowperpage;
             currentPage = (i_rowstart / 10) + 1;
             doCallAjax(checkSort);
         }

         function FirstLastPaging(value) {
             if (value == 1) {
                 i_rowcross = 1;
             }
             else {
                 if (i_totalPage > (i_rowcross + 7)) {
                     i_rowcross = value - 7;
                 }
             }
             i_rowstart = (i_rowperpage * value) - i_rowperpage;
             currentPage = (i_rowstart / 10) + 1;
             doCallAjax(checkSort);
         }

         function nextPaging(value) {

             if (value == "B") {
                 //            
                 if (currentPage == 1) {
                     currentPage = currentPage;
                 }
                 else {
                     currentPage = currentPage - 1;
                 }

                 // currentPage = currentPage - 1;
             }
             else {
                 if (currentPage == i_totalPage) {
                     currentPage = currentPage;
                 }
                 else {
                     currentPage = currentPage + 1;
                 }
                 // currentPage = currentPage + 1;
             }

             i_rowstart = (i_rowperpage * currentPage) - i_rowperpage;



             if (currentPage < i_rowcross) {
                 i_rowcross = i_rowcross - 7;
             }
             else if (currentPage > (i_rowcross + 7)) {
                 i_rowcross = i_rowcross + 7;
             }

             doCallAjax(checkSort);


         }



         function createPagination(totalrow) {


             var i_lastPage;
             var str_paging = "";
             if ((totalrow % i_rowperpage) == 0) {
                 i_totalPage = totalrow / i_rowperpage;
             }
             else {

                 i_totalPage = (totalrow / i_rowperpage) + 1
             }

             i_totalPage = parseInt(i_totalPage);

             i_startPage = i_rowcross;
             if (i_totalPage > (i_rowcross + 7)) {
                 i_lastPage = i_rowcross + 7;
             }
             else {
                 i_lastPage = i_totalPage;
             }


             str_paging += "<font size='3'> Total Page " + i_totalPage + " ( " + totalrow.formatMoney(0, '', ',') + " items ) ";
             str_paging += "<a href='JavaScript:FirstLastPaging(1)' style=\"text-decoration: none\">" + "<img src='../../Content/Image/BackFirst.jpg' ALIGN=ABSBOTTOM width='25' height='25' />" + "</a>";

             str_paging += "<a href='JavaScript:nextPaging(\"B\")' style=\"text-decoration: none\">" + "<img src='../../Content/Image/BackNew.gif' ALIGN=ABSBOTTOM width='25' height='25' />" + "</a>";

             if (i_rowcross != 1) {
                 str_paging += "<a href='JavaScript:crossPaging(-7)' style=\"text-decoration: none\"> ..... </a>";
             }

             var i;
             for (i = 1; i <= i_totalPage; i++) {
                 if (currentPage == i) {
                     str_paging += "<a href='JavaScript:doPaging(" + i + ")' style=\"text-decoration: none;font-weight:bold; color: #FF00FF; font-size: 20px;\">" + i + "</a>";
                     str_paging += "  ";
                 }
                 else {
                     str_paging += "<a href='JavaScript:doPaging(" + i + ")' style=\"text-decoration: none\">" + i + "</a>";
                     str_paging += "  ";
                 }


             }
             if (i_totalPage > ((i_rowcross + 7))) {
                 str_paging += "<a href='JavaScript:crossPaging(7)' style=\"text-decoration: none\"> ..... </a>";
             }

             str_paging += "<a href='JavaScript:nextPaging(\"N\")' style=\"text-decoration: none\">" + "<img src='../../Content/Image/NextNew.gif' ALIGN=ABSBOTTOM width='25' height='25' />" + "</a>";
             str_paging += "<a href='JavaScript:FirstLastPaging(" + i_totalPage + ")' style=\"text-decoration: none\">" + "<img src='../../Content/Image/NextLast.jpg' ALIGN=ABSBOTTOM width='25' height='25' />" + "</a>";
             str_paging += " </font> ";



             //                         <div id='PagingControl1_pnlPagin' style='margin:2px 0 0 0; padding:5px;'>
             //	
             //                        <center><table cellpadding='0' cellspacing='0'><tr><td> หน้าที่ </td><td>
             //                                <select name='PagingControl1$ddlCurrentPage' onchange='javascript:setTimeout(&#39;__doPostBack(\&#39;PagingControl1$ddlCurrentPage\&#39;,\&#39;\&#39;)&#39;, 0)' id='PagingControl1_ddlCurrentPage'>
             //		                        <option selected='selected' value='1'>1</option></select></td><td> / </td><td> 1 </td>
             //                                <td>&nbsp;&nbsp;จำนวน 10&nbsp;รายการ</td><td> จำนวนรายการต่อหน้า </td><td>
             //                                <select name="PagingControl1$ddlPageSize" onchange="javascript:setTimeout(&#39;__doPostBack(\&#39;PagingControl1$ddlPageSize\&#39;,\&#39;\&#39;)&#39;, 0)" id="PagingControl1_ddlPageSize">
             //		                        <option selected="selected" value="10">10</option>
             //		                        <option value="15">15</option>
             //		                        <option value="20">20</option>
             //		                        <option value="30">30</option>
             //		                        <option value="50">50</option>
             //		                        <option value="100">100</option>
             //	                        </select></td></tr></table></center></div>



             pagination = str_paging;

         }


         function GetTableHeadLot(obj) {
             var str_table;

             ProductID = obj;
             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetHeadLotDetail",
                 contentType: "application/json; charset=utf-8",
                 data: "{'str_TransactionID':'" + obj + "'}",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;

                     str_table = "";
                     var int_max = resultH.length;
                     var i = 0;
                     //                     for (var i = 0; i < int_max; i++) {

                     //                         alert(resultH[i].productID);
                     //                     }


                     //createPagination(int_max, "H"); //สร้าง pagination First 1 2 3 Last
                     //var int_i = 0; //กำหนดให้ เริ่มต้นวน loop ตั้งแต่ i_rowstart
                     // var i_rowend = (i_rowstart + i_rowperpage) - 1; //คำนวณหา แถวสุดท้ายที่ต้องโชว์

                     if (int_max > 0) {

                         // int_i = i_rowstart;

                         //str_table = "<table id='table-2'><thead><th><div align='center'>ลำดับ</div></th> ";
                         str_table = "<table BackColor='#3A8FE3'  style='color: White;width:780px' CellSpacing='1' CellPadding='1'> <tr class = 'GridHeader' ><th><div align='center'>ลบ</div></th>";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>ลำดับ</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>รายละเอียด</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>รหัสสินค้า</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>ชื่อสินค้า</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>จำนวน</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>หน่วย</div></th> ";
                         str_table = str_table + "<th Style=GridHeader > <div align='center'>รวมเงิน</div></th></tr> ";


                         while (i < int_max) {

                             if ((i + 1) % 2) {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #F6F6F6;color: #416285'>";
                             }
                             else {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #FFD6E4;color: #416285'>";

                             }
                             str_table = str_table + "<td >" + (i + 1) + "</td>";
                             str_table = str_table + "<td > <input type='checkbox' name=chk" + resultH[i].productID + "  value=" + resultH[i].productID + " ";
                             str_table = str_table + "onclick='JavaScript:chk_onClick(\"" + resultH[i].productID + "\")'></td> ";
                             str_table = str_table + "<td><a href='JavaScript:GetLot(\"" + resultH[i].productID + "\",\"" + resultH[i].productName + "\",\"" + resultH[i].productQty + "\",\"" + resultH[i].productType + "\")'>";
                             str_table = str_table + "รายละเอียด</a></td> ";
                             str_table = str_table + "<td>" + resultH[i].productID + "</td>";
                             str_table = str_table + "<td>" + resultH[i].productName + "</td>";
                             str_table = str_table + "<td>" + resultH[i].productQty + "</td>";
                             str_table = str_table + "<td>" + resultH[i].productTypeText + "</td>";
                             str_table = str_table + "<td>" + (resultH[i].productTotalPrice).formatMoney(4, '.', '') + "</td></tr>";

                             i++;
                         }

                         str_table = str_table + "</table>";
                         $("#myDetail").html(str_table);
                         $("#myDetail").show();
                     } // if (int_max > 0)
                     else {

                         $("#myDetail").html('');

                     }



                 },
                 failure: function (msg) {
                     alert("error");
                 }
             });

         }

         function btn_addReceiveOnClick() {

             revealModal('tablePopupDetail');
             clearHead();
             clearDetail();
             setStockDropDown();
             $("#detailReceviceTable").show();
             int_stock = 1;
             int_lot = 1;
             clearDetail();
         }

         function selectedItem(productID, productName) {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetPack",
                 data: "{'str_lotProduct':'" + productID + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;
                     var str_drp = "";
                     $("#drp_productPack >option").remove();
                     while (i < int_max) {
                         str_drp = str_drp + "<option value=" + resultH[i].Value + ">" + resultH[i].Text + "</option>";
                         i++;
                     }
                     $("#drp_productPack").append(str_drp);
                     $("#txt_productID").val(productID);
                     $("#txt_productName").val(productName);
                     hideModal('tablePopup');

                     setTimeout(function () { requestPrice(); }, 200);


                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }

         function selectedUser(userID, userName) {

             $("#hid_preReceive").val(userID);
             $("#txt_preReceive").val(userName);
             hideModal('tablePopup');

         }

         function requestPrice() {
             //-----

             
             if ($("#ContentPlaceHolder1_drp_typeReceive").val() == "9") {
                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetUnitPrice",
                     data: "{'str_ID':'" + pay_id + "' , 'str_itemID':'" + $("#txt_productID").val() + "' , 'str_packID':'" + $("#drp_productPack").val() + "' }",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         $("#txt_priceDetail1").val((data.d).formatMoney(4, '.', ''));

                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });
             }
             else {
                 return;
             }


             //-------
         }


         function productTab() { // คีย์รหัสครบแล้วโชว์เลย



             var str_productCode = $("#txt_productID").val();


             if (str_productCode.length != 12) {

                 return;
             }
             else {
                
                 if (str_productCode.substring(1, 2) == '-' && str_productCode.substring(4, 5) == '-' && str_productCode.substring(7, 8) == '-') {

                     $.ajax({
                         type: "POST",
                         url: "jsonService.asmx/GetProduct",
                         data: "{'str_lotProduct':'','str_lotProductCode':'" + $("#txt_productID").val() + "'}",
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         success: function (data) {
                             var resultH = data.d;
                             if (resultH.length > 0) {
                                 setTimeout(function () { selectedItem(resultH[i].Value, resultH[i].Text); }, 200);


 
                             }
                             else {
                                 alert("ไม่มีรหัสสินค้านี้");
                             }

                         },
                         failure: function (msg) {
                             alert("error");
                         }
                     });
                 }
 
             }

            

            
         }

         function img_searchProduct_Click() {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetProduct",
                 data: "{'str_lotProduct':'" + $("#txt_productName").val() + "','str_lotProductCode':'" + $("#txt_productID").val() + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;
                     var str_table = "";

                     if (int_max > 0) {
                         str_table = "<table id = 'productTable'BackColor='#3A8FE3' style='color: White;width:345px' CellSpacing='1' CellPadding='1'><tr class = 'GridHeader'><th> <div align='center'>ลำดับ</div></th> ";
                         str_table = str_table + "<th> <div align='center'>รหัส</div></th> ";
                         str_table = str_table + "<th> <div align='center'>รายการ</div></th> ";
                         str_table = str_table + "</tr>";
                         while (i < int_max) {

                             if ((i + 1) % 2) {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #F6F6F6;color: #416285'>";
                             }
                             else {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #FFD6E4;color: #416285'>";

                             }
                             str_table = str_table + "<td>" + (i + 1) + "</td>";
                             str_table = str_table + "<td><a href='JavaScript:selectedItem(\"" + resultH[i].Value + "\",\"" + resultH[i].Text + "\");'>" + resultH[i].Value + "</a></td>";
                             str_table = str_table + "<td>" + resultH[i].Text + "</td></tr>";
                             i++;

                         }
                         str_table = str_table + "</table>";
                         $("#tableSearchProduct").html(str_table);

                         revealModal('tablePopup');
                     }
                     else {
                         alert("ไม่พบข้อมูล");
                     }


                 },
                 failure: function (msg) {
                     alert("error");
                 }
             });
         }


         function img_searchUser_Click() {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetUser",
                 data: "{'str_lotUser':'" + $("#txt_preReceive").val() + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;
                     var str_table = "";

                     if (int_max > 0) {
                         str_table = "<table id = 'userTable' BackColor='#3A8FE3' style='color: White;width:345px' CellSpacing='1' CellPadding='1'><tr class = 'GridHeader'><th> <div align='center'></div></th><th> <div align='center'>ลำดับ</div></th> ";
                         str_table = str_table + "<th> <div align='center'>ชื่อ-สกุล</div></th> ";
                         str_table = str_table + "</tr>";
                         while (i < int_max) {

                             if ((i + 1) % 2) {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #F6F6F6;color: #416285'>";
                             }
                             else {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #FFD6E4;color: #416285'>";

                             }


                             str_table = str_table + "<td><a href='JavaScript:selectedUser(\"" + resultH[i].Value + "\",\"" + resultH[i].Text + "\");'>เลือก</a></td>";
                             str_table = str_table + "<td>" + (i + 1) + "</td>";
                             str_table = str_table + "<td>" + resultH[i].Text + "</td>";
                             str_table = str_table + "</tr>";
                             i++;

                         }
                         str_table = str_table + "</table>";
                         $("#tableSearchProduct").html(str_table);

                         revealModal('tablePopup');
                     }
                     else {
                         alert("ไม่พบข้อมูล");
                     }


                 },
                 failure: function (msg) {
                     alert("error");
                 }
             });
         }


         function GetHead(transactionID, receiveID, receiveDate, type, refNumber, supplier, receiveUser, receiveUserID, sendUser, reason, createBy, createDate, updateBy, updateDate, stockID, flag, payid) {

             clearHead();

             if (payid != "") {
                 $("#trRequest").show();
                 $("#txt_numberRequest").val("");
                 $("#txt_RequestDesc").val("");
                 $("#txt_RequestDate").val("");


                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetDateForPayID",
                     data: "{'str_ID':'" + payid + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         $("#txt_RequestDate").val(convertToDateServerByString(data.d));
                       

                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });


                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetRequestForPayID",
                     data: "{'str_ID':'" + payid + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         $("#txt_numberRequest").val(data.d);
                         

                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });


                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/GetOrgForPayID",
                     data: "{'str_ID':'" + payid + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         $("#txt_RequestDesc").val(data.d);
                        

                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });
                 
             


             }
             else {
                 $("#trRequest").hide();
                 $("#txt_numberRequest").val("");
                 $("#txt_RequestDesc").val("");
                 $("#txt_RequestDate").val("");
             }

             $("#txt_receiveID").val(receiveID);
             $("#ContentPlaceHolder1_uc_receiveDate_txtDate").val(convertToDateServerByString(receiveDate));
             $("#txt_number").val(refNumber);
             $("#ContentPlaceHolder1_drp_supplier").val(supplier);
             $("#ContentPlaceHolder1_drp_typeReceive").val(type);
             $("#txt_remark").val(reason);
             $("#ContentPlaceHolder1_drp_searchStock").val(stockID);

             $("#txt_preSend").val(sendUser);
             $("#txt_preReceive").val(receiveUser);
             $("#hid_preReceive").val(receiveUserID);
             $("#txt_createDate").val(convertToDateServerByString(createDate));
             $("#txt_create").val(createBy);
             $("#txt_updateDate").val(convertToDateServerByString(updateDate));
             $("#txt_update").val(updateBy);

             if (flag == "True") {
                 //$("#btn_save").show();
                 $("#btn_save").hide();
                 $("#btn_delete").show();
                 $("#btn_reset").show();
             }
             else {
                 $("#btn_save").hide();
                 $("#btn_delete").hide();
                 $("#btn_reset").hide();
             }

        

             GetTableHeadLot(transactionID);
         }

         function GetLot(productID, productName, productQty, productType ) {
             clearHead();
             clearDetail();
            

             revealModal('tablePopupDetail');

             $("#txt_productID").val(productID);
             $("#txt_productName").val(productName);
            
             $("#txt_total").val(productQty);
             

             selectedItem(productID, productName);
             int_stock = 1;
             int_lot = 1;


             setTimeout(function () { GetLotDetail(productID);  }, 300);

             setTimeout(function () { GetLotStockDetail(productID); }, 500);

             setTimeout(function () {$("#drp_productPack").val(productType); }, 550);
         }

         function GetLotDetail(productID) {


             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetLotDetail",
                 data: "{'str_product':'" + productID + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;

                     var i = 0;


                     while (i < int_max) {

                         if (i == 0) {


                             $("#txt_lotDetail" + (i + 1) + "").val(resultH[i].lotOther);
                             $("#txt_qtyDetail" + (i + 1) + "").val(resultH[i].qtyOther);
                             $("#txt_expireDateDetail" + (i + 1) + "").val(convertToDateServerByString(resultH[i].expireDateOther));
                             $("#txt_priceDetail" + (i + 1) + "").val(resultH[i].priceOther);
                             $("#txt_totalDetail" + (i + 1) + "").val(resultH[i].totalOther);
                             $("#hid_Lot" + (i + 1) + "").val("1");
                         }
                         else {

                             
                             CreateNewRowLot('', resultH[i].lotOther, resultH[i].qtyOther, resultH[i].expireDateOther, resultH[i].priceOther, resultH[i].totalOther);


                         }

                         i++;
                     }



                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });

            
         }

         function GetLotStockDetail(productID) {


             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetLotStockDetail",
                 data: "{'str_product':'" + productID + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     var resultH = data.d;
                     var int_max = resultH.length;
                     var i = 0;

                    


                     while (i < int_max) {

                         if (resultH[i].lotStockRun == 1) {
                             
                             
                             $("#hid_lotStock" + resultH[i].lotStock + "_" + resultH[i].lotStockRun).val(resultH[i].lotStock);
                             $("#txt_qtyStock" + resultH[i].lotStock + "_" + resultH[i].lotStockRun).val(resultH[i].qtyStock);
                             $("#drp_idStock" + resultH[i].lotStock + "_" + resultH[i].lotStockRun).val(resultH[i].idStock);


                         }
                         else {

                            
                             CreateNewRowStock(resultH[i].lotStock, resultH[i].qtyStock, resultH[i].idStock, resultH[i].nameStock);


                            
                         }



                         i++;
                     }
                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });

         }


         function btn_deleteOnClick() {

             if (confirm('ต้องการยกเลิก')) {
                 $.ajax({
                     type: "POST",
                     url: "jsonService.asmx/delete",
                     data: "{'str_ID':'" + $("#txt_receiveID").val() + "','str_type':'R','str_user':'" + $("#ContentPlaceHolder1_hidUser").val() + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (data) {
                         GetTableSearch();
                         clear();
                         clearDetail();
                         clearHead();
                         alert("ยกเลิกข้อมูลเรียบร้อยแล้ว");
                     },
                     failure: function (msg) {
                         alert(msg);
                     }
                 });
             }
             else {
             }
            
         }

         function btn_deleteDetailOnClick() {

             var i = 0;
             var strDelete = "";
             while (i < ValueLot.length) {

                 strDelete += ValueLot[i] + ",";
                 i++;
             }

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/deleteLotProduct",
                 data: "{'str_lotProduct':'" + strDelete + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     GetTableHeadLot(ProductID);
                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }


         function chk_onClick(Value, Type) {

             if ($("input[name=chk" + Value + "]").is(":checked") == true) {
                 ValueLot[ValueLot.length] = Value;
                 $("#chk" + Value).attr('checked', true);
             }
             else {
                 ValueLot.removeByValue(Value);
                 $("#chk" + Value).attr('checked', false);
             }

         }


         function btn_searchResetOnClick() {

             $("#txt_searchReceiveID").val('');
             $("#ContentPlaceHolder1_uc_searchDateStart_txtDate").val('');
             $("#ContentPlaceHolder1_uc_searchDateEnd_txtDate").val('');
         }

         function btn_resetOnClick() {
             clear();
             clearDetail();
             clearHead();
         }

         function btn_receiveOnClick() {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/clearList",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {

                     clearHead();
                     clearDetail();
                     clear();

                     $("#btn_save").show();
                     $("#btn_delete").show();
                     $("#btn_reset").show();



                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });

         }


         function fn_changeReceive() {
         
             if ($("#ContentPlaceHolder1_drp_typeReceive").val() == "9") {

                 $("#trRequest").show();
             }
             else {
                 $("#txt_numberRequest").val("");
                 $("#txt_RequestDesc").val("");
                 $("#txt_RequestDate").val("");
                 $("#trRequest").hide();
             }
         }


         function fn_changeRequest() {


             if ($("#txt_numberRequest").val() == "") {
                 
                 return;
             }

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetRequestID",
                 data: "{'str_ID':'" + $("#txt_numberRequest").val() + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {


                     if (data.d != "") {
                         Request_id = data.d;
                         fn_getRequestDesc(data.d);

                     }
                     else {
                         alert("ไม่พบเลขที่ใบเบิก");
                     }
                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }

         function fn_getRequestDesc(Value) {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetRequestDesc",
                 data: "{'str_ID':'" + Value + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                     $("#txt_RequestDesc").val(data.d);
                     fn_getRequestDate(Value);

                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }


         function fn_getRequestDate(Value) {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetRequestDate",
                 data: "{'str_ID':'" + Value + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {


                     //fn_getPayID(Value);

                     var resultH = data.d;
                     var int_max = resultH.length;

                     var i = 0;
                     var str_table = "";
                     if (int_max > 1) {

                         str_table = "<table id = 'requestTable' BackColor='#3A8FE3' style='color: White;width:345px' CellSpacing='1' CellPadding='1'><tr class = 'GridHeader'><th> <div align='center'></div></th><th> <div align='center'>ลำดับ</div></th> ";
                         str_table = str_table + "<th> <div align='center'>วันที่จ่าย</div></th> ";
                         str_table = str_table + "</tr>";
                         while (i < int_max) {

                             if ((i + 1) % 2) {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #F6F6F6;color: #416285'>";
                             }
                             else {
                                 str_table = str_table + "<tr id = 'tr" + (i + 1) + "' style='background-color: #FFD6E4;color: #416285'>";

                             }


                             str_table = str_table + "<td><a href='JavaScript:selectedRequest(\"" + resultH[i].Value + "\",\"" + resultH[i].Text + "\");'>เลือก</a></td>";
                             str_table = str_table + "<td>" + (i + 1) + "</td>";
                             str_table = str_table + "<td>" + resultH[i].Text + "</td>";
                             str_table = str_table + "</tr>";
                             i++;

                         }
                         str_table = str_table + "</table>";
                         $("#tableSearchProduct").html(str_table);

                         revealModal('tablePopup');
                     }
                     else {
                         $("#txt_RequestDate").val(convertToDateServerByString(resultH[0].Text));
                         pay_id = resultH[0].Value;
                     }



                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }


         function selectedRequest(reID, reDate) {

             pay_id = reID;
             $("#txt_RequestDate").val(convertToDateServerByString(reDate));
             hideModal('tablePopup');

         }


         function fn_getPayID(Value) {

             $.ajax({
                 type: "POST",
                 url: "jsonService.asmx/GetPayID",
                 data: "{'str_ID':'" + Value + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (data) {
                    pay_id = data.d ;

                 },
                 failure: function (msg) {
                     alert(msg);
                 }
             });
         }

      


         $(document).ready(function () {
             $("#txt_preReceive").val($("#ContentPlaceHolder1_hidUserFName").val() + " " + $("#ContentPlaceHolder1_hidUserLName").val());
             $("#hid_preReceive").val($("#ContentPlaceHolder1_hidUser").val())
             var d = new Date();
             var month = d.getMonth() + 1;
             var day = d.getDate();
             var year = d.getFullYear();
             if (year < 2500) {
                 year = year + 543;
             }
             var output = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;
             $("#ContentPlaceHolder1_uc_receiveDate_txtDate").val(output);
         });
         


        
      
        </script>
    <style type="text/css">
        .style1
        {
            width: 149px;
            text-align: left;
        }
        .style2
        {
            width: 160px;
        }
        .style3
        {
            width: 476px;
        }
        .style4
        {
            width: 77px;
        }
        .style5
        {
            width: 130px;
        }
        .style6
        {
            width: 100px;
        }
    </style>



    <center>
    <table cellpadding="0" cellspacing="0" style="width: 805px">
        <tr>
        <td class="tableHeader" align="left">
            <asp:HiddenField ID="hidUser" runat="server" />
            <asp:HiddenField ID="hidUserFName" runat="server" />
            <asp:HiddenField ID="hidUserLName" runat="server" />

         รับเข้าสินค้ากรณีอื่นๆ 
        </td></tr>
        <tr>
       
    
    <td  class="tableBody">
    

    
    <br />
    <table border = 0 cellpadding = 0 cellspacing = 0 >
    <tr>
    <td class="style3">
        <table>
        <tr>
            <td colspan = "4"    >
            </td>    
        </tr>
        <tr>
            <td align=right>
                เลขที่ใบคืนของ : 
            </td>
            <td colspan = "3" align =left>
                <%--<input id="txt_receiveID" type="text" />--%>
               <input type ="text" style="width:150px"  name ="txt_searchReceiveID"  id="txt_searchReceiveID" />
            </td>
        </tr>
        <tr>
            <td align=right>
                วันที่คืนของ : 
            </td>
           
            <td style="text-align: left">
           
                    <uc2:CalendarControl ID="uc_searchDateStart" runat="server" />
                

            </td>
           
            <td>
                ถึงวันที่
            </td>
            <td>
                <uc2:CalendarControl ID="uc_searchDateEnd" runat="server" />

            </td>
        </tr>
        <tr>
            <td >
            </td>
            <td  align =right>
          <input  type="button" value="ค้นหา" name="btn_search" 
                    onclick="JavaScript:btn_searchOnClick()" BorderStyle="None" 
                    Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 76px;" 
    Height="25" 
                    onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                    onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
&nbsp;
        <input  type="button" value="ยกเลิก" name="btn_searchReset" 
                    onclick="JavaScript:btn_searchResetOnClick()" BorderStyle="None" 
                    Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;"  
    Height="25" 
                    onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                    onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
            </td>
            <td colspan = 2 align =left>
                <input  type="button" value="รับสินค้า" name="btn_receive"  
                 onclick="JavaScript:btn_receiveOnClick()"BackColor="Transparent"  
                    BorderStyle="None" 
                    Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 74px;" 
    Height="25" 
                    onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                    onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
            </td>
        </tr>
        </table>
    </td>
    <td class="style2" align =right>
         คลัง : <asp:DropDownList ID="drp_searchStock" runat="server">
                </asp:DropDownList>
            </td>
           
            </tr>
        </table>
    </td>
    </tr>
 
    <tr>
    
        
        <td class="tableBody">
             <div id="mySpan">
             </div>
        </td>
    </tr>

    </table>

    <br />
    <br />
    <table cellpadding="0" cellspacing="0" style="width: 805px">
        <tr>
        <td class="tableHeader" align="left">
         รายละเอียด 
        </td></tr>
    <tr>
       
        <td class="tableBody">
            <%--style="display:none"--%>
                <table id = "receiveHead" border = 0 cellpadding = 0 cellspacing = 0 >
                    <tr>
                        <td>
                            
                            <table >
                                <tr>
                                    <td style="text-align: right" class="style6">
                                        เลขที่รับเข้า : 
                                    </td>
                                    <td>
                                        <input type ="text" style="width:150px"  name ="txt_receiveID"  id="txt_receiveID" readonly />
                                    </td>
                                    <td>
                                        วันที่รับเข้า :
                                    </td>
                                    <td class="style5">
                                         <uc2:CalendarControl ID="uc_receiveDate" runat="server" />
                         
                                    </td>
                                    <td style="text-align: right" class="style4">
                                        เลขที่อ้างอิง : 
                                    </td>
                                    <td class="style1">
                                         <input type ="text" style="width:150px"  name ="txt_number"  id="txt_number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style6">
                                        ประเภทการรับ : 
                                    </td>
                                    <td colspan = 3 style="text-align: left">
                                        <asp:DropDownList ID="drp_typeReceive" runat="server" onchange = "fn_changeReceive();">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right" class="style4">
                                        ระบุกรณีอื่นๆ :
                                    </td>
                                    <td class="style1">
                                        <input type ="text" style="width:150px"  name ="txt_remark"  id="txt_remark" />
                                    </td>
                                </tr>


                                <tr id = "trRequest"  style="display:none" > <%--  รับเข้าจากหน่วยงาน --%>
                                     <td style="text-align: right" class="style6">
                                        เลขที่ใบเบิก : 
                                    </td>
                                    <td style="text-align: left">
                                        <input type ="text" style="width:150px"  name ="txt_numberRequest"  id="txt_numberRequest" onchange="JavaScript:fn_changeRequest()" />
                                    </td>
                                     <td style="text-align: right" class="style6">
                                        ฝ่าย/ทีมงาน : 
                                    </td>
                                    <td style="text-align: left"  >
                                        <input type ="text" style="width:150px"  name ="txt_RequestDesc"  id="txt_RequestDesc" />
                                    </td>
                                    <td style="text-align: right" class="style4">
                                        วันที่จ่าย :
                                    </td>
                                    <td class="style1">
                                         <input type ="text" style="width:150px"  name ="txt_RequestDate"  id="txt_RequestDate" />
   
                                    </td>

                                </tr>


                                <tr>
                                      <td style="text-align: right" class="style6">
                                        ผู้จำหน่าย/ผู้ผลิต : 
                                    </td>
                                    <td colspan = 3 style="text-align: left">
                                        <asp:DropDownList ID="drp_supplier" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style4">
                                      
                                    </td>
                                    <td class="style1">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right" class="style6">
                                        ผู้ส่ง : 
                                    </td>
                                    <td colspan = 3 style="text-align: left">
                                        <input type ="text" style="width:150px"  name ="txt_preSend"  id="txt_preSend" />
                                    </td>
                                    <td style="text-align: right" class="style4">
                                        ผู้รับ :
                                    </td>
                                    <td class="style1">
                                        <input type ="text" style="width:150px"  name ="txt_preReceive"  id="txt_preReceive" />
                                        
                                        <input type ="hidden" style="width:150px"  name ="hid_preReceive"  id="hid_preReceive"  />
                                        
                                        <img  src="../images/icon_zoom.png" onclick="JavaScript:img_searchUser_Click()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=6 align=left style="text-align: center">
                                         <input  type="button" value="ลบ" name="btn_receive" 
                                         onclick="JavaScript:btn_deleteDetailOnClick()"  BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 74px;" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                                         <input  type="button" value="เพิ่มรายการรับเข้ากรณีอื่นๆ" name="btn_receive" 
                                         onclick="JavaScript:btn_addReceiveOnClick()" BackColor="Transparent" 
                                             BorderStyle="None" 
                                             Style="background-image: url('../Images/button_mll.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 190px;" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button_mll.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_mll_over.png)';this.style.color = '#EC467E';" />
        
                                    </td>


                                </tr>
                                <tr>
                                    <td colspan = 6>
                                        <div id="myDetail">
                                         </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan = 6>
                                        <table>
                                            <tr>
                                                <td align=right>
                                                    วันที่สร้างข้อมูล : 
                                                </td>
                                                <td>
                                                     <input type ="text" style="width:150px"  name ="txt_createDate"  id="txt_createDate" readonly />
                                                </td>
                                                <td align=right>
                                                    ผู้สร้าง : 
                                                </td>
                                                <td>
                                                     <input type ="text" style="width:150px"  name ="txt_create"  id="txt_create" readonly />
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align=right>
                                                    วันที่แก้ไขล่าสุด : 
                                                </td>
                                                <td>
                                                     <input type ="text" style="width:150px"  name ="txt_create" id="txt_updateDate" readonly />
                                                </td>
                                                <td align=right>
                                                    ผู้แก้ไขล่าสุด : 
                                                </td>
                                                <td>
                                                     <input type ="text" style="width:150px" ID="txt_update" runat="server" readonly>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td align=center colspan = 6>
                                         <input  type="button" value="บันทึก" name="btn_save" id ="btn_save" 
                                         onclick="JavaScript:btn_saveOnClick()"  BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                                         <input  type="button" value="ลบข้อมูล" name="btn_delete" id="btn_delete" 
                                         onclick="JavaScript:btn_deleteOnClick()"  BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;"  
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                                          <input  type="button" value="ยกเลิก" name="btn_reset" id="btn_reset"
                                         onclick="JavaScript:btn_resetOnClick()"  BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat;  cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;"  
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                                    </td>
                                   
                                </tr>
                                
                            </table>
                            
                        </td>
                        </tr>
                        </table>
                </td>
             
    </tr>


    </table>
    
     
    

    <br/>
   

  





   
    

    <div class="modalPage2"  id = "tablePopupDetail" >
    <div class="modalBackground2" id = "modalBackground2">
    </div>
    <div class="modalContainer2">
        <div class="modal2" style="text-align: left">


       <div align="right">
 <a href="javascript:hideModal('tablePopupDetail')"><img src="../images/close.png"  border=0 ></a></div>

            <div class="modalBody2" style="text-align: left">
     
    
    
    <table style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;width: 800px">
                <tr>
                    <td class="tableHeader" align="left">
                     บันทึกการรับสินค้ากรณีอื่นๆ 
                    </td>
                </tr>
            <tr>
            <td class="tableBody">
                <table class="none" border="0">
                    <tr>
                        <td align="right"> 
                            รหัส Barcode จาก Supplier : 
                        </td>
                        <td>
                            <input id = "txt_barcodeSupplier" name = "txt_barcodeSupplier" type ="text" style="width:150px"/>
                        </td>
                        <td colspan = 6>

                        </td>
                    </tr>
                    <tr>
                        <td align="right"> 
                            รหัสสินค้า :
                        </td>
                        <td>
                            <input type ="text" style="width:150px" id="txt_productID"  onchange="JavaScript:productTab()" />
                            <%-- <input id = "txt_productID" name = "txt_productID" type ="text" style="width:150px"/>--%>
                        </td>
                        <td align="right">
                            รายการ : 
                            
                        </td>
                        <td>
                            <input type ="text" style="width:150px" id="txt_productName" />
                            <%--<input id = "txt_productName" name = "txt_productName" type ="text" style="width:150px"/>--%>
                             <img  src="../images/icon_zoom.png" onclick="JavaScript:img_searchProduct_Click()" />
                             <%--<asp:ImageButton ID="img_searchProduct" runat="server" 
                                ImageUrl="~/images/icon_zoom.png" onclick="img_searchProduct_Click" />--%>
                        </td>
                        <td align="right">
                            
                            หน่วย : 
                        </td>
                        <td>
                             <select name="drp_productPack" id="drp_productPack" onchange="JavaScript:requestPrice()" ></select>
                        </td>
                        </tr>
                        <tr>
                        <td align="right"> 
                            จำนวนรวม : 
                        </td>
                        <td>
                            <input id = "txt_total" name = "txt_total" type ="text" style="width:150px" readonly />
                        </td>
                        <td>
                             <input  type="button" value="Add Lot NO." name="btn_addLotNo" 
                                         onclick="JavaScript:CreateNewRowLot('','','','','','')" BackColor="Transparent" BorderStyle="None" Style="background-image: url(../Images/button_mll.png);color:White;background-color:Transparent;border-style:None;background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px;" Width="185"  
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button_mll.png)';this.style.color = 'white';" onmouseover="this.style.backgroundImage = 'url(../Images/button_mll_over.png)';this.style.color = '#EC467E';" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tableBody">
            <form id="detailReceiveForm" name="detailReceiveForm"  method="post"  > 
                Lot No.
                <table>
                    <tr>
                        <td>
                            <div id = "runningLot1">1.</div>  <input type="hidden" name="hid_Lot" id="hid_Lot1" value="1" />    
                        </td>
                        <td>
                            รหัส Barcode : 
                        </td>
                        <td>
                             <input id = "txt_barcodeDetail1" name = "txt_barcodeDetail" type ="text" style="width:150px"/>
                        </td>
                        <td>
                            Lot No. 
                        </td>
                        <td colspan = 5>
                            <input id = "txt_lotDetail1" name = "txt_lotDetail" type ="text" style="width:150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            จำนวน : 
                        </td>
                        <td>
                            <input id = "txt_qtyDetail1" name = "txt_qtyDetail" type ="text" style="width:150px" onchange="JavaScript:calTotalLot('1')" />
                        </td>
                         <td>
                            Expire Date : 
                        </td>
                        <td>
                        <script src="../Script/js_calendardateUser.js" type="text/javascript"></script>
                        <script src="../Script/js_checkTime.js" type="text/javascript"></script>
                        <script src="../Script/js_checkDate.js" type="text/javascript"></script>
                        <script src="../Script/js_checkStr.js" type="text/javascript"></script>

                         <table id='uc_Expire_tblCalendar1' cellpadding='0' cellspacing='0'><tr>
                         <td style='text-align: left; width: 80px' align='left' valign='middle'>
                         <input name='txt_expireDateDetail1' type='text' id='txt_expireDateDetail1' onblur='if(!isDate(this.value))this.focus();' style='width:76px;' />
                         <input type='hidden' name='uc_Expire_tblCalendar" + int_lot + "$txtDate_MaskedEditExtender_ClientState' id='uc_txt_expireDateDetail_txtDate_MaskedEditExtender_ClientState' /></td>
                         <td align='left'><img src='../images/Commands/calendar_selector.gif' onclick="popUpCalendar_byID(this, 'txt_expireDateDetail1', 'dd/mm/yyyy')" style='cursor: pointer;' />
                         </td><td></td> </tr> </table>
                         
                         <script type="text/javascript">
                             //onclick="popUpCalendar(this, document.getElementById('txt_expireDateDetail1'), 'dd/mm/yyyy')"
                             var dtCh = "/";
                             var minYear = 2400;
                             var maxYear = 2700;

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



                             <%--<input id = "txt_expireDateDetail1" name = "txt_expireDateDetail" type ="text" style="width:150px"/> --%>
                        </td>
                         <td>
                            ราคาต่อหน่วย : 
                        </td>
                        <td>
                             <input id = "txt_priceDetail1" name = "txt_priceDetail" type ="text" style="width:150px" onchange="JavaScript:calTotalLot('1')"/> บาท
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan = 6 align =left>
                            มูลค่ารวม  <input id = "txt_totalDetail1" name = "txt_totalDetail" type ="text" style="width:150px" readOnly /> บาท
                        </td>
                  
                    </tr>
                    <tr>
                        <td colspan = 7>
                        สถานที่เก็บ
                        <table>
                            <tr>
                                <td>
                                     <div id = "runningStock1_1">1.</div><input type='hidden' id = "hid_lotStock1_1" name='hid_lotStock' value='1' />
                                </td>
                                <td>
                                    จำนวน : 
                                </td>
                                <td>
                                    <input id = "txt_qtyStock1_1" name = "txt_qtyStock1_1" type ="text" style="width:150px"/>
                                </td>
                                <td>
                                    รหัสสถานที่ : 
                                </td>
                                <td>
                                    <select ID="drp_idStock1_1" name ="drp_idStock1_1" >
                                        </select>    
                                </td>

                                <td>
                                    <input  type="button" value="Add สถานที่." name="btn_addStock" 
                                onclick="JavaScript:CreateNewRowStock('1','','','')" BackColor="Transparent" 
                                BorderStyle="None" 
                                Style="background-image: url('../Images/button_mll.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 191px;"  
    Height="25" 
                                onmouseout="this.style.backgroundImage = 'url(../Images/button_mll.png)';this.style.color = 'white';" 
                                onmouseover="this.style.backgroundImage = 'url(../Images/button_mll_over.png)';this.style.color = '#EC467E';" />


                                <input  type='button' value='ลบ' name='btn_lotDelete' id='btn_lotDelete' onclick='JavaScript:btn_deleteLotStockOnClick(1)' BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;" forecolor="white" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                                </td>
                            </tr>
                        </table>
                        <div id = "detailStockUnder1"></div>
                        </td>
                    </tr>
                </table>

                <div id = "detailLotUnder"></div>
                
                <input  type="button" value="บันทึก" name="btn_saveDetail" onclick="JavaScript:btn_saveDetailOnClick()" BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png');color:White;background-color:Transparent;border-style:None; background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" /> 
                <input  type="button" value="ยกเลิก" name="btn_resetDetail" onclick="JavaScript:btn_resetDetailOnClick()" BorderStyle="None" 
                                             Style="background-image: url('../Images/button.png'); color:White;background-color:Transparent;border-style:None;background-repeat:no-repeat; cursor: pointer;margin: 4px 4px 4px 4px; width: 75px;" 
    Height="25" onmouseout="this.style.backgroundImage = 'url(../Images/button.png)';this.style.color = 'white';" 
                                             onmouseover="this.style.backgroundImage = 'url(../Images/button_over.png)';this.style.color = '#EC467E';" />
                
            

                
            </td>
        </tr>  
        
                
       
        
    </table>
    
 



     </div>
        </div>
    </div>
    </div>

    
   
          
            
    


    <div class="modalPage3"  id = "tablePopup" >
    <div class="modalBackground3" id = "modalBackground">
    </div>
    <div class="modalContainer3">
        <div class="modal3" style="text-align: left">

        <div align="right">
 <a href="javascript:hideModal('tablePopup')"><img src="../images/close.png"   border=0 ></a></div>
  
            <div class="modalBody3" style="text-align: left"> 
            <table style="background-image: url(../Images/Stock/bg.jpg); padding-top: 7px;">
             <tr>
                    <td class="tableHeader" align="left">
                     โปรดเลือก 
                    </td>
                </tr>
                <tr>
                 <td  class="tableBody">
     
                        <div id = "tableSearchProduct"></div>        
                   </td>
                </tr>

                </table>
                </div>
                

            
        </div>
    </div>
    </div>
    </center>
    </asp:Content>



