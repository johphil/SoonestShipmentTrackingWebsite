using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class AccessControlHelper
    {
        /// <summary>
        /// Anonymous visitors are sent to Login.aspx (with a ReturnUrl so
        /// they land back here after signing in); authenticated visitors
        /// who aren't in the required role are sent to Forbidden.aspx.
        /// Returns true if the page should keep executing, false if a
        /// redirect was already issued (the caller should "return"
        /// immediately in that case).
        /// </summary>
        public static bool EnsureRole(Page page, string requiredRole)
        {
            bool isAuthenticated = page.User != null && page.User.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                var returnUrl = HttpUtility.UrlEncode(page.Request.Url.PathAndQuery);
                page.Response.Redirect("~/Views/Account/Login.aspx?ReturnUrl=" + returnUrl);
                return false;
            }

            if (!page.User.IsInRole(requiredRole))
            {
                page.Response.Redirect("~/Views/Forbidden.aspx");
                return false;
            }

            return true;
        }
    }
}