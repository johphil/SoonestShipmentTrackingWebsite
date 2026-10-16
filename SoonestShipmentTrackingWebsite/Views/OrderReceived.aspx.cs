using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Linq;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class OrderReceived : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var controlNumber = Request.QueryString["controlNumber"];
            var token = Request.QueryString["token"];

            if (!EmailHelper.VerifyReceiptToken(controlNumber, token))
            {
                ShowMessage("This confirmation link is invalid.", false);
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var shipment = db.Shipments.FirstOrDefault(s => s.ControlNumber == controlNumber);
                if (shipment == null)
                {
                    ShowMessage("Shipment not found.", false);
                    return;
                }

                if (shipment.CurrentStatus != ShipmentStatus.Delivered && shipment.CurrentStatus != ShipmentStatus.Received)
                {
                    ShowMessage("This shipment has not been marked as delivered yet.", false);
                    return;
                }

                if (shipment.IsOrderReceived)
                {
                    ShowMessage("Thank you! This order was already confirmed as received.", true);
                }
                else
                {
                    shipment.IsOrderReceived = true;
                    shipment.OrderReceivedDate = DateTime.Now;
                    shipment.CurrentStatus = ShipmentStatus.Received;
                    shipment.UpdatedDate = DateTime.Now;
                    db.ShipmentHistories.Add(new ShipmentHistory
                    {
                        ShipmentId = shipment.Id,
                        Status = ShipmentStatus.Received,
                        Location = shipment.RecipientAddress,
                        Notes = "Order received confirmed by recipient."
                    });
                    db.SaveChanges();
                    ShowMessage("Thank you! Your order has been confirmed as received.", true);
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litReceivedDate.Text = shipment.OrderReceivedDate.HasValue ? shipment.OrderReceivedDate.Value.ToString("MMM d, yyyy h:mm tt") : "-";
                pnlDetails.Visible = true;
            }
        }

        private void ShowMessage(string message, bool success)
        {
            litMessage.Text = "<div class=\"validation-summary\"" + (success ? " style=\"background:#e8f5e9;color:#2e7d32;\"" : "") + ">" + Server.HtmlEncode(message) + "</div>";
        }
    }
}
