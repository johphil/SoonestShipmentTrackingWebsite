using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Track : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var controlNumber = Request.QueryString["controlNumber"];
                if (!string.IsNullOrWhiteSpace(controlNumber))
                {
                    txtControlNumber.Text = controlNumber;
                    DoSearch(controlNumber);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoSearch(txtControlNumber.Text);
        }

        private void DoSearch(string controlNumber)
        {
            using (var _db = new ApplicationDbContext())
            {
                controlNumber = (controlNumber ?? "").Trim();
                pnlResult.Visible = false;
                pnlNotFound.Visible = false;

                if (string.IsNullOrEmpty(controlNumber))
                    return;

                var shipment = _db.Shipments
                    .Include(s => s.History)
                    .FirstOrDefault(s => s.ControlNumber == controlNumber);

                if (shipment == null)
                {
                    pnlNotFound.Visible = true;
                    return;
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litRecipientName.Text = Server.HtmlEncode(shipment.RecipientName);
                litDestinationCity.Text = Server.HtmlEncode(shipment.DestinationCity);
                litStatusBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litEta.Text = shipment.EstimatedDeliveryDate.HasValue
                    ? shipment.EstimatedDeliveryDate.Value.ToString("MMM d, yyyy")
                    : "TBD";

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();

                pnlResult.Visible = true;
            }
        }
    }
}