using SoonestShipmentTrackingWebsite.Helpers;
using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var name = txtName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var phone = txtPhone.Text.Trim();
            var subject = txtSubject.Text.Trim();
            var message = txtMessage.Text.Trim();

            try
            {
                // Where contact-form submissions are delivered.

                string adminEmails = "";

                using (var dbContext = new ApplicationDbContext())
                {

                    var adminRoleId = dbContext.Roles
                                            .Where(r => r.Name == "Admin")
                                            .Select(r => r.Id)
                                            .FirstOrDefault();

                    var adminList = dbContext.Users
                                            .Where(u => u.Roles.Any(ur => ur.RoleId == adminRoleId))
                                            .Select(uu => uu.Email)
                                            .ToList();

                    adminEmails = string.Join(",", adminList);
                }
                
        
                EmailHelper.SendInquiryEmail(adminEmails, name, email, phone, subject, message);

                pnlFormSuccess.Visible = true;
                pnlFormError.Visible = false;
                pnlContactForm.Visible = false;
            }
            catch (Exception)
            {
                // Don't leak SMTP/config details to the visitor — just let
                // them know it didn't go through and to try again or call.
                pnlFormError.Visible = true;
                litFormError.Text = "Sorry, we couldn't send your message right now. Please try again in a moment, or call us at +632 249 8970.";
            }
        }
    }
}
