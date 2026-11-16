using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class CreateShipment : System.Web.UI.Page
    {
        private static readonly string[] PackageDescriptionOptions =
        {
            "Carton Box", "Pallet", "Crate", "Drum/Barrel", "Sack", "Roll/Spool"
        };

        private static readonly string[] RegionAreaOptions =
        {
            "NCR",
            "CAR",
            "Region I",
            "Region II",
            "Region III",
            "Region IV-A",
            "MIMAROPA",
            "Region V",
            "Region VI",
            "NIR",
            "Region VII",
            "Region VIII",
            "Region IX",
            "Region X",
            "Region XI",
            "Region XII",
            "Region XIII",
            "BARMM"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AccessControlHelper.EnsureRole(this, "Admin", "Staff")) return;

            if (Page.Form != null)
                Page.Form.Attributes["enctype"] = "multipart/form-data";

            if (!IsPostBack)
            {
                BindShipmentTypes();
                BindCustomers();
                BindBranchOrigins();
                BindPackageDescriptions();
                BindRegionAreas();
            }
        }

        private void BindShipmentTypes()
        {
            ddlShipmentType.Items.Clear();
            ddlShipmentType.Items.Add(new ListItem("-- Select shipment type --", ""));
            ddlShipmentType.Items.Add(new ListItem("Domestic", ShipmentType.DOMESTIC.ToString()));
            ddlShipmentType.Items.Add(new ListItem("International", ShipmentType.INTERNATIONAL.ToString()));
        }

        private void BindPackageDescriptions()
        {
            cblPackageDescription.Items.Clear();
            foreach (var option in PackageDescriptionOptions)
                cblPackageDescription.Items.Add(new ListItem(option, option));
        }

        private void BindRegionAreas()
        {
            ddlRegionArea.Items.Clear();
            ddlRegionArea.Items.Add(new ListItem("-- Select region area --", ""));

            foreach (var region in RegionAreaOptions)
                ddlRegionArea.Items.Add(new ListItem(region, region));
        }

        protected void cvPackageDescription_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cblPackageDescription.Items.Cast<ListItem>().Any(i => i.Selected);
        }

        protected void cvShipmentTypeFields_ServerValidate(object source, ServerValidateEventArgs args)
        {
            cvShipmentTypeFields.ErrorMessage = string.Empty;

            if (!Enum.TryParse(ddlShipmentType.SelectedValue, out ShipmentType selectedType))
            {
                cvShipmentTypeFields.ErrorMessage = "Please select shipment type.";
                args.IsValid = false;
                return;
            }

            if (selectedType == ShipmentType.DOMESTIC)
            {
                args.IsValid = !string.IsNullOrWhiteSpace(ddlBranchOrigin.SelectedValue);
                if (!args.IsValid)
                    cvShipmentTypeFields.ErrorMessage = "Please select a branch origin for domestic shipment.";
                return;
            }

            args.IsValid = !string.IsNullOrWhiteSpace(txtContainerNumber.Text);
            if (!args.IsValid)
                cvShipmentTypeFields.ErrorMessage = "Container number is required for international shipment.";
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
                    BindShipmentTypes();
                    BindCustomers();
                    BindBranchOrigins();
                    BindPackageDescriptions();
                    BindRegionAreas();
                    return;
                }

                if (!Enum.TryParse(ddlShipmentType.SelectedValue, out ShipmentType selectedShipmentType))
                {
                    litError.Text = "Please select shipment type.";
                    pnlError.Visible = true;
                    return;
                }

                var branchOrigin = selectedShipmentType == ShipmentType.DOMESTIC
                    ? ddlBranchOrigin.SelectedValue
                    : string.Empty;
                var containerNumber = selectedShipmentType == ShipmentType.INTERNATIONAL
                    ? (txtContainerNumber.Text ?? string.Empty).Trim()
                    : string.Empty;

                var packingList = string.Empty;
                if (selectedShipmentType == ShipmentType.INTERNATIONAL && fuPackingListImage.HasFile)
                {
                    var extension = Path.GetExtension(fuPackingListImage.FileName) ?? string.Empty;
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                    if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        litError.Text = "Packing list image must be a valid image file (JPG, JPEG, PNG, GIF, BMP, WEBP).";
                        pnlError.Visible = true;
                        return;
                    }

                    var uploadFolderVirtual = "~/Uploads/PackingLists";
                    var uploadFolderPhysical = Server.MapPath(uploadFolderVirtual);
                    Directory.CreateDirectory(uploadFolderPhysical);

                    var fileName = $"packing_{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
                    var filePath = Path.Combine(uploadFolderPhysical, fileName);
                    fuPackingListImage.SaveAs(filePath);

                    packingList = VirtualPathUtility.ToAbsolute(uploadFolderVirtual + "/" + fileName);
                }

                if (selectedShipmentType == ShipmentType.DOMESTIC && string.IsNullOrWhiteSpace(branchOrigin))
                {
                    litError.Text = "Please select a branch origin for domestic shipment.";
                    pnlError.Visible = true;
                    return;
                }

                if (selectedShipmentType == ShipmentType.INTERNATIONAL && string.IsNullOrWhiteSpace(containerNumber))
                {
                    litError.Text = "Container number is required for international shipment.";
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
                    ShipmentType = selectedShipmentType,
                    SenderName = txtSenderName.Text.Trim(),
                    BranchOrigin = branchOrigin,
                    ContainerNumber = containerNumber,
                    PackingList = packingList,
                    RecipientName = txtRecipientName.Text.Trim(),
                    RecipientAddress = txtRecipientAddress.Text.Trim(),
                    RegionArea = ddlRegionArea.SelectedValue,
                    DestinationCity = txtRecipientAddress.Text.Trim(),
                    RecipientPhone = txtRecipientPhone.Text.Trim(),
                    RecipientEmail = txtRecipientEmail.Text.Trim(),
                    PackageDescription = string.Join(", ", cblPackageDescription.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text)),
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
                    Location = selectedShipmentType == ShipmentType.DOMESTIC ? branchOrigin : "-",
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
            return $"SGE{Helpers.TimeHelper.Now:yyyyMMdd}{suffix}";
        }
    }
}