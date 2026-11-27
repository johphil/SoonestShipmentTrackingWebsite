<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.Models" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.Helpers" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        LoadShipment();
    }

    protected void btnMarkDelivered_Click(object sender, EventArgs e)
    {
        var controlNumber = (Request.QueryString["controlNumber"] ?? string.Empty).Trim();
        var token = Request.QueryString["token"];

        if (string.IsNullOrWhiteSpace(controlNumber))
        {
            ShowMessage("Missing tracking number.", false);
            return;
        }

        if (!string.IsNullOrWhiteSpace(token) && !EmailHelper.VerifyReceiptToken(controlNumber, token))
        {
            ShowMessage("This link is invalid.", false);
            return;
        }

        if (!fileProof.HasFile)
        {
            ShowMessage("Please upload a proof of delivery image.", false);
            return;
        }

        var contentType = fileProof.PostedFile.ContentType;
        if (string.IsNullOrWhiteSpace(contentType) || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            ShowMessage("Proof of delivery must be an image file.", false);
            return;
        }

        if (fileProof.PostedFile.ContentLength > 5 * 1024 * 1024)
        {
            ShowMessage("Proof of delivery image must be 5 MB or smaller.", false);
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

            if (shipment.CurrentStatus == ShipmentStatus.Delivered || shipment.CurrentStatus == ShipmentStatus.Received)
            {
                ShowMessage("This shipment is already marked as delivered.", true);
                btnMarkDelivered.Visible = false;
                pnlProof.Visible = false;
                return;
            }

            if (shipment.CurrentStatus != ShipmentStatus.OutForDelivery)
            {
                ShowMessage("This shipment is not currently Out for Delivery.", false);
                return;
            }

            shipment.CurrentStatus = ShipmentStatus.Delivered;
            shipment.UpdatedDate = TimeHelper.Now;

            db.ShipmentHistories.Add(new ShipmentHistory
            {
                ShipmentId = shipment.Id,
                Status = ShipmentStatus.Delivered,
                Location = shipment.RecipientAddress,
                Notes = "",
                ProofOfDelivery = fileProof.FileBytes,
                ProofContentType = contentType
            });

            //Update rider status
            var rider = db.Riders.FirstOrDefault(e => e.RiderId == shipment.RiderId);
            if (rider != null)
            {
                rider.IsActive = true;
            }

            db.SaveChanges();
        }

        ShowMessage("Shipment marked as Delivered successfully.", true);
        btnMarkDelivered.Visible = false;
        pnlProof.Visible = false;
        LoadShipment();
    }

    private void LoadShipment()
    {
        var controlNumber = (Request.QueryString["controlNumber"] ?? string.Empty).Trim();
        var token = Request.QueryString["token"];

        if (string.IsNullOrWhiteSpace(controlNumber))
        {
            ShowMessage("Missing tracking number.", false);
            pnlDetails.Visible = false;
            return;
        }

        if (!string.IsNullOrWhiteSpace(token) && !EmailHelper.VerifyReceiptToken(controlNumber, token))
        {
            ShowMessage("This link is invalid.", false);
            pnlDetails.Visible = false;
            return;
        }

        using (var db = new ApplicationDbContext())
        {
            var shipment = db.Shipments.FirstOrDefault(s => s.ControlNumber == controlNumber);
            if (shipment == null)
            {
                ShowMessage("Shipment not found.", false);
                pnlDetails.Visible = false;
                return;
            }

            litControlNumber.Text = Server.HtmlEncode(shipment.ControlNumber);
            litRecipient.Text = Server.HtmlEncode(shipment.RecipientName);
            litAddress.Text = Server.HtmlEncode(shipment.RecipientAddress);
            litStatus.Text = StatusDisplayHelper.Badge(shipment.CurrentStatus, StatusDisplayHelper.IsNcrArea(shipment.RegionArea));
            litUpdated.Text = shipment.UpdatedDate.ToString("MMM d, yyyy h:mm tt");

            if (shipment.CurrentStatus == ShipmentStatus.Delivered || shipment.CurrentStatus == ShipmentStatus.Received)
            {
                ShowMessage("This shipment is already marked as delivered.", true);
                btnMarkDelivered.Visible = false;
                pnlProof.Visible = false;
            }
            else if (shipment.CurrentStatus == ShipmentStatus.OutForDelivery)
            {
                ShowMessage("Upload proof of delivery then click Mark as Delivered.", true);
                btnMarkDelivered.Visible = true;
                pnlProof.Visible = true;
            }
            else
            {
                ShowMessage("This shipment is not currently eligible for delivery confirmation.", false);
                btnMarkDelivered.Visible = false;
                pnlProof.Visible = false;
            }

            pnlDetails.Visible = true;
        }
    }

    private void ShowMessage(string message, bool success)
    {
        litMessage.Text = "<div class=\"" + (success ? "alert-success" : "validation-summary") + "\">" + Server.HtmlEncode(message) + "</div>";
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Mark Shipment as Delivered</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container container-md">
        <div class="card-panel">
            <div class="page-heading">Delivery Confirmation</div>
            <div class="page-subheading">Review shipment details and confirm delivery.</div>

            <asp:Literal ID="litMessage" runat="server" />

            <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                <div class="kv-grid">
                    <div class="kv"><div class="kv-label">Tracking Number</div><div class="kv-value"><asp:Literal ID="litControlNumber" runat="server" /></div></div>
                    <div class="kv"><div class="kv-label">Recipient</div><div class="kv-value"><asp:Literal ID="litRecipient" runat="server" /></div></div>
                    <div class="kv kv-wide"><div class="kv-label">Address</div><div class="kv-value"><asp:Literal ID="litAddress" runat="server" /></div></div>
                    <div class="kv"><div class="kv-label">Status</div><div class="kv-value"><asp:Literal ID="litStatus" runat="server" /></div></div>
                    <div class="kv"><div class="kv-label">Last Updated</div><div class="kv-value"><asp:Literal ID="litUpdated" runat="server" /></div></div>
                </div>

                <asp:Panel ID="pnlProof" runat="server">
                    <div class="form-group mt-16">
                        <label>Proof of Delivery Image</label>
                        <asp:FileUpload ID="fileProof" runat="server" CssClass="form-control" accept="image/*" />
                    </div>
                </asp:Panel>

                <div class="mt-16">
                    <asp:Button ID="btnMarkDelivered" runat="server" CssClass="btn btn-primary" Text="Mark as Delivered" OnClick="btnMarkDelivered_Click" OnClientClick="return confirm('Mark this shipment as delivered?');" />
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
</asp:Content>
