using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using SoonestShipmentTrackingWebsite.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class UpdateStatus : System.Web.UI.Page
    {
        private int ShipmentId => int.Parse(Request.QueryString["id"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin", "Staff", "Rider")) return;

            if (User.IsInRole("Rider"))
                NotificationHelper.MarkRiderSeen();

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["id"]) || !int.TryParse(Request.QueryString["id"], out _))
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }
                LoadShipment();
            }
        }

        private void LoadShipment()
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments
                .Include(s => s.History)
                .FirstOrDefault(s => s.Id == ShipmentId);

                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }

                var currentRiderKeys = GetCurrentRiderKeys(_db);
                if (User.IsInRole("Rider") && !IsShipmentAssignedToRider(shipment, currentRiderKeys))
                {
                    Response.Redirect("~/Views/Forbidden.aspx");
                    return;
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litRecipientName.Text = Server.HtmlEncode(shipment.RecipientName);
                litDestinationCity.Text = Server.HtmlEncode(shipment.RecipientAddress);
                litBranchOrigin.Text = Server.HtmlEncode(shipment.BranchOrigin);
                litCurrentBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litRiderInfo.Text = StatusDisplayHelper.RiderInfo(shipment.CurrentStatus, shipment.RiderName);
                litIssueIndicator.Text = StatusDisplayHelper.IssueIndicator(shipment.IssueReported, shipment.IsIssueResolved);
                BindRiders(shipment.RiderName);

                pnlIssue.Visible = shipment.IssueReported;
                if (shipment.IssueReported)
                {
                    litIssueReport.Text = Server.HtmlEncode(string.IsNullOrWhiteSpace(shipment.IssueReport) ? "No additional details provided." : shipment.IssueReport);
                    litIssueDate.Text = shipment.IssueReportedDate.HasValue
                        ? shipment.IssueReportedDate.Value.ToString("MMM d, yyyy h:mm tt")
                        : "-";
                    pnlIssueResolvedInfo.Visible = shipment.IsIssueResolved;
                    litIssueResolvedDate.Text = shipment.IssueResolvedDate.HasValue
                        ? shipment.IssueResolvedDate.Value.ToString("MMM d, yyyy h:mm tt")
                        : "-";
                    btnResolveIssue.Visible = !shipment.IsIssueResolved;
                }

                ddlNewStatus.Items.Clear();
                IEnumerable<ShipmentStatus> allowedStatuses;
                if (User.IsInRole("Rider"))
                {
                    allowedStatuses = new[]
                    {
                        ShipmentStatus.Delivered,
                        ShipmentStatus.FailedDelivery,
                        ShipmentStatus.Returned
                    };
                }
                else
                {
                    allowedStatuses = Enum.GetValues(typeof(ShipmentStatus)).Cast<ShipmentStatus>();
                }

                foreach (var status in allowedStatuses)
                    ddlNewStatus.Items.Add(new System.Web.UI.WebControls.ListItem(StatusDisplayHelper.Label(status), status.ToString()));

                if (ddlNewStatus.Items.FindByValue(shipment.CurrentStatus.ToString()) != null)
                    ddlNewStatus.SelectedValue = shipment.CurrentStatus.ToString();

                if (User.IsInRole("Rider"))
                    ddlRider.Enabled = false;

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();
            }
        }

        private static bool IsShipmentAssignedToRider(Shipment shipment, IEnumerable<string> riderKeys)
        {
            if (shipment == null || string.IsNullOrWhiteSpace(shipment.RiderName) || riderKeys == null)
                return false;

            return riderKeys.Any(k => string.Equals(k, shipment.RiderName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<string> GetCurrentRiderKeys(ApplicationDbContext db)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId);
            if (user == null) return Enumerable.Empty<string>();

            var keys = new List<string>();
            if (!string.IsNullOrWhiteSpace(user.FullName)) keys.Add(user.FullName.Trim());
            if (!string.IsNullOrWhiteSpace(user.Email)) keys.Add(user.Email.Trim());
            return keys.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private void BindRiders(string selectedRiderName)
        {
            using (var db = new ApplicationDbContext())
            {
                var riderRoleId = db.Roles
                    .Where(r => r.Name == "Rider")
                    .Select(r => r.Id)
                    .FirstOrDefault();

                var riders = riderRoleId == null
                    ? new List<ApplicationUser>()
                    : db.Users
                        .Where(u => u.Roles.Any(ur => ur.RoleId == riderRoleId))
                        .OrderBy(u => u.FullName)
                        .ToList();

                ddlRider.Items.Clear();
                ddlRider.Items.Add(new ListItem("-- Select rider --", ""));

                foreach (var rider in riders)
                {
                    var riderName = string.IsNullOrWhiteSpace(rider.FullName) ? rider.Email : rider.FullName;
                    ddlRider.Items.Add(new ListItem(riderName, riderName));
                }

                if (!string.IsNullOrWhiteSpace(selectedRiderName) && ddlRider.Items.FindByValue(selectedRiderName) != null)
                    ddlRider.SelectedValue = selectedRiderName;
            }
        }

        protected void btnResolveIssue_Click(object sender, EventArgs e)
        {
            using (var db = new ApplicationDbContext())
            {
                var shipment = db.Shipments.FirstOrDefault(s => s.Id == ShipmentId);
                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }

                if (!shipment.IssueReported || shipment.IsIssueResolved)
                {
                    Session["FlashMessage"] = "No unresolved issue found for this shipment.";
                    Response.Redirect(Request.RawUrl);
                    return;
                }

                shipment.IsIssueResolved = true;
                shipment.IssueResolvedDate = DateTime.Now;
                shipment.UpdatedDate = DateTime.Now;

                db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = shipment.CurrentStatus,
                    Location = string.IsNullOrWhiteSpace(txtLocation.Text) ? shipment.BranchOrigin : txtLocation.Text.Trim(),
                    Notes = "Customer-reported issue has been resolved."
                });

                db.SaveChanges();

                Session["FlashMessage"] = $"Issue for shipment {shipment.ControlNumber} has been marked as resolved.";
                Response.Redirect(Request.RawUrl);
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments.FirstOrDefault(s => s.Id == ShipmentId);
                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }

                var currentRiderKeys = GetCurrentRiderKeys(_db);
                if (User.IsInRole("Rider") && !IsShipmentAssignedToRider(shipment, currentRiderKeys))
                {
                    ShowError("You can only update shipments assigned to you.");
                    return;
                }

                var newStatus = (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), ddlNewStatus.SelectedValue);

                if (User.IsInRole("Rider"))
                {
                    var riderAllowed = new[]
                    {
                        ShipmentStatus.Delivered,
                        ShipmentStatus.FailedDelivery,
                        ShipmentStatus.Returned
                    };

                    if (!riderAllowed.Contains(newStatus))
                    {
                        ShowError("Rider accounts can only update status to Delivered, Failed Delivery, or Returned.");
                        return;
                    }
                }

                if (newStatus == ShipmentStatus.OutForDelivery && string.IsNullOrWhiteSpace(ddlRider.SelectedValue))
                {
                    ShowError("Please select a rider when the shipment is marked Out for Delivery.");
                    return;
                }
                else
                {
                    shipment.RiderName = string.Empty;
                }

                if (!User.IsInRole("Rider") && !string.IsNullOrWhiteSpace(ddlRider.SelectedValue))
                    shipment.RiderName = ddlRider.SelectedValue;

                //Prevent duplicate status na magkasunod
                //if (shipment.CurrentStatus == newStatus)
                //{
                //    ShowError("Shipment status cannot be the same.");
                //    return;
                //}

                shipment.CurrentStatus = newStatus;
                shipment.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();

                _db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = newStatus,
                    Location = txtLocation.Text.Trim(),
                    Notes = txtNotes.Text.Trim()
                });
                await _db.SaveChangesAsync();

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} updated to {StatusDisplayHelper.Label(newStatus)}.";
                
                //Send SMS
                if (newStatus == ShipmentStatus.OutForDelivery)
                {
                    await SMSHelper.SendDeliveryOnTheWaySMS(shipment.RecipientPhone, shipment.RecipientName, shipment.ControlNumber, shipment.RiderName);
                }

                string redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/Track.aspx?controlNumber=" + shipment.ControlNumber);
                EmailHelper.SendShipmentUpdateEmail(shipment, redirectUrl);

                Response.Redirect("~/Views/Admin/Shipments.aspx");
            }
        }

        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}
