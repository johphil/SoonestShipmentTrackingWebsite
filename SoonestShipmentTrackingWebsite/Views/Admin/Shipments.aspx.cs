using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Shipments : System.Web.UI.Page
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
                var list = _db.Shipments
                    .OrderByDescending(s => s.UpdatedDate)
                    .AsNoTracking()
                    .ToList()
                    .Select(s => new
                    {
                        s.Id,
                        s.ControlNumber,
                        CustomerName = _db.Users.FirstOrDefault(u => u.Id == s.CustomerId)?.FullName ?? "(unknown)",
                        CustomerEmail = _db.Users.FirstOrDefault(u => u.Id == s.CustomerId)?.Email ?? "",
                        s.RecipientName,
                        s.DestinationCity,
                        s.CurrentStatus,
                        s.UpdatedDate
                    })
                    .ToList();

                gvShipments.DataSource = list;
                gvShipments.DataBind();
            }
        }
    }
}