<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Log In</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:480px;">
        <div class="card-panel">
            <div class="page-heading">Log In</div>
            <div class="page-subheading">Access your customer account or the admin panel.</div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Email is required." ValidationGroup="Login" />
            </div>

            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" Display="Dynamic" CssClass="field-error" ErrorMessage="Password is required." ValidationGroup="Login" />
            </div>

            <div class="form-group">
                <label style="font-weight:normal;display:inline-flex;align-items:center;gap:6px;">
                    <asp:CheckBox ID="chkRememberMe" runat="server" /> Remember me
                </label>
            </div>

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-red btn-block" Text="Log In" OnClick="btnLogin_Click" ValidationGroup="Login" />

            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                No account yet? <a runat="server" href="~/Views/Account/Register.aspx">Register</a>
            </p>

            <hr style="border:none;border-top:1px solid #eceef1;" />
            <p style="font-size:12px;color:#6b7684;">
                Admin demo login: <code>admin@soonestglobalexpress.local</code> / <code>Admin@12345</code>
                (change this in <code>Migrations/Configuration.cs</code> before deploying).
            </p>
        </div>
    </div>
</div>
</asp:Content>
