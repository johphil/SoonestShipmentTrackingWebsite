using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Customer
{
    public partial class RequestShipment : System.Web.UI.Page
    {
        private static readonly string[] PackageDescriptionOptions =
         {
            "Documents","Clothing and Apparel","Shoes and Footwear","Bags and Accessories","Electronics and Gadgets","Computer Equipment","Mobile Devices","Household Items","Kitchenware","Furniture and Fixtures","Food Products","Beverages","Health and Beauty Products","Medical Supplies","Automotive Parts and Accessories","Hardware and Tools","Industrial Supplies","Agricultural Products","Toys and Games","Sports and Recreational Items","Books and Educational Materials","Office Supplies","Packaging Materials","Gifts and Souvenirs","Personal Belongings","Fragile Items","Perishable Goods","Miscellaneous Items","Others"
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
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Customer")) return;

            if (Page.Form != null)
                Page.Form.Attributes["enctype"] = "multipart/form-data";

            if (!IsEmailVerified())
            {
                litError.Text = "You need to verify your email address first before requesting a shipment. Please check your inbox for the verification email.";
                pnlError.Visible = true;
                btnRequest.Enabled = false;
                return;
            }

            if (!IsPostBack)
            {
                BindShipmentTypes();
                BindBranchOrigins();
                BindPackageDescriptions();
                BindRegionAreas();
            }
        }

        private bool IsEmailVerified()
        {
            var userId = User.Identity.GetUserId();
            using (var _db = new ApplicationDbContext())
            {
                return _db.Users.Any(u => u.Id == userId && u.EmailConfirmed);
            }
        }

        private void BindShipmentTypes()
        {
            ddlShipmentType.Items.Clear();
            ddlShipmentType.Items.Add(new ListItem("-- Select shipment type --", ""));
            ddlShipmentType.Items.Add(new ListItem("Domestic", ShipmentType.DOMESTIC.ToString()));
            ddlShipmentType.Items.Add(new ListItem("International", ShipmentType.INTERNATIONAL.ToString()));
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

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (!IsEmailVerified())
            {
                litError.Text = "You need to verify your email address first before requesting a shipment.";
                pnlError.Visible = true;
                return;
            }

            if (!Page.IsValid) return;

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

            var packageDescription = string.Join(", ", cblPackageDescription.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text));
            if (string.IsNullOrWhiteSpace(packageDescription))
            {
                litError.Text = "Please select at least one package description.";
                pnlError.Visible = true;
                return;
            }

            using (var _db = new ApplicationDbContext())
            {
                var customerId = User.Identity.GetUserId();
                var customer = _db.Users.FirstOrDefault(u => u.Id == customerId);

                var shipment = new Shipment
                {
                    ControlNumber = GenerateControlNumber(),
                    CustomerId = customerId,
                    ShipmentType = selectedShipmentType,
                    SenderName = txtSenderName.Text.Trim(),
                    BranchOrigin = branchOrigin,
                    ContainerNumber = containerNumber,
                    PackingList = packingList,
                    RecipientName = txtRecipientName.Text.Trim(),
                    RecipientAddress = txtRecipientAddress.Text.Trim(),
                    RegionArea = ddlRegionArea.SelectedValue,
                    RecipientPhone = txtRecipientPhone.Text.Trim(),
                    RecipientEmail = txtRecipientEmail.Text.Trim(),
                    PackageDescription = packageDescription,
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
                    Location = selectedShipmentType == ShipmentType.DOMESTIC ? shipment.BranchOrigin : shipment.ContainerNumber,
                    Notes = "Shipment requested by customer; awaiting staff approval."
                });
                _db.SaveChanges();

                var originInfo = selectedShipmentType == ShipmentType.DOMESTIC
                    ? shipment.BranchOrigin
                    : (string.IsNullOrWhiteSpace(shipment.ContainerNumber) ? "International shipment" : $"Container {shipment.ContainerNumber}");

                EmailHelper.SendShipmentRequestReceivedEmail(
                    customer?.Email,
                    customer?.FullName,
                    shipment.ControlNumber,
                    shipment.RecipientName,
                    originInfo);

                Session["FlashMessage"] = $"Shipment request {shipment.ControlNumber} submitted and is awaiting staff approval.";
            }

            Response.Redirect("~/Views/Customer/MyShipments.aspx");
        }

        private static string GenerateControlNumber()
        {
            var suffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            return $"SGE{Helpers.TimeHelper.Now:yyyyMMdd}{suffix}";
        }
    }
}
