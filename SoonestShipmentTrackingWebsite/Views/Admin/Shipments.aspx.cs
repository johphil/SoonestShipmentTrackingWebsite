using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Models;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Shipments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin", "Staff", "Rider")) return;

            pnlCreateShipment.Visible = !User.IsInRole("Rider") && !User.IsInRole("Customer");

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void gvShipments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShipments.PageIndex = e.NewPageIndex;
            LoadData();
        }

        private string BuildChips(List<KeyValuePair<ShipmentStatus, int>> counts, int issueCount, int shownCount)
        {
            var statusFilter = Request.QueryString["status"];
            var issuesFilter = string.Equals(Request.QueryString["issues"], "1", StringComparison.OrdinalIgnoreCase);
            ShipmentStatus activeStatus = ShipmentStatus.Pending;
            var hasStatus = !issuesFilter && !string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse(statusFilter, out activeStatus);

            var baseUrl = ResolveUrl("~/Views/Admin/Shipments.aspx");
            var total = counts.Sum(c => c.Value);
            var sb = new System.Text.StringBuilder();
            sb.Append("<div class=\"chip-row\">");
            sb.Append("<a class=\"chip" + (!hasStatus && !issuesFilter ? " active" : "") + "\" href=\"" + baseUrl + "\">All <b>" + total + "</b></a>");
            foreach (var status in (ShipmentStatus[])Enum.GetValues(typeof(ShipmentStatus)))
            {
                var count = counts.Where(c => c.Key == status).Select(c => c.Value).FirstOrDefault();
                var isActive = hasStatus && activeStatus == status;
                if (count == 0 && !isActive) continue;
                sb.Append("<a class=\"chip" + (isActive ? " active" : "") + "\" href=\"" + baseUrl + "?status=" + status + "\">"
                    + Server.HtmlEncode(Helpers.StatusDisplayHelper.Label(status)) + " <b>" + count + "</b></a>");
            }
            sb.Append("<a class=\"chip chip-alert" + (issuesFilter ? " active" : "") + "\" href=\"" + baseUrl + "?issues=1\">Open issues <b>" + issueCount + "</b></a>");
            sb.Append("<span class=\"chip-total\">Showing " + shownCount + " of " + total + "</span>");
            sb.Append("</div>");
            return sb.ToString();
        }

        private void LoadData()
        {
            using (var _db = new ApplicationDbContext())
            {
                var baseQuery = _db.Shipments.AsNoTracking().AsQueryable();

                if (User.IsInRole("Rider"))
                {
                    var riderUserId = User.Identity.GetUserId();
                    var rider = _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == riderUserId);

                    var riderKeys = new List<string>();
                    if (rider != null)
                    {
                        if (!string.IsNullOrWhiteSpace(rider.FullName)) riderKeys.Add(rider.FullName.Trim());
                        if (!string.IsNullOrWhiteSpace(rider.Email)) riderKeys.Add(rider.Email.Trim());
                    }

                    if (!riderKeys.Any())
                    {
                        litStatusChips.Text = BuildChips(new List<KeyValuePair<ShipmentStatus, int>>(), 0, 0);
                        gvShipments.DataSource = new List<object>();
                        gvShipments.DataBind();
                        return;
                    }

                    baseQuery = baseQuery.Where(s => riderKeys.Contains(s.RiderName));
                }

                var counts = baseQuery
                    .GroupBy(s => s.CurrentStatus)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList()
                    .Select(g => new KeyValuePair<ShipmentStatus, int>(g.Status, g.Count))
                    .ToList();
                var issueCount = baseQuery.Count(s => s.IssueReported && !s.IsIssueResolved);

                var query = baseQuery;

                var statusFilter = Request.QueryString["status"];
                if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse(statusFilter, out ShipmentStatus parsedStatus))
                {
                    query = query.Where(s => s.CurrentStatus == parsedStatus);
                }

                if (string.Equals(Request.QueryString["issues"], "1", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(s => s.IssueReported && !s.IsIssueResolved);
                }

                var shipments = query
                    .OrderByDescending(s => s.UpdatedDate)
                    .ToList();

                var customerIds = shipments.Select(s => s.CustomerId).Distinct().ToList();
                var users = _db.Users.AsNoTracking()
                    .Where(u => customerIds.Contains(u.Id))
                    .ToList()
                    .ToDictionary(u => u.Id, u => u);

                var list = shipments
                    .Select(s => new
                    {
                        s.Id,
                        s.ControlNumber,
                        s.CreatedDate,
                        CustomerName = users.ContainsKey(s.CustomerId) ? (users[s.CustomerId].FullName ?? "(unknown)") : "(unknown)",
                        CustomerEmail = users.ContainsKey(s.CustomerId) ? (users[s.CustomerId].Email ?? "") : "",
                        s.RecipientName,
                        s.RecipientPhone,
                        s.BranchOrigin,
                        s.RecipientAddress,
                        s.PackageDescription,
                        s.WeightKg,
                        s.EstimatedDeliveryDate,
                        s.CurrentStatus,
                        s.IssueReported,
                        s.IsIssueResolved,
                        s.RiderName,
                        s.UpdatedDate
                    })
                    .ToList();

                litStatusChips.Text = BuildChips(counts, issueCount, list.Count);
                gvShipments.DataSource = list;
                gvShipments.DataBind();
            }
        }
    }
}