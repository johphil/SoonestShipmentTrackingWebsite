<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="ShipmentDetails.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Customer.ShipmentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Shipment Details</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:820px;">

        <div class="card-panel" style="margin-bottom:20px;">
            <div style="display:flex;justify-content:space-between;align-items:flex-start;">
                <div>
                    <h2 class="mt-0" style="color:#0b2540;margin-bottom:4px;"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                    <p style="color:#6b7684;margin:0;">To <asp:Literal ID="litRecipientName" runat="server" />, <asp:Literal ID="litDestinationCity" runat="server" /></p>
                </div>
                <asp:Literal ID="litStatusBadge" runat="server" />
            </div>
            <hr style="border:none;border-top:1px solid #eceef1;margin:16px 0;" />
            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div>
                    <p><strong>Sender:</strong> <asp:Literal ID="litSenderName" runat="server" /></p>
                    <p><strong>Recipient Address:</strong> <asp:Literal ID="litRecipientAddress" runat="server" /></p>
                </div>
                <div>
                    <p><strong>Package:</strong> <asp:Literal ID="litPackageDescription" runat="server" /></p>
                    <p><strong>Estimated Delivery:</strong> <asp:Literal ID="litEta" runat="server" /></p>
                </div>
            </div>
        </div>

        <div class="card-panel">
            <h4 style="margin-top:0;color:#0b2540;">Shipment History</h4>
            <asp:Repeater ID="rptHistory" runat="server">
                <HeaderTemplate><table class="data-grid"><thead><tr><th>Status</th><th>Location</th><th>Notes</th><th>Date</th></tr></thead><tbody></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("Status")) %></td>
                        <td><%# Eval("Location") %></td>
                        <td><%# Eval("Notes") %></td>
                        <td><%# Eval("Timestamp", "{0:MMM d, yyyy h:mm tt}") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></tbody></table></FooterTemplate>
            </asp:Repeater>
            <asp:Label ID="lblNoHistory" runat="server" Text="No tracking events recorded yet." Visible="false" style="color:#6b7684;" />
        </div>

        <p style="margin-top:18px;">
            <a runat="server" href="~/Views/Customer/MyShipments.aspx">&larr; Back to My Shipments</a>
        </p>
    </div>
</div>
</asp:Content>
