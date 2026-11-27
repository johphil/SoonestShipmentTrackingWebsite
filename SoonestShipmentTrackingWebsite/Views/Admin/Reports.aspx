<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section app-section">
        <div class="container">
            <div class="card-panel">
                <div class="page-heading">Shipment Reports</div>
                <div class="page-subheading">Filter operational shipments, delivery exceptions, and customer feedback.</div>
                <div class="filter-grid">
                    <div class="form-group">
                        <label>Status</label><asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" /></div>
                    <div class="form-group">
                        <label>From date</label><asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
                    <div class="form-group">
                        <label>To date</label><asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
                </div>
                <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export to PDF" CssClass="btn btn-outline" OnClick="btnExport_Click" />
                <asp:Button ID="btnExportCSV" runat="server" Text="Export to Spreadsheet" CssClass="btn btn-outline" UseSubmitBehavior="false" OnClientClick="showXlsxPasswordModal(); return false;" />
                <asp:Label ID="lblError" runat="server" CssClass="field-error" Visible="false" />
            </div>
            <div class="stat-mini-grid">
                <div class="stat-mini"><div class="num"><asp:Literal ID="litTotal" runat="server" /></div><div class="lbl">Total Shipments</div></div>
                <div class="stat-mini"><div class="num"><asp:Literal ID="litDelivered" runat="server" /></div><div class="lbl">Delivered</div></div>
                <div class="stat-mini"><div class="num"><asp:Literal ID="litOutForDelivery" runat="server" /></div><div class="lbl">Out for delivery</div></div>
                <div class="stat-mini"><div class="num"><asp:Literal ID="litIssues" runat="server" /></div><div class="lbl">Issues Reported</div></div>
            </div>

            <div class="grid-2">
                <div class="card-panel">
                    <div class="panel-head">
                        <h4 class="panel-title">Monthly Shipment Summary</h4>
                        <asp:Button ID="btnExportMonthlyPdf" runat="server" Text="Export PDF" CssClass="btn btn-outline" OnClick="btnExportMonthlyPdf_Click" />
                    </div>
                    <div class="table-scroll">
                        <asp:GridView ID="gvMonthlySummary" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No monthly data available.">
                            <Columns>
                                <asp:BoundField DataField="Period" HeaderText="Month" />
                                <asp:BoundField DataField="TotalShipments" HeaderText="Total" />
                                <asp:BoundField DataField="Delivered" HeaderText="Delivered" />
                                <asp:BoundField DataField="IssuesReported" HeaderText="Issues" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-panel">
                    <div class="panel-head">
                        <h4 class="panel-title">Quarterly Shipment Summary</h4>
                        <asp:Button ID="btnExportQuarterlyPdf" runat="server" Text="Export PDF" CssClass="btn btn-outline" OnClick="btnExportQuarterlyPdf_Click" />
                    </div>
                    <div class="table-scroll">
                        <asp:GridView ID="gvQuarterlySummary" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No quarterly data available.">
                            <Columns>
                                <asp:BoundField DataField="Period" HeaderText="Quarter" />
                                <asp:BoundField DataField="TotalShipments" HeaderText="Total" />
                                <asp:BoundField DataField="Delivered" HeaderText="Delivered" />
                                <asp:BoundField DataField="IssuesReported" HeaderText="Issues" />
                            </Columns>
                        </asp:GridView>
                    </div>
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

    <div id="xlsxPasswordModal" class="modal-backdrop" role="dialog" aria-modal="true" aria-labelledby="xlsxModalTitle">
        <div class="modal-dialog">
            <h3 class="modal-title" id="xlsxModalTitle">Export to Spreadsheet</h3>
            <p class="modal-text">Enter a password to protect the Excel file.</p>
            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtCsvPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
            <div class="modal-actions">
                <button type="button" class="btn btn-outline" onclick="hideXlsxPasswordModal()">Cancel</button>
                <asp:Button ID="btnConfirmExportXlsx" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExportCSV_Click" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showXlsxPasswordModal() {
            var modal = document.getElementById('xlsxPasswordModal');
            if (!modal) return;
            modal.classList.add('is-open');
            var passwordBox = document.getElementById('<%= txtCsvPassword.ClientID %>');
            if (passwordBox) {
                passwordBox.value = '';
                setTimeout(function () { passwordBox.focus(); }, 10);
            }
        }

        function hideXlsxPasswordModal() {
            var modal = document.getElementById('xlsxPasswordModal');
            if (modal) modal.classList.remove('is-open');
        }

        (function () {
            var modal = document.getElementById('xlsxPasswordModal');
            if (!modal) return;
            modal.addEventListener('click', function (e) {
                if (e.target === modal) hideXlsxPasswordModal();
            });
        })();
    </script>
</asp:Content>
