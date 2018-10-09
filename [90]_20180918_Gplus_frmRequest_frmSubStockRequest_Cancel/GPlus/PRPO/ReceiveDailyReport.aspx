<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="ReceiveDailyReport.aspx.cs" Inherits="GPlus.PRPO.ReceiveDailyReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Src="../UserControls/CalendarControl.ascx" TagName="CalendarControl"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 99px;
        }
        .style3
        {
            width: 230px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language="javascript"  type="text/javascript">
  //  alert("ddd");
    $(function () {
        $("#receive_date1").datepicker({
            showOn: "button",
            buttonImage: "../images/calendar.gif",
            buttonImageOnly: true ,
            dateFormat: "dd/mm/yy",
            dayNamesMin: ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'],
            monthNamesShort: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
            changeMonth: true,
            changeYear: true,

            beforeShow: function () {
                if ($(this).val() != "") {
                    var arrayDate = $(this).val().split("/");
                    arrayDate[2] = parseInt(arrayDate[2]) - 543;
                    $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
                }
                setTimeout(function () {
                    $.each($(".ui-datepicker-year option"), function (j, k) {
                        var textYear = parseInt($(".ui-datepicker-year option").eq(j).val()) + 543;
                        $(".ui-datepicker-year option").eq(j).text(textYear);
                    });
                }, 50);

            },
            onChangeMonthYear: function () {
                setTimeout(function () {
                    $.each($(".ui-datepicker-year option"), function (j, k) {
                        var textYear = parseInt($(".ui-datepicker-year option").eq(j).val()) + 543;
                        $(".ui-datepicker-year option").eq(j).text(textYear);
                    });
                }, 50);
            },
            onClose: function () {
                if ($(this).val() != "" && $(this).val() == dateBefore) {
                    var arrayDate = dateBefore.split("/");
                    arrayDate[2] = parseInt(arrayDate[2]) + 543;
                    $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
                }
            },
            onSelect: function (dateText, inst) {
                dateBefore = $(this).val();
                var arrayDate = dateText.split("/");
                arrayDate[2] = parseInt(arrayDate[2]) + 543;
                $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
            }

        });  
      
    });

    $(function () {
        $("#receive_date2").datepicker({
            showOn: "button",
            buttonImage: "../images/calendar.gif",
            buttonImageOnly: true,
            dateFormat: "dd/mm/yy",
            dayNamesMin: ['อา', 'จ', 'อ', 'พ', 'พฤ', 'ศ', 'ส'],
            monthNamesShort: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
            changeMonth: true,
            changeYear: true,

            beforeShow: function () {
                if ($(this).val() != "") {
                    var arrayDate = $(this).val().split("/");
                    arrayDate[2] = parseInt(arrayDate[2]) - 543;
                    $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
                }
                setTimeout(function () {
                    $.each($(".ui-datepicker-year option"), function (j, k) {
                        var textYear = parseInt($(".ui-datepicker-year option").eq(j).val()) + 543;
                        $(".ui-datepicker-year option").eq(j).text(textYear);
                    });
                }, 50);

            },
            onChangeMonthYear: function () {
                setTimeout(function () {
                    $.each($(".ui-datepicker-year option"), function (j, k) {
                        var textYear = parseInt($(".ui-datepicker-year option").eq(j).val()) + 543;
                        $(".ui-datepicker-year option").eq(j).text(textYear);
                    });
                }, 50);
            },
            onClose: function () {
                if ($(this).val() != "" && $(this).val() == dateBefore) {
                    var arrayDate = dateBefore.split("/");
                    arrayDate[2] = parseInt(arrayDate[2]) + 543;
                    $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
                }
            },
            onSelect: function (dateText, inst) {
                dateBefore = $(this).val();
                var arrayDate = dateText.split("/");
                arrayDate[2] = parseInt(arrayDate[2]) + 543;
                $(this).val(arrayDate[0] + "/" + arrayDate[1] + "/" + arrayDate[2]);
            }

        });  
       
    });
</script>
  <table cellpadding="0" cellspacing="0" style="width: 840px">
        <tr>
            <td class="tableHeader">
                รายงานสรุปรายการรับ-เบิก ประจำวัน
            </td>
        </tr>


        <tr>
            <td class="tableBody">
             
                
                        <table width="100%">
                    <tr>
                        <td style="width: 130px;" align="right">
                            &nbsp;</td>
                        <td class="style1" colspan="3">
                             <fieldset style="width:310px">
                                 <legend>ประเภท</legend>
                                
                                 <asp:RadioButtonList ID="rdbOnHand0" runat="server" 
                                     RepeatDirection="Horizontal">
                                     <asp:ListItem Selected="True" Text="รายการรับ"></asp:ListItem>
                                     <asp:ListItem Text="รายการเบิก"></asp:ListItem>
                                 </asp:RadioButtonList>

                             </fieldset>
                             
                             </td>
                    </tr>
                    <tr>
                        <td style="width: 130px;" align="right">
                            &nbsp;</td>
                        <td colspan="3">
                             <table>
                                                <tr>
                                                 <td>
                                                    วันที่ :
                                                    </td>

                                                    <td>
                                                    <%
                                                        String txtReceiveDate1 = Request.Form["receive_date1"]; 
                                                         %>
                                                          <input type="text" name="receive_date1" id="receive_date1"   value="<%=txtReceiveDate1 %>" />
                                                    <td>
                                                    <td>ถึงวันที่</td>
                                                    <td>
                                                         <input type="text" name="receive_date2" id="receive_date2"  value="<%=Request.Form["receive_date2"] %>" /></td>
                                                </tr>
                                                </table> </td>
                    </tr>
                            <tr>
                                <td align="right" style="width: 130px;">
                                   คลังสินค้า
                                </td>
                                <td class="style3">

                                    <asp:DropDownList ID="ddlStock" runat="server" DataTextField="StockType_Name" 
                                        DataValueField="StockType_ID" Width="195">
                                    </asp:DropDownList>
                                </td>
                                <td align="right" class="style2">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                    <tr>
                        <%--<td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCate" runat="server" Width="195">
                            </asp:DropDownList>
                        </td>--%>
                         <td align="left" colspan="4">

                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                           <table >
                           <tr>
                           <td style="width: 130px;" align="right">
                            ประเภทวัสดุอุปกรณ์
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaterialType" runat="server" Width="195" 
                                    DataTextField="MaterialType_Name" DataValueField="MaterialType_ID"
                                    AutoPostBack="true" 
                                    onselectedindexchanged="ddlMaterialType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" align="right">
                            ประเภทอุปกรณ์ย่อย
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubMaterialType" runat="server" Width="195" DataTextField="SubMaterialType_Name" DataValueField="SubMaterialType_ID">
                            </asp:DropDownList>
                        </td>
                           </tr>
                           </table>
                           </ContentTemplate>
                </asp:UpdatePanel>



                        </td>

                    </tr>


                    <tr>
                        <td style="width: 130px;" align="right">
                            รหัสวัสดุอุปกรณ์
                        </td>
                        <td class="style3">
                            <asp:TextBox ID="txtItemCode" runat="server" MaxLength="20" Width="190"></asp:TextBox>
                        </td>
                        <td align="right" class="style2">
                            รายการสินค้า
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="100" Width="190"></asp:TextBox>
                        </td>
                    </tr>
                      </table>
                    


                           <table   align="center">
                    <tr>
                        <td colspan="4" alig n="center">
                            <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" CausesValidation="False" OnClick="btnSearch_Click" />&nbsp;&nbsp;

                            </td>
                            <td>  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                          <ContentTemplate>
                      <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CausesValidation="False" OnClick="btnCancel_Click" />
                          </ContentTemplate>
                </asp:UpdatePanel>
</td>
                           
                           
                        </td>
                    </tr>
              </table>
                  
                
              
                      
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
    AutoDataBind="true"   ToolPanelView="None" />

                     


                    
                  


</asp:Content>
