<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="OrderReceived.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.OrderReceived" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Order Received</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container container-md">
        <div class="card-panel text-center">
            <div class="page-heading">Order Received</div>
            <asp:Literal ID="litMessage" runat="server" />
            <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                <p><strong>Tracking number:</strong> <asp:Literal ID="litControlNumber" runat="server" /></p>
                <p><strong>Received on:</strong> <asp:Literal ID="litReceivedDate" runat="server" /></p>
            </asp:Panel>
            <a runat="server" href="~/Views/Default.aspx" class="btn btn-primary">Back to Home</a>
        </div>
    </div>
</div>
</asp:Content>
