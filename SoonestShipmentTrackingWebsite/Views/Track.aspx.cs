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
                litBranchOrigin.Text = Server.HtmlEncode(string.IsNullOrWhiteSpace(shipment.BranchOrigin) ? "-" : shipment.BranchOrigin);
                litContainerNumber.Text = Server.HtmlEncode(string.IsNullOrWhiteSpace(shipment.ContainerNumber) ? "-" : shipment.ContainerNumber);
                litShipmentType.Text = shipment.ShipmentType == ShipmentType.INTERNATIONAL ? "International" : "Domestic";

                pnlDomesticShipmentDetails.Visible = shipment.ShipmentType == ShipmentType.DOMESTIC;
                pnlInternationalShipmentDetails.Visible = shipment.ShipmentType == ShipmentType.INTERNATIONAL;
                pnlInternationalPackingList.Visible = shipment.ShipmentType == ShipmentType.INTERNATIONAL;

                if (shipment.ShipmentType == ShipmentType.INTERNATIONAL)
                {
                    if (!string.IsNullOrWhiteSpace(shipment.PackingList))
                    {
                        var imageUrl = ResolveUrl(shipment.PackingList.Trim());
                        var encodedUrl = HttpUtility.HtmlAttributeEncode(imageUrl);
                        litPackingListImage.Text = "<a href=\"" + encodedUrl + "\" target=\"_blank\" rel=\"noopener\"> View Packing List Image </a>";
                    }
                    else
                    {
                        litPackingListImage.Text = "-";
                    }
                }
                else
                {
                    litPackingListImage.Text = string.Empty;
                }

                litStatusBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);
                litEta.Text = StatusDisplayHelper.EtaText(shipment.EstimatedDeliveryDate, shipment.CurrentStatus);
                litRiderInfo.Text = StatusDisplayHelper.RiderInfo(shipment.CurrentStatus, shipment.RiderName);
                litProgress.Text = "<div class=\"stepper-wrap\">" + StatusDisplayHelper.Progress(shipment.CurrentStatus, shipment.ShipmentType) + "</div>";
                litSenderName.Text = StatusDisplayHelper.Enc(shipment.SenderName);
                litRecipientPhone.Text = StatusDisplayHelper.Enc(shipment.RecipientPhone);
                litRecipientEmail.Text = StatusDisplayHelper.Enc(shipment.RecipientEmail);
                litPackage.Text = StatusDisplayHelper.Enc(shipment.PackageDescription);
                litWeight.Text = StatusDisplayHelper.WeightText(shipment.WeightKg);
                litCreated.Text = shipment.CreatedDate.ToString("MMM d, yyyy h:mm tt");
                litUpdated.Text = shipment.UpdatedDate.ToString("MMM d, yyyy h:mm tt");

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                var endTime = StatusDisplayHelper.IsClosed(shipment.CurrentStatus) ? shipment.UpdatedDate : TimeHelper.Now;
                litElapsed.Text = StatusDisplayHelper.Elapsed(shipment.CreatedDate, endTime);
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();

                pnlResult.Visible = true;
            }
        }
    }
}

