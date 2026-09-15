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
    public partial class Register : System.Web.UI.Page
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager =>
            _signInManager ?? (_signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>());

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager =>
            _userManager ?? (_userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    Response.Redirect("~/Views/Admin/Dashboard.aspx");
                    return;
                }
                Response.Redirect("~/Views/Customer/MyShipments.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var user = new ApplicationUser
            {
                UserName = txtEmail.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Address = txtAddress.Text.Trim()
            };

            var result = UserManager.Create(user, txtPassword.Text);

            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Customer");

                // Don't sign the user in yet — send them to the code-entry
                // page first. EmailConfirmed stays false until they verify.
                user.EmailVerificationCode = VerificationCodeHelper.GenerateCode();
                user.EmailVerificationCodeExpiresAt = DateTime.UtcNow.Add(VerificationCodeHelper.CodeLifetime);
                UserManager.Update(user);

                string redirectUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/Views/Account/VerifyEmail.aspx?email=" + HttpUtility.UrlEncode(user.Email));
                EmailHelper.SendEmailVerification(user.Email, user.FullName, user.EmailVerificationCode, user.EmailVerificationCodeExpiresAt, redirectUrl);

                Response.Redirect("~/Views/Account/VerifyEmail.aspx?email=" + HttpUtility.UrlEncode(user.Email));
                return;
            }

            litError.Text = HttpUtility.HtmlEncode(string.Join(" ", result.Errors));
            pnlError.Visible = true;
        }
    }
}