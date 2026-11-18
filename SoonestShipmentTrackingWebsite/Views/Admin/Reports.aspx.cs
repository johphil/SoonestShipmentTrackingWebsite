using Microsoft.AspNet.Identity;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Reports : Page
    {
        private List<ReportRow> CurrentRows;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;
            if (!string.IsNullOrEmpty(Request.QueryString["proof"])) { ServeProof(Request.QueryString["proof"]); return; }
            if (!IsPostBack) { BindStatus(); Generate(); }
        }
        private void BindStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("All statuses", ""));
            foreach (ShipmentStatus s in Enum.GetValues(typeof(ShipmentStatus)))
                ddlStatus.Items.Add(new ListItem(Helpers.StatusDisplayHelper.Label(s), s.ToString()));
        }

        private void ServeProof(string idText)
        {
            int id;
            if (!int.TryParse(idText, out id)) { Response.StatusCode = 404; Response.End(); return; }
            using (var db = new ApplicationDbContext())
            {
                var h = db.ShipmentHistories.AsNoTracking().FirstOrDefault(x => x.Id == id && x.ProofOfDelivery != null);
                if (h == null) { Response.StatusCode = 404; Response.End(); return; }
                Response.Clear();
                Response.ContentType = (h.ProofContentType ?? "").StartsWith("image/") ? h.ProofContentType : "image/jpeg";
                Response.BinaryWrite(h.ProofOfDelivery);
                Response.End();
            }
        }

        private string ProofUrl(int historyId)
        {
            return ResolveUrl("~/Views/Admin/Reports.aspx?proof=" + historyId);
        }

        protected void btnGenerate_Click(object sender, EventArgs e) { Generate(); }

        private void Generate()
        {
            DateTime from, to;
            DateTime? fromDate = DateTime.TryParse(txtFromDate.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ? from.Date : (DateTime?)null;
            DateTime? toDate = DateTime.TryParse(txtToDate.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out to) ? to.Date.AddDays(1) : (DateTime?)null;
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !fromDate.HasValue || !string.IsNullOrWhiteSpace(txtToDate.Text) && !toDate.HasValue)
            { ShowError("Enter valid report dates."); return; }
            using (var db = new ApplicationDbContext())
            {
                var query = db.Shipments.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
                {
                    var status = (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), ddlStatus.SelectedValue);
                    query = query.Where(s => s.CurrentStatus == status);
                }
                if (fromDate.HasValue) query = query.Where(s => s.UpdatedDate >= fromDate.Value);
                if (toDate.HasValue) query = query.Where(s => s.UpdatedDate < toDate.Value);
                var shipments = query.OrderByDescending(s => s.UpdatedDate).ToList();
                var names = db.Users.ToDictionary(u => u.Id, u => string.IsNullOrWhiteSpace(u.FullName) ? u.Email : u.FullName);
                var proofIds = db.ShipmentHistories.AsNoTracking()
                    .Where(h => h.Status == ShipmentStatus.Delivered && h.ProofOfDelivery != null)
                    .Select(h => new { h.Id, h.ShipmentId, h.Timestamp })
                    .ToList()
                    .GroupBy(h => h.ShipmentId)
                    .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Timestamp).First().Id);
                CurrentRows = shipments.Select(s => new ReportRow
                {
                    Id = s.Id,
                    ControlNumber = s.ControlNumber,
                    CreatedDate = s.CreatedDate,
                    ShipmentType = s.ShipmentType,
                    ContainerNumber = s.ContainerNumber,
                    PackingList = s.PackingList,
                    RecipientName = s.RecipientName,
                    RecipientPhone = s.RecipientPhone,
                    PackageDescription = s.PackageDescription,
                    WeightKg = s.WeightKg,
                    EstimatedDeliveryDate = s.EstimatedDeliveryDate,
                    StatusValue = s.CurrentStatus,
                    IssueFlag = s.IssueReported,
                    IssueResolved = s.IsIssueResolved,
                    CustomerName = names.ContainsKey(s.CustomerId) ? names[s.CustomerId] : "(unknown)",
                    BranchOrigin = s.BranchOrigin,
                    RecipientAddress = s.RecipientAddress,
                    CurrentStatus = Helpers.StatusDisplayHelper.Label(s.CurrentStatus),
                    RiderName = s.RiderName ?? "-",
                    IsOrderReceived = s.IsOrderReceived ? "Yes" : "No",
                    IssueReported = s.IssueReported ? "Yes" : "No",
                    UpdatedDate = s.UpdatedDate,
                    ProofPath = proofIds.ContainsKey(s.Id) ? ProofUrl(proofIds[s.Id]) : null
                }).ToList();
                gvReport.DataSource = CurrentRows; gvReport.DataBind();
                litTotal.Text = shipments.Count.ToString();
                litDelivered.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Delivered).ToString();
                litOutForDelivery.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.OutForDelivery).ToString();
                litIssues.Text = shipments.Count(s => s.IssueReported).ToString();
                ViewState["ReportRows"] = SerializeRows(CurrentRows);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Generate();

            var rows = CurrentRows ?? new List<ReportRow>();
            var pdfBytes = GeneratePDF(rows);//BuildPdfReport(rows);
            var filename = "shipment-report-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".pdf";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.BinaryWrite(pdfBytes);
            Response.End();
        }

        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            Generate();

            var rows = CurrentRows ?? new List<ReportRow>();
            var pdfBytes = GeneratePDF(rows);//BuildPdfReport(rows);
            var filename = "shipment-report-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".pdf";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.BinaryWrite(pdfBytes);
            Response.End();
        }

        private static string FormatTableRow(int[] widths, params string[] values)
        {
            var cells = new List<string>();
            for (var i = 0; i < widths.Length; i++)
                cells.Add(FitCell(i < values.Length ? values[i] : string.Empty, widths[i]));
            return string.Join(" | ", cells);
        }

        private static string FitCell(string value, int width)
        {
            var text = (value ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            if (text.Length > width)
                text = text.Substring(0, width - 1) + "~";
            return text.PadRight(width);
        }


        private byte[] GeneratePDF(IEnumerable<ReportRow> shipments)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var data = shipments?.ToList() ?? new List<ReportRow>();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    page.DefaultTextStyle(x => x.FontSize(7));

                    // HEADER
                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("SHIPMENT TRACKING REPORT")
                            .FontSize(16)
                            .Bold()
                            .FontColor("#26364A");

                        column.Item().PaddingTop(5).Row(row =>
                        {
                            row.RelativeItem().Text(
                                $"Total Shipments: {data.Count}")
                                .FontSize(9);

                            row.AutoItem().Text(
                                $"Generated: {DateTime.Now:MMM dd, yyyy hh:mm tt}")
                                .FontSize(8)
                                .FontColor("#666666");
                        });

                        column.Item()
                            .PaddingTop(8)
                            .LineHorizontal(1)
                            .LineColor("#CCCCCC");
                    });

                    // CONTENT
                    page.Content()
                        .PaddingTop(10)
                        .Table(table =>
                        {
                            // COLUMN DEFINITIONS
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(65);   // Control No.
                                columns.RelativeColumn(1.2f); // Customer
                                columns.RelativeColumn(1.2f); // Origin
                                columns.RelativeColumn(1.5f); // Recipient
                                columns.RelativeColumn(0.9f); // Status
                                columns.RelativeColumn(0.9f); // Rider
                                columns.ConstantColumn(42);   // Received
                                columns.RelativeColumn(1.0f); // Issue
                                columns.ConstantColumn(65);   // Updated
                                columns.RelativeColumn(1.2f); // Proof
                            });

                            // TABLE HEADER
                            table.Header(header =>
                            {
                                string[] headers =
                                {
                            "Control No.",
                            "Customer Name",
                            "Branch Origin",
                            "Recipient Address",
                            "Shipment Status",
                            "Rider Name",
                            "Received",
                            "Issue Reported",
                            "Last Updated",
                            "Proof of Delivery"
                                };

                                foreach (var title in headers)
                                {
                                    header.Cell()
                                        .Background("#26364A")
                                        .Padding(5)
                                        .AlignMiddle()
                                        .Text(title)
                                        .FontColor("#FFFFFF")
                                        .Bold()
                                        .FontSize(7);
                                }
                            });

                            // TABLE ROWS
                            int index = 0;

                            foreach (var shipment in data)
                            {
                                var background = index++ % 2 == 0
                                    ? "#FFFFFF"
                                    : "#F2F5F8";

                                // REUSABLE CELL
                                void AddCell(string value)
                                {
                                    table.Cell()
                                        .Background(background)
                                        .BorderBottom(0.5f)
                                        .BorderColor("#DDDDDD")
                                        .Padding(5)
                                        .AlignMiddle()
                                        .Text(string.IsNullOrWhiteSpace(value)
                                            ? "-"
                                            : value)
                                        .FontSize(7);
                                }

                                AddCell(shipment.ControlNumber);
                                AddCell(shipment.CustomerName);
                                AddCell(shipment.BranchOrigin);
                                AddCell(shipment.RecipientAddress);
                                AddCell(shipment.CurrentStatus);
                                AddCell(shipment.RiderName);
                                AddCell(shipment.IsOrderReceived);
                                AddCell(shipment.IssueReported);

                                AddCell(
                                    shipment.UpdatedDate.ToString("MMM dd, yyyy")
                                );

                                // PROOF OF DELIVERY
                                var proofLink = Request.Url.GetLeftPart(UriPartial.Authority) + shipment.ProofPath;

                                var cell = table.Cell()
                                    .Background(background)
                                    .BorderBottom(0.5f)
                                    .BorderColor("#DDDDDD")
                                    .Padding(5)
                                    .AlignMiddle();

                                if (!string.IsNullOrWhiteSpace(shipment.ProofPath) &&
                                    Uri.TryCreate(
                                        proofLink,
                                        UriKind.Absolute,
                                        out Uri uri) &&
                                    (uri.Scheme == Uri.UriSchemeHttp ||
                                     uri.Scheme == Uri.UriSchemeHttps))
                                {
                                    cell.Hyperlink(proofLink)
                                        .Text("View Proof")
                                        .FontSize(7)
                                        .FontColor("#1677C8")
                                        .Underline();
                                }
                                else
                                {
                                    cell.Text("-").FontSize(7);
                                }
                            }
                        });

                    // FOOTER
                    page.Footer()
                        .PaddingTop(8)
                        .Row(row =>
                        {
                            row.RelativeItem().Text(
                                "Shipment Tracking System")
                                .FontSize(7)
                                .FontColor("#888888");

                            row.AutoItem().Text(text =>
                            {
                                text.Span("Page ").FontSize(7);
                                text.CurrentPageNumber().FontSize(7);
                                text.Span(" of ").FontSize(7);
                                text.TotalPages().FontSize(7);
                            });
                        });
                });
            });

            return document.GeneratePdf();
        }

        private static string PdfEscape(string value)
        {
            return (value ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }
        
        private static string SerializeRows(List<ReportRow> rows) { return rows.Count.ToString(); }
        
        private void ShowError(string message) { lblError.Text = HttpUtility.HtmlEncode(message); lblError.Visible = true; }
        
        private class ReportRow
        {
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; }
            public ShipmentType ShipmentType { get; set; }
            public string ContainerNumber { get; set; }
            public string PackingList { get; set; }
            public string RecipientName { get; set; }
            public string RecipientPhone { get; set; }
            public string PackageDescription { get; set; }
            public decimal? WeightKg { get; set; }
            public DateTime? EstimatedDeliveryDate { get; set; }
            public ShipmentStatus StatusValue { get; set; }
            public bool IssueFlag { get; set; }
            public bool IssueResolved { get; set; }
            public string ControlNumber { get; set; }
            public string CustomerName { get; set; }
            public string BranchOrigin { get; set; }
            public string RecipientAddress { get; set; }
            public string CurrentStatus { get; set; }
            public string RiderName { get; set; }
            public string IsOrderReceived { get; set; }
            public string IssueReported { get; set; }
            public DateTime UpdatedDate { get; set; }
            public string ProofPath { get; set; }
            public string ProofHtml
            {
                get
                {
                    return string.IsNullOrEmpty(ProofPath) ? "-" : "<a href=\"" + ProofPath + "\" target=\"_blank\"><img src=\"" + ProofPath + "\" alt=\"Proof of delivery\" style=\"max-width:44px;max-height:44px;border-radius:6px;border:1px solid #dbe6f0;\" /></a>";
                }
            }
        }
    }
}
