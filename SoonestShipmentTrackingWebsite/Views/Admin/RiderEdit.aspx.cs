using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class RiderEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
            {
                litHeading.Text = IsEditMode ? "Edit Rider" : "Add Rider";
                LoadRider();
            }
        }

        private bool IsEditMode
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) && id > 0;
            }
        }

        private int RiderId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : 0;
            }
        }

        private void LoadRider()
        {
            if (!IsEditMode)
                return;

            using (var db = new ApplicationDbContext())
            {
                var rider = db.Riders.FirstOrDefault(r => r.RiderId == RiderId);
                if (rider == null)
                {
                    Response.Redirect("~/Views/Admin/Riders.aspx");
                    return;
                }

                hdnRiderId.Value = rider.RiderId.ToString();
                txtFullName.Text = rider.FullName;
                txtEmail.Text = rider.Email;
                txtMobile.Text = rider.MobileNumber;
                txtLicenseNumber.Text = rider.LicenseNumber;
                txtAddress.Text = rider.Address;
                pnlMetadata.Visible = true;
                litDateCreated.Text = rider.DateCreated.ToString("MMM d, yyyy h:mm tt");
                litDateModified.Text = rider.DateModified.ToString("MMM d, yyyy h:mm tt");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            using (var db = new ApplicationDbContext())
            {
                var fullName = (txtFullName.Text ?? string.Empty).Trim();
                var email = (txtEmail.Text ?? string.Empty).Trim();
                var mobile = (txtMobile.Text ?? string.Empty).Trim();
                var license = (txtLicenseNumber.Text ?? string.Empty).Trim();
                var address = (txtAddress.Text ?? string.Empty).Trim();

                int existingId;
                var isEditing = int.TryParse(hdnRiderId.Value, out existingId) && existingId > 0;

                if (db.Riders.Any(r => r.Email == email && r.RiderId != existingId))
                {
                    ShowError("Email address is already used by another rider.");
                    return;
                }

                if (db.Riders.Any(r => r.LicenseNumber == license && r.RiderId != existingId))
                {
                    ShowError("License number is already used by another rider.");
                    return;
                }

                Rider rider;
                if (isEditing)
                {
                    rider = db.Riders.FirstOrDefault(r => r.RiderId == existingId);
                    if (rider == null)
                    {
                        Response.Redirect("~/Views/Admin/Riders.aspx");
                        return;
                    }
                }
                else
                {
                    rider = new Rider
                    {
                        DateCreated = Helpers.TimeHelper.Now,
                        IsActive = true
                    };
                    db.Riders.Add(rider);
                }

                rider.FullName = fullName;
                rider.Email = email;
                rider.MobileNumber = mobile;
                rider.LicenseNumber = license;
                rider.Address = address;
                rider.DateModified = Helpers.TimeHelper.Now;

                db.SaveChanges();

                Session["FlashMessage"] = isEditing
                    ? $"Rider {rider.FullName} was updated successfully."
                    : $"Rider {rider.FullName} was created successfully.";

                Response.Redirect("~/Views/Admin/Riders.aspx");
            }
        }

        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}
