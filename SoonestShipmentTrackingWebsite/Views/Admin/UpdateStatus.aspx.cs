using SoonestShipmentTrackingWebsite.Helpers;
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
    public partial class UpdateStatus : System.Web.UI.Page
    {
        private int ShipmentId => int.Parse(Request.QueryString["id"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["id"]) || !int.TryParse(Request.QueryString["id"], out _))
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }
                LoadShipment();
            }
        }

        private void LoadShipment()
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments
                .Include(s => s.History)
                .FirstOrDefault(s => s.Id == ShipmentId);

                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }

                litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
                litRecipientName.Text = Server.HtmlEncode(shipment.RecipientName);
                litDestinationCity.Text = Server.HtmlEncode(shipment.DestinationCity);
                litCurrentBadge.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus);

                ddlNewStatus.Items.Clear();
                foreach (ShipmentStatus status in Enum.GetValues(typeof(ShipmentStatus)))
                    ddlNewStatus.Items.Add(new System.Web.UI.WebControls.ListItem(StatusDisplayHelper.Label(status), status.ToString()));
                ddlNewStatus.SelectedValue = shipment.CurrentStatus.ToString();

                var history = shipment.History.OrderByDescending(h => h.Timestamp).ToList();
                rptHistory.DataSource = history;
                rptHistory.DataBind();
                lblNoHistory.Visible = !history.Any();
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            using (var _db = new ApplicationDbContext())
            {
                var shipment = _db.Shipments.FirstOrDefault(s => s.Id == ShipmentId);
                if (shipment == null)
                {
                    Response.Redirect("~/Views/Admin/Shipments.aspx");
                    return;
                }

                var newStatus = (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), ddlNewStatus.SelectedValue);

                //Prevent duplicate status na magkasunod
                //if (shipment.CurrentStatus == newStatus)
                //{
                //    ShowError("Shipment status cannot be the same.");
                //    return;
                //}

                shipment.CurrentStatus = newStatus;
                shipment.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();

                _db.ShipmentHistories.Add(new ShipmentHistory
                {
                    ShipmentId = shipment.Id,
                    Status = newStatus,
                    Location = txtLocation.Text.Trim(),
                    Notes = txtNotes.Text.Trim()
                });
                await _db.SaveChangesAsync();

                Session["FlashMessage"] = $"Shipment {shipment.ControlNumber} updated to {StatusDisplayHelper.Label(newStatus)}.";
                
                //Send SMS
                if (newStatus == ShipmentStatus.OutForDelivery)
                {
                    await SMSHelper.SendDeliveryOnTheWaySMS(shipment.RecipientPhone, shipment.RecipientName, shipment.ControlNumber);
                }
                else
                {                                                                                    
                    string redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/Track.aspx?controlNumber=" + shipment.ControlNumber);
                    EmailHelper.SendShipmentUpdateEmail(shipment, redirectUrl);
                }

                Response.Redirect("~/Views/Admin/Shipments.aspx");
            }
        }

        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}