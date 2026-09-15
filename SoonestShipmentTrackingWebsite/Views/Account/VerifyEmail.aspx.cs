using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SoonestShipmentTrackingWebsite.App_Start;
using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Account
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager =>
            _signInManager ?? (_signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>());

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager =>
            _userManager ?? (_userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var email = Request.QueryString["email"];

            if (string.IsNullOrWhiteSpace(email))
            {
                Response.Redirect("~/Views/Account/Register.aspx");
                return;
            }

            var user = UserManager.FindByEmail(email);
            if (user == null)
            {
                Response.Redirect("~/Views/Account/Register.aspx");
                return;
            }

            if (user.EmailConfirmed)
            {
                // Already verified — nothing to do here.
                Response.Redirect("~/Views/Account/Login.aspx");
                return;
            }

            hdnEmail.Value = email;
            litEmail.Text = HttpUtility.HtmlEncode(email);
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var user = UserManager.FindByEmail(hdnEmail.Value);
            if (user == null)
            {
                ShowError("We couldn't find that account. Please register again.");
                return;
            }

            var enteredCode = txtCode.Text.Trim();

            if (string.IsNullOrEmpty(user.EmailVerificationCode) ||
                !string.Equals(user.EmailVerificationCode, enteredCode, StringComparison.Ordinal))
            {
                ShowError("That code is incorrect. Please try again.");
                return;
            }

            if (!user.EmailVerificationCodeExpiresAt.HasValue ||
                user.EmailVerificationCodeExpiresAt.Value < DateTime.Now)
            {
                ShowError("This code has expired. Request a new one below.");
                return;
            }

            user.EmailConfirmed = true;
            user.EmailVerificationCode = null;
            user.EmailVerificationCodeExpiresAt = null;
            UserManager.Update(user);

            SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

            Session["FlashMessage"] = "Your email has been verified. Welcome to Soonest Global Express!";
            Response.Redirect("~/Views/Customer/MyShipments.aspx");
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            var user = UserManager.FindByEmail(hdnEmail.Value);
            if (user == null)
            {
                ShowError("We couldn't find that account. Please register again.");
                return;
            }

            user.EmailVerificationCode = VerificationCodeHelper.GenerateCode();
            user.EmailVerificationCodeExpiresAt = DateTime.Now.Add(VerificationCodeHelper.CodeLifetime);
            UserManager.Update(user);

            string redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/Account/VerifyEmail.aspx?email=" + HttpUtility.UrlEncode(user.Email));
            EmailHelper.SendEmailVerification(user.Email, user.FullName, user.EmailVerificationCode, user.EmailVerificationCodeExpiresAt, redirectUrl);

            pnlError.Visible = false;
            litInfo.Text = "A new verification code has been generated.";
            pnlInfo.Visible = true;
        }
        
        private void ShowError(string message)
        {
            litError.Text = HttpUtility.HtmlEncode(message);
            pnlError.Visible = true;
        }
    }
}