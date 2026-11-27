using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Riders : System.Web.UI.Page
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var q = (txtSearch.Text ?? string.Empty).Trim();
            var riders = _db.Riders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                riders = riders.Where(r =>
                    r.FullName.Contains(q) ||
                    r.Email.Contains(q) ||
                    r.MobileNumber.Contains(q) ||
                    r.LicenseNumber.Contains(q));
            }

            gvRiders.DataSource = riders
                .OrderByDescending(r => r.IsActive)
                .ThenBy(r => r.FullName)
                .ToList();
            gvRiders.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvRiders.PageIndex = 0;
            LoadData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            gvRiders.PageIndex = 0;
            LoadData();
        }

        protected void gvRiders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRiders.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void gvRiders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeactivateRider" && e.CommandName != "ActivateRider")
                return;

            int riderId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out riderId))
                return;

            var rider = _db.Riders.FirstOrDefault(r => r.RiderId == riderId);
            if (rider == null)
                return;

            rider.IsActive = e.CommandName == "ActivateRider";
            rider.DateModified = Helpers.TimeHelper.Now;
            _db.SaveChanges();

            var hasShipmentAssociation = _db.Shipments.Any(s => s.RiderId == rider.RiderId || s.RiderName == rider.FullName);
            if (!rider.IsActive)
                Session["FlashMessage"] = hasShipmentAssociation
                    ? $"Rider {rider.FullName} is now inactive. Existing shipment history is preserved."
                    : $"Rider {rider.FullName} is now inactive.";
            else
                Session["FlashMessage"] = $"Rider {rider.FullName} is active again.";

            LoadData();
        }
    }
}
