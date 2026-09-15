using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SoonestShipmentTrackingWebsite.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class UserRoleEdit : System.Web.UI.Page
    {
        private static readonly string[] AssignableRoles = { "Admin", "Staff", "Customer" };

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager =>
            _userManager ?? (_userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (IsPostBack) return;

            var targetUserId = Request.QueryString["id"];
            if (string.IsNullOrEmpty(targetUserId))
            {
                Response.Redirect("~/Views/Admin/UserAccounts.aspx");
                return;
            }

            // Safety: don't let an admin change their own access level from
            // this page and risk locking themselves out of the admin area.
            if (targetUserId == User.Identity.GetUserId())
            {
                Session["FlashMessage"] = "You can't change your own access level from this page.";
                Response.Redirect("~/Views/Admin/UserAccounts.aspx");
                return;
            }

            var user = UserManager.FindById(targetUserId);
            if (user == null)
            {
                Response.Redirect("~/Views/Admin/UserAccounts.aspx");
                return;
            }

            hdnUserId.Value = user.Id;
            litUserName.Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(user.FullName) ? "(no name)" : user.FullName);
            litUserEmail.Text = HttpUtility.HtmlEncode(user.Email);

            var currentRoles = UserManager.GetRoles(user.Id);
            var currentAssignable = currentRoles.FirstOrDefault(r => AssignableRoles.Contains(r));
            if (currentAssignable != null && rblRole.Items.FindByValue(currentAssignable) != null)
            {
                rblRole.SelectedValue = currentAssignable;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var userId = hdnUserId.Value;
            if (string.IsNullOrEmpty(userId))
            {
                Response.Redirect("~/Views/Admin/UserAccounts.aspx");
                return;
            }

            var user = UserManager.FindById(userId);
            if (user == null)
            {
                Response.Redirect("~/Views/Admin/UserAccounts.aspx");
                return;
            }

            var newRole = rblRole.SelectedValue;

            // Remove any existing roles this user has among the assignable
            // set, then add exactly the one selected — keeps a user in
            // exactly one of Admin/Staff/Customer at a time.
            var currentRoles = UserManager.GetRoles(user.Id);
            foreach (var role in currentRoles.Where(r => AssignableRoles.Contains(r)))
            {
                UserManager.RemoveFromRole(user.Id, role);
            }
            UserManager.AddToRole(user.Id, newRole);

            var displayName = string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName;
            Session["FlashMessage"] = $"{displayName} is now set to {newRole}.";
            Response.Redirect("~/Views/Admin/UserAccounts.aspx");
        }
    }
}