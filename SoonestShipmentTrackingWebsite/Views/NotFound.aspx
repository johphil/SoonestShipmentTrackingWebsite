<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="NotFound.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.NotFound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Page Not Found</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="container">
    <div class="error-page">
        <div class="error-code">4<span>0</span>4</div>
        <h2>Page Not Found</h2>
        <p>
            The page you're looking for doesn't exist, may have been moved, or the
            link you followed might be out of date.
        </p>
        <a runat="server" href="~/Views/Default.aspx" class="btn btn-red">Back to Home</a>
    </div>
</div>
</asp:Content>
