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
                <asp:Button ID="btnExport" runat="server" Text="Export to PDF" CssClass="btn btn-outline" OnClick="btnExport_Click" />
                <asp:Button ID="btnExportCSV" runat="server" Text="Export to Spreadsheet" CssClass="btn btn-outline" UseSubmitBehavior="false" OnClientClick="showXlsxPasswordModal(); return false;" />
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

            <div style="display:grid; grid-template-columns:1fr 1fr; gap:14px; margin-bottom:20px;">
                <div class="card-panel">
                    <div class="page-subheading" style="margin-bottom:10px;">Monthly Shipment Summary</div>
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
                    <div class="page-subheading" style="margin-bottom:10px;">Quarterly Shipment Summary</div>
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

    <div id="xlsxPasswordModal" style="display:none; position:fixed; inset:0; background:rgba(15,23,42,.48); z-index:2000; align-items:center; justify-content:center;">
        <div style="width:min(420px,92vw); background:#fff; border-radius:12px; box-shadow:0 16px 40px rgba(15,23,42,.24); padding:18px;">
            <div style="font-size:18px; font-weight:700; margin-bottom:8px; color:#1f2937;">Export XLSX</div>
            <div style="font-size:13px; color:#6b7280; margin-bottom:12px;">Enter a password to protect the Excel file.</div>
            <div class="form-group" style="margin-bottom:12px;">
                <label>Password</label>
                <asp:TextBox ID="txtCsvPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>
            <div style="display:flex; gap:8px; justify-content:flex-end;">
                <button type="button" class="btn btn-outline" onclick="hideXlsxPasswordModal()">Cancel</button>
                <asp:Button ID="btnConfirmExportXlsx" runat="server" Text="Export" CssClass="btn btn-red" OnClick="btnExportCSV_Click" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showXlsxPasswordModal() {
            var modal = document.getElementById('xlsxPasswordModal');
            if (!modal) return;
            modal.style.display = 'flex';
            var passwordBox = document.getElementById('<%= txtCsvPassword.ClientID %>');
            if (passwordBox) {
                passwordBox.value = '';
                setTimeout(function () { passwordBox.focus(); }, 10);
            }
        }

        function hideXlsxPasswordModal() {
            var modal = document.getElementById('xlsxPasswordModal');
            if (modal) modal.style.display = 'none';
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
