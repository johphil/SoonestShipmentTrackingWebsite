using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin", "Staff")) return;

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipments = _db.Shipments.ToList();

                litTotal.Text = shipments.Count.ToString();
                litPending.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Pending).ToString();
                litInTransit.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.InTransit).ToString();
                litOutForDelivery.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.OutForDelivery).ToString();
                litDelivered.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Delivered).ToString();
                litIssues.Text = shipments.Count(s => s.IssueReported && !s.IsIssueResolved).ToString();
                litCustomers.Text = GetCustomerCount().ToString();

                var customerIds = shipments.Select(s => s.CustomerId).Distinct().ToList();
                var users = _db.Users
                    .Where(u => customerIds.Contains(u.Id))
                    .ToList()
                    .ToDictionary(u => u.Id, u => u);

                var recent = shipments
                    .OrderByDescending(s => s.UpdatedDate)
                    .Select(s => new
                    {
                        s.Id,
                        s.ControlNumber,
                        s.CreatedDate,
                        CustomerName = users.ContainsKey(s.CustomerId) ? (users[s.CustomerId].FullName ?? "(unknown)") : "(unknown)",
                        s.RecipientName,
                        s.RecipientPhone,
                        s.BranchOrigin,
                        s.RecipientAddress,
                        s.EstimatedDeliveryDate,
                        s.CurrentStatus,
                        s.IssueReported,
                        s.IsIssueResolved,
                        s.RiderName,
                        s.UpdatedDate
                    })
                    .ToList();

                gvRecent.DataSource = recent;
                gvRecent.DataBind();

                litStatusBar.Text = BuildStatusBar(shipments);
                litAttention.Text = BuildAttention(shipments, users);
            }
        }

        private static string SegmentClass(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "seg-pending";
                case ShipmentStatus.InTransit: return "seg-transit";
                case ShipmentStatus.OutForDelivery: return "seg-ofd";
                case ShipmentStatus.Delivered: return "seg-delivered";
                case ShipmentStatus.Received: return "seg-received";
                case ShipmentStatus.FailedDelivery: return "seg-failed";
                case ShipmentStatus.Returned: return "seg-returned";
                default: return "seg-cancelled";
            }
        }

        private string BuildStatusBar(List<Shipment> shipments)
        {
            if (shipments.Count == 0) return "<div class=\"empty-note\">No shipments yet.</div>";

            var groups = ((ShipmentStatus[])Enum.GetValues(typeof(ShipmentStatus)))
                .Select(st => new { Status = st, Count = shipments.Count(s => s.CurrentStatus == st) })
                .Where(g => g.Count > 0)
                .ToList();

            var sb = new System.Text.StringBuilder();
            sb.Append("<div class=\"status-bar\">");
            foreach (var g in groups)
            {
                var pct = (g.Count * 100.0 / shipments.Count).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
                sb.Append("<span class=\"" + SegmentClass(g.Status) + "\" style=\"width:" + pct + "%\" title=\"" + Helpers.StatusDisplayHelper.Label(g.Status) + ": " + g.Count + "\"></span>");
            }
            sb.Append("</div><div class=\"status-legend\">");
            foreach (var g in groups)
            {
                sb.Append("<span><i class=\"" + SegmentClass(g.Status) + "\"></i>" + Helpers.StatusDisplayHelper.Label(g.Status) + " <b>" + g.Count + "</b></span>");
            }
            sb.Append("</div>");

            var open = shipments.Count(s => !Helpers.StatusDisplayHelper.IsClosed(s.CurrentStatus));
            var today = Helpers.TimeHelper.Now.Date;
            var createdToday = shipments.Count(s => s.CreatedDate.Date == today);
            var updatedToday = shipments.Count(s => s.UpdatedDate.Date == today);
            sb.Append("<ul class=\"fact-list\" style=\"margin-top:8px;\">");
            sb.Append("<li><span>Active (not closed)</span><b>" + open + "</b></li>");
            sb.Append("<li><span>Created today</span><b>" + createdToday + "</b></li>");
            sb.Append("<li><span>Updated today</span><b>" + updatedToday + "</b></li>");
            sb.Append("</ul>");
            return sb.ToString();
        }

        private string BuildAttention(List<Shipment> shipments, Dictionary<string, ApplicationUser> users)
        {
            var updateUrl = ResolveUrl("~/Views/Admin/UpdateStatus.aspx") + "?id=";
            var today = Helpers.TimeHelper.Now.Date;

            var issues = shipments
                .Where(s => s.IssueReported && !s.IsIssueResolved)
                .OrderByDescending(s => s.IssueReportedDate)
                .Take(6)
                .ToList();

            var overdue = shipments
                .Where(s => !Helpers.StatusDisplayHelper.IsClosed(s.CurrentStatus) && s.EstimatedDeliveryDate.HasValue && s.EstimatedDeliveryDate.Value.Date < today)
                .OrderBy(s => s.EstimatedDeliveryDate)
                .Take(6)
                .ToList();

            var sb = new System.Text.StringBuilder();

            sb.Append("<div class=\"card-panel\"><h4 class=\"panel-title\">Open Issues</h4>");
            if (issues.Count == 0) sb.Append("<div class=\"empty-note\">No open issues.</div>");
            else
            {
                sb.Append("<ul class=\"mini-list\">");
                foreach (var s in issues)
                {
                    sb.Append("<li><a href=\"" + updateUrl + s.Id + "\"><span class=\"cell-main mono\">" + Server.HtmlEncode(s.ControlNumber) + "</span><span class=\"cell-sub\">"
                        + Server.HtmlEncode(s.RecipientName) + "</span></a><span class=\"cell-sub nowrap\">"
                        + (s.IssueReportedDate.HasValue ? s.IssueReportedDate.Value.ToString("MMM d") : "-") + "</span></li>");
                }
                sb.Append("</ul>");
            }
            sb.Append("</div>");

            sb.Append("<div class=\"card-panel\"><h4 class=\"panel-title\">Overdue Deliveries</h4>");
            if (overdue.Count == 0) sb.Append("<div class=\"empty-note\">Nothing overdue.</div>");
            else
            {
                sb.Append("<ul class=\"mini-list\">");
                foreach (var s in overdue)
                {
                    sb.Append("<li><a href=\"" + updateUrl + s.Id + "\"><span class=\"cell-main mono\">" + Server.HtmlEncode(s.ControlNumber) + "</span><span class=\"cell-sub\">"
                        + Server.HtmlEncode(Helpers.StatusDisplayHelper.Label(s.CurrentStatus)) + "</span></a><span class=\"cell-sub nowrap\">ETA "
                        + s.EstimatedDeliveryDate.Value.ToString("MMM d") + "</span></li>");
                }
                sb.Append("</ul>");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        protected void gvRecent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRecent.PageIndex = e.NewPageIndex;
            LoadData();
        }

        private int GetCustomerCount()
        {
            using (var _db = new ApplicationDbContext())
            {
                var customerRoleId = _db.Roles.Where(r => r.Name == "Customer").Select(r => r.Id).FirstOrDefault();
                if (customerRoleId == null) return 0;
                return _db.Users.Count(u => u.Roles.Any(r => r.RoleId == customerRoleId));
            }
        }
    }
}