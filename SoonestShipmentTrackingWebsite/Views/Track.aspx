<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Track.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Track" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Track Shipment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section app-section">
        <div class="container">
            <div class="card-panel" style="margin-bottom:10px;">
                <div class="page-head" style="margin-bottom:0;">
                    <div>
                        <div class="page-heading">Track Your Shipment</div>
                        <div class="page-subheading" style="margin:0;">Enter your Control No. to see the current status, delivery details and full history.</div>
                    </div>
                    <div style="display:flex;gap:6px;flex:1;max-width:520px;min-width:260px;">
                        <asp:TextBox ID="txtControlNumber" runat="server" CssClass="form-control" placeholder="Enter Control No." />
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-red" Text="Track" OnClick="btnSearch_Click" Style="white-space: nowrap;" />
                    </div>
                </div>

                <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
                    <div class="alert-warning" style="margin:8px 0 0;">No shipment found for that Control No. Please check the number and try again.</div>
                </asp:Panel>
            </div>

            <asp:Panel ID="pnlResult" runat="server" Visible="false">
                <div class="card-panel">
                    <div class="ship-head">
                        <div class="ship-head-main">
                            <h2 class="ship-no"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                            <asp:Literal ID="litStatusBadge" runat="server" />
                        </div>
                        <div class="ship-head-meta">Last update: <b><asp:Literal ID="litUpdated" runat="server" /></b></div>
                    </div>

                    <asp:Literal ID="litProgress" runat="server" />

                    <div class="kv-grid">
                        <div class="kv"><div class="kv-label">Recipient</div><div class="kv-value"><asp:Literal ID="litRecipientName" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Branch Origin</div><div class="kv-value"><asp:Literal ID="litBranchOrigin" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Estimated Delivery</div><div class="kv-value"><asp:Literal ID="litEta" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Rider</div><div class="kv-value"><asp:Literal ID="litRiderInfo" runat="server" /></div></div>

                        <div class="kv kv-wide"><div class="kv-label">Recipient Address</div><div class="kv-value"><asp:Literal ID="litDestinationCity" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Package</div><div class="kv-value"><asp:Literal ID="litPackage" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Weight</div><div class="kv-value"><asp:Literal ID="litWeight" runat="server" /></div></div>

                        <div class="kv"><div class="kv-label">Created</div><div class="kv-value"><asp:Literal ID="litCreated" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Latest Location</div><div class="kv-value"><asp:Literal ID="litLastLocation" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Time in System</div><div class="kv-value"><asp:Literal ID="litElapsed" runat="server" /></div></div>
                        <div class="kv"><div class="kv-label">Tracking Events</div><div class="kv-value"><asp:Literal ID="litEventCount" runat="server" /></div></div>
                    </div>
                </div>
                <div class="card-panel">
                    <h4 class="panel-title">Shipment History</h4>
                    <div class="table-scroll">
                    <asp:Repeater ID="rptHistory" runat="server">
                        <HeaderTemplate>
                            <table class="data-grid">
                                <thead>
                                    <tr>
                                        <th style="width:150px;">Date</th>
                                        <th style="width:130px;">Status</th>
                                        <th style="width:240px;">Location</th>
                                        <th>Notes</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
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
            </asp:Panel>
        </div>
    </div>
</asp:Content>
