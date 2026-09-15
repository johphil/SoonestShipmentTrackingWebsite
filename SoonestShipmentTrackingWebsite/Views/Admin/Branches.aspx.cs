using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class Branches : System.Web.UI.Page
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
            var branches = _db.Branches.OrderBy(b => b.BranchName).ToList();
            gvBranches.DataSource = branches;
            gvBranches.DataBind();
        }

        protected void gvBranches_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeleteBranch")
                return;

            int id = Convert.ToInt32(e.CommandArgument);
            var branch = _db.Branches.FirstOrDefault(b => b.Id == id);
            if (branch != null)
            {
                var name = branch.BranchName;
                _db.Branches.Remove(branch);
                _db.SaveChanges();
                Session["FlashMessage"] = $"Branch \"{name}\" was deleted.";
            }

            LoadData();
        }
    }
}