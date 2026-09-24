using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Customer
{
    public partial class ShipmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AccessControlHelper.EnsureRole(this, "Customer")) return;

            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["id"], out var id))
            {
                Response.Redirect("~/Views/Customer/MyShipments.aspx");
                return;
            }

            var userId = User.Identity.GetUserId();

            // The customer opened a shipment (e.g. via a notification), so
            // clear the unread state on the bell.
            NotificationHelper.MarkAllSeen();

            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments
                    .Include(s => s.History)
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == id && s.CustomerId == userId);

                if (shipment == null)
                {
                    // Either it doesn't exist or it belongs to someone else —
                    // in both cases, just bounce back to the list rather than
                    // revealing which case it was.
                    Response.Redirect("~/Views/Customer/MyShipments.aspx");
                    return;
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litRecipientName.Text = Server.HtmlEncode(shipment.RecipientName);
                litRecipientAddress1.Text = Server.HtmlEncode(shipment.RecipientAddress);
                litBranchOrigin.Text = Server.HtmlEncode(shipment.BranchOrigin);
                litStatusBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litSenderName.Text = Server.HtmlEncode(shipment.SenderName);
                litRecipientAddress.Text = Server.HtmlEncode(shipment.RecipientAddress);
                litPackageDescription.Text = string.IsNullOrWhiteSpace(shipment.PackageDescription)
                    ? "-" : Server.HtmlEncode(shipment.PackageDescription);
                litEta.Text = shipment.EstimatedDeliveryDate.HasValue
                    ? shipment.EstimatedDeliveryDate.Value.ToString("MMM d, yyyy") : "TBD";
                litRiderInfo.Text = StatusDisplayHelper.RiderInfo(shipment.CurrentStatus, shipment.RiderName);
                pnlFeedback.Visible = shipment.CurrentStatus == ShipmentStatus.Delivered;
                btnOrderReceived.Visible = shipment.CurrentStatus == ShipmentStatus.Delivered && !shipment.IsOrderReceived;
                btnReportIssue.Visible = shipment.CurrentStatus == ShipmentStatus.Delivered && !shipment.IssueReported;
                litFeedback.Text = shipment.IsOrderReceived
                    ? "<div class=\"validation-summary\" style=\"background:#e8f5e9;color:#2e7d32;\">Order received on " + shipment.OrderReceivedDate.Value.ToString("MMM d, yyyy h:mm tt") + ".</div>"
                    : "";
                if (shipment.IssueReported)
                    litFeedback.Text += "<div class=\"validation-summary\">Issue report submitted on " + shipment.IssueReportedDate.Value.ToString("MMM d, yyyy h:mm tt") + ".</div>";
                if (shipment.IsIssueResolved)
                    litFeedback.Text += "<div class=\"validation-summary\" style=\"background:#e8f5e9;color:#2e7d32;\">Issue resolved on " + (shipment.IssueResolvedDate.HasValue ? shipment.IssueResolvedDate.Value.ToString("MMM d, yyyy h:mm tt") : "-") + ".</div>";

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();
            }
        }

        protected void btnOrderReceived_Click(object sender, EventArgs e)
        {
            UpdateCustomerFeedback(true, null);
        }

        protected void btnReportIssue_Click(object sender, EventArgs e)
        {
            var issue = (txtIssueReport.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(issue))
            {
                litFeedback.Text = "<div class=\"validation-summary\">Please describe the issue before submitting.</div>";
                pnlFeedback.Visible = true;
                return;
            }
            UpdateCustomerFeedback(false, issue);
        }

        private void UpdateCustomerFeedback(bool received, string issue)
        {
            if (!int.TryParse(Request.QueryString["id"], out var id)) return;
            using (var db = new ApplicationDbContext())
            {
                var cId = User.Identity.GetUserId();
                var shipment = db.Shipments.FirstOrDefault(s => s.Id == id && s.CustomerId == cId);
                if (shipment == null || shipment.CurrentStatus != ShipmentStatus.Delivered) return;
                if (received && !shipment.IsOrderReceived)
                {
                    shipment.IsOrderReceived = true;
                    shipment.OrderReceivedDate = DateTime.Now;
                }
                if (!received && !shipment.IssueReported)
                {
                    shipment.IssueReported = true;
                    shipment.IsIssueResolved = false;
                    shipment.IssueReport = issue;
                    shipment.IssueReportedDate = DateTime.Now;
                    shipment.IssueResolvedDate = null;
                }
                shipment.UpdatedDate = DateTime.Now;
                db.SaveChanges();
            }
            Response.Redirect(Request.RawUrl);
        }
    }
}
