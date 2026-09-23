using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Views
{
    public partial class Contact : System.Web.UI.Page
    {
        // Where contact-form submissions are delivered. Change this to the
        // real inbox the team monitors.
        private const string RecipientEmail = "info@soonestglobal.com";

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
                SendContactEmail(name, email, phone, subject, message);

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

        private static void SendContactEmail(string name, string fromEmail, string phone, string subject, string message)
        {
            var senderUsername = ConfigurationManager.AppSettings["EmailSenderUsername"];
            var senderPassword = ConfigurationManager.AppSettings["EmailSenderPassword"];

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(senderUsername, "Soonest Global Express Website");
                mail.To.Add(RecipientEmail);
                mail.ReplyToList.Add(new MailAddress(fromEmail, name));
                mail.Subject = $"[Contact Form] {subject}";
                mail.IsBodyHtml = true;
                mail.Body = $@"
                    <div style='font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#1f2937;'>
                        <h2 style='color:#1360b5;'>New Contact Form Submission</h2>
                        <p><strong>Name:</strong> {HttpUtility.HtmlEncode(name)}</p>
                        <p><strong>Email:</strong> {HttpUtility.HtmlEncode(fromEmail)}</p>
                        <p><strong>Phone:</strong> {(string.IsNullOrEmpty(phone) ? "(not provided)" : HttpUtility.HtmlEncode(phone))}</p>
                        <p><strong>Subject:</strong> {HttpUtility.HtmlEncode(subject)}</p>
                        <p><strong>Message:</strong><br />{HttpUtility.HtmlEncode(message).Replace(Environment.NewLine, "<br />")}</p>
                    </div>";

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(senderUsername, senderPassword);
                    smtp.Send(mail);
                }
            }
        }
    }
}
