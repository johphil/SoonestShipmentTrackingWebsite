using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Customer
{
    public partial class RequestShipment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Customer")) return;

            if (!IsEmailVerified())
            {
                litError.Text = "You need to verify your email address first before requesting a shipment. Please check your inbox for the verification email.";
                pnlError.Visible = true;
                btnRequest.Enabled = false;
                return;
            }

            if (!IsPostBack)
                BindBranchOrigins();
        }

        private bool IsEmailVerified()
        {
            var userId = User.Identity.GetUserId();
            using (var _db = new ApplicationDbContext())
            {
                return _db.Users.Any(u => u.Id == userId && u.EmailConfirmed);
            }
        }

        private void BindBranchOrigins()
        {
            using (var _db = new ApplicationDbContext())
            {
                var branches = _db.Branches.Where(b => b.IsActive).OrderBy(b => b.BranchName).ToList();

                ddlBranchOrigin.Items.Clear();
                ddlBranchOrigin.Items.Add(new ListItem("-- Select branch origin --", ""));
                foreach (var b in branches)
                    ddlBranchOrigin.Items.Add(new ListItem($"{b.BranchName} ({b.City})", b.BranchName));
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (!IsEmailVerified())
            {
                litError.Text = "You need to verify your email address first before requesting a shipment.";
                pnlError.Visible = true;
                return;
            }

            if (!Page.IsValid) return;

            if (!decimal.TryParse(txtWeight.Text, out var weight) || weight <= 0)
            {
                litError.Text = "Weight must be a valid number greater than zero.";
                pnlError.Visible = true;
                return;
            }

            if (!DateTime.TryParse(txtEstDelivery.Text, out var estDelivery))
            {
                litError.Text = "Please enter a valid delivery date.";
                pnlError.Visible = true;
                return;
            }

            using (var _db = new ApplicationDbContext())
            {
                var customerId = User.Identity.GetUserId();
                var customer = _db.Users.FirstOrDefault(u => u.Id == customerId);

                var shipment = new Shipment
                {
                    ControlNumber = "SGE" + DateTime.UtcNow.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                    CustomerId = customerId,
                    SenderName = txtSenderName.Text.Trim(),
                    BranchOrigin = ddlBranchOrigin.SelectedValue,
                    RecipientName = txtRecipientName.Text.Trim(),
                    RecipientAddress = txtRecipientAddress.Text.Trim(),
                    DestinationCity = txtRecipientAddress.Text.Trim(),
                    RecipientPhone = txtRecipientPhone.Text.Trim(),
                    RecipientEmail = txtRecipientEmail.Text.Trim(),
                    PackageDescription = txtPackageDescription.Text.Trim(),
                    WeightKg = weight,
                    EstimatedDeliveryDate = estDelivery,
                    CurrentStatus = ShipmentStatus.Pending
                };

                _db.Shipments.Add(shipment);
                _db.SaveChanges();

                _db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = ShipmentStatus.Pending,
                    Location = shipment.BranchOrigin,
                    Notes = "Shipment requested by customer; awaiting staff approval."
                });
                _db.SaveChanges();

                EmailHelper.SendShipmentRequestReceivedEmail(
                    customer?.Email,
                    customer?.FullName,
                    shipment.ControlNumber,
                    shipment.RecipientName,
                    shipment.BranchOrigin);

                Session["FlashMessage"] = $"Shipment request {shipment.ControlNumber} submitted and is awaiting staff approval.";
            }

            Response.Redirect("~/Views/Customer/MyShipments.aspx");
        }
    }
}
