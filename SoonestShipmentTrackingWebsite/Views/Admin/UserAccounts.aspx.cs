using Microsoft.AspNet.Identity;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class UserAccounts : System.Web.UI.Page
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            var currentUserId = User.Identity.GetUserId();
            var roleNamesById = _db.Roles.ToDictionary(r => r.Id, r => r.Name);

            var rows = _db.Users
                .OrderBy(u => u.FullName)
                .ToList()
                .Select(u => new UserAccountRow
                {
                    Id = u.Id,
                    FullName = string.IsNullOrWhiteSpace(u.FullName) ? "(no name)" : u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    CurrentRole = u.Roles.Any()
                        ? string.Join(", ", u.Roles.Select(r => roleNamesById.ContainsKey(r.RoleId) ? roleNamesById[r.RoleId] : "Unknown"))
                        : "(none)",
                    IsCurrentUser = u.Id == currentUserId
                })
                .ToList();

            gvUsers.DataSource = rows;
            gvUsers.DataBind();
        }

        protected string GetRoleBadgeClass(string roleName)
        {
            switch (roleName)
            {
                case "Admin": return "badge-danger";
                case "Staff": return "badge-info";
                case "Customer": return "badge-success";
                default: return "badge-pending";
            }
        }
    }
}