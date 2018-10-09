<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarPayGoodsControl.ascx.cs" Inherits="GPlus.UserControls.CalendarPayGoodsControl" %>

<%@ Register Src="~/UserControls/CalendarDependOnMonthYearControl.ascx" TagName="CalendarCtrl" TagPrefix="uc1" %>

<br />
<table border="0" width="100%" cellspacing="5">
    <tr>
        <td style="text-align: right; width:250px">
            เดือน
        </td>
        <td style="width: 50px">
            <asp:DropDownList runat="server" ID="ddlMonth" OnSelectedIndexChanged="MonthChanged" AutoPostBack="true">
                <asp:ListItem Value="1">มกราคม</asp:ListItem>
                <asp:ListItem Value="2">กุมภาพันธ์</asp:ListItem>
                <asp:ListItem Value="3">มีนาคม</asp:ListItem>
                <asp:ListItem Value="4">เมษายน</asp:ListItem>
                <asp:ListItem Value="5">พฤษภาคม</asp:ListItem>
                <asp:ListItem Value="6">มิถุนายน</asp:ListItem>
                <asp:ListItem Value="7">กรกฎาคม</asp:ListItem>
                <asp:ListItem Value="8">สิงหาคม</asp:ListItem>
                <asp:ListItem Value="9">กันยายน</asp:ListItem>
                <asp:ListItem Value="10">ตุลาคม</asp:ListItem>
                <asp:ListItem Value="11">พฤศจิกายน</asp:ListItem>
                <asp:ListItem Value="12">ธันวาคม</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 30px; text-align: right">
            ปี
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlYear" OnSelectedIndexChanged="YearChanged" AutoPostBack="true">
                <asp:ListItem Value="2007">2550</asp:ListItem>
                <asp:ListItem Value="2008">2551</asp:ListItem>
                <asp:ListItem Value="2009">2552</asp:ListItem>
                <asp:ListItem Value="2010">2553</asp:ListItem>
                <asp:ListItem Value="2011">2554</asp:ListItem>
                <asp:ListItem Value="2012">2555</asp:ListItem>
                <asp:ListItem Value="2013">2556</asp:ListItem>
                <asp:ListItem Value="2014">2557</asp:ListItem>
                <asp:ListItem Value="2015">2558</asp:ListItem>
                <asp:ListItem Value="2016">2559</asp:ListItem>
                <asp:ListItem Value="2017">2560</asp:ListItem>
                <asp:ListItem Value="2018">2561</asp:ListItem>
                <asp:ListItem Value="2019">2562</asp:ListItem>
                <asp:ListItem Value="2020">2563</asp:ListItem>
                <asp:ListItem Value="2021">2564</asp:ListItem>
                <asp:ListItem Value="2022">2565</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<table border="0" width="100%" cellspacing="10">
    <tr>
        <td style="width: 250px; text-align: right">สัปดาห์ที่</td><td style="width: 150px">ตั้งแต่วันที่</td><td>ถึงวันที่</td>
    </tr>
    <tr>
        <td align="right">1</td>
        <td><uc1:CalendarCtrl runat="server" ID="week1_1Calendar" /></td>
        <td><uc1:CalendarCtrl runat="server" ID="week1_2Calendar" /></td>
    </tr>
    <tr>
        <td align="right">2</td>
        <td><uc1:CalendarCtrl runat="server" ID="week2_1Calendar" /></td>
        <td><uc1:CalendarCtrl runat="server" ID="week2_2Calendar" /></td>
    </tr>
    <tr>
        <td align="right">3</td>
        <td><uc1:CalendarCtrl runat="server" ID="week3_1Calendar" /></td>
        <td><uc1:CalendarCtrl runat="server" ID="week3_2Calendar" /></td>
    </tr>
    <tr>
        <td align="right">4</td>
        <td><uc1:CalendarCtrl runat="server" ID="week4_1Calendar" /></td>
        <td><uc1:CalendarCtrl runat="server" ID="week4_2Calendar" /></td>
    </tr>
</table>
<table border="0" cellspacing="5" width="100%">
    <tr>
        <td style="width: 120px; text-align: right">สถานะการใช้งาน : </td>
        <td>                                
            <asp:RadioButtonList ID="rdbStatus" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Active" Selected="True" Value="1"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td></td>
    </tr>
    <tr>
        <td style="text-align: right">
            วันที่สร้าง : 
        </td>
        <td>
            <asp:Label ID="lblCreateDate" runat="server"></asp:Label>
        </td>
        <td style="text-align: right">
            ผู้ที่สร้าง : 
        </td>
        <td>
            <asp:Label ID="lblCreator" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">
            วันที่แก้ไขล่าสุด :
        </td>
        <td>
            <asp:Label ID="lblEditDate" runat="server"></asp:Label>
        </td>
        <td style="text-align: right">
            ผู้ที่แก้ไขล่าสุด : 
        </td>
        <td>
            <asp:Label ID="lblEditor" runat="server"></asp:Label>
        </td>
    </tr>
</table>

<table width="100%" border="0">
    <tr>
        <td style="width: 250px"></td>
        <td style="width: 50px">
            <asp:Button ID="btnSaveData" runat="server" Text="บันทึก" OnClick="btnSave_Click"/>
        </td>
        <td>
            <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" OnClick="btnCancel_Click" />
        </td>
    </tr>
</table>

