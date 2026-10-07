using SoonestShipmentTrackingWebsite.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Staff"))
                {
                    Response.Redirect("~/Views/Admin/Dashboard.aspx");
                    return;
                }
            }
        }

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            var controlNumber = (txtControlNumber.Text ?? "").Trim();
            Response.Redirect("Track.aspx?controlNumber=" + HttpUtility.UrlEncode(controlNumber));
        }
    }
}