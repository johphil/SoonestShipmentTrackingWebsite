<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Account.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Create Account</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:560px;">
        <div class="card-panel">
            <div class="page-heading">Create a Customer Account</div>
            <div class="page-subheading">Register to view and track every shipment tied to you.</div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" Display="Dynamic" CssClass="field-error" ErrorMessage="Full name is required." ValidationGroup="Register" />
            </div>

            <div class="form-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Email is required." ValidationGroup="Register" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error"
                    ErrorMessage="Enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ValidationGroup="Register" />
            </div>

            <div class="form-group">
                <label>Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+63 900 000 0000" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone" Display="Dynamic" CssClass="field-error" ErrorMessage="Phone number is required." ValidationGroup="Register" />
            </div>

            <div class="form-group">
                <label>Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" Display="Dynamic" CssClass="field-error" ErrorMessage="Password is required." ValidationGroup="Register" />
                <div class="form-hint">At least 6 characters, including a digit and a lowercase letter.</div>
            </div>

            <div class="form-group">
                <label>Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:CompareValidator runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Display="Dynamic" CssClass="field-error" ErrorMessage="Passwords do not match." ValidationGroup="Register" />
            </div>

            <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-red btn-block" Text="Register" OnClick="btnRegister_Click" ValidationGroup="Register" />

            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                Already have an account? <a runat="server" href="~/Views/Account/Login.aspx">Log in</a>
            </p>
        </div>
    </div>
</div>
</asp:Content>
