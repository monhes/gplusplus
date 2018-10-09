<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Main.Master" AutoEventWireup="true" CodeBehind="SupplierPriceMgt.aspx.cs" Inherits="GPlus.PRPO.SupplierPriceMgt" %>
<%@ Register src="../UserControls/HistoryPurchaseControl1.ascx" tagname="HistoryPurchaseControl1" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:HistoryPurchaseControl1 ID="HistoryPurchaseControl11" runat="server" />
</asp:Content>
