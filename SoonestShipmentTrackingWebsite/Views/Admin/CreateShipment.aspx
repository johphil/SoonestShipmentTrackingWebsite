<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="CreateShipment.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.CreateShipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">New Shipment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
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
                    <label>Sender Address</label>
                    <asp:TextBox ID="txtSenderAddress" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Recipient Name</label>
                    <asp:TextBox ID="txtRecipientName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientName" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Destination City</label>
                    <asp:TextBox ID="txtDestinationCity" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDestinationCity" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <div class="form-group">
                <label>Recipient Address</label>
                <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientAddress" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Recipient Phone</label>
                    <asp:TextBox ID="txtRecipientPhone" runat="server" CssClass="form-control" placeholder="+63 900 000 0000" />
                    <div class="form-hint">Leave blank to use the customer's registered phone.</div>
                </div>
                <div class="form-group">
                    <label>Recipient Email</label>
                    <asp:TextBox ID="txtRecipientEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <div class="form-hint">Leave blank to use the customer's registered email.</div>
                </div>
            </div>

            <div style="display:grid;grid-template-columns:1.4fr .8fr .8fr;gap:16px;">
                <div class="form-group">
                    <label>Package Description</label>
                    <asp:TextBox ID="txtPackageDescription" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label>Weight (kg)</label>
                    <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label>Est. Delivery Date</label>
                    <asp:TextBox ID="txtEstDelivery" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>

            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-red" Text="Create Shipment" OnClick="btnCreate_Click" ValidationGroup="CreateShipment" />
            <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Cancel</a>
        </div>
    </div>
</div>
</asp:Content>
