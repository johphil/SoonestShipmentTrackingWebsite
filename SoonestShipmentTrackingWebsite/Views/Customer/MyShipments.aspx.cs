using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Customer
{
    public partial class MyShipments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Customer")) return;

            if (!IsPostBack)
                LoadData();
        }

        protected void btnRequestShipment_Click(object sender, EventArgs e)
        {
            var userId = User.Identity.GetUserId();
            bool verified;
            using (var _db = new ApplicationDbContext())
            {
                verified = _db.Users.Any(u => u.Id == userId && u.EmailConfirmed);
            }

            if (!verified)
            {
                pnlVerifyError.Visible = true;
                LoadData();
                return;
            }

            Response.Redirect("~/Views/Customer/RequestShipment.aspx");
        }

        protected void gvShipments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShipments.PageIndex = e.NewPageIndex;
            LoadData();
        }

        private void LoadData()
        {
            using (var _db = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();

                var shipments = _db.Shipments
                    .Where(s => s.CustomerId == userId)
                    .OrderByDescending(s => s.UpdatedDate)
                    .AsNoTracking()
                    .ToList();

                var total = shipments.Count;
                var active = shipments.Count(s => !Helpers.StatusDisplayHelper.IsClosed(s.CurrentStatus));
                var delivered = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Delivered || s.CurrentStatus == ShipmentStatus.Received);
                var issues = shipments.Count(s => s.IssueReported && !s.IsIssueResolved);
                litSummary.Text = "<div class=\"chip-row\">"
                    + "<span class=\"chip\">Total <b>" + total + "</b></span>"
                    + "<span class=\"chip\">Active <b>" + active + "</b></span>"
                    + "<span class=\"chip\">Delivered / Received <b>" + delivered + "</b></span>"
                    + "<span class=\"chip chip-alert\">Open issues <b>" + issues + "</b></span>"
                    + "</div>";

                gvShipments.DataSource = shipments;
                gvShipments.DataBind();
            }
        }
    }
}