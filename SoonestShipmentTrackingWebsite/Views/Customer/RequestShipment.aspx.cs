using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Customer
{
    public partial class RequestShipment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AccessControlHelper.EnsureRole(this, "Customer")) return;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            using (var _db = new ApplicationDbContext())
            {
                var customerId = User.Identity.GetUserId();
                var customer = _db.Users.FirstOrDefault(u => u.Id == customerId);
                if (customer == null)
                {
                    litError.Text = "Your account could not be found. Please log in again.";
                    pnlError.Visible = true;
                    return;
                }

                decimal? weight = null;
                if (decimal.TryParse(txtWeight.Text, out var parsedWeight))
                    weight = parsedWeight;

                DateTime? estDelivery = null;
                if (DateTime.TryParse(txtEstDelivery.Text, out var parsedDate))
                    estDelivery = parsedDate;

                var shipment = new Shipment
                {
                    ControlNumber = GenerateControlNumber(),
                    CustomerId = customer.Id,
                    SenderName = txtSenderName.Text.Trim(),
                    SenderAddress = txtSenderAddress.Text.Trim(),
                    RecipientName = txtRecipientName.Text.Trim(),
                    RecipientAddress = txtRecipientAddress.Text.Trim(),
                    DestinationCity = txtDestinationCity.Text.Trim(),
                    RecipientPhone = string.IsNullOrWhiteSpace(txtRecipientPhone.Text) ? customer.PhoneNumber : txtRecipientPhone.Text.Trim(),
                    RecipientEmail = string.IsNullOrWhiteSpace(txtRecipientEmail.Text) ? customer.Email : txtRecipientEmail.Text.Trim(),
                    PackageDescription = txtPackageDescription.Text.Trim(),
                    WeightKg = weight,
                    EstimatedDeliveryDate = estDelivery,
                    // Customer-submitted shipments start in PendingApproval,
                    // not Pending — a Staff member has to approve them first
                    // (see Views/Staff/ReviewShipment.aspx) before they enter
                    // the normal fulfillment pipeline.
                    CurrentStatus = ShipmentStatus.PendingApproval
                };

                _db.Shipments.Add(shipment);
                _db.SaveChanges();

                _db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = ShipmentStatus.PendingApproval,
                    Location = txtSenderAddress.Text.Trim(),
                    Notes = "Shipment submitted by customer, awaiting staff approval."
                });
                int result = _db.SaveChanges();

                    //if (result > 0)
                    //{
                    //    EmailHelper.SendShipmentCreatedEmail(shipment);
                    //}

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} submitted and is awaiting staff approval.";
                Response.Redirect("~/Views/Customer/MyShipments.aspx");
            }
        }

        private static string GenerateControlNumber()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            return $"SGE{DateTime.Now:yyyyMMdd}{suffix}";
        }
    }
}