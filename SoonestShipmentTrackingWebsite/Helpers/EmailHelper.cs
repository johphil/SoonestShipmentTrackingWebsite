using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using SoonestShipmentTrackingWebsite.Models;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class EmailHelper
    {
        private const string SenderAddress = "noreply@yourdomain.com";
        private const string SenderName = "Soonest Global Express";
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public static void SendShipmentCreatedEmail(Shipment shipment)
        {
            using (var mail = CreateMailMessage($"Shipment Created - {shipment.ControlNumber}", EmailFormatShipmentCreated(
                shipment.ControlNumber,
                shipment.Customer.FullName,
                shipment.RecipientName,
                shipment.BranchOrigin,
                shipment.RecipientAddress)))
            {
                mail.To.Add(shipment.Customer.Email);
                SendMail(mail, swallowErrors: true);
            }
        }

        public static void SendInquiryEmail(string adminEmails, string customerName, string customerEmail, string customerPhone, string subject, string message)
        {
            using (var mail = CreateMailMessage($"[Customer Inquiry] {subject}", EmailFormatInquiry(customerName, customerEmail, customerPhone, subject, message)))
            {
                mail.To.Add(adminEmails);

                if (!string.IsNullOrWhiteSpace(customerEmail))
                    mail.ReplyToList.Add(new MailAddress(customerEmail.Trim(), Safe(customerName)));

                SendMail(mail);
            }
        }

        public static void SendEmailVerification(string emailRecipient, string customerName, string verificationCode, DateTime? expirationDateTime, string redirectUrl)
        {
            using (var mail = CreateMailMessage(
                "Email Verification Required",
                EmailFormatAccountEmailVerification(customerName, verificationCode, expirationDateTime?.ToString("MMM dd, yyyy HH:mm tt") ?? "Soon", redirectUrl)))
            {
                mail.To.Add(emailRecipient);
                SendMail(mail);
            }
        }

        public static void SendShipmentUpdateEmail(Shipment shipment, string shipmentTrackUrl)
        {
            using (var mail = CreateMailMessage($"Shipment Status Update - {StatusDisplayHelper.Label(shipment.CurrentStatus)}", string.Empty))
            {
                AddRecipientIfNotEmpty(mail, shipment.RecipientEmail);
                AddRecipientIfNotEmpty(mail, shipment.Customer?.Email);

                var lastHistory = shipment.History
                    .OrderByDescending(e => e.Timestamp)
                    .FirstOrDefault();

                mail.Body = EmailFormatShipmentStatusUpdate(
                    shipment.Customer?.FullName,
                    shipment.ControlNumber,
                    StatusDisplayHelper.Label(shipment.CurrentStatus),
                    shipment.UpdatedDate.ToString("MMM dd, yyyy hh:mm tt"),
                    shipment.BranchOrigin,
                    shipment.RecipientAddress,
                    lastHistory?.Location,
                    shipmentTrackUrl,
                    lastHistory?.Notes,
                    shipment.RiderName);

                SendMail(mail);
            }
        }

        public static string GenerateReceiptToken(string controlNumber)
        {
            var secret = ConfigurationManager.AppSettings["ReceiptTokenSecret"]
                         ?? ConfigurationManager.AppSettings["EmailSenderPassword"] ?? "soonest";
            using (var h = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret)))
            {
                var hash = h.ComputeHash(System.Text.Encoding.UTF8.GetBytes("receipt:" + controlNumber));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifyReceiptToken(string controlNumber, string token)
        {
            return !string.IsNullOrEmpty(controlNumber) && !string.IsNullOrEmpty(token)
                   && string.Equals(GenerateReceiptToken(controlNumber), token, StringComparison.OrdinalIgnoreCase);
        }

        public static void SendOrderReceivedEmail(Shipment shipment, string orderReceivedUrl)
        {
            if (string.IsNullOrWhiteSpace(shipment.RecipientEmail)) return;

            using (var mail = CreateMailMessage(
                $"Your Shipment Was Delivered - {shipment.ControlNumber}",
                EmailFormatOrderReceived(
                    shipment.RecipientName,
                    shipment.ControlNumber,
                    shipment.RecipientAddress,
                    shipment.UpdatedDate.ToString("MMM dd, yyyy hh:mm tt"),
                    orderReceivedUrl)))
            {
                mail.To.Add(shipment.RecipientEmail);
                SendMail(mail);
            }
        }

        public static void SendShipmentRequestReceivedEmail(string customerEmail, string customerName, string controlNumber, string recipientName, string branchOrigin)
        {
            if (string.IsNullOrWhiteSpace(customerEmail)) return;

            using (var mail = CreateMailMessage(
                $"Shipment Request Received - {controlNumber}",
                EmailFormatShipmentRequestReceived(customerName, controlNumber, recipientName, branchOrigin)))
            {
                mail.To.Add(customerEmail.Trim());
                SendMail(mail, swallowErrors: true);
            }
        }

        private static MailMessage CreateMailMessage(string subject, string body)
        {
            return new MailMessage
            {
                From = new MailAddress(SenderAddress, SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
        }

        private static void AddRecipientIfNotEmpty(MailMessage mail, string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            var normalized = email.Trim();
            if (mail.To.All(t => !string.Equals(t.Address, normalized, StringComparison.OrdinalIgnoreCase)))
                mail.To.Add(normalized);
        }

        private static void SendMail(MailMessage mail, bool swallowErrors = false)
        {
            using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = GetEmailCredentials();

                if (swallowErrors)
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch
                    {
                    }
                    return;
                }

                smtp.Send(mail);
            }
        }

        private static NetworkCredential GetEmailCredentials()
        {
            var username = ConfigurationManager.AppSettings["EmailSenderUsername"];
            var password = ConfigurationManager.AppSettings["EmailSenderPassword"];
            return new NetworkCredential(username, password);
        }

        private static string EmailFormatOrderReceived(string recipientName, string controlNumber, string address, string deliveredOn, string url)
        {
            var content = $@"
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border:1px solid #e4ebf2;border-radius:10px;overflow:hidden;"">
    <tr><td style=""padding:14px 16px;background:#f7fafd;font-size:12px;color:#557;letter-spacing:.5px;"">TRACKING NUMBER</td></tr>
    <tr><td style=""padding:14px 16px;font-size:24px;color:#07233d;font-weight:800;letter-spacing:.6px;"">{Encode(controlNumber)}</td></tr>
</table>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin-top:16px;border-collapse:collapse;"">
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;"">Delivered To</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;"">{Encode(address)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Delivered On</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(deliveredOn)}</td></tr>
</table>

<div style=""margin-top:16px;padding:12px 14px;background:#e9f7ef;color:#177245;border-radius:8px;font-size:13px;font-weight:600;"">Your shipment has been delivered. Please confirm that you received it.</div>

<div style=""text-align:center;margin-top:22px;"">
    <a href=""{EncodeUrl(url)}"" style=""display:inline-block;padding:13px 30px;background:#e63946;color:#ffffff;text-decoration:none;border-radius:8px;font-weight:700;font-size:15px;"">Order Received</a>
</div>";

            return BuildEmailTemplate(
                "Shipment Delivered",
                $"Hello {Safe(recipientName)}, your shipment has been delivered. Click the button below once you have received your order.",
                content);
        }

        private static string EmailFormatShipmentCreated(string controlNumber, string customerName, string recipient, string origin, string destination)
        {
            var content = $@"
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border:1px solid #e4ebf2;border-radius:10px;overflow:hidden;"">
    <tr><td style=""padding:14px 16px;background:#f7fafd;font-size:12px;color:#557;letter-spacing:.5px;"">TRACKING NUMBER</td></tr>
    <tr><td style=""padding:14px 16px;font-size:24px;color:#07233d;font-weight:800;letter-spacing:.6px;"">{Encode(controlNumber)}</td></tr>
</table>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin-top:16px;border-collapse:collapse;"">
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;"">Customer</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;"">{Encode(customerName)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Recipient</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(recipient)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Origin</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(origin)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Destination</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(destination)}</td></tr>
</table>

<div style=""margin-top:16px;padding:12px 14px;background:#e9f7ef;color:#177245;border-radius:8px;font-size:13px;font-weight:600;"">Shipment created successfully.</div>";

            return BuildEmailTemplate(
                "Shipment Created",
                $"Hello {Safe(customerName)}, your shipment has been created. Keep your tracking number for updates.",
                content);
        }

        private static string EmailFormatInquiry(string customerName, string customerEmail, string customerPhone, string subject, string message)
        {
            var content = $@"
<div style=""padding:14px 16px;background:#f7fafd;border:1px solid #e4ebf2;border-radius:10px;"">
    <div style=""font-size:12px;color:#60758a;letter-spacing:.5px;"">NEW CUSTOMER INQUIRY</div>
    <div style=""font-size:22px;color:#07233d;font-weight:800;margin-top:6px;"">{Encode(subject)}</div>
</div>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin-top:16px;border-collapse:collapse;"">
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;"">Customer Name</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;"">{Encode(Safe(customerName))}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Email</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(customerEmail)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Phone</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(string.IsNullOrWhiteSpace(customerPhone) ? "(not provided)" : customerPhone)}</td></tr>
</table>

<div style=""margin-top:16px;padding:12px 14px;border-left:4px solid #1e7fd6;background:#f1f7ff;color:#3f566c;font-size:13px;line-height:1.6;border-radius:6px;white-space:pre-line;"">{Encode(message)}</div>";

            return BuildEmailTemplate(
                "Customer Inquiry",
                $"A customer submitted a new inquiry through the website contact form.",
                content);
        }

        private static string EmailFormatAccountEmailVerification(string customerName, string verificationCode, string expirationDateTime, string redirectUrl)
        {
            var content = $@"
<div style=""padding:16px;border:1px solid #e4ebf2;border-radius:10px;background:#f7fafd;"">
    <div style=""font-size:12px;color:#60758a;letter-spacing:.5px;"">VERIFICATION CODE</div>
    <div style=""font-size:30px;letter-spacing:7px;color:#07233d;font-weight:800;margin-top:6px;"">{Encode(verificationCode)}</div>
    <div style=""font-size:12px;color:#6a7d91;margin-top:8px;"">Expires on {Encode(expirationDateTime)}</div>
</div>

<div style=""margin-top:18px;text-align:center;"">
    <a href=""{EncodeUrl(redirectUrl)}"" style=""display:inline-block;background:#f5a623;color:#041627;text-decoration:none;font-size:14px;font-weight:800;padding:11px 22px;border-radius:8px;"">Verify My Account</a>
</div>

<div style=""margin-top:16px;font-size:12px;color:#60758a;line-height:1.7;"">
    If the button does not work, open this link:<br>
    <a href=""{EncodeUrl(redirectUrl)}"" style=""color:#1e7fd6;word-break:break-all;"">{Encode(redirectUrl)}</a>
</div>";

            return BuildEmailTemplate(
                "Verify Your Account",
                $"Hello {Safe(customerName)}, use the code below to verify your email and activate your account.",
                content);
        }

        private static string EmailFormatShipmentStatusUpdate(string customerName, string controlNumber, string shipmentStatus, string statusDateTime, string origin, string destination, string currentLocation, string shipmentTrackUrl, string statusMessage, string riderName)
        {
            var isOutForDelivery = shipmentStatus.Equals("Out for Delivery", StringComparison.OrdinalIgnoreCase);
            var riderRow = isOutForDelivery
                ? $"<tr><td style=\"padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;\">Rider</td><td align=\"right\" style=\"padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;border-top:1px solid #eef3f8;\">{Encode(string.IsNullOrWhiteSpace(riderName) ? "Waiting for rider assignment" : riderName)}</td></tr>"
                : string.Empty;

            var content = $@"
<div style=""padding:14px 16px;background:#f7fafd;border:1px solid #e4ebf2;border-radius:10px;"">
    <div style=""font-size:12px;color:#60758a;letter-spacing:.5px;"">CURRENT STATUS</div>
    <div style=""font-size:23px;color:#07233d;font-weight:800;margin-top:6px;"">{Encode(shipmentStatus)}</div>
    <div style=""font-size:12px;color:#6a7d91;margin-top:8px;"">Updated on {Encode(statusDateTime)}</div>
</div>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin-top:16px;border-collapse:collapse;"">
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;"">Tracking Number</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:700;font-size:13px;"">{Encode(controlNumber)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Origin</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(origin)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Destination</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(destination)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Latest Location</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(string.IsNullOrWhiteSpace(currentLocation) ? "-" : currentLocation)}</td></tr>
    {riderRow}
</table>

<div style=""margin-top:16px;padding:12px 14px;border-left:4px solid #1e7fd6;background:#f1f7ff;color:#3f566c;font-size:13px;line-height:1.6;border-radius:6px;"">
    <strong style=""color:#07233d;"">Latest Update:</strong><br>{Encode(string.IsNullOrWhiteSpace(statusMessage) ? "No additional remarks." : statusMessage)}
</div>

<div style=""margin-top:18px;text-align:center;"">
    <a href=""{EncodeUrl(shipmentTrackUrl)}"" style=""display:inline-block;background:#f5a623;color:#041627;text-decoration:none;font-size:14px;font-weight:800;padding:11px 22px;border-radius:8px;"">Track My Shipment</a>
</div>

<div style=""margin-top:14px;font-size:12px;color:#60758a;line-height:1.7;"">
    Tracking link: <a href=""{EncodeUrl(shipmentTrackUrl)}"" style=""color:#1e7fd6;word-break:break-all;"">{Encode(shipmentTrackUrl)}</a>
</div>";

            return BuildEmailTemplate(
                "Shipment Status Update",
                $"Hello {Safe(customerName)}, your shipment has a new update.",
                content);
        }

        private static string EmailFormatShipmentRequestReceived(string customerName, string controlNumber, string recipientName, string branchOrigin)
        {
            var content = $@"
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""border:1px solid #e4ebf2;border-radius:10px;overflow:hidden;"">
    <tr><td style=""padding:14px 16px;background:#f7fafd;font-size:12px;color:#557;letter-spacing:.5px;"">REQUEST REFERENCE</td></tr>
    <tr><td style=""padding:14px 16px;font-size:24px;color:#07233d;font-weight:800;letter-spacing:.6px;"">{Encode(controlNumber)}</td></tr>
</table>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin-top:16px;border-collapse:collapse;"">
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;"">Recipient</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-weight:600;font-size:13px;"">{Encode(recipientName)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Branch Origin</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">{Encode(branchOrigin)}</td></tr>
    <tr><td style=""padding:9px 0;color:#6a7d91;font-size:13px;border-top:1px solid #eef3f8;"">Status</td><td align=""right"" style=""padding:9px 0;color:#1b2d3d;font-size:13px;border-top:1px solid #eef3f8;"">Pending Review</td></tr>
</table>

<div style=""margin-top:16px;padding:12px 14px;background:#fff4de;color:#8a5a08;border-radius:8px;font-size:13px;font-weight:600;"">Your shipment request is subject to review and approval by our staff. We will notify you once it is approved.</div>";

            return BuildEmailTemplate(
                "Shipment Request Submitted",
                $"Hello {Safe(customerName)}, we received your shipment request.",
                content);
        }

        private static string BuildEmailTemplate(string title, string introText, string contentHtml)
        {
            return $@"<!DOCTYPE html>
<html lang=""en""><head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<title>{Encode(title)}</title>
</head>
<body style=""margin:0;padding:0;background:#f3f6fa;font-family:Segoe UI,Arial,sans-serif;color:#1b2d3d;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""padding:26px 10px;background:#f3f6fa;"">
<tr><td align=""center""><table width=""640"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""max-width:640px;width:100%;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e4ebf2;"">
<tr><td style=""background:#07233d;padding:24px 28px;""><div style=""color:#fff;font-size:22px;font-weight:800;letter-spacing:.3px;"">SOONEST GLOBAL EXPRESS</div><div style=""color:#bcd0e4;font-size:13px;margin-top:6px;"">Fast. Reliable. Delivered.</div></td></tr>
<tr><td style=""padding:30px 28px 18px;""><h2 style=""margin:0 0 12px;color:#07233d;font-size:24px;"">{Encode(title)}</h2><p style=""margin:0;color:#4f6479;font-size:14px;line-height:1.65;"">{Encode(introText)}</p></td></tr>
<tr><td style=""padding:0 28px 28px;"">{contentHtml}</td></tr>
<tr><td style=""background:#f6f9fc;border-top:1px solid #e4ebf2;padding:18px 28px;text-align:center;""><div style=""font-size:11px;color:#73879b;line-height:1.6;"">This is an automated message from Soonest Global Express. Please do not reply to this email.</div><div style=""font-size:11px;color:#9ab;margin-top:8px;"">© 2026 Soonest Global Express</div></td></tr>
</table></td></tr></table>
</body></html>";
        }

        private static string Encode(string text)
        {
            return HttpUtility.HtmlEncode(text ?? string.Empty);
        }

        private static string EncodeUrl(string url)
        {
            return HttpUtility.HtmlAttributeEncode(string.IsNullOrWhiteSpace(url) ? "#" : url);
        }

        private static string Safe(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "Customer" : text.Trim();
        }
    }
}