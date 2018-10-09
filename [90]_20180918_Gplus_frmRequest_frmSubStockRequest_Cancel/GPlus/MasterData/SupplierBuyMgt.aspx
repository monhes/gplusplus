<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="SupplierBuyMgt.aspx.cs" Inherits="GPlus.MasterData.SupplierBuyMgt" %>
<%@ Register src="../UserControls/HistoryPurchaseControl2.ascx" tagname="HistoryPurchaseControl2" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:HistoryPurchaseControl2 ID="HistoryPurchaseControl21" runat="server" />
</asp:Content>
