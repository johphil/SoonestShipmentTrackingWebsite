using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SoonestShipmentTrackingWebsite.App_Start;
using SoonestShipmentTrackingWebsite.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Site : MasterPage
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager =>
            _signInManager ?? (_signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>());

        private IAuthenticationManager AuthenticationManager => Context.GetOwinContext().Authentication;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isAuthenticated = Context.User != null && Context.User.Identity.IsAuthenticated;

            pnlLoggedIn.Visible = isAuthenticated;
            pnlLoggedOut.Visible = !isAuthenticated;

            if (isAuthenticated)
            {
                var isAdmin = Context.User.IsInRole("Admin");
                var isStaff = Context.User.IsInRole("Staff");
                var isRider = Context.User.IsInRole("Rider");
                var isCustomer = Context.User.IsInRole("Customer");
                var isAdminStaffShell = isAdmin || isStaff;

                litUserName.Text = HttpUtility.HtmlEncode(Context.User.Identity.Name);
                pnlCustomerNav.Visible = isCustomer;
                pnlStaffNav.Visible = isStaff || isRider || isAdmin;
                pnlDashboardNav.Visible = isStaff || isAdmin;
                pnlAdminNav.Visible = isAdmin;

                pnlAdminSidebar.Visible = isAdminStaffShell;
                pnlSidebarDashboard.Visible = isStaff || isAdmin;
                pnlSidebarAdminOnly.Visible = isAdmin;
                litSidebarUserName.Text = HttpUtility.HtmlEncode(Context.User.Identity.Name);

                if (isAdminStaffShell)
                    mainForm.Attributes["class"] = "admin-shell-mode";

                if (isCustomer || isRider)
                    LoadNotifications();
            }

            // One-shot flash message set by a previous page via Session
            // before a Response.Redirect (the Web Forms equivalent of
            // MVC's TempData).
            var message = Session["FlashMessage"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                litFlashMessage.Text = HttpUtility.HtmlEncode(message);
                pnlFlashMessage.Visible = true;
                Session["FlashMessage"] = null;
            }
        }

        private void LoadNotifications()
        {
            var userId = Context.User.Identity.GetUserId();
            var isRider = Context.User.IsInRole("Rider");
            var items = isRider
                ? NotificationHelper.GetRiderOutForDeliveryNotifications(userId)
                : NotificationHelper.GetNotifications(userId);
            int unread = NotificationHelper.UnreadCount(items);

            pnlCustomerNotif.Visible = true;
            notifBadge.Visible = unread > 0;
            litNotifCount.Text = unread > 9 ? "9+" : unread.ToString();
            lnkNotifViewAll.NavigateUrl = isRider ? "~/Views/Admin/Shipments.aspx" : "~/Views/Customer/MyShipments.aspx";

            rptNotifications.DataSource = items;
            rptNotifications.DataBind();
            lblNoNotif.Text = isRider ? "No out-for-delivery shipments assigned to you." : "No shipment updates yet.";
            lblNoNotif.Visible = !items.Any();
        }

        protected void btnLogOff_Click(object sender, EventArgs e)
        {
            AuthenticationManager.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect("~/Views/Default.aspx");
        }
    }
}