using SoonestShipmentTrackingWebsite.Models;
using SoonestShipmentTrackingWebsite.Views.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class EmailHelper
    {
        private static string _emailSenderUsername;
        private static string _emailSenderPassword;

        public static void SendShipmentCreatedEmail(Shipment shipment)
        {
            _emailSenderUsername = ConfigurationManager.AppSettings["EmailSenderUsername"];
            _emailSenderPassword = ConfigurationManager.AppSettings["EmailSenderPassword"];

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(
                    "noreply@yourdomain.com",
                    "Soonest Global Express"
                );

                mail.To.Add(shipment.Customer.Email);
                mail.Subject = $"Shipment Created - {shipment.ControlNumber}";
                mail.Body = EmailFormatShipmentCreated(shipment.ControlNumber, shipment.Customer.FullName, shipment.RecipientName, shipment.SenderAddress, shipment.RecipientAddress);
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSenderUsername, _emailSenderPassword);

                    smtp.Send(mail);
                }
            }
        }

        public static void SendEmailVerification(string emailRecipient, string customerName, string verificationCode, DateTime? expirationDateTime, string redirectUrl)
        {
            _emailSenderUsername = ConfigurationManager.AppSettings["EmailSenderUsername"];
            _emailSenderPassword = ConfigurationManager.AppSettings["EmailSenderPassword"];

            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(
                    "noreply@yourdomain.com",
                    "Soonest Global Express"
                );

                mail.To.Add(emailRecipient);
                mail.Subject = $"Email Verification Required";
                mail.Body = EmailFormatAccountEmailVerification(customerName, verificationCode, expirationDateTime?.ToString("MMM dd, yyyy HH:mm tt"), redirectUrl);
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSenderUsername, _emailSenderPassword);

                    smtp.Send(mail);
                }
            }
        }

        public static void SendShipmentUpdateEmail(Shipment shipment, string shipmentTrackUrl)
        {
            _emailSenderUsername = ConfigurationManager.AppSettings["EmailSenderUsername"];
            _emailSenderPassword = ConfigurationManager.AppSettings["EmailSenderPassword"];
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(
                    "noreply@yourdomain.com",
                    "Soonest Global Express"
                );

                mail.To.Add(shipment.RecipientEmail);
                mail.To.Add(shipment.Customer.Email);
                mail.Subject = $"Shipment Status Update - {StatusDisplayHelper.Label(shipment.CurrentStatus)}";
                var lastHistory = shipment.History.OrderByDescending(e => e.Timestamp).First();
                mail.Body = EmailFormatShipmentStatusUpdate(shipment.Customer.FullName, shipment.ControlNumber, StatusDisplayHelper.Label(shipment.CurrentStatus), shipment.UpdatedDate.ToString(), shipment.SenderAddress, shipment.RecipientAddress, lastHistory.Location, shipmentTrackUrl, lastHistory.Notes);
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSenderUsername, _emailSenderPassword);

                    smtp.Send(mail);
                }
            }
        }

        private static string EmailFormatShipmentCreated(string controlNumber, string customerName, string recipient, string origin, string destination)
        {
            return $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Shipment Created</title>
                    </head>

                    <body style=""
                        margin:0;
                        padding:0;
                        background-color:#f4f6f8;
                        font-family:Arial, Helvetica, sans-serif;
                        color:#333333;
                    "">

                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                               style=""background-color:#f4f6f8; padding:30px 10px;"">

                            <tr>
                                <td align=""center"">

                                    <!-- Main Container -->
                                    <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                           style=""
                                               max-width:600px;
                                               width:100%;
                                               background:#ffffff;
                                               border-radius:10px;
                                               overflow:hidden;
                                           "">

                                        <!-- Header -->
                                        <tr>
                                            <td align=""center""
                                                style=""
                                                    background-color:#0b2540;
                                                    padding:28px 20px;
                                                "">

                                                <div style=""
                                                    font-size:24px;
                                                    font-weight:bold;
                                                    color:#ffffff;
                                                    letter-spacing:1px;
                                                "">
                                                    SOONEST GLOBAL EXPRESS
                                                </div>

                                                <div style=""
                                                    color:#dce5ee;
                                                    font-size:13px;
                                                    margin-top:6px;
                                                "">
                                                    Fast. Reliable. Delivered.
                                                </div>

                                            </td>
                                        </tr>

                                        <!-- Content -->
                                        <tr>
                                            <td style=""padding:35px 35px 20px 35px;"">

                                                <h1 style=""
                                                    margin:0 0 15px 0;
                                                    color:#0b2540;
                                                    font-size:24px;
                                                "">
                                                    Shipment Created
                                                </h1>

                                                <p style=""
                                                    margin:0 0 20px 0;
                                                    font-size:15px;
                                                    line-height:1.6;
                                                    color:#555555;
                                                "">
                                                    Hello <strong>{customerName}</strong>,
                                                </p>

                                                <p style=""
                                                    margin:0 0 20px 0;
                                                    font-size:15px;
                                                    line-height:1.6;
                                                    color:#555555;
                                                "">
                                                    Your shipment has been successfully created with
                                                    <strong>Soonest Global Express</strong>.
                                                    Please keep the tracking number below for future
                                                    reference.
                                                </p>

                                                <!-- Shipment Details -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0""
                                                       style=""
                                                           background-color:#f5f7fa;
                                                           border-radius:8px;
                                                           margin:25px 0;
                                                       "">

                                                    <tr>
                                                        <td style=""
                                                            padding:20px;
                                                            border-bottom:1px solid #e1e5e9;
                                                        "">
                                                            <div style=""
                                                                font-size:12px;
                                                                color:#777777;
                                                                margin-bottom:5px;
                                                            "">
                                                                TRACKING NUMBER
                                                            </div>

                                                            <div style=""
                                                                font-size:20px;
                                                                font-weight:bold;
                                                                color:#0b2540;
                                                                letter-spacing:1px;
                                                            "">
                                                                {controlNumber}
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style=""
                                                            padding:20px;
                                                        "">
                                                            <table width=""100%"" cellpadding=""0""
                                                                   cellspacing=""0"" border=""0"">

                                                                <tr>
                                                                    <td width=""50%""
                                                                        style=""vertical-align:top;"">

                                                                        <div style=""
                                                                            font-size:12px;
                                                                            color:#777777;
                                                                            margin-bottom:5px;
                                                                        "">
                                                                            SHIPPER
                                                                        </div>

                                                                        <div style=""
                                                                            font-size:14px;
                                                                            font-weight:bold;
                                                                            color:#333333;
                                                                        "">
                                                                            {customerName}
                                                                        </div>

                                                                    </td>

                                                                    <td width=""50%""
                                                                        style=""vertical-align:top;"">

                                                                        <div style=""
                                                                            font-size:12px;
                                                                            color:#777777;
                                                                            margin-bottom:5px;
                                                                        "">
                                                                            RECIPIENT
                                                                        </div>

                                                                        <div style=""
                                                                            font-size:14px;
                                                                            font-weight:bold;
                                                                            color:#333333;
                                                                        "">
                                                                            {recipient}
                                                                        </div>

                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style=""
                                                            padding:0 20px 20px 20px;
                                                        "">

                                                            <table width=""100%"" cellpadding=""0""
                                                                   cellspacing=""0"" border=""0"">

                                                                <tr>

                                                                    <td width=""50%""
                                                                        style=""vertical-align:top;"">

                                                                        <div style=""
                                                                            font-size:12px;
                                                                            color:#777777;
                                                                            margin-bottom:5px;
                                                                        "">
                                                                            ORIGIN
                                                                        </div>

                                                                        <div style=""
                                                                            font-size:14px;
                                                                            color:#333333;
                                                                        "">
                                                                            {origin}
                                                                        </div>

                                                                    </td>

                                                                    <td width=""50%""
                                                                        style=""vertical-align:top;"">

                                                                        <div style=""
                                                                            font-size:12px;
                                                                            color:#777777;
                                                                            margin-bottom:5px;
                                                                        "">
                                                                            DESTINATION
                                                                        </div>

                                                                        <div style=""
                                                                            font-size:14px;
                                                                            color:#333333;
                                                                        "">
                                                                            {destination}
                                                                        </div>

                                                                    </td>

                                                                </tr>

                                                            </table>

                                                        </td>
                                                    </tr>

                                                </table>

                                                <!-- Status -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0""
                                                       style=""margin:20px 0;"">

                                                    <tr>
                                                        <td align=""center"">

                                                            <div style=""
                                                                display:inline-block;
                                                                background-color:#e8f5e9;
                                                                color:#2e7d32;
                                                                padding:10px 20px;
                                                                border-radius:20px;
                                                                font-size:13px;
                                                                font-weight:bold;
                                                            "">
                                                                ● SHIPMENT CREATED
                                                            </div>

                                                        </td>
                                                    </tr>

                                                </table>

                                                <p style=""
                                                    margin:25px 0;
                                                    font-size:14px;
                                                    line-height:1.6;
                                                    color:#666666;
                                                    text-align:center;
                                                "">
                                                    You can track your shipment anytime using your
                                                    tracking number.
                                                </p>

                                                <!-- Track Button -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0"">

                                                    <tr>
                                                        <td align=""center"">

                                                            <a href=""https://www.google.com""
                                                               style=""
                                                                   display:inline-block;
                                                                   background-color:#d62828;
                                                                   color:#ffffff;
                                                                   text-decoration:none;
                                                                   font-size:14px;
                                                                   font-weight:bold;
                                                                   padding:14px 30px;
                                                                   border-radius:6px;
                                                               "">
                                                                Track My Shipment
                                                            </a>

                                                        </td>
                                                    </tr>

                                                </table>

                                            </td>
                                        </tr>

                                        <!-- Note -->
                                        <tr>
                                            <td style=""
                                                padding:0 35px 30px 35px;
                                            "">

                                                <div style=""
                                                    border-top:1px solid #eeeeee;
                                                    padding-top:20px;
                                                    font-size:12px;
                                                    line-height:1.6;
                                                    color:#888888;
                                                "">
                                                    Please keep your tracking number safe.
                                                    You may be asked for this number when
                                                    contacting Soonest Global Express regarding
                                                    your shipment.
                                                </div>

                                            </td>
                                        </tr>

                                        <!-- Footer -->
                                        <tr>
                                            <td align=""center""
                                                style=""
                                                    background-color:#f1f3f5;
                                                    padding:25px 20px;
                                                "">

                                                <div style=""
                                                    font-size:13px;
                                                    font-weight:bold;
                                                    color:#0b2540;
                                                    margin-bottom:8px;
                                                "">
                                                    SOONEST GLOBAL EXPRESS
                                                </div>

                                                <div style=""
                                                    font-size:11px;
                                                    color:#888888;
                                                    line-height:1.5;
                                                "">
                                                    This is an automated notification.
                                                    Please do not reply to this email.
                                                </div>

                                                <div style=""
                                                    font-size:11px;
                                                    color:#aaaaaa;
                                                    margin-top:10px;
                                                "">
                                                    © 2026 Soonest Global Express. All rights reserved.
                                                </div>

                                            </td>
                                        </tr>

                                    </table>

                                </td>
                            </tr>

                        </table>

                    </body>
                    </html>";
        }

        private static string EmailFormatAccountEmailVerification(string customerName, string verificationCode, string expirationDateTime, string redirectUrl)
        {
            return $@"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Verify Your Account</title>
                    </head>

                    <body style=""
                        margin:0;
                        padding:0;
                        background-color:#f4f6f8;
                        font-family:Arial, Helvetica, sans-serif;
                        color:#333333;
                    "">

                        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                               style=""background-color:#f4f6f8; padding:30px 10px;"">

                            <tr>
                                <td align=""center"">

                                    <!-- Main Container -->
                                    <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                           style=""
                                               max-width:600px;
                                               width:100%;
                                               background:#ffffff;
                                               border-radius:10px;
                                               overflow:hidden;
                                           "">

                                        <!-- Header -->
                                        <tr>
                                            <td align=""center""
                                                style=""
                                                    background-color:#0b2540;
                                                    padding:28px 20px;
                                                "">

                                                <div style=""
                                                    font-size:24px;
                                                    font-weight:bold;
                                                    color:#ffffff;
                                                    letter-spacing:1px;
                                                "">
                                                    SOONEST GLOBAL EXPRESS
                                                </div>

                                                <div style=""
                                                    color:#dce5ee;
                                                    font-size:13px;
                                                    margin-top:6px;
                                                "">
                                                    Fast. Reliable. Delivered.
                                                </div>

                                            </td>
                                        </tr>

                                        <!-- Content -->
                                        <tr>
                                            <td style=""padding:40px 35px 35px 35px;"">

                                                <h1 style=""
                                                    margin:0 0 18px 0;
                                                    color:#0b2540;
                                                    font-size:25px;
                                                    text-align:center;
                                                "">
                                                    Verify Your Account
                                                </h1>

                                                <p style=""
                                                    margin:0 0 18px 0;
                                                    font-size:15px;
                                                    line-height:1.6;
                                                    color:#555555;
                                                "">
                                                    Hello <strong>{customerName}</strong>,
                                                </p>

                                                <p style=""
                                                    margin:0 0 20px 0;
                                                    font-size:15px;
                                                    line-height:1.6;
                                                    color:#555555;
                                                "">
                                                    Thank you for creating an account with
                                                    <strong>Soonest Global Express</strong>.
                                                </p>

                                                <p style=""
                                                    margin:0 0 25px 0;
                                                    font-size:15px;
                                                    line-height:1.6;
                                                    color:#555555;
                                                "">
                                                    To complete your registration and activate your
                                                    account, please verify your email address by
                                                    clicking the button below.
                                                </p>

                                                <!-- Verification Button -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0"">

                                                    <tr>
                                                        <td align=""center"" style=""padding:10px 0 30px 0;"">

                                                            <a href=""{redirectUrl}""
                                                               style=""
                                                                   display:inline-block;
                                                                   background-color:#d62828;
                                                                   color:#ffffff;
                                                                   text-decoration:none;
                                                                   font-size:15px;
                                                                   font-weight:bold;
                                                                   padding:14px 35px;
                                                                   border-radius:6px;
                                                               "">
                                                                Verify My Account
                                                            </a>    
                                                        </td>
                                                    </tr>

                                                </table>

                                                <!-- Verification Code -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0""
                                                       style=""
                                                           background-color:#f5f7fa;
                                                           border-radius:8px;
                                                           margin-bottom:25px;
                                                       "">

                                                    <tr>
                                                        <td align=""center"" style=""padding:22px;"">

                                                            <div style=""
                                                                font-size:12px;
                                                                color:#777777;
                                                                margin-bottom:8px;
                                                                letter-spacing:.5px;
                                                            "">
                                                                VERIFICATION CODE
                                                            </div>

                                                            <div style=""
                                                                font-size:28px;
                                                                font-weight:bold;
                                                                letter-spacing:6px;
                                                                color:#0b2540;
                                                            "">
                                                                {verificationCode}
                                                            </div>

                                                            <div style=""
                                                                font-size:12px;
                                                                color:#888888;
                                                                margin-top:10px;
                                                            "">
                                                                This code will expire on {expirationDateTime}.
                                                            </div>

                                                        </td>
                                                    </tr>

                                                </table>

                                                <p style=""
                                                    margin:0 0 20px 0;
                                                    font-size:13px;
                                                    line-height:1.6;
                                                    color:#777777;
                                                "">
                                                    If the button above doesn't work, copy and paste
                                                    the following link into your browser:
                                                </p>

                                                <p style=""
                                                    margin:0 0 25px 0;
                                                    font-size:12px;
                                                    line-height:1.6;
                                                    word-break:break-all;
                                                "">
                                                    <a href=""{redirectUrl}""
                                                       style=""color:#0b2540;"">
                                                        {redirectUrl}
                                                    </a>
                                                </p>

                                                <!-- Security Notice -->
                                                <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                       border=""0""
                                                       style=""
                                                           border-left:4px solid #d62828;
                                                           background-color:#fff8f8;
                                                       "">

                                                    <tr>
                                                        <td style=""
                                                            padding:15px 18px;
                                                            font-size:12px;
                                                            line-height:1.6;
                                                            color:#666666;
                                                        "">
                                                            <strong style=""color:#0b2540;"">
                                                                Didn't create this account?
                                                            </strong>
                                                            <br>
                                                            You can safely ignore this email.
                                                            No changes will be made to your account.
                                                        </td>
                                                    </tr>

                                                </table>

                                            </td>
                                        </tr>

                                        <!-- Footer -->
                                        <tr>
                                            <td align=""center""
                                                style=""
                                                    background-color:#f1f3f5;
                                                    padding:25px 20px;
                                                "">

                                                <div style=""
                                                    font-size:13px;
                                                    font-weight:bold;
                                                    color:#0b2540;
                                                    margin-bottom:8px;
                                                "">
                                                    SOONEST GLOBAL EXPRESS
                                                </div>

                                                <div style=""
                                                    font-size:11px;
                                                    color:#888888;
                                                    line-height:1.5;
                                                "">
                                                    This is an automated email. Please do not reply
                                                    to this message.
                                                </div>

                                                <div style=""
                                                    font-size:11px;
                                                    color:#aaaaaa;
                                                    margin-top:10px;
                                                "">
                                                    © 2026 Soonest Global Express. All rights reserved.
                                                </div>

                                            </td>
                                        </tr>

                                    </table>

                                </td>
                            </tr>

                        </table>

                    </body>
                    </html>";
        }

        private static string EmailFormatShipmentStatusUpdate(string customerName, string controlNumber, string shipmentStatus, string statusDateTime, string origin, string destination, string currentLocation, string shipmentTrackUrl, string statusMessage)
        {
            return $@"<!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Shipment Status Update</title>
                    </head>

                    <body style=""
                        margin:0;
                        padding:0;
                        background-color:#f4f6f8;
                        font-family:Arial, Helvetica, sans-serif;
                        color:#333333;
                    "">

                    ```
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                            style=""background-color:#f4f6f8; padding:30px 10px;"">

                        <tr>
                            <td align=""center"">

                                <!-- Main Container -->
                                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0""
                                        style=""
                                            max-width:600px;
                                            width:100%;
                                            background:#ffffff;
                                            border-radius:10px;
                                            overflow:hidden;
                                        "">

                                    <!-- Header -->
                                    <tr>
                                        <td align=""center""
                                            style=""
                                                background-color:#0b2540;
                                                padding:28px 20px;
                                            "">

                                            <div style=""
                                                font-size:24px;
                                                font-weight:bold;
                                                color:#ffffff;
                                                letter-spacing:1px;
                                            "">
                                                SOONEST GLOBAL EXPRESS
                                            </div>

                                            <div style=""
                                                color:#dce5ee;
                                                font-size:13px;
                                                margin-top:6px;
                                            "">
                                                Fast. Reliable. Delivered.
                                            </div>

                                        </td>
                                    </tr>

                                    <!-- Content -->
                                    <tr>
                                        <td style=""padding:40px 35px 35px 35px;"">

                                            <h1 style=""
                                                margin:0 0 18px 0;
                                                color:#0b2540;
                                                font-size:25px;
                                                text-align:center;
                                            "">
                                                Shipment Status Update
                                            </h1>

                                            <p style=""
                                                margin:0 0 18px 0;
                                                font-size:15px;
                                                line-height:1.6;
                                                color:#555555;
                                            "">
                                                Hello <strong>{customerName}</strong>,
                                            </p>

                                            <p style=""
                                                margin:0 0 20px 0;
                                                font-size:15px;
                                                line-height:1.6;
                                                color:#555555;
                                            "">
                                                We would like to inform you that there has been an
                                                update to the status of your shipment.
                                            </p>

                                            <!-- Shipment Status -->
                                            <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                    border=""0""
                                                    style=""
                                                        background-color:#f5f7fa;
                                                        border-radius:8px;
                                                        margin-bottom:25px;
                                                    "">

                                                <tr>
                                                    <td align=""center"" style=""padding:22px;"">

                                                        <div style=""
                                                            font-size:12px;
                                                            color:#777777;
                                                            margin-bottom:8px;
                                                            letter-spacing:.5px;
                                                        "">
                                                            CURRENT SHIPMENT STATUS
                                                        </div>

                                                        <div style=""
                                                            font-size:24px;
                                                            font-weight:bold;
                                                            color:#0b2540;
                                                        "">
                                                            {shipmentStatus}
                                                        </div>

                                                        <div style=""
                                                            font-size:12px;
                                                            color:#888888;
                                                            margin-top:10px;
                                                        "">
                                                            Updated on {statusDateTime}
                                                        </div>

                                                    </td>
                                                </tr>

                                            </table>

                                            <!-- Shipment Details -->
                                            <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                    border=""0""
                                                    style=""
                                                        margin-bottom:25px;
                                                        border-collapse:collapse;
                                                    "">

                                                <tr>
                                                    <td colspan=""2""
                                                        style=""
                                                            padding:0 0 12px 0;
                                                            font-size:13px;
                                                            font-weight:bold;
                                                            color:#0b2540;
                                                        "">
                                                        SHIPMENT DETAILS
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style=""
                                                        padding:10px 0;
                                                        font-size:13px;
                                                        color:#777777;
                                                        border-bottom:1px solid #eeeeee;
                                                    "">
                                                        Tracking Number
                                                    </td>

                                                    <td align=""right""
                                                        style=""
                                                            padding:10px 0;
                                                            font-size:13px;
                                                            font-weight:bold;
                                                            color:#0b2540;
                                                            border-bottom:1px solid #eeeeee;
                                                        "">
                                                        {controlNumber}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style=""
                                                        padding:10px 0;
                                                        font-size:13px;
                                                        color:#777777;
                                                        border-bottom:1px solid #eeeeee;
                                                    "">
                                                        Origin
                                                    </td>

                                                    <td align=""right""
                                                        style=""
                                                            padding:10px 0;
                                                            font-size:13px;
                                                            color:#333333;
                                                            border-bottom:1px solid #eeeeee;
                                                        "">
                                                        {origin}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style=""
                                                        padding:10px 0;
                                                        font-size:13px;
                                                        color:#777777;
                                                        border-bottom:1px solid #eeeeee;
                                                    "">
                                                        Destination
                                                    </td>

                                                    <td align=""right""
                                                        style=""
                                                            padding:10px 0;
                                                            font-size:13px;
                                                            color:#333333;
                                                            border-bottom:1px solid #eeeeee;
                                                        "">
                                                        {destination}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style=""
                                                        padding:10px 0;
                                                        font-size:13px;
                                                        color:#777777;
                                                    "">
                                                        Latest Location
                                                    </td>

                                                    <td align=""right""
                                                        style=""
                                                            padding:10px 0;
                                                            font-size:13px;
                                                            color:#333333;
                                                        "">
                                                        {currentLocation}
                                                    </td>
                                                </tr>

                                            </table>

                                            <!-- Latest Update -->
                                            <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                    border=""0""
                                                    style=""
                                                        border-left:4px solid #d62828;
                                                        background-color:#fff8f8;
                                                        margin-bottom:25px;
                                                    "">

                                                <tr>
                                                    <td style=""
                                                        padding:15px 18px;
                                                        font-size:13px;
                                                        line-height:1.6;
                                                        color:#666666;
                                                    "">

                                                        <strong style=""color:#0b2540;"">
                                                            Latest Update
                                                        </strong>

                                                        <br>
                                                        {statusMessage}
                                                    </td>
                                                </tr>

                                            </table>

                                            <!-- Track Shipment Button -->
                                            <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                                    border=""0"">

                                                <tr>
                                                    <td align=""center""
                                                        style=""padding:5px 0 25px 0;"">

                                                        <a href=""{shipmentTrackUrl}""
                                                            style=""
                                                                display:inline-block;
                                                                background-color:#d62828;
                                                                color:#ffffff;
                                                                text-decoration:none;
                                                                font-size:15px;
                                                                font-weight:bold;
                                                                padding:14px 35px;
                                                                border-radius:6px;
                                                            "">
                                                            Track My Shipment
                                                        </a>

                                                    </td>
                                                </tr>

                                            </table>

                                            <p style=""
                                                margin:0 0 20px 0;
                                                font-size:13px;
                                                line-height:1.6;
                                                color:#777777;
                                            "">
                                                You can use your tracking number to view the latest
                                                shipment information and updates online.
                                            </p>

                                            <p style=""
                                                margin:0;
                                                font-size:12px;
                                                line-height:1.6;
                                                word-break:break-all;
                                            "">
                                                <a href=""{shipmentTrackUrl}""
                                                    style=""color:#0b2540;"">
                                                    {shipmentTrackUrl}
                                                </a>
                                            </p>

                                        </td>
                                    </tr>

                                    <!-- Footer -->
                                    <tr>
                                        <td align=""center""
                                            style=""
                                                background-color:#f1f3f5;
                                                padding:25px 20px;
                                            "">

                                            <div style=""
                                                font-size:13px;
                                                font-weight:bold;
                                                color:#0b2540;
                                                margin-bottom:8px;
                                            "">
                                                SOONEST GLOBAL EXPRESS
                                            </div>

                                            <div style=""
                                                font-size:11px;
                                                color:#888888;
                                                line-height:1.5;
                                            "">
                                                This is an automated email. Please do not reply
                                                to this message.
                                            </div>

                                            <div style=""
                                                font-size:11px;
                                                color:#aaaaaa;
                                                margin-top:10px;
                                            "">
                                                © 2026 Soonest Global Express. All rights reserved.
                                            </div>

                                        </td>
                                    </tr>

                                </table>

                            </td>
                        </tr>

                    </table>
                    ```

                    </body>
                    </html>";
        }
    }
}