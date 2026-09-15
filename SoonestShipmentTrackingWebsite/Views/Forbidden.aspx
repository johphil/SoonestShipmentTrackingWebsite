<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Forbidden.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Forbidden" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Access Denied</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="container">
    <div class="error-page">
        <div class="error-code">4<span>0</span>3</div>
        <h2>Access Denied</h2>
        <p>
            You don't have permission to view this page. If you think this is a
            mistake, please contact our support team.
        </p>

        <a runat="server" href="~/Default.aspx" class="btn btn-red">Back to Home</a>

        <asp:Panel ID="pnlLoggedIn" runat="server" Visible="false" style="display:inline;">
            <asp:LinkButton ID="btnLogOff" runat="server" CssClass="btn btn-outline" OnClick="btnLogOff_Click"
                style="border-color:#0b2540;color:#0b2540;">Log Off and Switch Account</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="pnlLoggedOut" runat="server" Visible="false" style="display:inline;">
            <a runat="server" href="~/Views/Account/Login.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Log In</a>
        </asp:Panel>
    </div>
</div>
</asp:Content>
