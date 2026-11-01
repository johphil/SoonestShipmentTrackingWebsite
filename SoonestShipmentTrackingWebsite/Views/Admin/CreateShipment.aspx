<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="CreateShipment.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.CreateShipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">New Shipment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section" style="padding-top:44px;">
    <div class="container" style="max-width:760px;">
        <div class="card-panel">
            <div class="page-heading">Create Shipment</div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <div class="form-group">
                <label>Customer</label>
                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer" InitialValue="" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select a customer." ValidationGroup="CreateShipment" />
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Sender Name</label>
                    <asp:TextBox ID="txtSenderName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSenderName" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Branch Origin</label>
                    <asp:DropDownList ID="ddlBranchOrigin" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlBranchOrigin" InitialValue="" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select a branch origin." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <div class="form-group">
                <label>Recipient Name</label>
                <asp:TextBox ID="txtRecipientName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientName" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
            </div>

            <div class="form-group">
                <label>Recipient Address</label>
                <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientAddress" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Recipient Phone</label>
                    <asp:TextBox ID="txtRecipientPhone" runat="server" CssClass="form-control" placeholder="0987 654 3210" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientPhone" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Recipient Email</label>
                    <asp:TextBox ID="txtRecipientEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <div style="display:grid;grid-template-columns:1.4fr .8fr .8fr;gap:16px;">
                <div class="form-group">
                    <label>Package Description</label>
                    <asp:TextBox ID="txtPackageDescription" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPackageDescription" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Weight (kg)</label>
                    <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtWeight" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Delivery Date</label>
                    <asp:TextBox ID="txtEstDelivery" runat="server" CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEstDelivery" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-red" Text="Create Shipment" OnClick="btnCreate_Click" ValidationGroup="CreateShipment" />
            <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#07233d;color:#07233d;">Cancel</a>
        </div>
    </div>
</div>
</asp:Content>
