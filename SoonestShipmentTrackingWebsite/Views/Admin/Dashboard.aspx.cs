using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipments = _db.Shipments.ToList();

                litTotal.Text = shipments.Count.ToString();
                litPending.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Pending).ToString();
                litInTransit.Text = shipments.Count(s =>
                    s.CurrentStatus == ShipmentStatus.InTransit ||
                    s.CurrentStatus == ShipmentStatus.PickedUp ||
                    s.CurrentStatus == ShipmentStatus.ArrivedAtHub).ToString();
                litOutForDelivery.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.OutForDelivery).ToString();
                litDelivered.Text = shipments.Count(s => s.CurrentStatus == ShipmentStatus.Delivered).ToString();
                litCustomers.Text = GetCustomerCount().ToString();

                var recent = shipments
                    .OrderByDescending(s => s.UpdatedDate)
                    .Take(10)
                    .Select(s => new
                    {
                        s.Id,
                        s.ControlNumber,
                        CustomerName = _db.Users.FirstOrDefault(u => u.Id == s.CustomerId)?.FullName ?? "(unknown)",
                        s.RecipientName,
                        s.DestinationCity,
                        s.CurrentStatus,
                        s.UpdatedDate
                    })
                    .ToList();

                gvRecent.DataSource = recent;
                gvRecent.DataBind();
            }
        }

        private int GetCustomerCount()
        {
            using (var _db = new ApplicationDbContext())
            {
                var customerRoleId = _db.Roles.Where(r => r.Name == "Customer").Select(r => r.Id).FirstOrDefault();
                if (customerRoleId == null) return 0;
                return _db.Users.Count(u => u.Roles.Any(r => r.RoleId == customerRoleId));
            }
        }
    }
}