using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class StatusDisplayHelper
    {
        public static string BadgeClass(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "badge-pending";
                case ShipmentStatus.InTransit: return "badge-info";
                case ShipmentStatus.OutForDelivery: return "badge-warning";
                case ShipmentStatus.Delivered: return "badge-success";
                case ShipmentStatus.FailedDelivery: return "badge-danger";
                case ShipmentStatus.Returned: return "badge-dark";
                case ShipmentStatus.Cancelled: return "badge-danger";
                case ShipmentStatus.Received: return "badge-success";
                default: return "badge-pending";
            }
        }

        public static string ProofImage(object bytesObj, object contentTypeObj)
        {
            var bytes = bytesObj as byte[];
            if (bytes == null || bytes.Length == 0) return string.Empty;
            var type = contentTypeObj as string;
            if (string.IsNullOrWhiteSpace(type) || !type.StartsWith("image/")) type = "image/jpeg";
            var src = "data:" + type + ";base64," + Convert.ToBase64String(bytes);
            return "<div style=\"margin-top:6px;\"><strong>Proof of delivery:</strong><br /><a href=\"" + src + "\" target=\"_blank\"><img src=\"" + src + "\" alt=\"Proof of delivery\" style=\"max-width:140px;max-height:140px;border-radius:6px;border:1px solid #dbe6f0;\" /></a></div>";
        }

        public static string Label(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Received: return "RECEIVED";
                case ShipmentStatus.InTransit: return "In Transit";
                case ShipmentStatus.OutForDelivery: return "Out for Delivery";
                case ShipmentStatus.FailedDelivery: return "Failed Delivery";
                default: return status.ToString();
            }
        }

        // Returns a ready-to-render <span> badge, used from .aspx inline
        // data-binding expressions: <%# StatusDisplayHelper.Badge(...) %>
        public static string Badge(ShipmentStatus status)
        {
            return "<span class=\"status-badge " + BadgeClass(status) + "\">" + Label(status) + "</span>";
        }

        public static string RiderInfo(object statusValue, object riderNameValue)
        {
            ShipmentStatus status;

            if (statusValue is ShipmentStatus)
                status = (ShipmentStatus)statusValue;
            else if (!Enum.TryParse(Convert.ToString(statusValue), out status))
                return "-";

            if (status != ShipmentStatus.OutForDelivery && status != ShipmentStatus.Delivered && status != ShipmentStatus.Returned && status != ShipmentStatus.FailedDelivery && status != ShipmentStatus.Received)
                return "-";

            var riderName = Convert.ToString(riderNameValue);
            if (string.IsNullOrWhiteSpace(riderName))
                return "Waiting for rider assignment";

            return HttpUtility.HtmlEncode(riderName.Trim());
        }

        public static string IssueIndicator(object issueReportedValue, object issueResolvedValue)
        {
            var issueReported = false;
            var issueResolved = false;

            if (issueReportedValue != null)
                bool.TryParse(Convert.ToString(issueReportedValue), out issueReported);

            if (issueResolvedValue != null)
                bool.TryParse(Convert.ToString(issueResolvedValue), out issueResolved);

            if (!issueReported)
                return "<span class=\"status-badge badge-pending\">No Issue</span>";

            if (issueResolved)
                return "<span class=\"status-badge badge-success\">Issue Resolved</span>";

            return "<span class=\"status-badge badge-danger\">Issue Reported</span>";
        }

        // ---- Compact-UI helpers (progress stepper, ETA, weight, elapsed time) ----

        // Horizontal progress stepper: Pending > In Transit > Out for Delivery > Delivered > Received.
        // Exceptional end states (failed / returned / cancelled) flag the step they stopped at.
        public static string Progress(ShipmentStatus status)
        {
            string[] labels = { "Pending", "In Transit", "Out for Delivery", "Delivered", "Received" };
            int current;
            bool exceptional = false;
            switch (status)
            {
                case ShipmentStatus.Pending: current = 0; break;
                case ShipmentStatus.InTransit: current = 1; break;
                case ShipmentStatus.OutForDelivery: current = 2; break;
                case ShipmentStatus.Delivered: current = 3; break;
                case ShipmentStatus.Received: current = 4; break;
                case ShipmentStatus.FailedDelivery: current = 2; exceptional = true; break;
                case ShipmentStatus.Returned: current = 2; exceptional = true; break;
                default: current = 0; exceptional = true; break; // Cancelled
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("<ol class=\"stepper\">");
            for (int i = 0; i < labels.Length; i++)
            {
                string cls = "";
                string marker = (i + 1).ToString();
                if (i < current || (i == current && current == 4 && !exceptional))
                {
                    cls = "done";
                    marker = "&#10003;";
                }
                else if (i == current)
                {
                    cls = exceptional ? "fail" : "current";
                    if (exceptional) marker = "!";
                }
                sb.Append("<li class=\"" + cls + "\"><span class=\"step-dot\">" + marker + "</span><span class=\"step-label\">" + labels[i] + "</span></li>");
            }
            sb.Append("</ol>");
            if (exceptional)
                sb.Append("<div class=\"stepper-note\">Current state: " + Label(status) + "</div>");
            return sb.ToString();
        }

        public static bool IsClosed(ShipmentStatus status)
        {
            return status == ShipmentStatus.Delivered || status == ShipmentStatus.Received
                || status == ShipmentStatus.Returned || status == ShipmentStatus.Cancelled;
        }

        // ETA date, with an "Overdue" tag when the date has passed and the shipment is still open.
        public static string EtaText(DateTime? eta, ShipmentStatus status)
        {
            if (!eta.HasValue) return "TBD";
            var text = eta.Value.ToString("MMM d, yyyy");
            if (!IsClosed(status) && eta.Value.Date < TimeHelper.Now.Date)
                text += " <span class=\"status-badge badge-danger\">Overdue</span>";
            return text;
        }

        public static string WeightText(decimal? kg)
        {
            return kg.HasValue ? kg.Value.ToString("0.##") + " kg" : "-";
        }

        public static string Elapsed(DateTime from, DateTime to)
        {
            var span = to - from;
            if (span < TimeSpan.Zero) span = TimeSpan.Zero;
            if (span.TotalDays >= 1) return (int)span.TotalDays + "d " + span.Hours + "h";
            if (span.TotalHours >= 1) return (int)span.TotalHours + "h " + span.Minutes + "m";
            return Math.Max(1, (int)span.TotalMinutes) + "m";
        }

        // Key/value cell for the compact detail grids.
        public static string Kv(string label, string valueHtml)
        {
            if (string.IsNullOrWhiteSpace(valueHtml)) valueHtml = "-";
            return "<div class=\"kv\"><div class=\"kv-label\">" + label + "</div><div class=\"kv-value\">" + valueHtml + "</div></div>";
        }

        public static string Enc(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "-" : HttpUtility.HtmlEncode(s.Trim());
        }
    }
}