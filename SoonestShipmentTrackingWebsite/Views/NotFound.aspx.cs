using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class NotFound : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Preserve the real 404 status for the HTTP response even
            // though we're rendering a friendly page (see the customErrors
            // / httpErrors config in Web.config, which routes here with
            // ResponseRewrite / ExecuteURL so the status code isn't
            // overwritten to 200).
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
        }
    }
}