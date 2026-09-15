using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoonestShipmentTrackingWebsite.Views.Admin
{
    public partial class BranchEdit : System.Web.UI.Page
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private int? BranchId
        {
            get
            {
                if (int.TryParse(Request.QueryString["id"], out var id)) return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helpers.AccessControlHelper.EnsureRole(this, "Admin")) return;

            if (IsPostBack) return;

            if (BranchId.HasValue)
            {
                var branch = _db.Branches.FirstOrDefault(b => b.Id == BranchId.Value);
                if (branch == null)
                {
                    Response.Redirect("~/Admin/Branches.aspx");
                    return;
                }

                litHeading.Text = "Edit Branch";
                hdnId.Value = branch.Id.ToString();
                txtBranchName.Text = branch.BranchName;
                txtCity.Text = branch.City;
                txtAddress.Text = branch.Address;
                txtContactNumber.Text = branch.ContactNumber;
                txtEmail.Text = branch.Email;
                chkIsActive.Checked = branch.IsActive;
            }
            else
            {
                litHeading.Text = "Add Branch";
                chkIsActive.Checked = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            bool isNew = string.IsNullOrEmpty(hdnId.Value);
            Branch branch;

            if (isNew)
            {
                branch = new Branch();
            }
            else
            {
                var id = int.Parse(hdnId.Value);
                branch = _db.Branches.FirstOrDefault(b => b.Id == id);
                if (branch == null)
                {
                    Response.Redirect("~/Views/Admin/Branches.aspx");
                    return;
                }
            }

            branch.BranchName = txtBranchName.Text.Trim();
            branch.City = txtCity.Text.Trim();
            branch.Address = txtAddress.Text.Trim();
            branch.ContactNumber = txtContactNumber.Text.Trim();
            branch.Email = txtEmail.Text.Trim();
            branch.IsActive = chkIsActive.Checked;
            branch.UpdatedDate = DateTime.Now;

            if (isNew)
                _db.Branches.Add(branch);

            _db.SaveChanges();

            Session["FlashMessage"] = isNew
                ? $"Branch \"{branch.BranchName}\" was created."
                : $"Branch \"{branch.BranchName}\" was updated.";
            Response.Redirect("~/Views/Admin/Branches.aspx");
        }
    }
}