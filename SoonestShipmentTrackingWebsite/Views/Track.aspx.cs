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
                litDestinationCity.Text = Server.HtmlEncode(shipment.RecipientAddress);
                litBranchOrigin.Text = Server.HtmlEncode(shipment.BranchOrigin);
                litStatusBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litEta.Text = StatusDisplayHelper.EtaText(shipment.EstimatedDeliveryDate, shipment.CurrentStatus);
                litRiderInfo.Text = StatusDisplayHelper.RiderInfo(shipment.CurrentStatus, shipment.RiderName);
                litProgress.Text = "<div class=\"stepper-wrap\">" + StatusDisplayHelper.Progress(shipment.CurrentStatus) + "</div>";
                litPackage.Text = StatusDisplayHelper.Enc(shipment.PackageDescription);
                litWeight.Text = StatusDisplayHelper.WeightText(shipment.WeightKg);
                litCreated.Text = shipment.CreatedDate.ToString("MMM d, yyyy h:mm tt");
                litUpdated.Text = shipment.UpdatedDate.ToString("MMM d, yyyy h:mm tt");

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                var latest = history.FirstOrDefault();
                litLastLocation.Text = StatusDisplayHelper.Enc(latest != null ? latest.Location : null);
                var endTime = StatusDisplayHelper.IsClosed(shipment.CurrentStatus) ? shipment.UpdatedDate : TimeHelper.Now;
                litElapsed.Text = StatusDisplayHelper.Elapsed(shipment.CreatedDate, endTime);
                litEventCount.Text = history.Count.ToString();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();

                pnlResult.Visible = true;
            }
        }
    }
}

