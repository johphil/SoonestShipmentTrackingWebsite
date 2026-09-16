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

                gvShipments.DataSource = shipments;
                gvShipments.DataBind();
            }
        }
    }
}