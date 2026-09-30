using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Models;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Shipments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin", "Staff", "Rider")) return;

            pnlCreateShipment.Visible = !User.IsInRole("Rider") && !User.IsInRole("Customer");

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            using (var _db = new ApplicationDbContext())
            {
                var query = _db.Shipments.AsNoTracking().AsQueryable();

                if (User.IsInRole("Rider"))
                {
                    var riderUserId = User.Identity.GetUserId();
                    var rider = _db.Users.AsNoTracking().FirstOrDefault(u => u.Id == riderUserId);

                    if (rider == null)
                    {
                        gvShipments.DataSource = new List<object>();
                        gvShipments.DataBind();
                        return;
                    }

                    var riderKeys = new List<string>();
                    if (!string.IsNullOrWhiteSpace(rider.FullName)) riderKeys.Add(rider.FullName.Trim());
                    if (!string.IsNullOrWhiteSpace(rider.Email)) riderKeys.Add(rider.Email.Trim());

                    if (!riderKeys.Any())
                    {
                        gvShipments.DataSource = new List<object>();
                        gvShipments.DataBind();
                        return;
                    }

                    query = query.Where(s => riderKeys.Contains(s.RiderName));
                }

                var list = query
                    .OrderByDescending(s => s.UpdatedDate)
                    .ToList()
                    .Select(s => new
                    {
                        s.Id,
                        s.ControlNumber,
                        CustomerName = _db.Users.FirstOrDefault(u => u.Id == s.CustomerId)?.FullName ?? "(unknown)",
                        CustomerEmail = _db.Users.FirstOrDefault(u => u.Id == s.CustomerId)?.Email ?? "",
                        s.RecipientName,
                        s.BranchOrigin,
                        s.RecipientAddress,
                        s.CurrentStatus,
                        s.IssueReported,
                        s.IsIssueResolved,
                        s.RiderName,
                        s.UpdatedDate
                    })
                    .ToList();

                gvShipments.DataSource = list;
                gvShipments.DataBind();
            }
        }
    }
}