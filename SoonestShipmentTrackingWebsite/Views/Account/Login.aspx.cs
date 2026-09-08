using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoonestShipmentTrackingWebsite.App_Start;

namespace SoonestShipmentTrackingWebsite.Views.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager =>
            _signInManager ?? (_signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>());

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager =>
            _userManager ?? (_userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>());

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var result = SignInManager.PasswordSignIn(
                txtEmail.Text.Trim(), txtPassword.Text, chkRememberMe.Checked, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    RedirectAfterLogin(txtEmail.Text.Trim());
                    return;
                case SignInStatus.LockedOut:
                    ShowError("This account has been locked out due to multiple failed login attempts. Please try again later.");
                    return;
                default:
                    ShowError("Invalid login attempt.");
                    return;
            }
        }

        private void RedirectAfterLogin(string email)
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                Response.Redirect(returnUrl);
                return;
            }

            var user = UserManager.FindByEmail(email);
            if (user != null && UserManager.IsInRole(user.Id, "Admin"))
            {
                Response.Redirect("~/Views/Admin/Dashboard.aspx");
                return;
            }

            Response.Redirect("~/Views/Customer/MyShipments.aspx");
        }

        // Guards against open-redirect attacks: only allow root-relative
        // paths ("/something"), not scheme-relative ("//evil.com") or
        // absolute URLs.
        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url)
                && url.StartsWith("/", StringComparison.Ordinal)
                && !url.StartsWith("//", StringComparison.Ordinal)
                && !url.StartsWith("/\\", StringComparison.Ordinal);
        }

        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}