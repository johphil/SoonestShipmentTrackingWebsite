using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
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
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
            {
                using (var _db = new ApplicationDbContext())
                {
                    var customer = _db.Users.FirstOrDefault(u => u.Id == User.Identity.GetUserId());
                    if (!customer.EmailConfirmed)
                    {

                        return;
                    }
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            using (var _db = new ApplicationDbContext())
            {
                if (!Page.IsValid)
                    return;

                var customer = _db.Users.FirstOrDefault(u => u.Id == User.Identity.GetUserId());

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
                    CurrentStatus = ShipmentStatus.Pending
                };

                _db.Shipments.Add(shipment);
                _db.SaveChanges();

                _db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = ShipmentStatus.Pending,
                    Location = txtSenderAddress.Text.Trim(),
                    Notes = "Shipment created."
                });
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    EmailHelper.SendShipmentCreatedEmail(shipment);
                }

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} created for {customer.FullName}.";
                Response.Redirect("~/Views/Admin/Shipments.aspx");
            }
        }

        private static string GenerateControlNumber()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            return $"SGE{DateTime.UtcNow:yyyyMMdd}{suffix}";
        }
    }
}