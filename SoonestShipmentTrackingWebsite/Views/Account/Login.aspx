<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Log In</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section auth-section">
        <div class="auth-card">
            <div class="page-heading">Log In</div>
            <div class="page-subheading">Access your account.</div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary">
                    <asp:Literal ID="litError" runat="server" /></div>
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
                <label class="check-row">
                    <asp:CheckBox ID="chkRememberMe" runat="server" /> Remember me
                </label>
            </div>

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary btn-block" Text="Log In" OnClick="btnLogin_Click" ValidationGroup="Login" />

            <p class="auth-links">
                <a runat="server" href="~/Views/Account/ForgotPassword.aspx">Forgot password?</a><br />
                No account yet? <a runat="server" href="~/Views/Account/Register.aspx">Register</a>
            </p>
        </div>
    </div>
</asp:Content>
