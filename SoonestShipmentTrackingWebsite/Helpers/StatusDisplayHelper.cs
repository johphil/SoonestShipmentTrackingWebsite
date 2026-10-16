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

            if (status != ShipmentStatus.OutForDelivery && status != ShipmentStatus.Delivered && status != ShipmentStatus.Returned && status != ShipmentStatus.FailedDelivery)
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
    }
}