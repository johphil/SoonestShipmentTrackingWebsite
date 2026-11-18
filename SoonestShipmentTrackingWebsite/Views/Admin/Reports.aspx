<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section app-section" style="padding-top: 44px;">
        <div class="container">
            <div class="card-panel" style="margin-bottom: 20px;">
                <div class="page-heading">Shipment Reports</div>
                <div class="page-subheading">Filter operational shipments, delivery exceptions, and customer feedback.</div>
                <div style="display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 14px; align-items: end;">
                    <div class="form-group">
                        <label>Status</label><asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" /></div>
                    <div class="form-group">
                        <label>From date</label><asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
                    <div class="form-group">
                        <label>To date</label><asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
                </div>
                <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-red" OnClick="btnGenerate_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export PDF" CssClass="btn btn-outline" OnClick="btnExport_Click" />
                <asp:Button ID="btnExportCSV" runat="server" Text="Export CSV" CssClass="btn btn-outline" OnClick="btnExportCSV_Click" />
                <asp:Label ID="lblError" runat="server" CssClass="field-error" Visible="false" />
            </div>
            <div style="display: grid; grid-template-columns: repeat(4,1fr); gap: 14px; margin-bottom: 20px;">
                <div class="card-panel"><strong>Total Shipments</strong><h2>
                    <asp:Literal ID="litTotal" runat="server" /></h2>
                </div>
                <div class="card-panel"><strong>Delivered</strong><h2>
                    <asp:Literal ID="litDelivered" runat="server" /></h2>
                </div>
                <div class="card-panel"><strong>Out for delivery</strong><h2>
                    <asp:Literal ID="litOutForDelivery" runat="server" /></h2>
                </div>
                <div class="card-panel"><strong>Issues Reported</strong><h2>
                    <asp:Literal ID="litIssues" runat="server" /></h2>
                </div>
            </div>
            <div class="card-panel">
                <div class="table-scroll">
                <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="false" CssClass="data-grid data-grid-shipments" GridLines="None" EmptyDataText="No shipments match the selected filters.">
                    <Columns>
                        <asp:TemplateField HeaderText="Shipment"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.ShipmentCell(Eval("ControlNumber"), Eval("ShipmentType"), Eval("CreatedDate")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Enc(Convert.ToString(Eval("CustomerName"))) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Recipient"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RecipientCell(Eval("RecipientName"), Eval("RecipientPhone")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Route / Container"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RouteCell(Eval("ShipmentType"), Eval("BranchOrigin"), Eval("ContainerNumber"), Eval("PackingList"), Eval("RegionArea"), Eval("RecipientAddress")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.CargoCell(Eval("PackageDescription"), Eval("WeightKg")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Status"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.StatusCell(Eval("StatusValue"), Eval("IssueFlag"), Eval("IssueResolved")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Rider"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Enc(Convert.ToString(Eval("RiderName"))) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Est. Delivery"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.EtaCell(Eval("EstimatedDeliveryDate"), Eval("StatusValue")) %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Received"><ItemTemplate><%# Convert.ToString(Eval("IsOrderReceived")) == "Yes" ? "<span class=\"status-badge badge-success\">Yes</span>" : "<span class=\"muted\">No</span>" %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Updated"><ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.UpdatedCell(Eval("UpdatedDate")) %></ItemTemplate></asp:TemplateField>
                        <asp:BoundField DataField="ProofHtml" HeaderText="Proof" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
