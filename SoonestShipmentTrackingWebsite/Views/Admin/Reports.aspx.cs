using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
            if (!IsPostBack) { BindStatus(); Generate(); }
        }
        private void BindStatus()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("All statuses", ""));
            foreach (ShipmentStatus s in Enum.GetValues(typeof(ShipmentStatus)))
                ddlStatus.Items.Add(new ListItem(Helpers.StatusDisplayHelper.Label(s), s.ToString()));
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
                CurrentRows = shipments.Select(s => new ReportRow { ControlNumber = s.ControlNumber, CustomerName = names.ContainsKey(s.CustomerId) ? names[s.CustomerId] : "(unknown)", DestinationCity = s.DestinationCity, CurrentStatus = Helpers.StatusDisplayHelper.Label(s.CurrentStatus), RiderName = s.RiderName ?? "-", IsOrderReceived = s.IsOrderReceived ? "Yes" : "No", IssueReported = s.IssueReported ? "Yes" : "No", UpdatedDate = s.UpdatedDate }).ToList();
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
            var csv = new StringBuilder("Control Number,Customer,Destination,Status,Rider,Order Received,Issue Reported,Last Update\r\n");
            foreach (var r in rows) csv.AppendLine(string.Join(",", new[] { r.ControlNumber, r.CustomerName, r.DestinationCity, r.CurrentStatus, r.RiderName, r.IsOrderReceived, r.IssueReported, r.UpdatedDate.ToString("yyyy-MM-dd HH:mm") }.Select(CsvField)));
            Response.Clear(); Response.ContentType = "text/csv"; Response.AddHeader("Content-Disposition", "attachment;filename=shipment-report.csv"); Response.Write(csv.ToString()); Response.End();
        }
        private static string CsvField(string value) { return "\"" + (value ?? "").Replace("\"", "\"\"") + "\""; }
        private static string SerializeRows(List<ReportRow> rows) { return rows.Count.ToString(); }
        private void ShowError(string message) { lblError.Text = HttpUtility.HtmlEncode(message); lblError.Visible = true; }
        private class ReportRow
        {
            public string ControlNumber { get; set; } public string CustomerName { get; set; } public string DestinationCity { get; set; } public string CurrentStatus { get; set; } public string RiderName { get; set; } public string IsOrderReceived { get; set; } public string IssueReported { get; set; } public DateTime UpdatedDate { get; set; }
        }
    }
}
