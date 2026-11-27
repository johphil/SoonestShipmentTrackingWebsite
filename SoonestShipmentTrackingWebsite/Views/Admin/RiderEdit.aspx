<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="RiderEdit.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.RiderEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Rider Details</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section app-section">
        <div class="container container-lg">
            <div class="card-panel">
                <div class="page-heading">
                    <asp:Literal ID="litHeading" runat="server" /></div>
                <div class="page-subheading">Create and maintain rider records used by shipment assignment.</div>

                <asp:Panel ID="pnlError" runat="server" Visible="false">
                    <div class="validation-summary">
                        <asp:Literal ID="litError" runat="server" /></div>
                </asp:Panel>

                <asp:HiddenField ID="hdnRiderId" runat="server" />

                <div class="form-group">
                    <label>Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ErrorMessage="Full name is required." />
                </div>

                <div class="form-group">
                    <label>Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="256" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ErrorMessage="Email is required." />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." />
                </div>

                <div class="form-group">
                    <label>SMS / Mobile Number</label>

                    <div class="phone-input-group">
                        <span class="phone-prefix">+63</span>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control phone-input" MaxLength="20" placeholder="9XX XXX XXXX" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMobile" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ErrorMessage="Mobile number is required." />
                </div>

                <div class="form-group">
                    <label>License Number</label>
                    <asp:TextBox ID="txtLicenseNumber" runat="server" CssClass="form-control" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLicenseNumber" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ErrorMessage="License number is required." />
                </div>

                <div class="form-group">
                    <label>Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" MaxLength="250" TextMode="MultiLine" Rows="3" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" CssClass="field-error" Display="Dynamic" ValidationGroup="Rider" ErrorMessage="Address is required." />
                </div>

                <asp:Panel ID="pnlMetadata" runat="server" Visible="false" CssClass="form-hint">
                    <div><strong>Date Created:</strong>
                        <asp:Literal ID="litDateCreated" runat="server" /></div>
                    <div><strong>Date Modified:</strong>
                        <asp:Literal ID="litDateModified" runat="server" /></div>
                </asp:Panel>

                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Rider" ValidationGroup="Rider" OnClick="btnSave_Click" />
                <a runat="server" href="~/Views/Admin/Riders.aspx" class="btn btn-outline">Cancel</a>
            </div>
        </div>
    </div>
</asp:Content>
