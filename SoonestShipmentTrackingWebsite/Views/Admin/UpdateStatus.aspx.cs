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
using System.IO;
using System.Linq.Expressions;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class UpdateStatus : System.Web.UI.Page
    {
        private int ShipmentId => int.Parse(Request.QueryString["id"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin", "Staff", "Rider")) return;

            if (User.IsInRole("Rider"))
                NotificationHelper.MarkAllSeen(User.Identity.GetUserId());

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
                litDestinationCity.Text = Server.HtmlEncode(shipment.RecipientAddress + $" ({shipment.RegionArea})");
                litBranchOrigin.Text = Server.HtmlEncode(string.IsNullOrWhiteSpace(shipment.BranchOrigin) ? "-" : shipment.BranchOrigin);
                litContainerNumber.Text = Server.HtmlEncode(string.IsNullOrWhiteSpace(shipment.ContainerNumber) ? "-" : shipment.ContainerNumber);
                litShipmentType.Text = shipment.ShipmentType == ShipmentType.INTERNATIONAL ? "International" : "Domestic";

                pnlDomesticShipmentDetails.Visible = shipment.ShipmentType == ShipmentType.DOMESTIC;
                pnlInternationalShipmentDetails.Visible = shipment.ShipmentType == ShipmentType.INTERNATIONAL;
                pnlInternationalPackingList.Visible = shipment.ShipmentType == ShipmentType.INTERNATIONAL;

                if (shipment.ShipmentType == ShipmentType.INTERNATIONAL)
                {
                    if (!string.IsNullOrWhiteSpace(shipment.PackingList))
                    {
                        var imageUrl = ResolveUrl(shipment.PackingList.Trim());
                        var encodedUrl = HttpUtility.HtmlAttributeEncode(imageUrl);

                        litPackingListImage.Text = "<a href=\"" + encodedUrl + "\" target=\"_blank\" rel=\"noopener\"> View Packing List Image </a>";
                    }
                    else
                    {
                        litPackingListImage.Text = "No uploaded image.";
                    }
                }
                else
                {
                    litPackingListImage.Text = string.Empty;
                }

                var isNcrArea = StatusDisplayHelper.IsNcrArea(shipment.RegionArea);
                litCurrentBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus, isNcrArea);
                litRiderInfo.Text = StatusDisplayHelper.RiderInfo(shipment.CurrentStatus, shipment.RiderName);
                litIssueIndicator.Text = StatusDisplayHelper.IssueIndicator(shipment.IssueReported, shipment.IsIssueResolved);
                litProgress.Text = "<div class=\"stepper-wrap\">" + StatusDisplayHelper.Progress(shipment.CurrentStatus, shipment.ShipmentType, isNcrArea) + "</div>";
                litRecipientPhone.Text = StatusDisplayHelper.Enc(shipment.RecipientPhone);
                litSenderName.Text = StatusDisplayHelper.Enc(shipment.SenderName);
                litPackage.Text = StatusDisplayHelper.Enc(shipment.PackageDescription);
                litWeight.Text = StatusDisplayHelper.WeightText(shipment.WeightKg);
                litEta.Text = StatusDisplayHelper.EtaText(shipment.EstimatedDeliveryDate, shipment.CurrentStatus);
                litCreated.Text = shipment.CreatedDate.ToString("MMM d, yyyy h:mm tt");
                litUpdated.Text = shipment.UpdatedDate.ToString("MMM d, yyyy h:mm tt");
                litElapsed.Text = StatusDisplayHelper.Elapsed(shipment.CreatedDate,
                    StatusDisplayHelper.IsClosed(shipment.CurrentStatus) ? shipment.UpdatedDate : TimeHelper.Now);
                var customerUser = _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == shipment.CustomerId);
                litCustomer.Text = customerUser == null
                    ? "-"
                    : Server.HtmlEncode(customerUser.FullName ?? customerUser.Email ?? "-");
                BindRiders(shipment.RiderId, shipment.RiderName);

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
                    if (shipment.ShipmentType == ShipmentType.INTERNATIONAL)
                    {
                        allowedStatuses = isNcrArea
                            ? new[]
                            {
                                ShipmentStatus.Pending,
                                ShipmentStatus.ProcessingCustoms,
                                ShipmentStatus.ArrivedAtWarehouse,
                                ShipmentStatus.OutForDelivery,
                                ShipmentStatus.Delivered,
                                ShipmentStatus.Received,
                                ShipmentStatus.FailedDelivery,
                                ShipmentStatus.Returned
                            }
                            : new[]
                            {
                                ShipmentStatus.Pending,
                                ShipmentStatus.ProcessingCustoms,
                                ShipmentStatus.ArrivedAtWarehouse,
                                ShipmentStatus.InTransit,
                                ShipmentStatus.OutForDelivery,
                                ShipmentStatus.Delivered,
                                ShipmentStatus.Received,
                                ShipmentStatus.FailedDelivery,
                                ShipmentStatus.Returned
                            };
                    }
                    else
                    {
                        allowedStatuses = isNcrArea
                            ? new[]
                            {
                                ShipmentStatus.Pending,
                                ShipmentStatus.ArrivedAtWarehouse,
                                ShipmentStatus.OutForDelivery,
                                ShipmentStatus.Delivered,
                                ShipmentStatus.Received,
                                ShipmentStatus.FailedDelivery,
                                ShipmentStatus.Returned
                            }
                            : new[]
                            {
                                ShipmentStatus.Pending,
                                ShipmentStatus.InTransit,
                                ShipmentStatus.OutForDelivery,
                                ShipmentStatus.Delivered,
                                ShipmentStatus.Received,
                                ShipmentStatus.FailedDelivery,
                                ShipmentStatus.Returned
                            };
                    }
                }

                if (shipment.CurrentStatus == ShipmentStatus.Received || shipment.IsOrderReceived)
                {
                    allowedStatuses = new[] { shipment.CurrentStatus };
                    ddlNewStatus.Enabled = false;
                    ddlRider.Enabled = false;
                    txtLocation.Enabled = false;
                    txtNotes.Enabled = false;
                    fileProof.Enabled = false;
                    btnSave.Enabled = false;
                    ShowError("This shipment has been received by the recipient. Its status can no longer be changed.");
                }

                foreach (var status in allowedStatuses)
                    ddlNewStatus.Items.Add(new System.Web.UI.WebControls.ListItem(StatusDisplayHelper.Label(status, isNcrArea), status.ToString()));

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

        private void BindRiders(int? selectedRiderId, string selectedRiderName)
        {
            using (var db = new ApplicationDbContext())
            {
                var riders = db.Riders
                    .Where(r => r.IsActive)
                    .OrderBy(r => r.FullName)
                    .ToList();

                if (selectedRiderId.HasValue && riders.All(r => r.RiderId != selectedRiderId.Value))
                {
                    var selectedInactive = db.Riders.FirstOrDefault(r => r.RiderId == selectedRiderId.Value);
                    if (selectedInactive != null)
                        riders.Insert(0, selectedInactive);
                }

                ddlRider.Items.Clear();
                ddlRider.Items.Add(new ListItem("-- Select rider --", ""));

                foreach (var rider in riders)
                {
                    var label = rider.FullName + (rider.IsActive ? string.Empty : " (Inactive)");
                    ddlRider.Items.Add(new ListItem(label, rider.RiderId.ToString()));
                }

                if (selectedRiderId.HasValue && ddlRider.Items.FindByValue(selectedRiderId.Value.ToString()) != null)
                    ddlRider.SelectedValue = selectedRiderId.Value.ToString();
                else if (!string.IsNullOrWhiteSpace(selectedRiderName))
                {
                    var byName = riders.FirstOrDefault(r => string.Equals(r.FullName, selectedRiderName, StringComparison.OrdinalIgnoreCase));
                    if (byName != null && ddlRider.Items.FindByValue(byName.RiderId.ToString()) != null)
                        ddlRider.SelectedValue = byName.RiderId.ToString();
                }
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
                shipment.IssueResolvedDate = Helpers.TimeHelper.Now;
                shipment.UpdatedDate = Helpers.TimeHelper.Now;

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(SaveStatusAsync));
        }

        private async System.Threading.Tasks.Task SaveStatusAsync()
        {
            try
            {
                await SaveStatusCoreAsync();
            }
            catch (Exception ex)
            {
                ShowError("Unable to save the status update: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private async System.Threading.Tasks.Task SaveStatusCoreAsync()
        {
            string redirectTo = null;
            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments
                    .Include(s => s.Customer)
                    .Include(s => s.History)
                    .FirstOrDefault(s => s.Id == ShipmentId);
                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx", false);
                    return;
                }

                var currentRiderKeys = GetCurrentRiderKeys(_db);
                if (User.IsInRole("Rider") && !IsShipmentAssignedToRider(shipment, currentRiderKeys))
                {
                    ShowError("You can only update shipments assigned to you.");
                    return;
                }

                var newStatus = (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), ddlNewStatus.SelectedValue);

                if (shipment.CurrentStatus == ShipmentStatus.Received || shipment.IsOrderReceived)
                {
                    ShowError("This shipment has been received by the recipient. Its status can no longer be changed.");
                    return;
                }

                if (newStatus == ShipmentStatus.Received)
                {
                    ShowError("Staff and admin cannot set the status to RECEIVED. It is set when the recipient confirms receipt.");
                    return;
                }

                if (shipment.ShipmentType != ShipmentType.INTERNATIONAL && (newStatus == ShipmentStatus.ArrivedAtWarehouse || newStatus == ShipmentStatus.ProcessingCustoms))
                {
                    ShowError("Processing Customs and Arrived at Warehouse are available only for international shipments.");
                    return;
                }

                if (shipment.ShipmentType == ShipmentType.INTERNATIONAL && newStatus == ShipmentStatus.InTransit && shipment.RegionArea == "NCR")
                {
                    ShowError("You cannot select In-Transit if the region of delivery is NCR.");
                    return;
                }

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

                byte[] proofBytes = null;
                string proofType = null;
                if (newStatus == ShipmentStatus.Delivered)
                {
                    try
                    {
                        if (fileProof.HasFile)
                        {
                            proofType = fileProof.PostedFile.ContentType;
                            if (proofType == null || !proofType.StartsWith("image/"))
                            {
                                ShowError("Proof of delivery must be an image file.");
                                return;
                            }
                            if (fileProof.PostedFile.ContentLength > 5 * 1024 * 1024)
                            {
                                ShowError("Proof of delivery photo must be 5 MB or smaller.");
                                return;
                            }
                            proofBytes = fileProof.FileBytes;
                        }
                        else if (User.IsInRole("Rider"))
                        {
                            ShowError("Please take or upload a proof of delivery photo before marking as Delivered.");
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        ShowError(e.Message);
                    }
                }

                if (newStatus == ShipmentStatus.OutForDelivery && string.IsNullOrWhiteSpace(ddlRider.SelectedValue))
                {
                    ShowError("Please select a rider when the shipment is marked Out for Delivery.");
                    return;
                }

                if (!User.IsInRole("Rider") && !string.IsNullOrWhiteSpace(ddlRider.SelectedValue))
                {
                    int riderId;
                    if (!int.TryParse(ddlRider.SelectedValue, out riderId))
                    {
                        ShowError("Please select a valid rider.");
                        return;
                    }

                    var rider = _db.Riders.FirstOrDefault(r => r.RiderId == riderId);
                    if (rider == null)
                    {
                        ShowError("Selected rider record no longer exists.");
                        return;
                    }

                    if (newStatus == ShipmentStatus.OutForDelivery && !rider.IsActive)
                    {
                        ShowError("Selected rider is inactive and cannot be assigned.");
                        return;
                    }

                    shipment.RiderId = rider.RiderId;
                    shipment.RiderName = rider.FullName;
                }

                //Prevent duplicate status na magkasunod
                //if (shipment.CurrentStatus == newStatus)
                //{
                //    ShowError("Shipment status cannot be the same.");
                //    return;
                //}

                var previousStatus = shipment.CurrentStatus;
                shipment.CurrentStatus = newStatus;
                shipment.UpdatedDate = Helpers.TimeHelper.Now;
                await _db.SaveChangesAsync();

                string loc = "";

                if (newStatus == ShipmentStatus.Delivered)
                {
                    loc = shipment.RecipientAddress;
                }
                else if (newStatus == ShipmentStatus.ProcessingCustoms)
                {
                    loc = "Philippines";
                }
                else if (newStatus == ShipmentStatus.ArrivedAtWarehouse)
                {
                    loc = "Las Pinas Branch";
                }
                else
                {
                    loc = txtLocation.Text.Trim();
                }


                _db.ShipmentHistories.Add(new ShipmentHistory
                    {
                        ShipmentId = shipment.Id,
                        Status = newStatus,
                        Location = loc,
                        Notes = txtNotes.Text.Trim(),
                        ProofOfDelivery = proofBytes,
                        ProofContentType = proofType
                    });
                await _db.SaveChangesAsync();

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} updated to {StatusDisplayHelper.Label(newStatus)}.";
                
                //Send SMS / email (best effort: failures must not break the save)
                try
                {
                    if (newStatus == ShipmentStatus.OutForDelivery)
                    {
                        await SMSHelper.SendRiderAssignmentSMS(shipment.Rider.MobileNumber, shipment.Rider.FullName, shipment.ControlNumber);
                        await SMSHelper.SendDeliveryOnTheWaySMS(shipment.RecipientPhone, shipment.RecipientName, shipment.ControlNumber, shipment.RiderName);

                        var riderUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/MarkDelivered.aspx?controlNumber=" + Uri.EscapeDataString(shipment.ControlNumber) + "&token=" + EmailHelper.GenerateReceiptToken(shipment.ControlNumber));
                        EmailHelper.SendRiderShipmentAssignmentEmail(shipment, riderUrl);
                    }

                    string redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/Track.aspx?controlNumber=" + shipment.ControlNumber);
                    EmailHelper.SendShipmentUpdateEmail(shipment, redirectUrl);

                    if (newStatus == ShipmentStatus.Delivered)
                    {
                        var receivedUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/OrderReceived.aspx?controlNumber=" + Uri.EscapeDataString(shipment.ControlNumber) + "&token=" + EmailHelper.GenerateReceiptToken(shipment.ControlNumber));
                        EmailHelper.SendOrderReceivedEmail(shipment, receivedUrl);
                    }
                }
                catch (Exception)
                {
                }

                Response.Redirect("~/Views/Admin/Shipments.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void ShowError(string message)        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}
