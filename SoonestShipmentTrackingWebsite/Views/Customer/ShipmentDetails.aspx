<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="ShipmentDetails.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Customer.ShipmentDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Shipment Details</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">

        <div class="card-panel">
            <div class="ship-head">
                <div class="ship-head-main">
                    <h2 class="ship-no"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                    <asp:Literal ID="litStatusBadge" runat="server" />
                </div>
                <a runat="server" href="~/Views/Customer/MyShipments.aspx" class="btn btn-outline">&larr; Back to My Shipments</a>
            </div>
            <asp:Literal ID="litProgress" runat="server" />
        </div>

        <div class="split">
            <div class="split-col">
                <div class="card-panel">
                    <h4 class="panel-title">Shipment Details</h4>
                    <div class="kv-grid cols-2">
                        <div class="kv"><div class="kv-label">Sender</div><div class="kv-value"><asp:Literal ID="litSenderName" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Branch Origin</div><div class="kv-value"><asp:Literal ID="litBranchOrigin" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Recipient</div><div class="kv-value"><asp:Literal ID="litRecipientName" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Recipient Phone</div><div class="kv-value"><asp:Literal ID="litRecipientPhone" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Recipient Email</div><div class="kv-value"><asp:Literal ID="litRecipientEmail" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Estimated Delivery</div><div class="kv-value"><asp:Literal ID="litEta" runat="server" /></div></div>
                        <div class="kv kv-wide"><div class="kv-label">Recipient Address</div><div class="kv-value"><asp:Literal ID="litRecipientAddress" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Package</div><div class="kv-value"><asp:Literal ID="litPackageDescription" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Weight</div><div class="kv-value"><asp:Literal ID="litWeight" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Rider</div><div class="kv-value"><asp:Literal ID="litRiderInfo" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Created</div><div class="kv-value"><asp:Literal ID="litCreated" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Time in System</div><div class="kv-value"><asp:Literal ID="litElapsed" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Last Update</div><div class="kv-value"><asp:Literal ID="litUpdated" runat="server" /></div></div>
                    </div>
                </div>

                <asp:Panel ID="pnlFeedback" runat="server" Visible="false" CssClass="card-panel">
                    <h4 class="panel-title">Delivery confirmation</h4>
                    <asp:Literal ID="litFeedback" runat="server" />
                    <div style="margin-top:6px;">
                        <asp:Button ID="btnOrderReceived" runat="server" CssClass="btn btn-red" Text="Order received" OnClick="btnOrderReceived_Click" />
                    </div>
                    <hr style="border:none;border-top:1px solid #dbe6f0;margin:10px 0;" />
                    <label style="font-size:11.5px;font-weight:700;">Report an issue with your received items</label>
                    <asp:TextBox ID="txtIssueReport" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="1000" placeholder="Tell us what went wrong..." />
                    <div style="margin-top:6px;"><asp:Button ID="btnReportIssue" runat="server" CssClass="btn btn-outline" Text="Issue Report" OnClick="btnReportIssue_Click" /></div>
                </asp:Panel>
            </div>

            <div class="split-col">
                <div class="card-panel">
                    <h4 class="panel-title">Shipment History</h4>
                    <div class="table-scroll">
                    <asp:Repeater ID="rptHistory" runat="server">
                        <HeaderTemplate><table class="data-grid"><thead><tr><th>Date</th><th>Status</th><th>Location</th><th>Notes</th></tr></thead><tbody></HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="nowrap"><%# Eval("Timestamp", "{0:MMM d, yyyy}") %><span class="cell-sub"><%# Eval("Timestamp", "{0:h:mm tt}") %></span></td>
                                <td><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("Status")) %></td>
                                <td><%# Eval("Location") %></td>
                                <td><%# Eval("Notes") %><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.ProofImage(Eval("ProofOfDelivery"), Eval("ProofContentType")) %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate></tbody></table></FooterTemplate>
                    </asp:Repeater>
                    </div>
                    <asp:Label ID="lblNoHistory" runat="server" Text="No tracking events recorded yet." Visible="false" CssClass="empty-note" />
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
