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
using OfficeOpenXml;

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
            lblError.Visible = false;
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

                var monthlySummary = shipments
                    .GroupBy(s => new { s.UpdatedDate.Year, s.UpdatedDate.Month })
                    .OrderByDescending(g => g.Key.Year)
                    .ThenByDescending(g => g.Key.Month)
                    .Select(g => new ShipmentPeriodSummaryRow
                    {
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy", CultureInfo.InvariantCulture),
                        TotalShipments = g.Count(),
                        Delivered = g.Count(x => x.CurrentStatus == ShipmentStatus.Delivered),
                        IssuesReported = g.Count(x => x.IssueReported)
                    })
                    .ToList();

                var quarterlySummary = shipments
                    .GroupBy(s => new { s.UpdatedDate.Year, Quarter = ((s.UpdatedDate.Month - 1) / 3) + 1 })
                    .OrderByDescending(g => g.Key.Year)
                    .ThenByDescending(g => g.Key.Quarter)
                    .Select(g => new ShipmentPeriodSummaryRow
                    {
                        Period = "Q" + g.Key.Quarter + " " + g.Key.Year,
                        TotalShipments = g.Count(),
                        Delivered = g.Count(x => x.CurrentStatus == ShipmentStatus.Delivered),
                        IssuesReported = g.Count(x => x.IssueReported)
                    })
                    .ToList();

                gvMonthlySummary.DataSource = monthlySummary;
                gvMonthlySummary.DataBind();
                gvQuarterlySummary.DataSource = quarterlySummary;
                gvQuarterlySummary.DataBind();

                CurrentRows = shipments.Select(s => new ReportRow
                {
                    Id = s.Id,
                    ControlNumber = s.ControlNumber,
                    CreatedDate = s.CreatedDate,
                    ShipmentType = s.ShipmentType,
                    SenderName = s.SenderName,
                    ContainerNumber = s.ContainerNumber,
                    PackingList = s.PackingList,
                    RegionArea = s.RegionArea,
                    RecipientName = s.RecipientName,
                    RecipientPhone = s.RecipientPhone,
                    RecipientEmail = s.RecipientEmail,
                    PackageDescription = s.PackageDescription,
                    WeightKg = s.WeightKg,
                    EstimatedDeliveryDate = s.EstimatedDeliveryDate,
                    StatusValue = s.CurrentStatus,
                    IssueFlag = s.IssueReported,
                    IssueResolved = s.IsIssueResolved,
                    IssueReport = s.IssueReport,
                    OrderReceivedDate = s.OrderReceivedDate,
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
            var pdfBytes = GeneratePDF(rows, ddlStatus.SelectedValue);//BuildPdfReport(rows);
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

            var password = txtCsvPassword.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Enter a password for Excel export.");
                return;
            }

            var rows = CurrentRows ?? new List<ReportRow>();
            var xlsxBytes = BuildProtectedXlsx(rows, password);
            var filename = "shipment-report-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".xlsx";

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.BinaryWrite(xlsxBytes);
            Response.End();
        }

        private byte[] BuildProtectedXlsx(IEnumerable<ReportRow> rows, string password)
        {
            ExcelPackage.License.SetNonCommercialPersonal("SoonestGlobalExpress");
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Report");

                string[] headers =
                {
                    "Control Number",
                    "Shipment Type",
                    "Status",
                    "Customer Name",
                    "Sender Name",
                    "Recipient Name",
                    "Recipient Phone",
                    "Recipient Email",
                    "Region/Area",
                    "Branch Origin",
                    "Container Number",
                    "Recipient Address",
                    "Package Description",
                    "Weight (kg)",
                    "Rider Name",
                    "Order Received",
                    "Order Received Date",
                    "Issue Reported",
                    "Issue Details",
                    "Created Date",
                    "Estimated Delivery",
                    "Updated Date",
                    "Proof Url"
                };

                for (var i = 0; i < headers.Length; i++)
                    sheet.Cells[1, i + 1].Value = headers[i];

                var rowIndex = 2;
                foreach (var row in rows)
                {
                    var proofUrl = string.IsNullOrWhiteSpace(row.ProofPath)
                        ? string.Empty
                        : Request.Url.GetLeftPart(UriPartial.Authority) + row.ProofPath;

                    sheet.Cells[rowIndex, 1].Value = row.ControlNumber;
                    sheet.Cells[rowIndex, 2].Value = row.ShipmentType.ToString();
                    sheet.Cells[rowIndex, 3].Value = row.CurrentStatus;
                    sheet.Cells[rowIndex, 4].Value = row.CustomerName;
                    sheet.Cells[rowIndex, 5].Value = row.SenderName;
                    sheet.Cells[rowIndex, 6].Value = row.RecipientName;
                    sheet.Cells[rowIndex, 7].Value = row.RecipientPhone;
                    sheet.Cells[rowIndex, 8].Value = row.RecipientEmail;
                    sheet.Cells[rowIndex, 9].Value = row.RegionArea;
                    sheet.Cells[rowIndex, 10].Value = row.BranchOrigin;
                    sheet.Cells[rowIndex, 11].Value = row.ContainerNumber;
                    sheet.Cells[rowIndex, 12].Value = row.RecipientAddress;
                    sheet.Cells[rowIndex, 13].Value = row.PackageDescription;
                    sheet.Cells[rowIndex, 14].Value = row.WeightKg.HasValue ? (double?)row.WeightKg.Value : null;
                    sheet.Cells[rowIndex, 15].Value = row.RiderName;
                    sheet.Cells[rowIndex, 16].Value = row.IsOrderReceived;
                    sheet.Cells[rowIndex, 17].Value = row.OrderReceivedDate.HasValue ? row.OrderReceivedDate.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) : string.Empty;
                    sheet.Cells[rowIndex, 18].Value = row.IssueReported;
                    sheet.Cells[rowIndex, 19].Value = row.IssueFlag ? (string.IsNullOrWhiteSpace(row.IssueReport) ? "Issue reported" : row.IssueReport) : string.Empty;
                    sheet.Cells[rowIndex, 20].Value = row.CreatedDate.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    sheet.Cells[rowIndex, 21].Value = row.EstimatedDeliveryDate.HasValue ? row.EstimatedDeliveryDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
                    sheet.Cells[rowIndex, 22].Value = row.UpdatedDate.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    sheet.Cells[rowIndex, 23].Value = proofUrl;

                    rowIndex++;
                }

                if (sheet.Dimension != null)
                    sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                package.Encryption.IsEncrypted = true;
                package.Encryption.Password = password;

                return package.GetAsByteArray();
            }
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


        private byte[] GeneratePDF(IEnumerable<ReportRow> shipments, string status)
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
                        string statusType = string.IsNullOrWhiteSpace(status) || status.ToLower() == "All statuses" ? "All" : status;
                      
                        column.Item()
                            .Text($"SHIPMENT TRACKING REPORT")
                            .FontSize(16)
                            .Bold()
                            .FontColor("#26364A");

                        column.Item().PaddingTop(5)
                            .Text($"Filtered Status: {statusType}")
                            .FontSize(9)
                            .FontColor("#666666");

                        if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
                        {
                            column.Item()
                                .Text($"Filtered Date: {DateTime.Parse(txtFromDate.Text).ToString("MMM dd, yyyy")} to {DateTime.Parse(txtToDate.Text).ToString("MMM dd, yyyy")}")
                                .FontSize(9)
                                .FontColor("#666666");
                        }

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text(
                                $"Total Shipments: {data.Count}")
                                .FontSize(9)
                                .FontColor("#666666");

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
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);   // Control / Type
                                columns.RelativeColumn(1.25f); // Parties
                                columns.RelativeColumn(1.40f); // Route + Cargo
                                columns.RelativeColumn(1.00f); // Status / Rider
                                columns.RelativeColumn(1.10f); // Timeline
                                columns.ConstantColumn(55);   // Proof
                            });

                            table.Header(header =>
                            {
                                string[] headers =
                                {
                                    "Shipment",
                                    "Parties",
                                    "Route / Cargo",
                                    "Status",
                                    "Timeline",
                                    "Proof"
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

                            int index = 0;

                            foreach (var shipment in data)
                            {
                                var background = index++ % 2 == 0 ? "#FFFFFF" : "#F2F5F8";

                                void AddCell(string value)
                                {
                                    table.Cell()
                                        .Background(background)
                                        .BorderBottom(0.5f)
                                        .BorderColor("#DDDDDD")
                                        .Padding(5)
                                        .AlignMiddle()
                                        .Text(string.IsNullOrWhiteSpace(value) ? "-" : value)
                                        .FontSize(7);
                                }

                                var shipmentType = shipment.ShipmentType == ShipmentType.DOMESTIC ? "Domestic" : "International";
                                AddCell(shipment.ControlNumber + "\n" + shipmentType);

                                AddCell(
                                    "Customer: " + (shipment.CustomerName ?? "-") +
                                    "\nSender: " + (shipment.SenderName ?? "-") +
                                    "\nRecipient: " + (shipment.RecipientName ?? "-") +
                                    "\nPhone: " + (shipment.RecipientPhone ?? "-") +
                                    "\nEmail: " + (shipment.RecipientEmail ?? "-"));

                                var routeLine = shipment.ShipmentType == ShipmentType.DOMESTIC
                                    ? "Branch: " + (string.IsNullOrWhiteSpace(shipment.BranchOrigin) ? "-" : shipment.BranchOrigin)
                                    : "Container: " + (string.IsNullOrWhiteSpace(shipment.ContainerNumber) ? "-" : shipment.ContainerNumber);
                                var packingLine = shipment.ShipmentType == ShipmentType.INTERNATIONAL
                                    ? "\nPacking: " + (string.IsNullOrWhiteSpace(shipment.PackingList) ? "-" : shipment.PackingList)
                                    : string.Empty;

                                AddCell(
                                    routeLine +
                                    packingLine +
                                    "\nRegion: " + (string.IsNullOrWhiteSpace(shipment.RegionArea) ? "-" : shipment.RegionArea) +
                                    "\nAddress: " + (string.IsNullOrWhiteSpace(shipment.RecipientAddress) ? "-" : shipment.RecipientAddress) +
                                    "\nPackage: " + (string.IsNullOrWhiteSpace(shipment.PackageDescription) ? "-" : shipment.PackageDescription) +
                                    "\nWeight: " + (shipment.WeightKg.HasValue ? shipment.WeightKg.Value.ToString("0.##") + " kg" : "-"));

                                var issueDetails = shipment.IssueFlag
                                    ? (string.IsNullOrWhiteSpace(shipment.IssueReport) ? "Issue reported" : shipment.IssueReport)
                                    : "No";
                                AddCell(
                                    "Status: " + (shipment.CurrentStatus ?? "-") +
                                    "\nRider: " + (string.IsNullOrWhiteSpace(shipment.RiderName) ? "-" : shipment.RiderName) +
                                    "\nReceived: " + shipment.IsOrderReceived +
                                    "\nIssue: " + issueDetails);

                                AddCell(
                                    "Created: " + shipment.CreatedDate.ToString("MMM dd, yyyy HH:mm") +
                                    "\nETA: " + (shipment.EstimatedDeliveryDate.HasValue ? shipment.EstimatedDeliveryDate.Value.ToString("MMM dd, yyyy") : "-") +
                                    "\nUpdated: " + shipment.UpdatedDate.ToString("MMM dd, yyyy HH:mm") +
                                    "\nReceived At: " + (shipment.OrderReceivedDate.HasValue ? shipment.OrderReceivedDate.Value.ToString("MMM dd, yyyy HH:mm") : "-"));

                                var proofLink = string.IsNullOrWhiteSpace(shipment.ProofPath)
                                    ? string.Empty
                                    : Request.Url.GetLeftPart(UriPartial.Authority) + shipment.ProofPath;

                                var cell = table.Cell()
                                    .Background(background)
                                    .BorderBottom(0.5f)
                                    .BorderColor("#DDDDDD")
                                    .Padding(5)
                                    .AlignMiddle();

                                if (!string.IsNullOrWhiteSpace(proofLink) &&
                                    Uri.TryCreate(proofLink, UriKind.Absolute, out Uri uri) &&
                                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
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
            public string SenderName { get; set; }
            public string ContainerNumber { get; set; }
            public string PackingList { get; set; }
            public string RegionArea { get; set; }
            public string RecipientName { get; set; }
            public string RecipientPhone { get; set; }
            public string RecipientEmail { get; set; }
            public string PackageDescription { get; set; }
            public decimal? WeightKg { get; set; }
            public DateTime? EstimatedDeliveryDate { get; set; }
            public ShipmentStatus StatusValue { get; set; }
            public bool IssueFlag { get; set; }
            public bool IssueResolved { get; set; }
            public string IssueReport { get; set; }
            public DateTime? OrderReceivedDate { get; set; }
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

        private class ShipmentPeriodSummaryRow
        {
            public string Period { get; set; }
            public int TotalShipments { get; set; }
            public int Delivered { get; set; }
            public int IssuesReported { get; set; }
        }
    }
}
