using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class CreateShipment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
            {
                BindCustomers();
                BindBranchOrigins();
            }
        }

        private void BindCustomers()
        {
            using (var _db = new ApplicationDbContext())
            {
                var customerRoleId = _db.Roles.Where(r => r.Name == "Customer").Select(r => r.Id).FirstOrDefault();
                var customers = customerRoleId == null
                    ? new System.Collections.Generic.List<ApplicationUser>()
                    : _db.Users.Where(u => u.Roles.Any(r => r.RoleId == customerRoleId) && u.EmailConfirmed).OrderBy(u => u.FullName).ToList();

                ddlCustomer.Items.Clear();
                ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select registered customer --", ""));
                foreach (var c in customers)
                    ddlCustomer.Items.Add(new System.Web.UI.WebControls.ListItem($"{c.FullName} ({c.Email})", c.Id));
            }
        }

        private void BindBranchOrigins()
        {
            using (var _db = new ApplicationDbContext())
            {
                var branches = _db.Branches
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.BranchName)
                    .ToList();

                ddlBranchOrigin.Items.Clear();
                ddlBranchOrigin.Items.Add(new ListItem("-- Select branch origin --", ""));

                foreach (var b in branches)
                {
                    ddlBranchOrigin.Items.Add(new ListItem($"{b.BranchName} ({b.City})", b.BranchName));
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            using (var _db = new ApplicationDbContext())
            {
                if (!Page.IsValid)
                    return;

                var customer = _db.Users.FirstOrDefault(u => u.Id == ddlCustomer.SelectedValue);
                if (customer == null)
                {
                    litError.Text = "Selected customer was not found.";
                    pnlError.Visible = true;
                    BindCustomers();
                    BindBranchOrigins();
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlBranchOrigin.SelectedValue))
                {
                    litError.Text = "Please select a branch origin.";
                    pnlError.Visible = true;
                    return;
                }

                decimal? weight = null;
                if (decimal.TryParse(txtWeight.Text, out var parsedWeight) && parsedWeight > 0)
                    weight = parsedWeight;
                else
                {
                    litError.Text = "Weight must be a valid number greater than zero.";
                    pnlError.Visible = true;
                    return;
                }

                DateTime? estDelivery = null;
                if (DateTime.TryParse(txtEstDelivery.Text, out var parsedDate))
                    estDelivery = parsedDate;
                else
                {
                    litError.Text = "Please enter a valid delivery date.";
                    pnlError.Visible = true;
                    return;
                }

                var shipment = new Shipment
                {
                    ControlNumber = GenerateControlNumber(),
                    CustomerId = customer.Id,
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
                    Location = ddlBranchOrigin.SelectedValue,
                    Notes = "Shipment created."
                });
                int result = _db.SaveChanges();

                if (result > 0)
                {
                    shipment.Customer = customer;
                    EmailHelper.SendShipmentCreatedEmail(shipment);
                }

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} created for {customer.FullName}.";
                Response.Redirect("~/Views/Admin/Shipments.aspx");
            }
        }

        private static string GenerateControlNumber()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            return $"SGE{DateTime.Now:yyyyMMdd}{suffix}";
        }
    }
}