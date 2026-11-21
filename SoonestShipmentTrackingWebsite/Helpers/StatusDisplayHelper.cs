using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                case ShipmentStatus.ProcessingCustoms: return "badge-info";
                case ShipmentStatus.InTransit: return "badge-info";
                case ShipmentStatus.ArrivedAtWarehouse: return "badge-info";
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
                case ShipmentStatus.Received: return "Received";
                case ShipmentStatus.ProcessingCustoms: return "Processing Customs";
                case ShipmentStatus.InTransit: return "In Transit";
                case ShipmentStatus.ArrivedAtWarehouse: return "Arrived at Warehouse";
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
        public static string Progress(ShipmentStatus status, ShipmentType shipmentType = ShipmentType.DOMESTIC)
        {
            var isInternational = shipmentType == ShipmentType.INTERNATIONAL;
            string[] labels = isInternational
                ? new[] { "Pending", "Processing Customs", "Arrived at Warehouse", "In Transit", "Out for Delivery", "Delivered", "Received" }
                : new[] { "Pending", "In Transit", "Out for Delivery", "Delivered", "Received" };

            int current;
            bool exceptional = false;
            switch (status)
            {
                case ShipmentStatus.Pending: current = 0; break;
                case ShipmentStatus.ProcessingCustoms: current = isInternational ? 1 : 1; break;
                case ShipmentStatus.ArrivedAtWarehouse: current = isInternational ? 2 : 1; break;
                case ShipmentStatus.InTransit: current = isInternational ? 3 : 1; break;
                case ShipmentStatus.OutForDelivery: current = isInternational ? 4 : 2; break;
                case ShipmentStatus.Delivered: current = isInternational ? 5 : 3; break;
                case ShipmentStatus.Received: current = isInternational ? 6 : 4; break;
                case ShipmentStatus.FailedDelivery: current = isInternational ? 4 : 2; exceptional = true; break;
                case ShipmentStatus.Returned: current = isInternational ? 4 : 2; exceptional = true; break;
                default: current = 0; exceptional = true; break;
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("<ol class=\"stepper\">");
            for (int i = 0; i < labels.Length; i++)
            {
                string cls = "";
                string marker = (i + 1).ToString();
                if (i < current || (i == current && current == labels.Length - 1 && !exceptional))
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

        // ---- Dense shipment-table cell builders (shared by every shipment table) ----

        private static ShipmentType AsType(object v)
        {
            if (v is ShipmentType) return (ShipmentType)v;
            ShipmentType t;
            return Enum.TryParse(Convert.ToString(v), out t) ? t : ShipmentType.DOMESTIC;
        }

        private static ShipmentStatus AsStatus(object v)
        {
            if (v is ShipmentStatus) return (ShipmentStatus)v;
            ShipmentStatus s;
            return Enum.TryParse(Convert.ToString(v), out s) ? s : ShipmentStatus.Pending;
        }

        private static string Txt(object v)
        {
            var s = Convert.ToString(v);
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        private static string Cell(string main, string sub, string subTitle = null)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("<span class=\"cell-main\">" + main + "</span>");
            if (!string.IsNullOrEmpty(sub))
                sb.Append("<span class=\"cell-sub ellipsis\"" + (subTitle != null ? " title=\"" + HttpUtility.HtmlAttributeEncode(subTitle) + "\"" : "") + ">" + sub + "</span>");
            return sb.ToString();
        }

        public static string TypeTag(object type)
        {
            return AsType(type) == ShipmentType.INTERNATIONAL
                ? "<span class=\"type-tag type-intl\">INTL</span>"
                : "<span class=\"type-tag type-dom\">DOM</span>";
        }

        // Control number + shipment type tag, with creation date underneath.
        public static string ShipmentCell(object control, object type, object created)
        {
            var date = created is DateTime ? ((DateTime)created).ToString("MMM d, yyyy") : "-";
            return "<span class=\"cell-main mono\">" + Enc(Convert.ToString(control)) + " " + TypeTag(type) + "</span>"
                 + "<span class=\"cell-sub\">" + date + "</span>";
        }

        // Customer (account holder) with e-mail underneath.
        public static string PartyCell(object name, object email)
        {
            return Cell(Enc(Convert.ToString(name)), Txt(email) == null ? null : Enc(Convert.ToString(email)), Txt(email));
        }

        // Recipient with phone underneath.
        public static string RecipientCell(object name, object phone)
        {
            return Cell(Enc(Convert.ToString(name)), Txt(phone) == null ? null : Enc(Convert.ToString(phone)));
        }

        // Region area
        public static string RegionAreaCell(object region)
        {
            return  $"<span class=\"cell-main nowrap\">{Enc(region.ToString())}</span>";
        }

        // Domestic: origin branch -> destination addr, regionArea underneath.
        // International: container number, packing list / destination underneath.
        public static string RouteCell(object type, object branch, object container, object packingList, object regionArea, object address)
        {
            var addr = Txt(address);
            var area = Txt(regionArea);

            if (AsType(type) == ShipmentType.INTERNATIONAL)
            {
                var c = Txt(container);
                var main = "Customs <span class=\"route-arrow\">&rarr;</span> " + HttpUtility.HtmlEncode(TrimText(area));
                return Cell(main, $"Container No. {c}");
            }

            var origin = Txt(branch);
            var head = (origin == null ? "<span class=\"muted\">Origin n/a</span>" : HttpUtility.HtmlEncode(TrimText(origin)))
                     + (addr != null ? " <span class=\"route-arrow\">&rarr;</span> " + HttpUtility.HtmlEncode(TrimText(area)) : "");
            return $"<span class=\"cell-main nowrap\">{head}</span>";
        }

        // Customer over "To: recipient" for narrow tables.
        public static string PartiesCell(object customer, object recipient, object phone)
        {
            var sub = "To: " + HttpUtility.HtmlEncode(Convert.ToString(recipient)) + (Txt(phone) != null ? " &middot; " + HttpUtility.HtmlEncode(Txt(phone)) : "");
            return Cell(Enc(Convert.ToString(customer)), sub);
        }

        // Package description with weight underneath.
        public static string CargoCell(object description, object weightKg)
        {
            return Cell(TrimText(Enc(Convert.ToString(description))), WeightText(weightKg as decimal?));
        }

        // Status badge, with an issue flag stacked beneath when an issue was reported.
        public static string StatusCell(object status, object issueReported, object issueResolved)
        {
            var html = Badge(AsStatus(status));
            bool reported = false, resolved = false;
            if (issueReported != null) bool.TryParse(Convert.ToString(issueReported), out reported);
            if (issueResolved != null) bool.TryParse(Convert.ToString(issueResolved), out resolved);
            if (reported)
                html += resolved
                    ? "<span class=\"issue-flag resolved\">&#10003; Issue resolved</span>"
                    : "<span class=\"issue-flag open\">&#9873; Issue open</span>";
            return html;
        }

        // Rider name (compact), "Unassigned" once the shipment is at a rider stage.
        public static string RiderCell(object status, object riderName)
        {
            var s = AsStatus(status);
            var name = Txt(riderName);
            if (name != null) return "<span class=\"cell-main\">" + HttpUtility.HtmlEncode(name) + "</span>";
            if (s == ShipmentStatus.OutForDelivery || s == ShipmentStatus.FailedDelivery)
                return "<span class=\"muted\">Unassigned</span>";
            return "<span class=\"muted\">&ndash;</span>";
        }

        // ETA with overdue marker; delivered/received shipments show the received date when known.
        public static string EtaCell(object eta, object status)
        {
            var st = AsStatus(status);
            var d = eta as DateTime?;
            if (!d.HasValue) return "<span class=\"muted\">TBD</span>";
            var text = "<span class=\"cell-main nowrap\">" + d.Value.ToString("MMM d, yyyy") + "</span>";
            if (!IsClosed(st) && d.Value.Date < TimeHelper.Now.Date)
            {
                var days = (TimeHelper.Now.Date - d.Value.Date).Days;
                text += "<span class=\"issue-flag open\">" + days + "d overdue</span>";
            }
            return text;
        }

        // Last update, date over time.
        public static string UpdatedCell(object updated)
        {
            if (!(updated is DateTime)) return "-";
            var d = (DateTime)updated;
            return "<span class=\"cell-main nowrap\">" + d.ToString("MMM d, yyyy") + "</span><span class=\"cell-sub\">" + d.ToString("h:mm tt") + "</span>";
        }

        public static string Enc(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "-" : HttpUtility.HtmlEncode(s.Trim());
        }

        public static string TrimText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Length > 15
                ? text.Substring(0, 15) + "..."
                : text;
        }
    }
}