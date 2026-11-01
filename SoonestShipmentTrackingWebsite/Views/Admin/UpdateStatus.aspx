<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="UpdateStatus.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.UpdateStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Update Status</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section app-section">
    <div class="container">

        <div class="card-panel">
            <div class="ship-head">
                <div class="ship-head-main">
                    <h2 class="ship-no"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                    <asp:Literal ID="litCurrentBadge" runat="server" />
                </div>
                <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline">&larr; Back to Shipments</a>
            </div>
            <asp:Literal ID="litProgress" runat="server" />
            <div class="kv-grid">
                <div class="kv"><div class="kv-label">Recipient</div><div class="kv-value"><asp:Literal ID="litRecipientName" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Recipient Phone</div><div class="kv-value"><asp:Literal ID="litRecipientPhone" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Branch Origin</div><div class="kv-value"><asp:Literal ID="litBranchOrigin" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Rider</div><div class="kv-value"><asp:Literal ID="litRiderInfo" runat="server" /></div></div>

                <div class="kv kv-wide"><div class="kv-label">Recipient Address</div><div class="kv-value"><asp:Literal ID="litDestinationCity" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Customer</div><div class="kv-value"><asp:Literal ID="litCustomer" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Sender</div><div class="kv-value"><asp:Literal ID="litSenderName" runat="server" /></div></div>

                <div class="kv"><div class="kv-label">Package</div><div class="kv-value"><asp:Literal ID="litPackage" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Weight</div><div class="kv-value"><asp:Literal ID="litWeight" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Estimated Delivery</div><div class="kv-value"><asp:Literal ID="litEta" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Time in System</div><div class="kv-value"><asp:Literal ID="litElapsed" runat="server" /></div></div>

                <div class="kv"><div class="kv-label">Created</div><div class="kv-value"><asp:Literal ID="litCreated" runat="server" /></div></div>
                <div class="kv"><div class="kv-label">Last Update</div><div class="kv-value"><asp:Literal ID="litUpdated" runat="server" /></div></div>
                <div class="kv kv-wide"><div class="kv-label">Issue</div><div class="kv-value"><asp:Literal ID="litIssueIndicator" runat="server" /></div></div>
            </div>
        </div>

        <div class="split">
            <div class="split-col">
                <asp:Panel ID="pnlIssue" runat="server" Visible="false" CssClass="alert-warning">
                    <div><strong>Reported Issue:</strong> <asp:Literal ID="litIssueReport" runat="server" /></div>
                    <div style="margin-top:3px;"><strong>Reported On:</strong> <asp:Literal ID="litIssueDate" runat="server" /></div>
                    <asp:Panel ID="pnlIssueResolvedInfo" runat="server" Visible="false" style="margin-top:3px;color:#1f7a2e;">
                        <strong>Resolved On:</strong> <asp:Literal ID="litIssueResolvedDate" runat="server" />
                    </asp:Panel>
                    <asp:Button ID="btnResolveIssue" runat="server" CssClass="btn btn-red" Text="Mark Issue as Resolved" OnClick="btnResolveIssue_Click" style="margin-top:6px;" />
                </asp:Panel>

                <div class="card-panel">
                    <h4 class="panel-title">Update Status</h4>

                    <asp:Panel ID="pnlError" runat="server" Visible="false">
                        <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
                    </asp:Panel>

                    <div class="form-group">
                        <label>New Status</label>
                        <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group" id="grpCurrentLocation">
                        <label>Current Location</label>
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="e.g. Las Pi&ntilde;as Sorting Hub" />
                    </div>

                    <div class="form-group" id="grpRider">
                        <label>Rider Name <small>(required for Out for Delivery)</small></label>
                        <asp:DropDownList ID="ddlRider" runat="server" CssClass="form-control" />
                    </div>

                    <div class="form-group" id="grpProof">
                        <label>Proof of Delivery <small>(photo required for riders when Delivered)</small></label>
                        <asp:FileUpload ID="fileProof" runat="server" CssClass="form-control" accept="image/*" />
                    </div>

                    <div class="form-group">
                        <label>Notes</label>
                        <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                    </div>

                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red" Text="Save Status Update" OnClick="btnSave_Click" />
                    <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#07233d;color:#07233d;">Cancel</a>
                </div>
            </div>

            <div class="split-col">
                <div class="card-panel">
                    <h4 class="panel-title">History</h4>
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
                    <asp:Label ID="lblNoHistory" runat="server" Text="No tracking events yet." Visible="false" CssClass="empty-note" />
                </div>
            </div>
        </div>

    </div>
</div>
<script type="text/javascript">
    (function () {
        var ddl = document.getElementById('<%= ddlNewStatus.ClientID %>');
        var locationGroup = document.getElementById('grpCurrentLocation');
        var txtLocation = document.getElementById('<%= txtLocation.ClientID %>');
        var grpProof = document.getElementById('grpProof');
        var grpRider = document.getElementById('grpRider');

        function toggleProof() { if (grpProof) grpProof.style.display = ddl.value === 'Delivered' ? '' : 'none'; }
        ddl.addEventListener('change', toggleProof);

        function toggleRider() { if (grpRider) grpRider.style.display = ddl.value === 'OutForDelivery' ? '' : 'none'; }
        ddl.addEventListener('change', toggleRider);

        function toggleLocation() {
            if (locationGroup) {
                locationGroup.style.display =
                    (ddl.value === 'InTransit' || ddl.value === 'OutForDelivery') ? '' : 'none';
            }
        }
        ddl.addEventListener('change', toggleLocation);

        toggleProof();
        toggleRider();
        toggleLocation();
    })();
</script>
</asp:Content>
