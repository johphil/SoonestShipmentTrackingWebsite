using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Forbidden : System.Web.UI.Page
    {
        private IAuthenticationManager AuthenticationManager => Context.GetOwinContext().Authentication;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;

            bool isAuthenticated = Context.User != null && Context.User.Identity.IsAuthenticated;
            pnlLoggedIn.Visible = isAuthenticated;
            pnlLoggedOut.Visible = !isAuthenticated;
        }

        protected void btnLogOff_Click(object sender, EventArgs e)
        {
            AuthenticationManager.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("~/Views/Default.aspx");
        }
    }
}