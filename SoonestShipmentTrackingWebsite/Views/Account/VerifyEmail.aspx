<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Account.VerifyEmail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Verify Your Email</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:460px;">
        <div class="card-panel">
            <div class="page-heading">Verify Your Email</div>
            <div class="page-subheading">
                Enter the 6-digit code we sent to <strong><asp:Literal ID="litEmail" runat="server" /></strong>.
            </div>

            <asp:Panel ID="pnlInfo" runat="server" Visible="false">
                <div class="alert-success"><asp:Literal ID="litInfo" runat="server" /></div>
            </asp:Panel>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <asp:HiddenField ID="hdnEmail" runat="server" />

            <div class="form-group">
                <label for="txtCode">Verification Code</label>
                <asp:TextBox ID="txtCode" runat="server" CssClass="code-input" MaxLength="6"
                    TextMode="SingleLine" autocomplete="one-time-code" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCode" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter the 6-digit code." ValidationGroup="Verify" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCode" Display="Dynamic" CssClass="field-error"
                    ErrorMessage="The code must be exactly 6 digits." ValidationExpression="^\d{6}$" ValidationGroup="Verify" />
            </div>

            <asp:Button ID="btnVerify" runat="server" CssClass="btn btn-red btn-block" Text="Verify Email" OnClick="btnVerify_Click" ValidationGroup="Verify" />

            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                Didn't get a code?
                <asp:LinkButton ID="btnResend" runat="server" OnClick="btnResend_Click" CausesValidation="false">Resend Code</asp:LinkButton>
            </p>
        </div>
    </div>
</div>
</asp:Content>
