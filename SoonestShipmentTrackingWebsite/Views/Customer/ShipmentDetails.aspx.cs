using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Helpers;
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
    public partial class ShipmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!int.TryParse(Request.QueryString["id"], out var id))
            {
                Response.Redirect("~/Customer/MyShipments.aspx");
                return;
            }

            var userId = User.Identity.GetUserId();

            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments
                    .Include(s => s.History)
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == id && s.CustomerId == userId);

                if (shipment == null)
                {
                    // Either it doesn't exist or it belongs to someone else —
                    // in both cases, just bounce back to the list rather than
                    // revealing which case it was.
                    Response.Redirect("~/Views/Customer/MyShipments.aspx");
                    return;
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litRecipientName.Text = Server.HtmlEncode(shipment.RecipientName);
                litDestinationCity.Text = Server.HtmlEncode(shipment.DestinationCity);
                litStatusBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litSenderName.Text = Server.HtmlEncode(shipment.SenderName);
                litRecipientAddress.Text = Server.HtmlEncode(shipment.RecipientAddress);
                litPackageDescription.Text = string.IsNullOrWhiteSpace(shipment.PackageDescription)
                    ? "-" : Server.HtmlEncode(shipment.PackageDescription);
                litEta.Text = shipment.EstimatedDeliveryDate.HasValue
                    ? shipment.EstimatedDeliveryDate.Value.ToString("MMM d, yyyy") : "TBD";

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();
            }
        }
    }
}